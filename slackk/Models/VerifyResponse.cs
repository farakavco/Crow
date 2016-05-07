using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace slackk.Models
{
    public class VerifyResponse
    {
        public bool OK { get; set; }
        public HttpStatusCode Error { get; set; }
    }
}