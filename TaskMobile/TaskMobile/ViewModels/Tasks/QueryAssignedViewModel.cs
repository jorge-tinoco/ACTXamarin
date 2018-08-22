using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Thread = System.Threading.Tasks;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices.REST;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.QueryAssigned"/> view.
    /// </summary>
    public class QueryAssignedViewModel : BaseViewModel, INavigatingAware
    {
        private bool _isRefreshing = true;
        private DelegateCommand<object> _toActivity;
        private ObservableCollection<Models.Task> _AssignedTasks;

        public QueryAssignedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Driver = "Jorge Tinoco";
            AssignedTasks = new ObservableCollection<Models.Task>();
        }

        #region COMMANDS

        public DelegateCommand<object> ToActivityCommand =>
            _toActivity ?? (_toActivity = new DelegateCommand<object>(GoToActivities));

        public DelegateCommand<Models.Task> DetailsCommand
        {
            get
            {
                return new DelegateCommand<Models.Task>(async (task) =>
                {
                    await ShowDetails(task);
                });
            }
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsRefreshing = true;

                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public ObservableCollection<Models.Task> AssignedTasks
        {
            get { return _AssignedTasks; }
            set
            {
                SetProperty(ref _AssignedTasks, value);
            }
        }

        /// <summary>
        /// Indicates if the listview is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }


        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                await CheckVehicle();
                if (CurrentVehicle != null)
                    await RefreshData();
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas asignadas", "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.AssignedToExecuted"/> view that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("SelectedTask", selectedTask);
            await _navigationService.NavigateAsync("AssignedToExecuted", Parameters);
        }

        /// <summary>
        /// Navigate to <see cref="AssignedToExecuted"/> view.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToActivities(object tappedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskToRun", tappedTask);
            await _navigationService.NavigateAsync("AssignedToExecuted", Parameters);
        }

        /// <summary>
        /// Query REST web services to get task details.
        /// </summary>
        /// <param name="tappedTask">Selected task by the user.</param>
        private async Thread.Task ShowDetails(Models.Task tappedTask)
        {
            try
            {
                IsRefreshing = true;
                int TaskToExpand = tappedTask.Number;
                string StockType = tappedTask.Type;
                if (!tappedTask.Expanded)
                {
                    tappedTask.Clear();
                    Client RESTClient = new Client(WebServices.URL.RequestDetails);
                    Request<WebServices.Entities.DetailsRequest> Requests = new Request<WebServices.Entities.DetailsRequest>();
                    Requests.MessageBody.TaskId = TaskToExpand;
                    var Response = await RESTClient.Post<Response<WebServices.Entities.DetailsResponse>>(Requests);
                    if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.QueryTaskDetailsResult != null)
                    {
                        Models.TaskDetail DetailHeaders = new Models.TaskDetail();
                        DetailHeaders.WorkOrder = "OP";
                        DetailHeaders.Lot = "Lote";
                        DetailHeaders.SapCode = "Código SAP";
                        DetailHeaders.PiecesText = "Piezas";
                        DetailHeaders.TonsText = "Toneladas";
                        DetailHeaders.RowIsHeader = true; 
                        tappedTask.Add(DetailHeaders);
                        IEnumerable<Models.TaskDetail> LocalDetails = Response.MessageBody.QueryTaskDetailsResult.DETAILS.
                                                                        Select(detailToConvert => Converters.TaskDetail(detailToConvert, TaskToExpand, StockType));
                        tappedTask.Add(LocalDetails);
                    }
                    else
                    {
                        await _dialogService.DisplayAlertAsync("Información", "No se encontró detalles", "Entiendo");
                    }
                }
                tappedTask.Expanded = !tappedTask.Expanded;
                IsRefreshing = false;
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al mostrar detalles de la tarea " + tappedTask.Number, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo ocurrió cuando mostrábamos los detalles", "Entiendo");
            }
        }

        /// <summary>
        /// Refresh the assigned task list view.
        /// </summary>
        /// <returns></returns>
        private async Thread.Task RefreshData()
        {
            Client RESTClient = new Client(WebServices.URL.GetTasks);
            Request<WebServices.Entities.TaskRequest> Requests = new Request<WebServices.Entities.TaskRequest>();
            Requests.MessageBody.VehicleId = int.Parse( CurrentVehicle.Identifier); // TO do: change for VehicleId
            Requests.MessageBody.Status = "A";
            Requests.MessageBody.InitialDate = new DateTime(2016, 01, 01);
            Requests.MessageBody.FinalDate = DateTime.Now;
            var Response = await RESTClient.Post<Response<WebServices.Entities.TaskResponse>>(Requests);
            AssignedTasks.Clear();
            if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.QueryTaskResult.Count() > 0)
            {
                var AllTasks = Response.MessageBody.QueryTaskResult.SelectMany(x => x.TASK);
                var Ordered = AllTasks.OrderByDescending(x => x.CREATED_DATE);
                var TasksConverted = Ordered.Select(taskToConvert => Converters.Task(taskToConvert));
                foreach (var item in TasksConverted)
                {
                    AssignedTasks.Add(item);
                }
            }
            else
                await _dialogService.DisplayAlertAsync("Información", "No se encontró tareas asociadas al vehículo ", "Entiendo");
        }

    }
}
