using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class DownloadManagerClient
    {
        string baseUri = default(string);
        GenericRestClient client;
        public DownloadManagerClient()
        {
            baseUri = "http://127.0.0.1:8001";
            client = new GenericRestClient(baseUri);
        }


        internal async Task callDownloadManager(string UpgradeReferenceId)
        {
            string relativeUrl = string.Format("/updateservice/download/{0}", UpgradeReferenceId);
            ValidationResponse versionResult = null;
            Action<ValidationResponse> onSuccess = new Action<ValidationResponse>((validateResult =>
            {
                versionResult = validateResult;
            }));
            Action<HttpFailure> onFailure = new Action<HttpFailure>((failure) =>
            {
                Console.WriteLine("http failure " + failure.Message);
            });
            client.GetAsync(onSuccess, onFailure, relativeUrl);
        }
    }
}
