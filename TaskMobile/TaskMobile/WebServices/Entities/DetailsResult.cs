using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    public class DetailsResult
    {
        public DetailsResult()
        {
            DETAILS = new List<Tasks.Detail>();
        }
        public List<Tasks.Detail> DETAILS { get; set; }
    }
}
