using slackk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using slackk.Services;
using Varzesh3.Leo.Utility;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;

namespace slackk.Services
{
    public class MessageVerifier
    {
        public VerifyResponse Verify(CrowMessage Message, HttpRequest Request)
        {
            //Checking the IP
            //if (!ConfigurationManager.AppSettings["AllowedIPs"].Split('&').Contains(Request.UserHostAddress))
            //    return new VerifyResponse()
            //    {
            //        OK = false,
            //        Error = HttpStatusCode.Unauthorized
            //    };

            // Checking the validity of token
            if (Request.Headers["X-JWT-Token"] != string.Empty)
            {
                string ProvidedJWTToken = JwtHelper.Decode(Request.Headers["X-JWT-Token"], ConfigurationManager.AppSettings["SecretKey"], false);
                string PurifiedJWTToken = JsonConvert.DeserializeObject<Authentication>(ProvidedJWTToken).Token;
                if (PurifiedJWTToken != ConfigurationManager.AppSettings["SlackToken"])
                    return new VerifyResponse()
                    {
                        OK = false,
                        Error = HttpStatusCode.Unauthorized
                    };
            }
            // Checking the validity of the message
            if (Message == null)
                return new VerifyResponse()
                {
                    OK = false,
                    Error = HttpStatusCode.BadRequest
                };
            // Caring for the exception caused by multipart-data
            else if (Message.Text == ConfigurationManager.AppSettings["MultiPartException"])
                return new VerifyResponse()
                {
                    OK = false,
                    Error = HttpStatusCode.NotAcceptable
                };
            else
                return new VerifyResponse()
                {
                    OK = true,
                    Error = HttpStatusCode.OK
                };
        }
    }
}