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
    public class QueryRejectedViewModel : BaseViewModel, INavigatingAware
    {
        public QueryRejectedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Driver = "Jorge Tinoco";
        }

        #region VIEW MODEL PROPERTIES

        private DateTime _start = DateTime.Now.AddDays(-15);
        private DateTime _end = DateTime.Now;
        private bool _isRefreshing = false;
        private bool _isFirstLoad = true;

        public ObservableCollection<Models.Task> RejectedTasks { get; private set; }
            = new ObservableCollection<Models.Task>();

        /// <summary>
        /// Used for filter finished tasks.
        /// </summary>
        public DateTime StartDate
        {
            get { return _start; }
            set
            {
                SetProperty(ref _start, value);
                if(!IsFirstLoad)
                    RefreshCommand.Execute();
            }
        }

        /// <summary>
        /// Used for filter finished tasks.
        /// </summary>
        public DateTime EndDate
        {
            get { return _end; }
            set
            {
                SetProperty(ref _end, value.AddDays(1).AddTicks(-1) );
                if ( !IsFirstLoad )
                    RefreshCommand.Execute();
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

        public bool IsFirstLoad
        {
            get { return _isFirstLoad; }
            set { SetProperty(ref _isFirstLoad, value); }
        }
        #endregion

        #region COMMANDS

        private DelegateCommand<object> _toActivity;
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
                    await RefreshData();
                });
            }
        }

        #endregion


        /// <summary>
        /// Navigate to <see cref="Views.Tasks.RejectedDetail"/>  that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToActivities(object tappedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskWithActivities", tappedTask);
            await _navigationService.NavigateAsync("RejectedDetail", Parameters);
        }

        /// <summary>
        /// Query REST web services to get rejected tasks.
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
        /// Refresh the rejected task list view.
        /// </summary>
        private async Task RefreshData()
        {
            IsRefreshing = true;
            Client RESTClient = new Client(WebServices.URL.GetTasks);
            Request<WebServices.Entities.TaskRequest> Requests = new Request<WebServices.Entities.TaskRequest>();
            Requests.MessageBody.VehicleId = int.Parse(CurrentVehicle.Identifier);
            Requests.MessageBody.Status = "R";
            Requests.MessageBody.InitialDate = StartDate;
            Requests.MessageBody.FinalDate = EndDate;
            var Response = await RESTClient.Post<Response<WebServices.Entities.TaskResponse>>(Requests);
            RejectedTasks.Clear();
            if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.QueryTaskResult.Count() > 0)
            {
                var AllTasks = Response.MessageBody.QueryTaskResult.SelectMany(x => x.TASK);
                var Ordered = AllTasks.OrderByDescending(x => x.CREATED_DATE);
                var TasksConverted = Ordered.Select(taskToConvert => Converters.Task(taskToConvert));
                foreach (var item in TasksConverted)
                {
                    RejectedTasks.Add(item);
                }
            }
            else
                await _dialogService.DisplayAlertAsync("Información", "No se encontró tareas asociadas al vehículo ", "Entiendo");
            IsRefreshing = false;
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                IsFirstLoad = false;
                await CheckVehicle();
                if (CurrentVehicle != null)
                    await RefreshData();
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas rechazadas", "Entiendo");
            }
            finally {
                IsRefreshing = false;
            }
        }

    }
}
