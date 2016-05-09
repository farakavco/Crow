using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

namespace slackk.Http
{
    public class AddressRestrictionHttpModule : IHttpModule
    {
        public string ModuleName
        {
            get { return "AddressRestrictionHttpModule"; }
        }
        private static List<int> allowedAddresses;

        private static List<int> AllowedAddresses
        {
            get
            {
                if (allowedAddresses == null)
                {
                    allowedAddresses = new List<int>();
                    foreach (var i in ConfigurationManager.AppSettings["AllowedIPs"].Split(','))
                    {
                        allowedAddresses.Add(i.GetHashCode());
                    }
                }
                return allowedAddresses;
            }
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
            //if (!AllowedAddresses.Contains(context.Request.UserHostAddress.GetHashCode()))
            //{ 
            //    context.Response.StatusCode = 401;
            //    context.Response.End();
            //}

        }
    }
}

