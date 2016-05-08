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
        MessageVerifier Verifier = new MessageVerifier();

        [HttpPost]
        [Route("")]
        public CrowResponse Upload(CrowMessage Message)
        {
            if (!Message.Verify())
            {
                return new CrowResponse() { OK = false, Error = "" };
            }
            return SlackClient.Deliver(Message) as CrowResponse;
        }

    }
}
