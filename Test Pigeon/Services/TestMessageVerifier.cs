using NUnit.Framework;
using System;
using slackk.Services;
using slackk.Models;
using System.Configuration;
using System.Web;
using Varzesh3.Leo.Utility;
using System.Net;
using RestSharp;
using System.Collections.Specialized;

namespace Test_Pigeon
{
    [TestFixture]
    public class Test
    {
        MessageVerifier MessageVerifier = new MessageVerifier();

        [Test]
        public void TestMessageVerifier()
        {
            // Setup of proper data
            var Authentication = new
            {
                Token = ConfigurationManager.AppSettings["SlackToken"]
            };
            NameValueCollection Headers = new NameValueCollection();
            string Token = JwtHelper.Encode(Authentication, ConfigurationManager.AppSettings["SecretKey"], JwtHashAlgorithm.HS256);
            Headers.Add("X-JWT-Token", Token);


            // Testing Functionality When Json is Passed
            CrowMessage JsonMessage = new CrowMessage()
            {
                Channel = ConfigurationManager.AppSettings["channel"],
                Text = "Text",
            };

            VerifyResponse ExpectedResponseForJson = new VerifyResponse()
            {
                OK = true,
                Error = HttpStatusCode.OK
            };
            VerifyResponse ActualResponse = MessageVerifier.Verify(JsonMessage, Headers);
            Assert.AreEqual(ExpectedResponseForJson, ActualResponse);

            // Testing Functionality When Message Is Null;
            CrowMessage EmptyMessage = new CrowMessage()
            {

            };
            VerifyResponse ExpectedResponseForEmpty = new VerifyResponse()
            {
                OK = false,
                Error = HttpStatusCode.BadRequest
            };
            VerifyResponse ActualResponseForEmpty = MessageVerifier.Verify(EmptyMessage, Headers);
            Assert.AreEqual(ExpectedResponseForEmpty, ActualResponseForEmpty);
            // Testing Functionality When Header Doesn't Include Token
            Headers.Remove("X-JWT-Token");
            VerifyResponse ExpectedResponseForNullHeader = new VerifyResponse()
            {
                OK = false,
                Error = HttpStatusCode.Unauthorized
            };
            VerifyResponse ActualResponseForNullHeader = MessageVerifier.Verify(JsonMessage, Headers);
            Assert.AreEqual(ExpectedResponseForNullHeader, ActualResponseForNullHeader);

            // Testing Functionality When Token Is Not Valid
            Headers["X-JWT-Token"] = "Invalid";
            VerifyResponse ExpectedResponseForInvalidHeader = new VerifyResponse()
            {
                OK = false,
                Error = HttpStatusCode.Unauthorized
            };
            VerifyResponse ActualResponseForInvalidHeader = MessageVerifier.Verify(JsonMessage, Headers);
            Assert.AreEqual(ExpectedResponseForInvalidHeader, ActualResponseForInvalidHeader);


            // Testing the IP
            // Pending
        }
    }
}
