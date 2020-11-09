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
            UpdateClient updateclient = new UpdateClient();
            DownloadManagerClient downloadmanagerclient = new DownloadManagerClient();

            string clientVersion = "1.1";

            string clientplatform = GetOperatingSystem();

            string OS_Bit = null;

            if (Environment.Is64BitOperatingSystem)
            {
                OS_Bit = "64-bit";
            }
            else
            {
                OS_Bit = "32-bit";
            }

            ValidationResponse clientconfiguration = new ValidationResponse();
            {
                clientconfiguration.clientPlatform = clientplatform;
                clientconfiguration.ClientOS_Bit = OS_Bit;
                clientconfiguration.ClientVersionNumber = clientVersion;
            }

            string json = JsonConvert.SerializeObject(clientconfiguration);

            /*
             * GET Request
             */
            ValidationResponse validationResult = updateclient.ValidateClientVersion(json).GetAwaiter().GetResult();

            if(validationResult.error_code != 0)
            {
                if(validationResult.MandatoryUpdate)
                {
                    Console.WriteLine("Its a mandatory update, calling Download Manager.");
                    downloadmanagerclient.callDownloadManager(validationResult.CurrentStableVersion);
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
                        downloadmanagerclient.callDownloadManager(validationResult.CurrentStableVersion);
                    }
                }
            }
            else
            {
                Console.WriteLine("Your Application is Up-To-Date");
            }

            //Console.WriteLine("Continuing with ostore application without update.");

            Console.ReadKey();
        }

        public static string GetOperatingSystem()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                return "OSX";
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
