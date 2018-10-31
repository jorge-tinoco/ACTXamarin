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
    public class RejectedDetailViewModel : BaseViewModel, INavigatingAware
    {
        private readonly WebServices.REST.Activities _service;

        public RejectedDetailViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
        : base(navigationService, dialogService,client)
        {
            _service = new WebServices.REST.Activities(client);
        }


        #region  VIEW MODEL PROPERTIES

        private List<Models.Activity> _activities;
        private int _currentTask;

        /// <summary>
        /// Current rejected  task details.
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
        /// Current task that contains the showed activities by this view model.
        /// </summary>
        public int CurrentTask
        {
            get { return _currentTask; }
            set { SetProperty(ref _currentTask, value); }
        }

        #endregion
        #region  COMMANDS
        //Command implementations goes here.
        private DelegateCommand _finish;
        public DelegateCommand FinishCommand =>
            _finish ?? (_finish = new DelegateCommand(ExecuteFinishCommand));

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

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void ExecuteFinishCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            int tappedTask = (int) parameters["TaskWithActivities"] ;
            CurrentTask = tappedTask;
            Models.Vehicle current = await App.SettingsInDb.CurrentVehicle();
            Vehicle = current.NameToShow;
            await ShowActivities();
        }

        private async Task ShowActivities()
        {
            try
            {
                IsRefreshing = true;
                _service.GetAll(CurrentTask, "R",
                    activities =>
                    {
                        Activities = activities.ToList();
                        IsRefreshing = false;
                    }, OnWebServiceError);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                App.LogToDb.Error("Error al consultar actividades de la tarea: " + CurrentTask, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo sucedió al consultar las actividades", "Entiendo");
            }
        }
    }
}
