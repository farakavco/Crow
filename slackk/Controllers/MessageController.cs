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
            CrowResponse Response = null;
            var Request = HttpContext.Current.Request;
            var VerificationResult = Verifier.Verify(Message, Request.Headers, Request.UserHostAddress);
            if (VerificationResult.OK)
            {
                Message.IP = Request.UserHostAddress;
                SlackResponse SlackResponse = SlackClient.Deliver(Message);
                Response = new CrowResponse()
                {
                    OK = SlackResponse.OK,
                    Error = SlackResponse.Error
                };
            }
            else
            {
                Response = new CrowResponse()
                {
                    OK = VerificationResult.OK,
                    Error = VerificationResult.Error.ToString()
                };
            }
            return Response;
        }

    }
}
