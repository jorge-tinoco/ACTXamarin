using Prism.Commands;
using Prism.Navigation;
using System;

namespace TaskMobile.ViewModels.Tasks
{
    public class ExecutedViewModel : BaseViewModel, INavigatingAware
    {

        private int _taskNumber;
        private string _origin;
        private string _destination;
        private DelegateCommand _other;
        private DelegateCommand _finish;

        public ExecutedViewModel(INavigationService navigationService):base (navigationService)
        {
        }

        #region Commands
        public DelegateCommand OtherCommand =>
            _other ?? (_other = new DelegateCommand(OtherActivity));

        public DelegateCommand FinishCommand =>
            _finish ?? (_finish = new DelegateCommand(JobDone));
        #endregion


        #region VIEW MODEL PROPERTIES
        /// <summary>
        /// Executed task number.
        /// </summary>
        public int TaskNumber
        {
            get { return _taskNumber; }
            set { SetProperty(ref _taskNumber, value); }
        }

        /// <summary>
        /// Where the executed tasks come from
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { SetProperty(ref _origin, value); }
        }

        /// <summary>
        /// Where the executed task goes 
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }
        #endregion

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            var Executed = parameters["ExecutedActivity"] as Models.Activity;
            TaskNumber = Executed.Id;
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.QueryAssigned"/> view for continue working in assigned tasks.
        /// </summary>
        private async void  OtherActivity()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/QueryAssigned");
        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void JobDone()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

    }
}
