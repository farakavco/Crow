using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using slackk.Services;
using slackk.Models;

namespace slackk.Controllers
{
    [RoutePrefix("api/message")]
    public class MessageController : ApiController
    {
        SlackClient SlackClient = new SlackClient();
        [HttpPost]
        [Route("")]
        public CrowResponse Upload(CrowMessage message)
        {
           if (message != null && (message.Channel is string) && (message.Token is string))
            {
                SlackResponse SlackResponse =  SlackClient.Deliver(message);
                return new CrowResponse()
                {
                    OK = SlackResponse.OK,
                    Error = SlackResponse.Error
                };
            }
            else
            {
                return new CrowResponse()
                {
                    OK = "False",
                    Error = "Bad Request"
                };
            }

        }
    }
}
