using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class Message
    {
        public string Text { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
        public string Subtype { get; set;}
        public string Ts { get; set; }

    }
}