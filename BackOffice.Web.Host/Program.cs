using BackOffice.Common;
using Nancy.Hosting.Self;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Web.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var host = new NancyHost(new[] {
                new Uri(@"http:\\localhost:5000")
            }))
            {
                Logging.Log().Information(@"Starting listening on http:\\localhost:5000");
                host.Start();
                Console.Read();
            }
        }
    }
}
