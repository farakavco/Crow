using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace slackk.Models
{
    public class SlackResponse : CrowResponse
    {
        public string Channel { get; set; }
        public Message Message { get; set; }

    }
}