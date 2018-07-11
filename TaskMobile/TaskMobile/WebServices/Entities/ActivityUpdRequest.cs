using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    ///  This class represents the <see cref="Common.Request{T}.MessageBody"/>.
    /// </summary>
    public class ActivityUpdRequest
    {
        /// <summary>
        /// Task identifier.
        /// </summary>
        public double TaskId { get; set; }

        /// <summary>
        /// Activity identifier.
        /// </summary>
        public double ActivityId { get; set; }

        /// <summary>
        /// Last person who updates the task.
        /// </summary>
        public string LastUpdateBy { get; set; }

        /// <summary>
        /// Destination location.
        /// </summary>
        public string DestinyLocation { get; set; }

        /// <summary>
        /// Rejection cause.
        /// </summary>
        public int RejectionId { get; set; }
    }
}
