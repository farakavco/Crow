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
        public CrowResponse Upload(CrowMessage message)
        {
            var Request = HttpContext.Current.Request;
            message.IP = HttpContext.Current.Request.UserHostAddress;
            var VerificationResult = Verifier.Verify(message, Request);
            if (VerificationResult.OK == true)
            {
                message.IP = Request.UserHostAddress;
                message.Time = DateTime.Now;
                SlackResponse SlackResponse = SlackClient.Deliver(message);
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
                    OK = VerificationResult.OK,
                    Error = VerificationResult.Error.ToString()
                };
            }
        }

    }
}
