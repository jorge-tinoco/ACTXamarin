using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.ExecutedToFinish"/> view.
    /// </summary>
    public class ExecutedToFinishViewModel : BaseViewModel, INavigatedAware
    {
        public ExecutedToFinishViewModel(INavigationService navigationService) : base(navigationService)
        {
            FinishTaskCommand = new DelegateCommand<object>(FinishTaskAction);
            RejectTaskCommand = new DelegateCommand<object>(RejectTaskAction);
            Driver = "Jorge Tinocos";
            Vehicle = "Hyster";
        }
        #region  VIEW MODEL PROPERTIES

        private List<Models.TaskDetail> _Details;
        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public List<Models.TaskDetail> Details
        {
            get { return _Details; }
            set
            {
                SetProperty(ref _Details, value);
            }
        }

        private bool _isRefreshing = false;
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
        #region  COMMANDS
        public DelegateCommand<object> FinishTaskCommand { get; private set; }
        public DelegateCommand<object> RejectTaskCommand { get; private set; }
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
        /// Execute selected detail task, and navigate to <see cref="Views.Tasks.Executed"/> view.
        /// </summary>
        /// <param name="parameter">Task detail to execute.</param>
        private async void FinishTaskAction(object parameter)
        {
            Models.TaskDetail SelectedDetail = parameter as Models.TaskDetail;
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("FinishedTask", SelectedDetail);
            await _navigationService.NavigateAsync("Finished", Parameters);
        }

        /// <summary>
        /// Reject task and navigate to <see cref="Views.Tasks.Canceled"/> view.
        /// </summary>
        /// <param name="parameter">Detail of canceled task.</param>
        private async void RejectTaskAction(object parameter)
        {
            Models.TaskDetail SelectedDetail = parameter as Models.TaskDetail;
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("CanceledTask", SelectedDetail);
            await _navigationService.NavigateAsync("RejectedTask", Parameters);
        }

        /// <summary>
        /// Refresh detail  task list.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            await Task.Delay(1000);
        }

        public void OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        /// <summary>
        /// Used for getting passed parameters.
        /// </summary>
        /// <param name="parameters">Task that could be finished by the user.</param>
        public void OnNavigatedTo(NavigationParameters parameters)
        {
            Models.Task Selected = parameters["TaskToFinish"] as Models.Task;
            Details = Selected.Details.ToList();
        }
    }
}
