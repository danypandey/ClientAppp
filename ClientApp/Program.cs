using System;
using UserCommonApp;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            UpdateClient updateclient = new UpdateClient();
            DownloadManagerClient downloadmanagerclient = new DownloadManagerClient();

            /*
             * GET Request
             */
            ValidationResponse validationResult = updateclient.ValidateClientVersion("1.2").GetAwaiter().GetResult();

            if(validationResult.error_code != 0)
            {
                if(validationResult.MandatoryUpdate)
                {
                    Console.WriteLine("Its a mandatory update, calling Download Manager.");
                    downloadmanagerclient.callDownloadManager(validationResult.CurrentStableVersion);
                }
                else
                {
                    Console.WriteLine("Its not a mandatory update, please press 1 to update or 0 to cancel");
                    int userInput = int.Parse(Console.ReadLine());
                    if (userInput == 0)
                    {
                        Console.WriteLine("Update Cancelled");
                        Environment.Exit(0);
                    }
                    else if (userInput == 1)
                    {
                        downloadmanagerclient.callDownloadManager(validationResult.CurrentStableVersion);
                    }
                }
            }
            else
            {
                Console.WriteLine("Your Application is Up-To-Date");
                Environment.Exit(0);
            }
            

            Console.ReadKey();
        }
    }
}
