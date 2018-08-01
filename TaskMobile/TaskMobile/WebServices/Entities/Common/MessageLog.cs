using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class MessageLog
    {

        public double ProcessingResultCode { set; get; }

        public double MaximumLogItemSeverityCode { set; get; }

        public LogItem LogItem { get; set; }

    }
}
