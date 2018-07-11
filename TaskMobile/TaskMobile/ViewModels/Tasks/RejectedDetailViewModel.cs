using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TaskMobile.WebServices.REST;
using Xamarin.Forms;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.ViewModels.Tasks
{
    public class RejectedDetailViewModel : BaseViewModel, INavigatingAware
    {
        public RejectedDetailViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {

        }


        #region  VIEW MODEL PROPERTIES

        private List<Models.Activity> _activities;
        private bool _isRefreshing = false;
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
        private DelegateCommand _Finish;
        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(ExecuteFinishCommand));

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
        /// Refresh detail  task list.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            await ShowActivities(CurrentTask);
        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void ExecuteFinishCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            int TappedTask = (int) parameters["TaskWithActivities"] ;
            CurrentTask = TappedTask;
            Models.Vehicle Current = await App.SettingsInDb.CurrentVehicle();
            Vehicle = Current.NameToShow;
            await ShowActivities(TappedTask);
        }

        private async Task ShowActivities(int taskToQuery)
        {
            try
            {
                IsRefreshing = true;
                var RESTClient = new Client(WebServices.URL.GetActivities);
                var Requests = new Request<ActivityRequest>();
                Requests.MessageBody.TaskId = taskToQuery;
                var Response = await RESTClient.Post< Response<ActivityResponse> >(Requests);
                var ResultCode = Response.MessageLog.ProcessingResultCode;
                var ActivitiesFromWS = Response.MessageBody.QueryTaskActivitiesResult.ACTIVITIES;
                
                if (ResultCode == 0 && ActivitiesFromWS.Count() >= 0)
                {
                    Activities = ActivitiesFromWS.Select(activityToConvert => Converters.Activity(activityToConvert) ).ToList();
                }
                else
                {
                    if (Response.MessageLog.LogItems.Count() >= 0)
                    {
                        foreach (LogItem error in Response.MessageLog.LogItems)
                        {
                            await _dialogService.DisplayAlertAsync("Error", error.ErrorDescription, "Entiendo");
                        }
                    }else
                        await _dialogService.DisplayAlertAsync("Información", "No se encontraron actividades", "Entiendo");
                }
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al consultar actividades de la tarea: " + taskToQuery, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo sucedió al consultar las actividades", "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }
    }
}
