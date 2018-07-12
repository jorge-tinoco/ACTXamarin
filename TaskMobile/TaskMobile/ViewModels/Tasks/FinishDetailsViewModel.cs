using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices.REST;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishDetailsViewModel : BaseViewModel, INavigatingAware
    {
        private bool _isRefreshing = false;
        private int _currentTask;
        private List<Models.Activity> _activities;
        private DelegateCommand _Finish;

        public FinishDetailsViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {

        }

        #region  COMMANDS
        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(GoToMainPage));

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
        /// Stablish when the list view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                SetProperty(ref _isRefreshing, value);
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
            int TappedTask = (int)parameters["TaskWithDetail"];
            CurrentTask = TappedTask;
            Models.Vehicle Current = await App.SettingsInDb.CurrentVehicle();
            Vehicle = Current.NameToShow;
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
                var RESTClient = new Client(WebServices.URL.GetActivities);
                var Requests = new Request<ActivityRequest>();
                Requests.MessageBody.TaskId = CurrentTask;
                var Response = await RESTClient.Post<Response<ActivityResponse>>(Requests);
                var ResultCode = Response.MessageLog.ProcessingResultCode;
                var ActivitiesFromWS = Response.MessageBody.QueryTaskActivitiesResult.ACTIVITIES;

                if (ResultCode == 0 && ActivitiesFromWS.Count() >= 0)
                {
                    Activities = ActivitiesFromWS.Select(activityToConvert => Converters.Activity(activityToConvert)).ToList();
                }
                else
                {
                    if (Response.MessageLog.LogItems.Count() >= 0)
                    {
                        foreach (LogItem error in Response.MessageLog.LogItems)
                        {
                            await _dialogService.DisplayAlertAsync("Error", error.ErrorDescription, "Entiendo");
                        }
                    }
                    else
                        await _dialogService.DisplayAlertAsync("Información", "No se encontraron actividades", "Entiendo");
                }
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
