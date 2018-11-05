using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using TaskMobile.Views.Tasks;
using TaskMobile.WebServices;
using Xamarin.Forms;
using Thread = System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="QueryAssigned"/> view.
    /// </summary>
    public class QueryAssignedViewModel : BaseViewModel, INavigatingAware
    {
        private DelegateCommand<object> _toActivity;
        private ObservableCollection<Models.Task> _assignedTasks;
        private readonly WebServices.REST.Tasks _service;

        public QueryAssignedViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
                               : base(navigationService, dialogService, client)
        {
            _service = new WebServices.REST.Tasks(client);
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

        public DelegateCommand RefreshCommand => new DelegateCommand(RefreshData);

        #endregion

        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public ObservableCollection<Models.Task> AssignedTasks
        {
            get { return _assignedTasks; }
            set
            {
                SetProperty(ref _assignedTasks, value);
            }
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                await CheckVehicle();
                if (CurrentVehicle != null)
                    RefreshData();
                Models.Driver driver = await App.SettingsInDb.Driver();
                Driver = driver.User;
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas asignadas", "Entiendo");
            }
        }

        /// <summary>
        /// Navigate to <see cref="AssignedToExecuted"/> view.
        /// </summary>
        /// <param name="tapped">Selected <see cref="Models.Task"/> by the user.</param>
        private async void GoToActivities(object tapped)
        {
            var parameters = new NavigationParameters();
            parameters.Add("TaskToRun", tapped);
            await _navigationService.NavigateAsync("AssignedToExecuted", parameters);
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
                if (!tappedTask.Expanded)
                {
                    tappedTask.Clear();
                    _service.Details( tappedTask,
                        response =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if ( !response.Any() )
                                    await _dialogService.DisplayAlertAsync("Información", "No se encontró detalles",
                                        "Entiendo");
                                else
                                {
                                    foreach (var taskToAdd in response)
                                    {
                                        tappedTask.Add(taskToAdd);
                                    }
                                }
                                tappedTask.Expanded = !tappedTask.Expanded;
                                IsRefreshing = false;
                            });
                        }, OnWebServiceError);
                }
                else
                {
                    tappedTask.Expanded = !tappedTask.Expanded;
                    IsRefreshing = false;
                }
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
        //private async Thread.Task RefreshData()
        private void RefreshData()
        {
            IsRefreshing = true;
            AssignedTasks.Clear();
            _service.All( VehicleId, "A",
                response =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var taskToAdd in response)
                        {
                            AssignedTasks.Add(taskToAdd);
                        }
                        IsRefreshing = false;
                    });
                }, OnWebServiceError);
        }
    }
}
