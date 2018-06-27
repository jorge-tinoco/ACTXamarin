using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class SenderParty
    {
        public string ApplicationId { get; set; }
        public string ProgramName { get; set; }
        public string TransactionCode { get; set; }
        public int IntegrationTypeCode { get; set; }
        public string UserId { get; set; }
    }
}
