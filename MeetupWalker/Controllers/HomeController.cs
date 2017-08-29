using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MeetupAPIGrp;
using System.Collections;

namespace MeetupWalker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

         public IActionResult Index()
        {
            // Sends a message to configured loggers, including the Stackdriver logger.
            // The Microsoft.AspNetCore.Mvc.Internal.ControllerActionInvoker logger will log all controller actions with
            // log level information. This log is for additional information.
            _logger.LogInformation("Home page hit!");
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";
            return View();
        }

        public IActionResult Error()
        {
            // Log messages with different log levels.
            _logger.LogError("Error page hit!");
            return View();
        }

        public ActionResult GetGroups()
        {
            var groupsId = new MeetupAPI.GetGroups();
            List<string[]> grpsId = groupsId.GetGroupsId("CA", "AB", "Calgary", "100");
            ViewBag.allGroupIds = grpsId;
            return View("Groups", grpsId);
        }

        [HttpGet]
        [ActionName("GroupsAndMembers")]
        public ActionResult GroupsAndMembers_Get()
        {
            return View("GroupsAndMembers_Get");
        }

        [HttpPost]
        [ActionName("GroupsAndMembers")]
        public ActionResult GroupsAndMembers_Post(string[] GroupIdsArg)
        {

            string tmpString = GroupIdsArg[0];
            var getMembers = new MeetupAPI.GetMembers();
            List<string[]> tmpGrpsId = new List<string[]>();

            List<string[]> CombinedGrpsId = new List<string[]>();
            List<string> GroupIds = tmpString.Split(',').ToList();
            
            foreach (string item in GroupIds)
            {

                ////first get group info then get that group members info
                var grpId = new MeetupAPI.GetGroups();
                string[] grpIdinfo = grpId.GetGroupInfo(item);
                //ViewBag.allGroupIds = grpsId;

                //get all members info for this item(group id)
                List<string[]> grpMembersInfo = getMembers.GetMembersDetail(item);

                foreach(var member in grpMembersInfo)
                {

                    string[] tmp = new string[13];

                    grpIdinfo.CopyTo(tmp, 0);
                    member.CopyTo(tmp, grpIdinfo.Length);

                    CombinedGrpsId.Add(tmp);

                }
            }
             return View("GroupsAndMembers_Post", CombinedGrpsId);

        }

     
    }
}
