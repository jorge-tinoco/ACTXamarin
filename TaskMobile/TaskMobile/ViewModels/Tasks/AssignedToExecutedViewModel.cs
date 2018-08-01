using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using Thread = System.Threading.Tasks;
using System.Windows.Input;
using TaskMobile.Models;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices.REST;
using Xamarin.Forms;
using System;
using TaskMobile.WebServices.Entities;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.AssignedToExecuted"/> view.
    /// </summary>
    public class AssignedToExecutedViewModel : BaseViewModel, INavigatingAware
    {
        private string _rejectionColor;
        private bool _isRefreshing = false;
        private Rejection _rejection;
        private List<Activity> _activities;
        private IEnumerable<Rejection> _rejections;
        private DelegateCommand<Picker> _pickerCommand;

        /// <summary>
        /// Constructor that implements navigation service.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AssignedToExecutedViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService, dialogService)
        {
            Driver = "TINOCO";
        }

        #region  COMMANDS
        public DelegateCommand<Activity> RunCommand
        {
            get
            {
                return new DelegateCommand<Activity>(async (activity) =>
                {
                    await RunActivity(activity);
                });
            }
        }
        public DelegateCommand<Activity> RejectCommand
        {
            get
            {
                return new DelegateCommand<Activity>(async (activity) =>
                {
                    await RejectActivity(activity);
                });
            }
        }
        public DelegateCommand<Picker> PickerCommand =>
            _pickerCommand ?? (_pickerCommand = new DelegateCommand<Picker>(FocusPicker));
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
        /// Current pending/assigned  tasks.
        /// </summary>
        public List<Activity> Activities
        {
            get { return _activities; }
            set
            {
                SetProperty(ref _activities, value);
            }
        }

        public int CurrentTask { get; set; }

        /// <summary>
        /// Available rejections reasons.
        /// </summary>
        public IEnumerable<Rejection> Rejections
        {
            get { return _rejections; }
            set { SetProperty(ref _rejections, value); }
        }

        /// <summary>
        /// Selected reason for rejection
        /// </summary>
        public Rejection Rejection
        {
            get { return _rejection; }
            set
            {
                RaisePropertyChanged("RejectionColor");
                SetProperty(ref _rejection, value);
            }
        }

        /// <summary>
        /// Defines the rejection icon color.
        /// </summary>
        public string RejectionColor
        {
            get
            {
                if (Rejection == null)
                    return Utilities.TenarisColors.ShamrockGreen;
                else
                    return Utilities.TenarisColors.OrangePeel;
            }
            set { SetProperty(ref _rejectionColor, value); }
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

        #endregion

        #region METHODS
        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                int TappedTask = (int)parameters["TaskToRun"];
                CurrentTask = TappedTask;
                Models.Vehicle Current = await App.SettingsInDb.CurrentVehicle();
                Rejections = await App.SettingsInDb.Rejections();
                Vehicle = Current.NameToShow;
                await ShowActivities();
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error cuanso se consultaban las actividades", "Entiendo");
            }
        }

        /// <summary>
        /// Run the selected activity, and navigate to <see cref="Views.Tasks.Executed"/> view.
        /// </summary>
        /// <param name="parameter">Task activity to run.</param>
        private async Thread.Task RunActivity(Activity tappedActivity)
        {
            try
            {
                IsRefreshing = true;
                var RESTClient = new Client(WebServices.URL.ActivityStart);
                var Requests = new Request<ActivityUpdRequest>();
                Requests.MessageBody.TaskId = CurrentTask;
                Requests.MessageBody.RejectionId = 0;
                Requests.MessageBody.LastUpdateBy = Driver;
                Requests.MessageBody.ActivityId = tappedActivity.Id;
                var Response = await RESTClient.Post<Response<ActivityUpdResponse>>(Requests);
                var ResultCode = Response.MessageLog.ProcessingResultCode;
                if (ResultCode == 0 && Response.MessageBody.Result == "0")
                {
                    NavigationParameters Parameters = new NavigationParameters();
                    Parameters.Add("ExecutedActivity", tappedActivity);
                    await _navigationService.NavigateAsync("ExecutedTask", Parameters);
                }
                else
                {
                    if (Response.MessageLog.LogItem != null)
                        await _dialogService.DisplayAlertAsync("Error", Response.MessageLog.LogItem.ErrorDescription, "Entiendo");
                    else
                        await _dialogService.DisplayAlertAsync("Información", "No se ha podido ejecutar la actividad : " + tappedActivity.Name, "Entiendo");
                }
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al ejecutar la actividad " + tappedActivity.Id, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo fue mal al ejecutar la actividad :" + tappedActivity.Id, "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Reject activity and navigate to <see cref="Rejected"/> view.
        /// </summary>
        /// <param name="tappedActivity">Activity to reject.</param>
        private async Thread.Task RejectActivity(Activity tappedActivity)
        {
            try
            {
                if (Rejection == null)
                    await _dialogService.DisplayAlertAsync("Espera", "Es necesario  seleccionar el motivo de rechazo", "Lo haré");
                else
                {
                    IsRefreshing = true;
                    var RESTClient = new Client(WebServices.URL.ActivityRejected);
                    var Requests = new Request<ActivityUpdRequest>();
                    Requests.MessageBody.TaskId = CurrentTask;
                    Requests.MessageBody.RejectionId = Rejection.Number;
                    Requests.MessageBody.LastUpdateBy = Driver;
                    Requests.MessageBody.ActivityId = tappedActivity.Id;
                    Requests.MessageBody.DestinyLocation = "sample string 4";
                    var Response = await RESTClient.Post<Response<ActivityUpdResponse>>(Requests);
                    var ResultCode = Response.MessageLog.ProcessingResultCode;
                    if (ResultCode == 0 && Response.MessageBody.Result == "0")
                    {
                        NavigationParameters Parameters = new NavigationParameters();
                        Parameters.Add("RejectedActivity", tappedActivity);
                        Parameters.Add("ComesFrom", "Assigned");
                        await _navigationService.NavigateAsync("RejectedTask", Parameters);
                    }
                    else
                    {
                        if (Response.MessageLog.LogItem != null )
                            await _dialogService.DisplayAlertAsync("Error", Response.MessageLog.LogItem.ErrorDescription, "Entiendo");
                        else
                            await _dialogService.DisplayAlertAsync("Información", "No se ha podido rechazar la actividad : " + tappedActivity.Name, "Entiendo");
                    }
                }

            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al rechazar actividad " + tappedActivity.Id, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo fue mal al rechazar la actividad :" + tappedActivity.Id, "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Refresh activities list.
        /// </summary>
        private async Thread.Task ShowActivities()
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
                    if (Response.MessageLog.LogItem != null )
                            await _dialogService.DisplayAlertAsync("Error", Response.MessageLog.LogItem.ErrorDescription, "Entiendo");
                    else
                        await _dialogService.DisplayAlertAsync("Información", "No se encontraron actividades", "Entiendo");
                }
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al descargar actividades de la tarea: " + CurrentTask, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo sucedió al descargar las actividades", "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Open the picker visual element.
        /// </summary>
        /// <param name="rejectionPicker">Picker to open</param>
        private void FocusPicker(Picker rejectionPicker)
        {
            rejectionPicker.Focus();
        }
        #endregion
    }
}
