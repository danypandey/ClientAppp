using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class SampleClient 
    {
        SampleClient1 sampleclient1 = new SampleClient1();
        string baseUri = default(string);
        GenericRestClient client;
        public SampleClient()
        {
            baseUri = "http://127.0.0.1:8080";
            client = new GenericRestClient(baseUri);
        }

        public async Task CheckVersionUpdate(string clientVersion)
        {
            UserCommonApp.Result clientUpdateResult = await ValidateClientVersion(clientVersion);
            Console.WriteLine(clientUpdateResult.Error_code);
            Console.WriteLine(clientUpdateResult.CurrentStableVersion);
            Console.WriteLine(clientUpdateResult.MandatoryUpdate);
            sampleclient1.callDownloadManager(clientUpdateResult.CurrentStableVersion);
        }


        private async Task<UserCommonApp.Result> ValidateClientVersion(string CurrentClientVersion)
        {
            string relativeUrl = string.Format("/updateservice/{0}", CurrentClientVersion);
            UserCommonApp.Result versionResult = null;
            Action<UserCommonApp.Result> onSuccess = new Action<UserCommonApp.Result>((validateResult =>
            {
                versionResult = validateResult;
            }));
            Action<HttpFailure> onFailure = new Action<HttpFailure>((failure) =>
            {
                Console.WriteLine("http failure " + failure.Message);
            });
            await client.GetAsync(onSuccess, onFailure, relativeUrl);

            return versionResult;
        }        
    }
}
