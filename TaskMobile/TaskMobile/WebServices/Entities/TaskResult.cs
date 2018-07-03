using System.Collections.Generic;


namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    /// This represent the task for every single task returned in the web service. 
    /// </summary>
    public class TaskResult
    {
        public TaskResult()
        {
            TASK = new List<Tasks.Task>();
        }
        public int ID { get; set; }
        public string LEGACY_REQUEST_ID { get; set; }
        public string SOURCE_SYSTEM { get; set; }
        public string STATUS { get; set; }
        public List<Tasks.Task> TASK { get; set; }
    }
}
