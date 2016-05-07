using System;
using slackk.Models;
using Newtonsoft.Json;
using RestSharp;
using slackk.Exceptions;
using System.Configuration;

namespace slackk.Services
{
    public class SlackClient
    {
        public SlackResponse Deliver(CrowMessage CrowMessage)
        {
            IRestResponse Response = null;
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
                Response = client.Execute(Request);
                if (Response.ErrorMessage != null)
                {
                    throw new RestSharpException(Message.Text, Message.Channel, Message.IP, Response.ErrorMessage);
                }
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
                Request.AddFile("file", Message.File, "image");
                Request.AddParameter("token", ConfigurationManager.AppSettings["SlackToken"]);
                Request.AddParameter("filename", Message.FileName);
                Request.AddParameter("initial_comment", TextMaker(Message.FileName, Message.IP, Message.Time));
                Response = client.Execute(Request);
                if (Response.ErrorMessage != null)
                {
                    throw new RestSharpException(Message.FileName, Message.Channel, Message.IP, Response.ErrorMessage);
                }
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);
            }
        }
        public string TextMaker(string Text, string IP, DateTime DateTime)
        {
            return string.Format("{0} From {1} On {2}", Text, IP, DateTime);
        }
    }
}


