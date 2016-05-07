using NUnit.Framework;
using System;
using slackk.Services;
using slackk.Models;
using System.Configuration;
using System.Web;
using Varzesh3.Leo.Utility;
using System.Net;
using RestSharp;

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
            HttpRequest Request = new HttpRequest("name", "url", "querstring");
            string Token = JwtHelper.Encode(Authentication, ConfigurationManager.AppSettings["SecretKey"], JwtHashAlgorithm.HS256);
            Request.Headers["X-JWT-Token"] = Token;


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
            VerifyResponse ActualResponse = MessageVerifier.Verify(JsonMessage, Request);
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
            VerifyResponse ActualResponseForEmpty = MessageVerifier.Verify(EmptyMessage, Request);
            Assert.AreEqual(ExpectedResponseForEmpty, ActualResponseForEmpty);
            // Testing Functionality When Header Doesn't Include Token
            Request.Headers.Remove("X-JWT-Token");
            VerifyResponse ExpectedResponseForNullHeader = new VerifyResponse()
            {
                OK = false,
                Error = HttpStatusCode.Unauthorized
            };
            VerifyResponse ActualResponseForNullHeader = MessageVerifier.Verify(JsonMessage, Request);
            Assert.AreEqual(ExpectedResponseForNullHeader, ActualResponseForNullHeader);

            // Testing Functionality When Token Is Not Valid
            Request.Headers["X-JWT-Token"] = "Invalid";
            VerifyResponse ExpectedResponseForInvalidHeader = new VerifyResponse()
            {
                OK = false,
                Error = HttpStatusCode.Unauthorized
            };
            VerifyResponse ActualResponseForInvalidHeader = MessageVerifier.Verify(JsonMessage, Request);
            Assert.AreEqual(ExpectedResponseForInvalidHeader, ActualResponseForInvalidHeader);
            

            // Testing the IP
            // Pending
        }
    }
}
