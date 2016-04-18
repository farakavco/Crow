﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace slackk.Models
{
    public class CrowMessage
    {
        public string Token { get; set; }
        public string Channel { get; set; }
        public string Text { get; set; }
        public string File { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string InitialComment { get; set; }

    }
}