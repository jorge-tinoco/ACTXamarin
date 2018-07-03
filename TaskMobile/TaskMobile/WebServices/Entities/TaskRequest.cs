using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    ///  Used for make a web service request . 
    ///  The properties of this class are the <see cref="Common.Request{T}.MessageBody"/>.
    /// </summary>
    public class TaskRequest
    {
        /// <summary>
        /// Vehicle identifier to query.
        /// </summary>
        public int VehicleId { get; set; }

        /// <summary>
        /// Task status to query.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Frame time to query. (Start date)
        /// </summary>
        public DateTime InitialDate { get; set; }

        /// <summary>
        /// Frame time to query. (End date)
        /// </summary>
        public DateTime FinalDate { get; set; }
    }
}
