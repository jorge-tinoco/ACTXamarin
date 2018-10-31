using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskMobile.WebServices;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishDetailsViewModel : BaseViewModel, INavigatingAware
    {
        private int _currentTask;
        private List<Models.Activity> _activities;
        private DelegateCommand _finish;
        private readonly WebServices.REST.Activities _service;

        public FinishDetailsViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
            : base(navigationService, dialogService, client)
        {
            _service = new WebServices.REST.Activities(client);
        }

        #region  COMMANDS
        public DelegateCommand FinishCommand =>
            _finish ?? (_finish = new DelegateCommand(GoToMainPage));

        public ICommand RefreshCommand
        {
            get
            {
                return new Command(async () =>
                {
                    await ShowActivities();
                });
            }
        }

        #endregion

        #region  VIEW MODEL PROPERTIES
        /// <summary>
        /// Current finished  tasks details.
        /// </summary>
        public List<Models.Activity> Activities
        {
            get { return _activities; }
            set
            {
                SetProperty(ref _activities, value);
            }
        }

        /// <summary>
        /// Task that contains the showed <see cref="Activities"/>.
        /// </summary>
        private int CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); }
        }

        #endregion

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            int tappedTask = (int)parameters["TaskWithDetail"];
            CurrentTask = tappedTask;
            Models.Vehicle current = await App.SettingsInDb.CurrentVehicle();
            Vehicle = current.NameToShow;
            await ShowActivities();
        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void GoToMainPage()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        /// <summary>
        /// Query the REST web service to get the activities for the current task.
        /// </summary>
        private async Task ShowActivities()
        {
            try
            {
                IsRefreshing = true;
                _service.GetAll(CurrentTask, "F",
                    activities =>
                    {
                        Activities = activities.ToList();
                        IsRefreshing = false;
                    }, OnWebServiceError);
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al consultar actividades de la tarea: " + CurrentTask, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo sucedió al consultar las actividades", "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
       
    }
}
