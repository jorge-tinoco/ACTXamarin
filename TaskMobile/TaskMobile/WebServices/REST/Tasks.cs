using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.WebServices.REST
{
    public class Tasks
    {
        /// <summary>
        /// TMAP route.
        /// </summary>
        private string _route;
        /// <summary>
        /// Data send in TMAP requests.
        /// </summary>
        private string _data;
        /// <summary>
        /// Backfield for web service client.
        /// </summary>
        private readonly IClient _service;

        public Tasks(IClient webService)
        {
            _service = webService;
        }

       /// <summary>
       /// Get all tasks for specific vehicle.
       /// </summary>
       /// <param name="vehicle">Vehicle that contains the tasks.</param>
       /// <param name="status">Task status to query.</param>
       /// <param name="success">Callback.</param>
       /// <param name="error">Callback.</param>
        public void All(int vehicle, string status, Action<IEnumerable<Models.Task>> success, Action<object> error)
        {
            _route = "GetRequestTasks";
            var tasksResult = new List<Models.Task>();
            var request = new Request<TaskRequest>
            {
                MessageBody =
                {
                    VehicleId = vehicle,
                    Status = status,
                    InitialDate = new DateTime(2016, 01, 01),
                    FinalDate = DateTime.Now
                }
            };
            _data = JsonConvert.SerializeObject(request);
            _service.Post<Response<TaskResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    List<TaskResult> tasks = response.MessageBody.QueryTaskResult;
                    if (code.Equals(0) && tasks.Any())
                    {
                        foreach (TaskResult result in tasks)
                        {
                            IEnumerable<Models.Task> tasksConverted = result.TASK
                                .Select(taskToConvert => ViewModels.Converters.Task(taskToConvert));
                            foreach (var taskToAdd in tasksConverted)
                            {
                                tasksResult.Add(taskToAdd);
                            }
                        }
                        success(tasksResult);
                    }
                    else
                    {
                        error(new Exceptions.WttException("No se encontró tareas asociadas al vehículo"));
                    }
                }, errorcito =>
                {
                    error(errorcito);
                });
        }

        /// <summary>
        /// Tasks by date range.
        /// </summary>
        /// <param name="vehicle">Vehicle identifier.</param>
        /// <param name="status">Tasks status filter.</param>
        /// <param name="start">Initial date.</param>
        /// <param name="end">Final date.</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public void ByRange(int vehicle, string status, DateTime start, DateTime end, Action<IEnumerable<Models.Task>> success, Action<object> error)
        {
            _route = "GetRequestTasks";
            var tasksResult = new List<Models.Task>();
            var request = new Request<TaskRequest>
            {
                MessageBody =
                {
                    VehicleId = vehicle,
                    Status = status,
                    InitialDate = start,
                    FinalDate = end
                }
            };
            _data = JsonConvert.SerializeObject(request);
            _service.Post<Response<TaskResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    List<TaskResult> tasks = response.MessageBody.QueryTaskResult;
                    if (code.Equals(0) && tasks.Any())
                    {
                        foreach (TaskResult result in tasks)
                        {
                            IEnumerable<Models.Task> tasksConverted = result.TASK
                                .Select(taskToConvert => ViewModels.Converters.Task(taskToConvert));
                            foreach (var taskToAdd in tasksConverted)
                            {
                                tasksResult.Add(taskToAdd);
                            }
                        }
                        success(tasksResult);
                    }
                    else
                    {
                        error(new Exceptions.WttException("No se encontró tareas asociadas al vehículo"));
                    }
                }, errorcito =>
                {
                    error(errorcito);
                });
        }

        /// <summary>
        /// Get details for an specific task.
        /// </summary>
        /// <param name="task">Task to query.</param>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        public virtual void Details(Models.Task task, Action<ICollection<Models.TaskDetail>> success, Action<object> error)
        {
            _route = "GetRequestTasksDetails";
            var details = new List<Models.TaskDetail>();
            var request = new Request<DetailsRequest>
            {
                MessageBody =
                {
                   TaskId = task.Number
                }
            };
            _data = JsonConvert.SerializeObject(request);
            _service.Post<Response<DetailsResponse>>(_route, _data,
                response =>
                {
                    double code = response.MessageLog.ProcessingResultCode;
                    List<Entities.Tasks.Detail> returned = response.MessageBody.QueryTaskDetailsResult.DETAILS;
                    if (code.Equals(0) && returned.Any())
                    {
                        var detailHeaders = new Models.TaskDetail
                        {
                            WorkOrder = "OP",
                            Lot = "Lote",
                            SapCode = "Código SAP",
                            PiecesText = "Piezas",
                            TonsText = "Toneladas",
                            RowIsHeader = true
                        };
                        details.Add(detailHeaders);
                        IEnumerable<Models.TaskDetail> localDetails = returned.
                                                                        Select(detailToConvert => ViewModels.Converters.TaskDetail(detailToConvert, task.Number, task.Type)).
                                                                        ToList();
                        details.AddRange(localDetails);
                    }
                    success(details);
                }, error);
        }
    }
}
