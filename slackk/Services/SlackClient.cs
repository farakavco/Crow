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
using System.Configuration;

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
                    Channel = CrowMessage.Channel,
                    IP = CrowMessage.IP,
                    Time = CrowMessage.Time
                };
                var Request = new RestRequest("api/chat.postMessage", Method.POST);
                Request.AddParameter("channel", Message.Channel);
                Request.AddParameter("token", ConfigurationManager.AppSettings["SlackToken"]);
                Request.AddParameter("text", TextMaker(Message.Text, Message.IP, Message.Time));
                IRestResponse Response = client.Execute(Request);
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);
            }
            else
            {
                CrowMessage Message = new CrowMessage()
                {
                    Channel = CrowMessage.Channel,
                    File = CrowMessage.File,
                    FileName = CrowMessage.FileName,
                    IP = CrowMessage.IP,
                    Time = CrowMessage.Time
                };
                var Request = new RestRequest("api/files.upload", Method.POST);
                Request.AddHeader("content-type", "multipart/form-data");
                Request.AddParameter("channels", Message.Channel);
                Request.AddParameter("token", ConfigurationManager.AppSettings["SlackToken"]);
                Request.AddFile("file", Message.File, "image");
                Request.AddParameter("filename", Message.FileName);
                Request.AddParameter("initial_comment", TextMaker(Message.FileName, Message.IP, Message.Time));
                IRestResponse Response = client.Execute(Request);
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);
            }
        }
        public string TextMaker(string Text, string IP, DateTime DateTime)
        {
            return string.Format("{0} From {1} On {2}", Text, IP, DateTime);
        }
    }
}


