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

        public DownloadManagerClient(string baseUri)
        {
            baseUri = baseUri;
            client = new GenericRestClient(baseUri);
        }

        internal async Task callDownloadManager(string UpgradeReferenceId)
        {
            string relativeUrl = string.Format("/updateservice/download/{0}", UpgradeReferenceId);
            ValidationResponse DownloadManagerServerResponse = null;
            Action<ValidationResponse> onSuccess = new Action<ValidationResponse>((validateResult =>
            {
                DownloadManagerServerResponse = validateResult;
            }));
            Action<HttpFailure> onFailure = new Action<HttpFailure>((failure) =>
            {
                DownloadManagerServerResponse = new ValidationResponse()
                {
                    error_code = 1,
                    error_message = failure.Message
                };
            });
            client.GetAsync(onSuccess, onFailure, relativeUrl);
        }
    }
}
