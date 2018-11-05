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
    public class QueryFinishedViewModel : BaseViewModel, INavigatingAware
    {
        private DateTime _start = DateTime.Now.AddDays(-15);
        private DateTime _end = DateTime.Now;
        private bool _isFirstLoad = true;
        private DelegateCommand<object> _toActivity;
        private readonly WebServices.REST.Tasks _service;

        public QueryFinishedViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
            : base(navigationService,dialogService, client)
        {
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
        public ObservableCollection<Models.Task> FinishedTasks { get; private set; }
            = new ObservableCollection<Models.Task>();

        /// <summary>
        /// Used for filter finished tasks.
        /// </summary>
        public DateTime StartDate
        {
            get { return  _start; }
            set
            {
                SetProperty(ref _start, value);
                if (!_isFirstLoad)
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
                SetProperty(ref _end, value.AddDays(1).AddTicks(-1));
                if (!_isFirstLoad)
                    RefreshCommand.Execute();
            }
        }

        #endregion

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                _isFirstLoad = false;
                await CheckVehicle();
                if ( CurrentVehicle != null)
                    RefreshData();
                Models.Driver driver = await App.SettingsInDb.Driver();
                Driver = driver.User;
            }
            catch (Exception e)
            {
                IsRefreshing = false;
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al descargar las tareas finalizadas", "Entiendo");
            }
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.FinishDetails"/>  that shows activities for the selected task.
        /// </summary>
        /// <param name="tappedTask">Selected task by the user.</param>
        private async void GoToActivities(object tappedTask)
        {
            var parameters = new NavigationParameters();
            parameters.Add("TaskWithDetail", tappedTask);
            await _navigationService.NavigateAsync("FinishDetails", parameters);
        }

        /// <summary>
        /// Query REST web services to get finished tasks.
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
                IsRefreshing = false;
                App.LogToDb.Error("Error al mostrar detalles de la tarea " + tappedTask.Number, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo ocurrió cuando mostrábamos los detalles", "Entiendo");
            }
        }

        /// <summary>
        /// Refresh the finished task list view.
        /// </summary>
        /// <returns></returns>
        private void RefreshData()
        {
            IsRefreshing = true;
            FinishedTasks.Clear();
            _service.ByRange(VehicleId, "F", StartDate, EndDate,
                tasks =>
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        tasks = tasks.OrderByDescending(x => x.DateTask);
                        foreach (var taskToAdd in tasks)
                        {
                            FinishedTasks.Add(taskToAdd);
                        }
                        IsRefreshing = false;
                    });
                }, OnWebServiceError);
        }
        
    }
}
