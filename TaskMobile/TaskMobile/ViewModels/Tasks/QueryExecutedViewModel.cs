using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    public class QueryExecutedViewModel : BaseViewModel, INavigatingAware
    {
        public QueryExecutedViewModel(INavigationService navigationService) : base(navigationService)
        {
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster";
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
        }

        #region VIEW MODEL PROPERTIES
        private List<Models.Task> _ExecutedTasks;
        /// <summary>
        /// Current executed  tasks.
        /// </summary>
        public List<Models.Task> ExecutedTasks
        {
            get { return _ExecutedTasks; }
            set
            {
                SetProperty(ref _ExecutedTasks, value);

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
            ToDetailCommand { get; private set; }
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
        /// Navigate to <see cref="Views.Tasks.ExecutedToFinish"/> view that shows details of selected task.
        /// </summary>
        /// <param name="selectedTask">Selected task by the user.</param>
        private async void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("TaskToFinish", selectedTask);
            await _navigationService.NavigateAsync("ExecutedToFinish", Parameters);
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
            ExecutedTasks = TaskWsClient.GetExecutedTasks();
        }
    }
}
