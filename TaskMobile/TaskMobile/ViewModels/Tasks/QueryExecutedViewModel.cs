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

namespace TaskMobile.ViewModels.Tasks
{
    public class QueryExecutedViewModel : BaseViewModel, INavigatingAware
    {
        public QueryExecutedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster";
            ExecutedTasks = new ObservableCollection<Models.Task>();
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
        }

        #region VIEW MODEL PROPERTIES
        private ObservableCollection<Models.Task> _ExecutedTasks;
        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public ObservableCollection<Models.Task> ExecutedTasks
        {
            get { return _ExecutedTasks; }
            set
            {
                SetProperty(ref _ExecutedTasks, value);

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
        #endregion

        #region COMMANDS
        public DelegateCommand<object> 
            ToDetailCommand { get; private set; }
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
        /// Navigate to <see cref="Views.Tasks.ExecutedToFinish"/> view that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskToFinish", selectedTask);
            await _navigationService.NavigateAsync("ExecutedToFinish", Parameters);
        }

        /// <summary>
        /// Refresh the assigned task list view.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task RefreshData()
        {
            int VehicleId;
            bool VehicleWithId = int.TryParse(CurrentVehicle.Identifier , out VehicleId);
            if (VehicleWithId == false)
                await _dialogService.DisplayAlertAsync("Error", "Un minuto, el vehículo no cuenta con un identificador. Configura el vehículo", "Entiendo");
            else
            {
                Client RESTClient = new Client(WebServices.URL.RequestDetails);
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

        public async  void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                CurrentVehicle = await App.SettingsInDb.CurrentVehicle();
                Vehicle = CurrentVehicle.NameToShow;
                await RefreshData();
                IsRefreshing = false;
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas ejecutadas", "Entiendo");
            }

        }

    }
}
