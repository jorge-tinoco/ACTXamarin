using Prism.Commands;
using Prism.Navigation;
using Prism.Services;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishedViewModel : BaseViewModel, INavigatingAware
    {
        private int _activity;
        private string _origin;
        private string _destination;
        private DelegateCommand _other;
        private DelegateCommand _finish;

        public FinishedViewModel(INavigationService navigationService, IPageDialogService dialogService) 
            : base(navigationService, dialogService)
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
        /// Executed activity number.
        /// </summary>
        public int Activity
        {
            get { return _activity; }
            set { SetProperty(ref _activity, value); }
        }

        /// <summary>
        /// Origin warehouse.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { SetProperty(ref _origin, value); }
        }

        /// <summary>
        /// Destination warehouse.
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }
        #endregion

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.QueryAssigned"/> view for continue working in assigned tasks.
        /// </summary>
        private async void ExecuteOtherCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/QueryExecuted");
        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void ExecuteFinishCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        /// <summary>
        /// Getting the finished task.
        /// </summary>
        /// <param name="parameters">Finished task.</param>
        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
            var finished = parameters["ActivityFinished"] as Models.Activity;
            var task = parameters["CurrentTask"] as Models.Task;
            if (task != null)
            {
                Origin = task.Origin;
                Destination = task.Destination;
            }
            if (finished != null)
                Activity = finished.Id;
        }
    }
}
