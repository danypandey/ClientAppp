using System;
using System.Threading.Tasks;
using UserCommonApp;
using Ziroh.Misc.Common;

namespace ClientApp
{
    class SampleClient 
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
