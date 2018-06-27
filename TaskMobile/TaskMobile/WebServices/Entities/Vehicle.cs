using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    /// Used for make a request for web service. The properties of this class are the <see cref="Common.Request{T}.MessageBody"/>
    /// </summary>
    class Vehicle
    {
        /// <summary>
        /// Unix system to query.
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// Requester user.
        /// </summary>
        public string User { get; set; }
    }
}
