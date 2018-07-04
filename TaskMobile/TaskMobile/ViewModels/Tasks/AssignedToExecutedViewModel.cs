using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices.REST;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.AssignedToExecuted"/> view.
    /// </summary>
    public class AssignedToExecutedViewModel : BaseViewModel, INavigationAware
    {
        #region  VIEW MODEL PROPERTIES

        private List<Models.TaskDetail> _Details;
        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public List<Models.TaskDetail> Details
        {
            get { return _Details; }
            set
            {
                SetProperty(ref _Details, value);
            }
        }

        private bool _isRefreshing = false;
        /// <summary>
        /// Flag for stablish when the list view is refreshing.
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
        #region  COMMANDS
        public DelegateCommand<object> RunTaskCommand { get; private set; }
        public DelegateCommand<object> RejectTaskCommand { get; private set; }
        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    IsRefreshing = true;
                    await RefreshData();
                    IsRefreshing = false;
                });
            }
        }
        #endregion

        /// <summary>
        /// Constructor that implements navigation service.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AssignedToExecutedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            RunTaskCommand = new DelegateCommand<object>(RunTaskAction);
            RejectTaskCommand = new DelegateCommand<object>(RejectTaskAction);
            Driver = "Jorge Tinocos";
            Vehicle = "Hyster";
        }

        #region ACTIONS

        /// <summary>
        /// Execute selected detail task, and navigate to <see cref="Views.Tasks.Executed"/> view.
        /// </summary>
        /// <param name="parameter">Task detail to execute.</param>
        private async void RunTaskAction(object parameter)
        {
            Models.TaskDetail SelectedDetail = parameter as Models.TaskDetail;
            RunTask(SelectedDetail);
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("ExecutedTask", SelectedDetail);
            await _navigationService.NavigateAsync("ExecutedTask", Parameters);
        }

        /// <summary>
        /// Reject task and navigate to <see cref="Views.Tasks.Canceled"/> view.
        /// </summary>
        /// <param name="parameter">Detail of canceled task.</param>
        private async void RejectTaskAction(object parameter)
        {
            Models.TaskDetail SelectedDetail = parameter as Models.TaskDetail;
            RejectTask(SelectedDetail);
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("CanceledTask", SelectedDetail);
            await _navigationService.NavigateAsync("RejectedTask", Parameters);
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Refresh detail  task list.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            await Task.Delay(1000);
        }

        /// <summary>
        /// Call the web service to run the task.
        /// </summary>
        /// <param name="taskToRun">Task to run</param>
        public async void RunTask(Models.TaskDetail taskToRun)
        {
            try
            {
                Client RESTClient = new Client(WebServices.URL.ActivityStart);
                Request<WebServices.Entities.ActivityRequest> Requests = new Request<WebServices.Entities.ActivityRequest>();
                Requests.MessageBody.ActivityId = 0;
                Requests.MessageBody.DestinyLocation = taskToRun.Destination;
                Requests.MessageBody.LastUpdateBy = "T1004"; // TO DO: substitute with the current user.
                Requests.MessageBody.RejectionId = 0;
                Requests.MessageBody.TaskId = taskToRun.TaskNumber;
                var Response = await RESTClient.Post<Response<WebServices.Entities.ActivityResponse>>(Requests);
                if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.Result != null)
                {
                    string LogMessage = string.Format("Tarea ejecutada: {0}", taskToRun.TaskNumber);
                    App.LogToDb.Info(LogMessage, Response.MessageBody.Result);
                    await _dialogService.DisplayAlertAsync("WS Response", Response.MessageBody.Result, "Entiendo");
                }
                else
                    await _dialogService.DisplayAlertAsync("Espera", "Algo salió mal cuando intentabamos ejecutar la tarea anterior, por favor intenta de nuevo", "Entiendo");
            }
            catch (System.Exception ex)
            {
                App.LogToDb.Error("Consumiendo servicio: ejecutar tarea", ex);
                await _dialogService.DisplayAlertAsync("Oops", "Hubo un problema para ejecutar la tarea", "Entiendo");
            }
        }

        /// <summary>
        /// Call the web service to reject the task.
        /// </summary>
        /// <param name="taskToReject">Task to reject.</param>
        public async void RejectTask(Models.TaskDetail taskToReject)
        {
            try
            {
                Client RESTClient = new Client(WebServices.URL.ActivityRejected);
                Request<WebServices.Entities.ActivityRequest> Requests = new Request<WebServices.Entities.ActivityRequest>();
                Requests.MessageBody.ActivityId = 0;
                Requests.MessageBody.DestinyLocation = taskToReject.Destination;
                Requests.MessageBody.LastUpdateBy = "T1004"; // TO DO: substitute with the current user.
                Requests.MessageBody.RejectionId = 0;
                Requests.MessageBody.TaskId = taskToReject.TaskNumber;

                var Response = await RESTClient.Post<Response<WebServices.Entities.ActivityResponse>>(Requests);
                if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.Result != null)
                {
                    string LogMessage = string.Format("Tarea rechazada: {0}", taskToReject.TaskNumber);
                    App.LogToDb.Info(LogMessage, Response.MessageBody.Result);
                    await _dialogService.DisplayAlertAsync("WS Response", Response.MessageBody.Result, "Entiendo");
                }
                else
                    await _dialogService.DisplayAlertAsync("Espera", "Algo salió mal cuando intentabamos rechazar la tarea anterior, por favor intenta de nuevo", "Entiendo");
            }
            catch (System.Exception ex)
            {
                App.LogToDb.Error("Consumiendo servicio: rechazar tarea", ex);
                await _dialogService.DisplayAlertAsync("Oops", "Hubo un problema al rechazar la tarea", "Entiendo");
            }
        }
        #endregion

        #region NAVIGATION ACTIONS
        void INavigatedAware.OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        /// < summary>
        /// Executed when navigated from other page. In this case, the request comes from <see cref="ViewModels.TaskViewModel"/>
        /// </summary>
        /// <param name="parameters">Selected <see cref="Models.Task"/> passed as parameter.</param>
        void INavigatedAware.OnNavigatedTo(NavigationParameters parameters)
        {
            Models.Task Selected = parameters["SelectedTask"] as Models.Task;
            Details = Selected.Details.ToList();
        }

        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
           
        }
        #endregion
       
    }
}
