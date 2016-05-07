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

namespace Test_Pigeon.Controllers
{
    [TestFixture]
    class TestMessageController
    {
        MessageController MessageController = new MessageController();
        [Test]
        public void TestUpload()
        {
            var Authentication = new
            {
                Token = ConfigurationManager.AppSettings["SlackToken"]
            };
            RestClient Client = new RestClient(ConfigurationManager.AppSettings["CrowServer"]);
            string Token = JwtHelper.Encode(Authentication, ConfigurationManager.AppSettings["SecretKey"], JwtHashAlgorithm.HS256);
            RestRequest Req = new RestRequest(ConfigurationManager.AppSettings["CrowApiAddress"], Method.POST);
            var TestFile = System.IO.File.ReadAllBytes(Environment.CurrentDirectory + ConfigurationManager.AppSettings["TestFile"]);

            // Testing Upload With Valid Json Request
            Req.AddHeader("X-JWT-Token", Token);
            Req.AddParameter("Channel", ConfigurationManager.AppSettings["Channel"]);
            Req.AddParameter("Text", "Testing Upload With Valid Json Request");
            IRestResponse ResponseForValidJson = Client.Execute(Req);
            var JsonResponseForValidJson = JsonConvert.DeserializeObject<SlackResponse>(ResponseForValidJson.Content);
            CrowResponse ExpectedResponseForJson = new CrowResponse()
            {
                OK = true,
                Error = null
            };
            CrowResponse ActualResponseForJson = new CrowResponse()
            {
                OK = JsonResponseForValidJson.OK,
                Error = JsonResponseForValidJson.Error
            };
            Assert.AreEqual(ExpectedResponseForJson, ActualResponseForJson);

            // Testing Upload With Invalid Json Request (Invalid Channel)
            RestRequest InvalidReq = new RestRequest(ConfigurationManager.AppSettings["CrowApiAddress"], Method.POST);
            InvalidReq.AddHeader("X-JWT-Token", Token);
            InvalidReq.AddParameter("Channel", ConfigurationManager.AppSettings["Channel"]);
            InvalidReq.AddParameter("Text", "Testing Upload With Valid Json Request");
            IRestResponse ReponsesForInvalidReq = Client.Execute(InvalidReq);
            var JsonReponsesForInvalidReq = JsonConvert.DeserializeObject<SlackResponse>(ReponsesForInvalidReq.Content);
            CrowResponse ExpectedResponseForInvalidJson = new CrowResponse()
            {
                OK = false,
                Error = "channel_not_found"
            };
            CrowResponse ActualResponseForInvalidJson = new CrowResponse()
            {
                OK = JsonReponsesForInvalidReq.OK,
                Error = JsonReponsesForInvalidReq.Error
            };
            Assert.AreEqual(ExpectedResponseForInvalidJson, ActualResponseForInvalidJson);
            // Testing Upload With Valid MultiPart-FormData

            RestRequest ValidMultiReq = new RestRequest(ConfigurationManager.AppSettings["CrowApiAddress"], Method.POST);
            ValidMultiReq.AddHeader("X-JWT-Token", Token);
            ValidMultiReq.AddParameter("Channels", ConfigurationManager.AppSettings["Channel"]);
            ValidMultiReq.AddParameter("filename", "Testing Upload With Valid MultiPart-FormData");
            ValidMultiReq.AddFile("file", TestFile, "Test");
            IRestResponse ReponsesForValidMultiReq = Client.Execute(ValidMultiReq);
            var JsonResponseForValidMultioPart = JsonConvert.DeserializeObject<SlackResponse>(ReponsesForInvalidReq.Content);
            CrowResponse ExpectedResponseForValidMulti = new CrowResponse()
            {
                OK = true,
                Error = null
            };
            CrowResponse ActualResponseForValidMulti = new CrowResponse()
            {
                OK = JsonResponseForValidMultioPart.OK,
                Error = JsonResponseForValidMultioPart.Error
            };
            Assert.AreEqual(ExpectedResponseForValidMulti, ActualResponseForValidMulti);
            // Testing Upload With Invalid MultiPart-FromData (no file is attached)

            RestRequest InvalidMultiReq = new RestRequest(ConfigurationManager.AppSettings["CrowApiAddress"], Method.POST);
            ValidMultiReq.AddHeader("X-JWT-Token", Token);
            ValidMultiReq.AddParameter("Channels", ConfigurationManager.AppSettings["Channel"]);
            ValidMultiReq.AddParameter("filename", "Testing Upload With Valid MultiPart-FormData");
            IRestResponse ResponseForInvalidMultiReq = Client.Execute(InvalidMultiReq);
            var JsonResponseForInvalidMultiReq = JsonConvert.DeserializeObject<SlackResponse>(ResponseForInvalidMultiReq.Content);
            CrowResponse ExpectedResponseForInvalidMulti = new CrowResponse()
            {
                OK = false,
                Error = HttpStatusCode.BadRequest.ToString()
            };

        }
    }
}
