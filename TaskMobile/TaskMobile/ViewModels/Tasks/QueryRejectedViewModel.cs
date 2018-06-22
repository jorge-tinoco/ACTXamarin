using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    public class QueryRejectedViewModel : BaseViewModel,INavigatingAware
    {
        public QueryRejectedViewModel(INavigationService navigationService) : base(navigationService)
        {
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster";
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
        }

        #region VIEW MODEL PROPERTIES
        private List<Models.Task> _rejected;
        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public List<Models.Task> RejectedTasks
        {
            get { return _rejected; }
            set
            {
                SetProperty(ref _rejected, value);

            }
        }

        private DateTime _start;
        /// <summary>
        /// Used for filter finished tasks.
        /// </summary>
        public DateTime StartDate
        {
            get { return _start; }
            set
            {
                RefreshCommand.Execute();
                SetProperty(ref _start, value);
            }
        }

        private DateTime _end;
        /// <summary>
        /// Used for filter finished tasks.
        /// </summary>
        public DateTime EndDate
        {
            get { return _end; }
            set
            {
                RefreshCommand.Execute();
                SetProperty(ref _end, value);
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
        #endregion

        #region COMMANDS
        public DelegateCommand<object>
            ToDetailCommand
        { get; private set; }
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


        /// <summary>
        /// Navigate to <see cref="Views.Tasks.ExecutedToFinish"/>  that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskWithDetail", selectedTask);
            await _navigationService.NavigateAsync("RejectedDetail", Parameters);
        }

        /// <summary>
        /// Refresh the assigned task list view.
        /// </summary>
        /// <returns></returns>
        private async System.Threading.Tasks.Task RefreshData()
        {
            await System.Threading.Tasks.Task.Delay(1000);
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            WebServices.SOAP.TaskClient TaskWsClient = new WebServices.SOAP.TaskClient();
            RejectedTasks = TaskWsClient.RejectedTasks();
        }
    }
}
