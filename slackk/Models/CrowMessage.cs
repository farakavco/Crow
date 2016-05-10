using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class CrowMessage
    {
        public string Channel { get; set; }
        public string Text { get; set; }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public string IP { get; set; }
        public string Time { get; set; }
        public string TelegramChannel { get; set; }

        public bool Verify()
        {
            return
                // Caring for the exception caused by multipart-data
                !(Text == ConfigurationManager.AppSettings["MultiPartException"]) &&
                // Checking to ensure if both file and filename are provided
                !(FileName == null ^ File == null) &&
                // Checking to ensure if message attributes are not null
                !(File == null && FileName == null && Channel == null && TelegramChannel == null && Text == null);
        }
    
    }
}