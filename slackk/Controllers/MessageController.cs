using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using slackk.Services;
using slackk.Models;
using System.Web;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Configuration;


namespace slackk.Controllers
{
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {
        SlackClient SlackClient = new SlackClient();

        [HttpPost]
        [Route("")]
        public CrowResponse Upload(CrowMessage Message)
        {
            if (Message == null || !Message.Verify())
            {
                return new CrowResponse() { OK = false, Error = "Bad Request" };
            }
            Message.IP = HttpContext.Current.Request.UserHostAddress;
            return SlackClient.Deliver(Message);
        }

    }
}
