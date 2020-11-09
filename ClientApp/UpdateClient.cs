using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class UpdateClient 
    {
        DownloadManagerClient sampleclient1 = new DownloadManagerClient();
        string baseUri = default(string);
        GenericRestClient client;
        public UpdateClient()
        {
            baseUri = "http://127.0.0.1:8080";
            client = new GenericRestClient(baseUri);
        }


        internal async Task<ValidationResponse> ValidateClientVersion(string ClienConfiguration)
        {
            string relativeUrl = string.Format("/updateservice/{0}", ClienConfiguration);
            ValidationResponse versionResult = null;
            Action<ValidationResponse> onSuccess = new Action<ValidationResponse>((validateResult =>
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
