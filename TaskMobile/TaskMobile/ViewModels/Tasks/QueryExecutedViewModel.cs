using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TaskMobile.WebServices;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Tasks
{
    public class QueryExecutedViewModel : BaseViewModel, INavigatingAware
    {
        private DelegateCommand<object> _toActivity;
        private readonly WebServices.REST.Tasks _service;

        public QueryExecutedViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
            : base(navigationService, dialogService,client)
        {
            Driver = "Jorge Tinoco";
            ExecutedTasks = new ObservableCollection<Models.Task>();
            _service = new WebServices.REST.Tasks(client);
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

        public DelegateCommand RefreshCommand => new DelegateCommand( RefreshData);

        #endregion

        #region VIEW MODEL PROPERTIES

        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public ObservableCollection<Models.Task> ExecutedTasks { get; private set; }
        #endregion

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                await CheckVehicle();
                if (CurrentVehicle != null)
                    RefreshData();
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas ejecutadas", "Entiendo");
            }
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.ExecutedToFinish"/> view.
        /// </summary>
        /// <param name="tapped">Selected task by the user.</param>
        private async void GoToActivities(object tapped)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("TaskToFinish", tapped);
            await _navigationService.NavigateAsync("ExecutedToFinish", parameters);
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
                if (!tappedTask.Expanded)
                {
                    tappedTask.Clear();
                    _service.Details(tappedTask,
                        response =>
                        {
                            Device.BeginInvokeOnMainThread(async () =>
                            {
                                if (!response.Any())
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
        private void RefreshData()
        {
            IsRefreshing = true;
            ExecutedTasks.Clear();
            _service.All(VehicleId, "E",
                response =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        foreach (var taskToAdd in response)
                        {
                            ExecutedTasks.Add(taskToAdd);
                        }
                        IsRefreshing = false;
                    });
                }, OnWebServiceError);
        }

    }
}
