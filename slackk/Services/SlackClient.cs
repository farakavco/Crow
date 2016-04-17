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
            if (CrowMessage.Content == null)
            {
                SlackMessage Message = new SlackMessage()
                {
                    Text = CrowMessage.Text,
                    Token = CrowMessage.Token,
                    Channel = CrowMessage.Channel
                }

                var Request = new RestRequest("api/chat.postMessage", Method.POST);
                Request.AddParameter("channel", Message.Channel);
                Request.AddParameter("token", Message.Token);
                Request.AddParameter("text", Message.Text);
                IRestResponse Response = client.Execute(Request);
                return JsonConvert.DeserializeObject<SlackResponse>(Response.Content);
            }
            else
            {
                SlackFile File = new SlackFile()
                {
                    Channel = CrowMessage.Channel,
                    Content = System.Text.Encoding.ASCII.GetBytes(CrowMessage.Content),
                    FileName = CrowMessage.FileName,
                    Token = CrowMessage.Token,
                };
                HttpWebRequest Request = (HttpWebRequest)WebRequest.Create(WebConfigurationManager.AppSettings["SlackAPI"]);
                Request.ContentType = "multipart/form-data";
                Request.Headers.Add("token", File.Token);
                Request.Headers.Add("")
                Request.Method = "POST";
                Stream RequestStream = Request.GetRequestStream();
                RequestStream.Write(File.Content, 0, File.Content.Length);
                RequestStream.Close();





            }

        }
    }
}

