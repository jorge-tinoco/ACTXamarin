using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class MessageHeader
    {
        public MessageHeader()
        {
            MessageId = string.Empty;
            ReferenceMessageId = string.Empty;
            SenderParty = new SenderParty();
        }
        public string MessageId { get; set; }
        public string ReferenceMessageId { get; set; }
        public string FacilityCode { get; set; }
        public string ServiceName { get; set; }
        public string OperationName { get; set; }
        public DateTime CreationDateTime { get; set; }
        public SenderParty SenderParty { get; set; }
    }
}
