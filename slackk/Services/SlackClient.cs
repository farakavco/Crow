using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using slackk.Models;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;
using System.Text;
using System.IO;
using RestSharp;

namespace slackk.Services
{
    public class SlackClient
    {
        public SlackResponse Deliver(CrowMessage CrowMessage)
        {

            var client = new RestClient("https://slack.com");
            if (CrowMessage.File == null)
            {
                CrowMessage Message = new CrowMessage()
                {
                    Text = CrowMessage.Text,
                    Token = CrowMessage.Token,
                    Channel = CrowMessage.Channel
                };
                var Request = new RestRequest("api/chat.postMessage", Method.POST);
                Request.AddParameter("channel", Message.Channel);
                Request.AddParameter("token", Message.Token);
                Request.AddParameter("text", Message.Text);
                IRestResponse Response = client.Execute(Request);
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);
            }
            else
            {
                CrowMessage Message = new CrowMessage()
                {
                    Text = CrowMessage.Text,
                    Token = CrowMessage.Token,
                    Channel = CrowMessage.Channel,
                    File = CrowMessage.File,
                    FileName = CrowMessage.FileName
                };

                var Request = new RestRequest("api/files.upload", Method.POST);
                Request.AddHeader("content-type", "multipart/form-data");
                Request.AddParameter("channels", Message.Channel);
                Request.AddParameter("token", Message.Token);
                Request.AddFile("file", Message.File, "image");
                Request.AddParameter("filename", "image");
                Request.AddParameter("filetype", "jpeg");
                IRestResponse Response = client.Execute(Request);
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);

            }

        }
    }
}


