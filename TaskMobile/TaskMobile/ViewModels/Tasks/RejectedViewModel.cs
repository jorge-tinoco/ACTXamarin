using Prism.Commands;
using Prism.Navigation;

namespace TaskMobile.ViewModels.Tasks
{
    public class RejectedViewModel : BaseViewModel, INavigatingAware
    {
        private int _activity;
        private string _origin;
        private string _destination;
        private string _cameFrom;
        private DelegateCommand _other;
        private DelegateCommand _finish;

        public RejectedViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        #region COMMANDS

        public DelegateCommand OtherCommand =>
            _other ?? (_other = new DelegateCommand(ExecuteOtherCommand));

        public DelegateCommand FinishCommand =>
            _finish ?? (_finish = new DelegateCommand(ExecuteFinishCommand));
        #endregion

        #region VIEW MODEL PROPERTIES

        /// <summary>
        /// Rejected activity number.
        /// </summary>
        public int Activity
        {
            get { return _activity; }
            set { SetProperty(ref _activity, value); }
        }

        /// <summary>
        /// Where the rejected task/activity  came from.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { SetProperty(ref _origin, value); }
        }

        /// <summary>
        /// Where the rejected task/activity went
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }
        #endregion

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            _cameFrom = (string)parameters["ComesFrom"] ;
            var rejected = parameters["RejectedActivity"] as Models.Activity;
            var task = parameters["CurrentTask"] as Models.Task;
            if (task != null)
            {
                Origin = task.Origin;
                Destination = task.Destination;
            }
            if (rejected != null) Activity = rejected.Id;
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.QueryAssigned"/> view for continue working in assigned tasks.
        /// </summary>
        private async void ExecuteOtherCommand()
        {
            if (_cameFrom == "Executed")
                await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/QueryExecuted");
            if (_cameFrom == "Assigned")
                await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/QueryAssigned");

        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void ExecuteFinishCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

    }
}
