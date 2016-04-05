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

namespace slackk.Services
{
    public class SlackClient
    {
        public SlackResponse Deliver(CrowMessage CrowMessage)
        {
            SlackMessage Message = new SlackMessage()
            {
                Token = CrowMessage.Token,
                Channel = CrowMessage.Channel,
                Text = CrowMessage.Text
            };
            WebRequest Request = WebRequest.Create(WebConfigurationManager.AppSettings["SlackAPI"]);
            string DataFormat = "token={0}&text={1}&channel={2}";
            string Output = string.Format(DataFormat, Message.Token, Message.Text, Message.Channel);
            byte[] Data = Encoding.Default.GetBytes(Output);
            Request.Method = "POST";
            Request.ContentType = "application/x-www-form-urlencoded";
            Request.ContentLength = Data.Length;

            Stream DataStream = Request.GetRequestStream();
            DataStream.Write(Data, 0, Data.Length);
            DataStream.Flush();
            DataStream.Close();

            WebResponse Response = Request.GetResponse();
            StreamReader StreamReader = new StreamReader(Response.GetResponseStream());
            string SlackServerResponse = StreamReader.ReadToEnd();
            return JsonConvert.DeserializeObject<SlackResponse>(SlackServerResponse);
 
        }
    }
}

