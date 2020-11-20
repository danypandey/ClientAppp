using System;
using System.Runtime.InteropServices;
using UserCommonApp;
using Newtonsoft.Json;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            string UpdateClientBaseUri = "http://127.0.0.1:8080";

            string DownloadManagerClientBaseUri = "http://127.0.0.1:8001";

            UpgradeManagerClient updateclient = new UpgradeManagerClient(UpdateClientBaseUri);
            DownloadManagerClient downloadmanagerclient = new DownloadManagerClient(DownloadManagerClientBaseUri);

            string clientVersion = "1.1";

            string clientplatform = GetOperatingSystem();

            bool is64bit;

            if (Environment.Is64BitOperatingSystem)
            {
                is64bit = true;
            }
            else
            {
                is64bit = false;
            }


            ClientResponse clientconfiguration = new ClientResponse();
            {
                clientconfiguration.clientPlatform = clientplatform;
                clientconfiguration.is64Bit = is64bit;
                clientconfiguration.ClientVersionNumber = clientVersion;
            }

            string clientConfig = JsonConvert.SerializeObject(clientconfiguration);

            /*
             * GET Request
             */
            ValidationResponse validationResult = updateclient.ValidateClientVersion(clientConfig).GetAwaiter().GetResult();

            if(validationResult.error_code == 0)
            {
                if(validationResult.isUpdateAvailable)
                {
                    if (validationResult.MandatoryUpdate)
                    {
                        Console.WriteLine("Its a mandatory update, calling Download Manager.");
                        downloadmanagerclient.callDownloadManager(validationResult.UpgradeReferenceId);
                    }
                    else
                    {
                        Console.WriteLine("Its not a mandatory update, please press 1 to update or 0 to skip.");
                        int userInput = int.Parse(Console.ReadLine());
                        if (userInput == 0)
                        {
                            Console.WriteLine("Update Cancelled");

                            //set skipped version of user in server
                        }
                        else if (userInput == 1)
                        {
                            Console.WriteLine("Calling Download Manager.");
                            downloadmanagerclient.callDownloadManager(validationResult.UpgradeReferenceId);
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Your Application is Up-To-Date");
                }
            }
            /*else
            {
                // Handle errors here
                Console.WriteLine(validationResult.error_code);
            }*/

            //Console.WriteLine("Continuing with ostore application without update.");

            Console.ReadKey();
        }

        public static string GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "MAC";
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                return "Linux";
            }

            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return "Windows";
            }

            else
            {
                throw new Exception("Cannot Determine Operating System");
            }
        }
    }
}
