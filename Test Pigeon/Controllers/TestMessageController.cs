using NUnit.Framework;
using slackk.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using slackk.Controllers;
using System.Web;
using Varzesh3.Leo.Utility;
using RestSharp;
using Newtonsoft.Json;
using System.Net;
using System.Reflection;
using System.IO;

namespace Test_Pigeon.Controllers
{
    [TestFixture]
    class TestMessageController
    {
        MessageController messageController = new MessageController();
        [Test]
        public void TestUpload()
        {

            var TestFile = System.IO.File.ReadAllBytes(Path.Combine(Environment.CurrentDirectory, ConfigurationManager.AppSettings["TestFile"]));

            // Testing Upload With Valid Json Request
            CrowMessage jsonRequest = new CrowMessage()
            {
                Channel = ConfigurationManager.AppSettings["Channel"],
                Text = "Test"
            };
            var JsonResponseForValidJson = messageController.Upload(jsonRequest);
            CrowResponse ExpectedResponseForJson = new CrowResponse()
            {
                OK = true,
                Error = null
            };
            Assert.AreEqual(ExpectedResponseForJson.Error, JsonResponseForValidJson.Error);
            // Testing Upload With Invalid Json Request (Invalid Channel)
            CrowMessage invalidJsonRequest = new CrowMessage()
            {
                Text = "Test",
                Channel = " "
            };
            var JsonReponsesForInvalidReq = messageController.Upload(invalidJsonRequest);
            CrowResponse ExpectedResponseForInvalidJson = new CrowResponse()
            {
                OK = false,
                Error = "channel_not_found"
            };

            Assert.AreEqual(ExpectedResponseForInvalidJson.Error, JsonReponsesForInvalidReq.Error);
            // Testing Upload With Valid MultiPart-FormData
            CrowMessage validMulti = new CrowMessage()
            {
                File = TestFile,
                Channel = ConfigurationManager.AppSettings["Channel"],
                FileName = "name",

            };

            var JsonResponseForValidMultioPart = messageController.Upload(validMulti);
            CrowResponse ExpectedResponseForValidMulti = new CrowResponse()
            {
                OK = true,
                Error = null
            };
            Assert.AreEqual(ExpectedResponseForValidMulti.Error, JsonResponseForValidMultioPart.Error);

            // Testing Upload With Invalid MultiPart-FromData (no file is attached)

            CrowMessage invalidMulti = new CrowMessage()
            {
                FileName = "name",
                Channel = ConfigurationManager.AppSettings["Channel"]
            };
            var JsonResponseForInvalidMultiReq = messageController.Upload(invalidMulti);
            CrowResponse ExpectedResponseForInvalidMulti = new CrowResponse()
            {
                OK = false,
                Error = "Bad Request"
            };

            Assert.AreEqual(ExpectedResponseForInvalidMulti.Error, JsonResponseForInvalidMultiReq.Error);

        }
    }
}
