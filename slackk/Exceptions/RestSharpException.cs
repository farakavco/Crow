using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using slackk.Services;

namespace slackk.Exceptions
{
    public class RestSharpException : Exception
    {
        public RestSharpException(string Text, string Channel, string IP, string Exception, string TelegramChannel)
    : base()
        {
            TelegramBot.DeliverMessage(Text, Channel, IP, Exception, TelegramChannel);
        }
    }
}