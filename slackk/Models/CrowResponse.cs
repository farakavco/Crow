using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class CrowResponse : IResponse
    {
        public bool OK { get; set; }
        public string Error { get; set; }

        public static implicit operator CrowResponse(CrowMessage v)
        {
            throw new NotImplementedException();
        }
    }
}