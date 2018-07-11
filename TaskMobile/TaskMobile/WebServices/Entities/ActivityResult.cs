using System.Collections.Generic;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    /// Wrapper entity that contains activities collection.
    /// </summary>
    public class ActivityResult
    {
        public ActivityResult()
        {
            ACTIVITIES = new List<Tasks.Activity>();
        }
        public List<Tasks.Activity> ACTIVITIES { get; set; }
    }
}
