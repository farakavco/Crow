using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class SlackMessage
    {
        public string Token { get; set; }
        public string Channel { get; set; }
        public string Text { get; set; }
    }
}