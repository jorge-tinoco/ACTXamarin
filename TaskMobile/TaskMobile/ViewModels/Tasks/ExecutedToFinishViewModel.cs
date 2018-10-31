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
    /// View model representing <see cref="Views.Tasks.ExecutedToFinish"/> view.
    /// </summary>
    public class ExecutedToFinishViewModel : BaseViewModel, INavigatingAware
    {
        private string _rejectionColor;
        private Rejection _rejection;
        private List<Activity> _activities;
        private IEnumerable<Rejection> _rejections;
        private DelegateCommand<Picker> _pickerCommand;
        private readonly WebServices.REST.Activities _service;

        public ExecutedToFinishViewModel(INavigationService navigationService , IPageDialogService dialogService, IClient client) 
            :base(navigationService, dialogService, client)
        {
            Driver = "TINOCO";
            _service = new WebServices.REST.Activities(client);
        }

        #region  COMMANDS

        public DelegateCommand<Activity> FinishTaskCommand
        {
            get
            {
                return new DelegateCommand<Activity>(async (activity) =>
                {
                    await FinishActivity(activity);
                });
            }
        }
        public DelegateCommand<Activity> RejectCommand
        {
            get
            {
                return new DelegateCommand<Activity> (async (activity) =>
                {
                    await RejectActivity(activity);
                });
            }
        }
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
        public DelegateCommand<Picker> PickerCommand =>
            _pickerCommand ?? (_pickerCommand = new DelegateCommand<Picker>(FocusPicker));
        #endregion

        #region  VIEW MODEL PROPERTIES

        /// <summary>
        /// Current activities for selected task.
        /// </summary>
        public List<Activity> Activities
        {
            get { return _activities; }
            set
            {
                SetProperty(ref _activities, value);
            }
        }

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
            set {
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

        private int CurrentTask { get; set; }
        #endregion

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                var tappedTask = (int)parameters["TaskToFinish"];
                CurrentTask = tappedTask;
                Models.Vehicle current = await App.SettingsInDb.CurrentVehicle();
                Rejections = await App.SettingsInDb.Rejections();
                Vehicle = current.NameToShow;
                await ShowActivities();
            }
            catch (Exception e)
            {
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error cuanso se consultaban las actividades", "Entiendo");
            }
        }

        /// <summary>
        /// Execute selected detail task, and navigate to <see cref="Views.Tasks.Executed"/> view.
        /// </summary>
        /// <param name="tappedActivity">Task detail to execute.</param>
        private async Thread.Task FinishActivity(Activity tappedActivity)
        {
            try
            {
                IsRefreshing = true;
                _service.Finish(CurrentTask, tappedActivity.Id, Driver,
                   finished =>
                   {
                       Device.BeginInvokeOnMainThread(async () =>
                       {
                           IsRefreshing = false;
                           if (finished)
                           {
                               var parameters = new NavigationParameters
                                  {
                                      {"ActivityFinished", tappedActivity}
                                  };
                               await _navigationService.NavigateAsync("Finished", parameters);
                           }
                       });
                   }, OnWebServiceError);
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al finalizar la actividad " + tappedActivity.Id, ex);
                await _dialogService.DisplayAlertAsync("Error", "Algo fue mal al finalizar la actividad :" + tappedActivity.Id, "Entiendo");
            }
            finally
            {
                IsRefreshing = false;
            }
        }

        /// <summary>
        /// Reject activity and navigate to <see cref="Views.Tasks.Canceled"/> view.
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
                                          {"ComesFrom", "Executed"}
                                      };
                                      await _navigationService.NavigateAsync("RejectedTask", parameters);
                                  }
                              });
                          }, OnWebServiceError);
                }
            }
            catch (Exception ex)
            {
                App.LogToDb.Error("Error al rechazar actividad "+ tappedActivity.Id, ex);
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
                _service.GetAll(CurrentTask, "E",
                    activities =>
                    {
                        Activities = activities.ToList();
                        IsRefreshing = false;
                    }, OnWebServiceError);
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

    }
}
