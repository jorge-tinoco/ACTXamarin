using Prism.Commands;
using Prism.Navigation;
using System.Collections.Generic;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.Assigned"/> view.
    /// </summary>
    public class AssignedViewModel : BaseViewModel
    {
        private List<Models.Task> _AssignedTasks;
        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public List<Models.Task> AssignedTasks
        {
            get { return _AssignedTasks; }
            set
            {
                SetProperty(ref _AssignedTasks, value);

            }
        }

        /// <summary>
        /// Indicates if the listview is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }
        private bool _isRefreshing = false;

        #region COMMANDS
        public DelegateCommand<object> ToDetailCommand { get; private set; }
        public DelegateCommand RefreshCommand
        {
            get
            {
                return new DelegateCommand(async () =>
                {
                    IsRefreshing = true;

                    await RefreshData();

                    IsRefreshing = false;
                });
            }
        }
        #endregion

        public AssignedViewModel(INavigationService navigationService):base(navigationService)
        {
            WebServices.SOAP.TaskClient TaskWsClient = new WebServices.SOAP.TaskClient();
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster"; 
            AssignedTasks = TaskWsClient.GetAssignedTasks();
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.AssignedToExecuted"/> view that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async  void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("SelectedTask", selectedTask);
            await _navigationService.NavigateAsync("AssignedToExecuted", Parameters);
        }

        /// <summary>
        /// Refresh the assigned task list view.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task RefreshData()
        {
            await System.Threading.Tasks.Task.Delay(1000);
        }
    }
}
