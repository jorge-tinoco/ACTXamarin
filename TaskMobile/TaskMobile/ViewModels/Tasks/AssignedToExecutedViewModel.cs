using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using TaskMobile.Models;
using TaskMobile.WebServices;
using Xamarin.Forms;
using Thread = System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.AssignedToExecuted"/> view.
    /// </summary>
    public class AssignedToExecutedViewModel : BaseViewModel, INavigatingAware
    {
        private string _rejectionColor;
        private Rejection _rejection;
        private List<Activity> _activities;
        private IEnumerable<Rejection> _rejections;
        private DelegateCommand<Picker> _pickerCommand;
        private readonly WebServices.REST.Activities _service;

        /// <summary>
        /// Constructor that implements navigation service.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        /// <param name="dialog">Prism dialog service.</param>
        /// <param name="client">Web service client.</param>
        public AssignedToExecutedViewModel(INavigationService navigationService, IPageDialogService dialog, IClient client) 
            : base(navigationService, dialog, client)
        {
            Driver = "TINOCO";
            _service = new WebServices.REST.Activities(client);
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
        #endregion

        #region METHODS
        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                var tappedTask = (int)parameters["TaskToRun"];
                CurrentTask = tappedTask;
                Models.Vehicle current = await App.SettingsInDb.CurrentVehicle();
                Rejections = await App.SettingsInDb.Rejections();
                Vehicle = current.NameToShow;
                await ShowActivities();
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error cuando se consultaban las actividades", "Entiendo");
            }
        }

        /// <summary>
        /// Run the selected activity, and navigate to <see cref="Views.Tasks.Executed"/> view.
        /// </summary>
        /// <param name="tappedActivity"></param>
        private async Thread.Task RunActivity(Activity tappedActivity)
        {
            try
            {
                IsRefreshing = true;
                _service.Start( CurrentTask, tappedActivity.Id, Driver,
                    started =>
                    {
                        Device.BeginInvokeOnMainThread( async () =>
                        {
                            IsRefreshing = false;
                            if (started)
                            {
                                var parameters = new NavigationParameters
                                  {
                                      {"ExecutedActivity", tappedActivity}
                                  };
                                await _navigationService.NavigateAsync("ExecutedTask", parameters);
                            }
                        });
                    }, OnWebServiceError);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                App.LogToDb.Error("Error al ejecutar la actividad " + tappedActivity.Id, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo fue mal al ejecutar la actividad :" + tappedActivity.Id, "Entiendo");
            }
        }

        /// <summary>
        /// Reject activity and navigate to <see cref="Views.Tasks.Rejected"/> view.
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
                    _service.Reject(CurrentTask, tappedActivity.Id, Rejection.Number, Driver,
                          rejected =>
                          {
                              Device.BeginInvokeOnMainThread(async () =>
                              {
                                  IsRefreshing = false;
                                  if (rejected)
                                  {
                                      var parameters = new NavigationParameters
                                      {
                                          {"RejectedActivity", tappedActivity},
                                          {"ComesFrom", "Assigned"}
                                      };
                                      await _navigationService.NavigateAsync("RejectedTask", parameters);
                                  }
                              });
                          }, OnWebServiceError);
                }
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                App.LogToDb.Error("Error al rechazar actividad " + tappedActivity.Id, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo fue mal al rechazar la actividad :" + tappedActivity.Id, "Entiendo");
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
                _service.GetAll(CurrentTask, "A" ,
                    activities =>
                    {
                        Activities = activities.ToList();
                        IsRefreshing = false;
                    }, OnWebServiceError);
            }
            catch (Exception ex)
            {
                IsRefreshing = false;
                App.LogToDb.Error("Error al descargar actividades de la tarea: " + CurrentTask, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo sucedió al descargar las actividades", "Entiendo");
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
