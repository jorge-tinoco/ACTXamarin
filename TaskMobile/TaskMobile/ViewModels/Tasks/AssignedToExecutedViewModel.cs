using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.AssignedToExecuted"/> view.
    /// </summary>
    public class AssignedToExecutedViewModel : BaseViewModel, INavigationAware
    {
        #region  VIEW MODEL PROPERTIES

        private List<Models.TaskDetail> _Details;
        /// <summary>
        /// Current pending/assigned  tasks.
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
        public DelegateCommand<object> RunTaskCommand { get; private set; }
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
        /// Constructor that implements navigation service.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AssignedToExecutedViewModel(INavigationService navigationService):base(navigationService)
        {
            RunTaskCommand = new DelegateCommand<object>(RunTaskAction);
            RejectTaskCommand = new DelegateCommand<object>(RejectTaskAction);
            Driver = "Jorge Tinocos";
            Vehicle = "Hyster";
        }

       /// <summary>
       /// Execute selected detail task, and navigate to <see cref="Views.Tasks.Executed"/> view.
       /// </summary>
       /// <param name="parameter">Task detail to execute.</param>
        private async void RunTaskAction(object parameter)
        {
            Models.TaskDetail SelectedDetail = parameter as Models.TaskDetail;
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("ExecutedTask", SelectedDetail);
            await _navigationService.NavigateAsync("ExecutedTask", Parameters);
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

        #region NAVIGATION ACTIONS
        void INavigatedAware.OnNavigatedFrom(NavigationParameters parameters)
        {
        }

        /// <summary>
        /// Executed when navigated from other page. In this case, the request comes from <see cref="ViewModels.TaskViewModel"/>
        /// </summary>
        /// <param name="parameters">Selected <see cref="Models.Task"/> passed as parameter.</param>
        void INavigatedAware.OnNavigatedTo(NavigationParameters parameters)
        {
            Models.Task Selected = parameters["SelectedTask"] as Models.Task;
            Details = Selected.Details.ToList();
        }

        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
           
        }
        #endregion
       
    }
}
