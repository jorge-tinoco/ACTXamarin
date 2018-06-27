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

        public List<LogItem> LogItems { get; set; }

        public MessageLog()
        {
            ProcessingResultCode = 0;
            MaximumLogItemSeverityCode = 0;
            LogItems = new List<LogItem>();
        }

    }
}
