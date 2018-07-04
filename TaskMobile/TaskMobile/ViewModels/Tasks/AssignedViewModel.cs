using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System;
using TaskMobile.WebServices.REST;
using TaskMobile.WebServices.Entities.Common;
using Prism.Services;
using System.Linq;
using System.Collections.ObjectModel;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.Assigned"/> view.
    /// </summary>
    public class AssignedViewModel : BaseViewModel, INavigatingAware
    {
        private ObservableCollection<Models.Task> _AssignedTasks;
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
        private bool _isRefreshing = true;

        #region COMMANDS
        public DelegateCommand<object> ToDetailCommand { get; private set; }
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

        public AssignedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService,dialogService)
        {
            Driver = "Jorge Tinoco";
            AssignedTasks = new ObservableCollection<Models.Task>();
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
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
        /// Refresh the assigned task list view.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task RefreshData()
        {
            Client RESTClient = new Client(WebServices.URL.RequestDetails);
            Request<WebServices.Entities.TaskRequest> Requests = new Request<WebServices.Entities.TaskRequest>();
            Requests.MessageBody.VehicleId = 369;
            Requests.MessageBody.Status = "A";
            Requests.MessageBody.InitialDate = new DateTime(2016, 01, 01);
            Requests.MessageBody.FinalDate = DateTime.Now;
            var Response = await RESTClient.Post<Response<WebServices.Entities.TaskResponse>>(Requests);
            AssignedTasks.Clear();
            if (Response.MessageLog.ProcessingResultCode == 0 && Response.MessageBody.QueryTaskResult.Count() > 0)
            {
                foreach (WebServices.Entities.TaskResult Result in Response.MessageBody.QueryTaskResult)
                {
                    IEnumerable<Models.Task> TasksConverted = Result.TASK
                                                                    .Select(taskToConvert => Converters.Task(taskToConvert));
                    foreach (var TaskToAdd in TasksConverted)
                    {
                        AssignedTasks.Add(TaskToAdd);
                    }
                }
            }
            else
                await _dialogService.DisplayAlertAsync("Información", "No se encontró tareas asociadas al vehículo ", "Entiendo");
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                Models.Vehicle Current = await App.SettingsInDb.CurrentVehicle();
                Vehicle = Current.NameToShow;
                await RefreshData();
                IsRefreshing = false;
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas asignadas", "Entiendo");
            }
        }
    }
}
