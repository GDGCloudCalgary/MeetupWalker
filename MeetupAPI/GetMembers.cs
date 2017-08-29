using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace MeetupAPI
{
    public class GetMembers
    {
        public void GetMembersDetail()
        {

        }

        public List<string[]> GetMembersDetail(string GroupIds)
        {
            //string GroupId = GroupIds;
            // get total number of counts
            string ratelimit;
            var url0 = new Uri("https://api.meetup.com/2/members?key=" + WebGen.APIKey + "&format=json&sign=true" + "&group_id=" + GroupIds);
            string jsonString0 = WebGen.GetPageAsString(url0,out ratelimit);
            var rootObject0 = JsonConvert.DeserializeObject<MeetupAPIGrp.Rootobject>(jsonString0);

            int total_Count = rootObject0.meta.total_count;
            int pageSize = 200;
            int total_number_request = total_Count / pageSize;

            // list to keep group naem and id
            List<string[]> JsonDS = new List<string[]>();

            for (int i = 0; i < total_number_request + 1; i++)
            {
                var url = new Uri("https://api.meetup.com/2/members?key=" + WebGen.APIKey + "&format=json&sign=true" + "&group_id="+ GroupIds + "&offset=" + i.ToString());
                string jsonString = WebGen.GetPageAsString(url,out ratelimit);
                var rootObject = JsonConvert.DeserializeObject<MeetupAPIGrp.Rootobject>(jsonString);

                foreach (var result in rootObject.results)
                {
                    string name = result.name;
                    string id = result.id.ToString();
                    string joined = DateTimeOffset.FromUnixTimeMilliseconds(result.joined).ToString("yyyy-MM-dd");
                    string status = result.status.ToString();
                    
                    //string description = result.description?.ToString() ?? "" ;
                    //string description = result.description.ToString();
                    JsonDS.Add(new string[] { name, id, joined, status });
                }
            }

            return JsonDS;
        }

    }
}
