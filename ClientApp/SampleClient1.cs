using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class SampleClient1
    {
        string baseUri = default(string);
        GenericRestClient client;
        public SampleClient1()
        {
            baseUri = "http://127.0.0.1:8001";
            client = new GenericRestClient(baseUri);
        }

        public async Task callDownloadManager(string versionNumber)
        {
            CallingDownloadManager(versionNumber);
        }

        private async Task CallingDownloadManager(string VersionNumber)
        {
            string relativeUrl = string.Format("/updateservice/download/{0}", VersionNumber);
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
        }
    }
}
