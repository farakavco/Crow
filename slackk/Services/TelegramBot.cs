using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Telegram.Bot;
namespace slackk.Services
{
    public class TelegramBot
    {
        public static void DeliverMessage(string Text, string Channel, string IP, string Exception, string TelegramChannel)
        {
            string TelegramID = ConfigurationManager.AppSettings["TargetTelegramChannel"];
            if (TelegramChannel != null) TelegramID = TelegramChannel;
            Api TelegramBot = new Api(ConfigurationManager.AppSettings["TelegramCrowToken"]);
            string TelegramReportMessage = string.Format("{0} tried to send {1} to {2} but failed due to {3}", IP, Text, Channel, Exception.ToString());
            TelegramBot.SendTextMessage(TelegramID, TelegramReportMessage);
        }
    }
}