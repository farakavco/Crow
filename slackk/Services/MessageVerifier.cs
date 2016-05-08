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
using System.Collections.Specialized;

namespace slackk.Services
{
    public class MessageVerifier
    {
        public VerifyResponse Verify(CrowMessage Message, NameValueCollection Headers, string UserHostAddress)
        {
            if (Authenticate(Headers, UserHostAddress))
            {
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
                        Error = HttpStatusCode.BadRequest
                    };
                // Checking to ensure if both file and filename are provided
                else if ((Message.FileName != null && Message.File == null) || (Message.File != null && Message.FileName == null))
                {
                    return new VerifyResponse()
                    {
                        OK = false,
                        Error = HttpStatusCode.BadRequest
                    };
                }
                else
                    return new VerifyResponse()
                    {
                        OK = true,
                        Error = HttpStatusCode.OK
                    };
            }
            else
            {
                return new VerifyResponse()
                {
                    OK = false,
                    Error = HttpStatusCode.Unauthorized
                };
            }
        }
        public bool Authenticate(NameValueCollection Headers, string UserHostAddress)
        {
            bool Authenticated = false;
            if (/*ConfigurationManager.AppSettings["AllowedIPs"].Split('&').Contains(UserHostAddress) && */(Headers["X-JWT-Token"] != null) && ((Headers["X-JWT-Token"].Split('.')).Length == 3))
            {
                string ProvidedJWTToken = JwtHelper.Decode(Headers["X-JWT-Token"], ConfigurationManager.AppSettings["SecretKey"], false);
                string PurifiedJWTToken = JsonConvert.DeserializeObject<Authentication>(ProvidedJWTToken).Token;
                if (PurifiedJWTToken == ConfigurationManager.AppSettings["SlackToken"])
                    Authenticated = true;
            }
            return Authenticated;
        }

    }
}