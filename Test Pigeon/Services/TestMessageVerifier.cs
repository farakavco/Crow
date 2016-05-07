using NUnit.Framework;
using System;
using slackk.Services;
using slackk.Models;
using System.Configuration;
using System.Web;

namespace Test_Pigeon
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void TestMessageVerifier()
        {
            // Testing Functionality When Json is Passed
            CrowMessage JsonMessage = new CrowMessage()
            {
                Channel = ConfigurationManager.AppSettings["channel"],
                Text = "Text",
            };
            HttpRequest Request = new HttpRequest("name", ;


        }
    }
}
