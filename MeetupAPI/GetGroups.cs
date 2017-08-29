using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using MeetupAPIGrp;
using MeetupAPIevents;

namespace MeetupAPI
{
    public class GetGroups
    {
        public void GetGroupsId()
        {

        }


        public string[] GetGroupInfo(string GroupId)
        {
            // get one group info

            // get total number of counts
            string ratelimit;
            var url0 = new Uri("https://api.meetup.com/2/groups?key=" + WebGen.APIKey + "&format=json&sign=true" + "&country=Ca" + "&city=Calgary" + "&state=AB" + "&radius=25" + "&page=20" + "&group_id=" + GroupId);
            string jsonString0 = WebGen.GetPageAsString(url0, out ratelimit);
            var rootObject0 = JsonConvert.DeserializeObject<MeetupAPIOneGroupInfo.Rootobject>(jsonString0);
            var result = rootObject0.results;
            
            // list to keep group naem and id
            string[] JsonDS = new string[9];
            
            string GroupName = result[0].name;
            string GroupOrganizer_name = result[0].organizer.name;
            string GroupDescription = result[0].description;
            string total_events = "";
            string name = result[0].name;
            string strGroupId = result[0].id.ToString();
            string GroupMembers = result[0].members.ToString();
            string GroupCreated = DateTimeOffset.FromUnixTimeMilliseconds(result[0].created).ToString("yyyy-MM-dd");
            string GroupUrlName = result[0].urlname.ToString();

            //if (result.organizer != null)
            //{
            //    organizer_name = result.organizer.name.ToString();
            //}
            //else
            //{
            //    organizer_name = "unknown";
            //}
            //if (result.description != null)
            //{
            //    description = result.description.ToString();
            //}
            //else
            //{
            //    description = "unknown";
            //}

            total_events = "can not be enumerated with this app";
            ratelimit = "can not be enumerated with this app";
            JsonDS = new string[] { GroupName, strGroupId, GroupMembers, GroupCreated, GroupUrlName, GroupOrganizer_name, GroupDescription, total_events, ratelimit };

            return JsonDS;
        }

        public List<string[]> GetGroupsId(string Country, string Province, string City, string Radius)
        {

            // get total number of counts
            string ratelimit;
            var url0 = new Uri("https://api.meetup.com/2/groups?key=" + WebGen.APIKey + "&format=json&sign=true" + "&country=" + Country + "&city=" + City + "&state=" + Province + "&radius=25");
            string jsonString0 = WebGen.GetPageAsString(url0, out ratelimit);
            var rootObject0 = JsonConvert.DeserializeObject<MeetupAPIGrp.Rootobject>(jsonString0);

            int total_Count = rootObject0.meta.total_count;
            int pageSize = 200;
            int total_number_request = total_Count / pageSize;



            // list to keep group naem and id
            List<string[]> JsonDS = new List<string[]>();

            for (int i = 0; i < total_number_request + 1; i++)
            {
                var url = new Uri("https://api.meetup.com/2/groups?key=" + WebGen.APIKey + "&format=json&sign=true" + "&country=" + Country + "&city=" + City + "&state=" + Province + "&radius=25" + "&page=200" + "&offset=" + i.ToString());
                string jsonString = WebGen.GetPageAsString(url, out ratelimit);
                var rootObject = JsonConvert.DeserializeObject<Rootobject>(jsonString);

                foreach (var result in rootObject.results)
                {

                    string organizer_name = "";
                    string description = "";
                    string total_events = "";
                    string name = result.name;
                    string id = result.id.ToString();
                    string members = result.members.ToString();
                    string created = DateTimeOffset.FromUnixTimeMilliseconds(result.created).ToString("yyyy-MM-dd");
                    string urlName = result.urlname.ToString();

                    if (result.organizer != null)
                    {
                        organizer_name = result.organizer.name.ToString();
                    }
                    else
                    {
                        organizer_name = "unknown";
                    }
                    if (result.description != null)
                    {
                        description = result.description.ToString();
                    }
                    else
                    {
                        description = "unknown";
                    }

                    /////
                    ///// can not use this in web app it will goes beyond 200 reg/hour
                    //////https://api.meetup.com/events?offset=0&format=json&group_id=17081152&photo-host=public&page=20&radius=25.0&fields=&order=time&sig_id=81958852&
                    ////// get total number of events for this group
                    //var url1 = new Uri("https://api.meetup.com/2/events?key=" + WebGen.APIKey + "&format=json&sign=true" + "&group_id=" + id);
                    //string jsonString1 = WebGen.GetPageAsString(url1,out ratelimit);
                    //var rootObject1 = JsonConvert.DeserializeObject<MeetupAPIevents.Rootobject>(jsonString1);

                    //if(rootObject1.meta.total_count != 0)
                    //{
                    //    total_events = rootObject1.meta.total_count.ToString();
                    //}
                    //else
                    //{
                    //    total_events = "unknown";
                    //}

                    // controlling rate limit
                    //if (JsonDS.Count == 100)
                    //{
                    //    System.Threading.Thread.Sleep(35000);
                    //}


                    total_events = "can not be enumerated with this app";
                    ratelimit = "can not be enumerated with this app";
                    JsonDS.Add(new string[] { name, id, members, created, urlName, organizer_name, description, total_events, ratelimit });
                }
            }

            return JsonDS;
        }


    }
}
