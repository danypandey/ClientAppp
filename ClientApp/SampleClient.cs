using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TagLib.IFD.Tags;
using UserCommonApp;
using Ziroh.Misc.Common;
using Ziroh.Misc.Logging;

namespace ClientApp
{
    class SampleClient : IUserService
    {
        string baseUri = default(string);
        GenericRestClient client;
        public SampleClient()
        {
            baseUri = "http://127.0.0.1:8080";
            client = new GenericRestClient(baseUri);
        }

        public async Task<UserCommonApp.Result> ValidateClientVersion(string CurrentClientVersion)
        {
            string relativeUrl = string.Format("/updateservice/{0}", CurrentClientVersion);
            UserCommonApp.Result versionResult = null;
            Action<UserCommonApp.Result> onSuccess = new Action<UserCommonApp.Result>((credential =>
            {
                versionResult = credential;
                if(versionResult.Error_code == 0)
                {
                    Console.WriteLine("Your App is up-to-date");
                }
                else if(versionResult.Error_code == 1 && versionResult.MandatoryUpdate)
                {
                    Console.WriteLine(versionResult.CurrentStableVersion);
                    Console.WriteLine(versionResult.MandatoryUpdate);
                    Console.WriteLine("Its a mandatory update, please press 1 continue or 0 to cancel");
                    int userInput = int.Parse(Console.ReadLine());
                    if(userInput == 0)
                    {
                        Console.WriteLine("Update Cancelled");
                    }
                    else if(userInput == 1)
                    {
                        Console.WriteLine("Calling Download Manager");
                    }
                }
                else if (versionResult.Error_code == 1 && !versionResult.MandatoryUpdate)
                {
                    Console.WriteLine(versionResult.CurrentStableVersion);
                    Console.WriteLine(versionResult.MandatoryUpdate);
                    Console.WriteLine("Its not a mandatory update, please press 1 continue or 0 to cancel");
                    int userInput = int.Parse(Console.ReadLine());
                    if (userInput == 0)
                    {
                        Console.WriteLine("Update Cancelled");
                    }
                    else if (userInput == 1)
                    {
                        Console.WriteLine("Calling Download Manager");
                    }
                }
            }));
            Action<HttpFailure> onFailure = new Action<HttpFailure>((failure) =>
            {
                Console.WriteLine("http failure " + failure.Message);
            });
            await client.GetAsync(onSuccess, onFailure, relativeUrl);

            return versionResult;
        }

        public Task<UserCommonApp.Result> DownloadBinaries(string VersionNumber)
        {
            throw new NotImplementedException();
        }

        public Task<UserCommonApp.Result> NotifyAllClients()
        {
            throw new NotImplementedException();
        }
    }
}
