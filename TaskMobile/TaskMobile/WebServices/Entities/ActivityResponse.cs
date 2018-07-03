using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    /// Activity body response.
    /// </summary>
    /// <remarks>
    /// Used for make a web service response . 
    ///     The properties of this class are the <see cref="Common.Response{T}.MessageBody"/>.
    /// </remarks>
    public class ActivityResponse
    {
        /// <summary>
        /// Result for the last action executed on the task.
        /// </summary>
        public string Result { get; set; }
    }
}
