using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.WebServices.REST
{
    class Activities
    {
        /// <summary>
        /// TMAP route.
        /// </summary>
        private string _route;
        /// <summary>
        /// Data send in TMAP requests.
        /// </summary>
        private string _data;
        private readonly IClient _service; /// Back field

        public Activities(IClient service)
        {
            _service = service;
            
        }
        /// <summary>
        /// Get activities for a task.
        /// </summary>
        /// <param name="task">Task to query.</param>
        /// <param name="filterBy">Status to filter. Use null when no filter is needed.</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public void GetAll(int task,string filterBy , Action<ICollection<Models.Activity>> success, Action<object> error)
        {
            _route = "GetRequestTasksActivities";
            var request = new Request<ActivityRequest>
            {
                MessageBody =
                    {
                        TaskId = task
                    }
            };
            _data = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            _service.Post<Response<ActivityResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    var result = response.MessageBody.QueryTaskActivitiesResult.ACTIVITIES;
                    if (code.Equals(0) && result.Any())
                    {
                        ICollection<Models.Activity> activities;
                        if(filterBy != null)
                          activities = result.
                                            Where(x => x.STATUS == filterBy).
                                            Select(activityToConvert => ViewModels.Converters.Activity(activityToConvert)).
                                            OrderBy(x => x.Order).
                                            ToList();
                        else
                            activities = result.
                                            Select(activityToConvert => ViewModels.Converters.Activity(activityToConvert)).
                                            OrderBy(x => x.Order).
                                            ToList();
                        success(activities);
                    }
                    else
                    {
                        var exception = new Exceptions.WttException(Exceptions.Severity.Low);
                        if (response.MessageLog.LogItem != null)
                            exception.Message = response.MessageLog.LogItem.ErrorDescription;
                        else
                            exception.Message = "No se encontraron actividades";
                        error(exception);
                    }
                }, error);
        }

        /// <summary>
        /// Start/execute a task activity.
        /// </summary>
        /// <param name="task">Task that contains the activity. </param>
        /// <param name="activity">Activity to start.</param>
        /// <param name="user">User who has started the activity</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public void Start(double task , double activity, string user,  Action<bool> success, Action<object> error)
        {
            _route = "UpdateActivityStart";
            var request = new Request<ActivityUpdRequest>
            {
                MessageBody =
                    {
                        TaskId = task,
                        RejectionId = 0,
                        LastUpdateBy = user,
                        ActivityId = activity
                    }
            };
            _data = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            _service.Post<Response<ActivityUpdResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    string result = response.MessageBody.Result;
                    if (code.Equals(0) && result == "0")
                        success(true);
                    else
                    {
                        var exception = new Exceptions.WttException( Exceptions.Severity.High);
                        if (response.MessageLog.LogItem != null)
                            exception.Message = response.MessageLog.LogItem.ErrorDescription;
                        else
                            exception.Message = "No se logró iniciar la actividad";
                        error(exception);
                    }
                }, error);
        }

        /// <summary>
        /// Finish an specific activity.
        /// </summary>
        /// <param name="task">Task that contain the activity.</param>
        /// <param name="activity">Activity to finish.</param>
        /// <param name="user">User who has finished the activity.</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public void Finish(double task, double activity, string user, Action<bool> success, Action<object> error)
        {
            _route = "UpdateActivityEnd";
            var request = new Request<ActivityUpdRequest>
            {
                MessageBody =
                    {
                        TaskId = task,
                        RejectionId = 0,
                        LastUpdateBy = user,
                        ActivityId = activity
                    }
            };
            _data = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            _service.Post<Response<ActivityUpdResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    string result = response.MessageBody.Result;
                    if (code.Equals(0) && result == "0")
                        success(true);
                    else
                    {
                        var exception = new Exceptions.WttException(Exceptions.Severity.High);
                        if (response.MessageLog.LogItem != null)
                            exception.Message = response.MessageLog.LogItem.ErrorDescription;
                        else
                            exception.Message = "No se ha podido finalizar la actividad :" + activity;
                        error(exception);
                    }
                }, error);
        }

        /// <summary>
        /// Reject a task activity.
        /// </summary>
        /// <param name="task">Task that contains the activity.</param>
        /// <param name="activity">Activity to start.</param>
        /// <param name="rejection">Rejectino id.</param>
        /// <param name="user">Who has rejected the activity.</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public void Reject(double task, double activity, int rejection, string user, Action<bool> success, Action<object> error)
        {
             _route = "UpdateActivityRejected";
            var request = new Request<ActivityUpdRequest>
            {
                MessageBody =
                    {
                        TaskId = task,
                        RejectionId = rejection,
                        LastUpdateBy = user,
                        ActivityId = activity,
                        DestinyLocation = "sample string 4"
                    }
            };
            _data = Newtonsoft.Json.JsonConvert.SerializeObject(request);
            _service.Post<Response<ActivityUpdResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    string result = response.MessageBody.Result;
                    if (code.Equals(0) && result == "0")
                        success(true);
                    else
                    {
                        var exception = new Exceptions.WttException(Exceptions.Severity.High);
                        if (response.MessageLog.LogItem != null)
                            exception.Message = response.MessageLog.LogItem.ErrorDescription;
                        else
                            exception.Message = "No se logró rechazar la actividad: " + activity;
                        error(exception);
                    }
                }, error);
        }
    }
}
