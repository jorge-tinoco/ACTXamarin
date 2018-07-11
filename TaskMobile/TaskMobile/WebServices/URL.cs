
namespace TaskMobile.WebServices
{
    /// <summary>
    /// Defines webservices url for task mobile.
    /// </summary>
    internal static  class  URL
    {
      
        /// <summary>
        /// Url for rest service that gets the availabe vehicles.
        /// </summary>
        internal static string GetVehicles
        {
            get {
                return @"http://myyardtl-mx.tenaris.net/TaskManagerServices/api/ILOQueryVehicle/GetVehicles";
            }
        }

        /// <summary>
        /// Url REST service for getting the ILO  tasks.
        /// </summary>
        internal static string GetTasks
        {
            get {
                return @"http://myyardtl-mx.tenaris.net/TaskManagerServices/api/QueryILOTasks/GetRequestTasks";
            }
        }

        /// <summary>
        /// Rest service for getting task details.
        /// </summary>
        internal static string RequestDetails
        {
            get
            {
                return "http://myyardtl-mx.tenaris.net/TaskManagerServices/api/QueryILOTasks/GetRequestTasksDetails";
            }
        }

        /// <summary>
        /// Url REST service for getting the ILO  tasks activities.
        /// </summary>
        internal static string GetActivities
        {
            get
            {
                return @"http://myyardtl-mx.tenaris.net/TaskManagerServices/api/QueryILOTasks/GetRequestTasksActivities";
            }
        }

        /// <summary>
        /// Rest web service to indicate that the task has started.
        /// </summary>
        internal static string ActivityStart
        {
            get
            {
                return "http://myyardtl-mx.tenaris.net/TaskManagerServices/api/ManageILOTasks/UpdateActivityStart";
            }
        }
        /// <summary>
        /// Rest web service to indicate that the task has been ended.
        /// </summary>
        internal static string ActivityEnd
        {
            get
            {
                return "http://myyardtl-mx.tenaris.net/TaskManagerServices/api/ManageILOTasks/UpdateActivityEnd";
            }
        }
        /// <summary>
        /// Rest web service to indicate that the task has been rejected.
        /// </summary>
        internal static string ActivityRejected
        {
            get
            {
                return "http://myyardtl-mx.tenaris.net/TaskManagerServices/api/ManageILOTasks/UpdateActivityRejected";
            }
        }
    }
}
