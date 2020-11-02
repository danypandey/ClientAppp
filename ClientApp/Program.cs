using System;
using System.Threading.Tasks;
using UserCommonApp;

namespace ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            SampleClient result = new SampleClient();

           /*
            * GET Request
            */
            result.CheckVersionUpdate("1.2");

            Console.ReadKey();
        }
    }
}
