using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class UpgradeManagerClient
    {
        string baseUri = default(string);
        GenericRestClient client;

        public UpgradeManagerClient(string baseUri)
        {
            baseUri = baseUri;
            client = new GenericRestClient(baseUri);
        }

        internal async Task<ValidationResponse> ValidateClientVersion(string ClienConfiguration)
        {
            string relativeUrl = string.Format("/updateservice/{0}", ClienConfiguration);
            ValidationResponse UpgradeClientManagerResponse = null;
            Action<ValidationResponse> onSuccess = new Action<ValidationResponse>((validateResult =>
            {
                UpgradeClientManagerResponse = validateResult;
            }));
            Action<HttpFailure> onFailure = new Action<HttpFailure>((failure) =>
            {
                UpgradeClientManagerResponse = new ValidationResponse()
                {
                    error_code = 1,
                    error_message = failure.Message
                };
            });
            await client.GetAsync(onSuccess, onFailure, relativeUrl);
            return UpgradeClientManagerResponse;
        }
    }
}
