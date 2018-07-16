using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices.REST;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    public class QueryExecutedViewModel : BaseViewModel, INavigatingAware
    {
        private bool _isRefreshing = true;
        private DelegateCommand<object> _toActivity;

        public QueryExecutedViewModel(INavigationService navigationService, IPageDialogService dialogService) 
            : base(navigationService, dialogService)
        {
            Driver = "Jorge Tinoco";
            ExecutedTasks = new ObservableCollection<Models.Task>();
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

        #region VIEW MODEL PROPERTIES

        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public ObservableCollection<Models.Task> ExecutedTasks { get; private set; }

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
        #endregion

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
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas ejecutadas", "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Navigate to <see cref="ExecutedToFinish"/> view.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToActivities(object tappedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskToFinish", tappedTask);
            await _navigationService.NavigateAsync("ExecutedToFinish", Parameters);
        }

        /// <summary>
        /// Query REST web services to get task details.
        /// </summary>
        /// <param name="tappedTask">Selected task by the user.</param>
        private async Task ShowDetails(Models.Task tappedTask)
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
        private async Task RefreshData()
        {
            Client RESTClient = new Client(WebServices.URL.GetTasks);
            Request<WebServices.Entities.TaskRequest> Requests = new Request<WebServices.Entities.TaskRequest>();
            Requests.MessageBody.VehicleId = 369; // TO do: change for VehicleId
            Requests.MessageBody.Status = "E";
            Requests.MessageBody.InitialDate = new DateTime(2016, 01, 01);
            Requests.MessageBody.FinalDate = DateTime.Now;
            var Response = await RESTClient.Post<Response<WebServices.Entities.TaskResponse>>(Requests);
            ExecutedTasks.Clear();
            if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.QueryTaskResult.Count() > 0)
            {
                foreach (WebServices.Entities.TaskResult Result in Response.MessageBody.QueryTaskResult)
                {
                    IEnumerable<Models.Task> TasksConverted = Result.TASK
                                                                    .Select(taskToConvert => Converters.Task(taskToConvert));
                    foreach (var TaskToAdd in TasksConverted)
                    {
                        ExecutedTasks.Add(TaskToAdd);
                    }
                }
            }
            else
                await _dialogService.DisplayAlertAsync("Información", "No se encontró tareas asociadas al vehículo ", "Entiendo");
        }

    }
}
