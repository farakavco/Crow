using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class SlackResponse
    {
        public bool OK { get; set; }
        public string Channel { get; set; }
        public string Error { get; set; }
        public Message Message { get; set; }

    }
}