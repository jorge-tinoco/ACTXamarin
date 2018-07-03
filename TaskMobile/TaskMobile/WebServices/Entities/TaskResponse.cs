using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    ///  Used for make a web service response . 
    ///  The properties of this class are the <see cref="Common.Response{T}.MessageBody"/>.
    /// </summary>
    public class TaskResponse
    {
        public TaskResponse()
        {
            QueryTaskResult = new List<Entities.TaskResult>();
        }

        /// <summary>
        /// Tag that wraps the result.
        /// </summary>
        public List<TaskResult> QueryTaskResult { get; set; }
    }
}
