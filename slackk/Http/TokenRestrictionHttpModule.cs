using Newtonsoft.Json;
using slackk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Varzesh3.Leo.Utility;

namespace slackk.Http
{
    public class TokenRestrictionHttpModule : IHttpModule
    {
        public string ModuleName
        {
            get { return "TokenRestrictionHttpModule"; }
        }
        public void Dispose()
        {
         //
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += Context_BeginRequest;
            context.EndRequest += Context_EndRequest;
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            //
        }

        private void Context_BeginRequest(object sender, EventArgs e)
        {
            var application = (HttpApplication)sender;
            var context = application.Context;
            var headers = context.Request.Headers;
            if ((headers["X-JWT-Token"] != null) && ((headers["X-JWT-Token"].Split('.')).Length == 3))
            {
                string ProvidedJWTToken = JwtHelper.Decode(headers["X-JWT-Token"], ConfigurationManager.AppSettings["SecretKey"], false);
                if (ProvidedJWTToken == "Bad Format")
                {
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
                string PurifiedJWTToken = JsonConvert.DeserializeObject<Authentication>(ProvidedJWTToken).Token;
                if (PurifiedJWTToken != ConfigurationManager.AppSettings["SlackToken"])
                { 
                    context.Response.StatusCode = 401;
                    context.Response.End();
                }
            }
            else
            {
                context.Response.StatusCode = 401;
                context.Response.End();
            }
        }
    }
}
