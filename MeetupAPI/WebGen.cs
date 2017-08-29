using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace MeetupAPI
{
    class WebGen
    {
        public const string APIKey = "6dcb6c2e1f664511a49537623f51";

        public static string GetPageAsString(Uri address, out string ratelimit)
        {
            string result = "";
            var request = WebRequest.Create(address) as HttpWebRequest;
            using (var response = request.GetResponseAsync().Result)
            {
                var reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();

                ratelimit = response.Headers["X-RateLimit-Reset"];
            }

            if (Convert.ToInt16(ratelimit) < 3)
            {
                System.Threading.Thread.Sleep(15000);
                ratelimit = "ratelimit reached";
            }
            ratelimit = "___";
            return result;
        }
    }
}
