using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UserCommonApp;
using Ziroh.Misc.Logging;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleClient result = new SampleClient();

            /*
             *GET Request
             */
            Task<Result> detail = result.ValidateClientVersion("1.0");

            Console.ReadKey();
        }
    }
}
