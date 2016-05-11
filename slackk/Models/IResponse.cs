using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace slackk.Models
{
    public interface IResponse
    {
        bool OK { get; set; }
        string Error { get; set; }
    }
}
