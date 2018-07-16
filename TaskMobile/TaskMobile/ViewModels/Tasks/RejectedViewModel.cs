using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    public class RejectedViewModel : BaseViewModel, INavigatingAware
    {
        private int _taskNumber;
        private string _origin;
        private string _destination;
        private string cameFrom;

        public RejectedViewModel(INavigationService navigationService) : base(navigationService)
        {

        }
        #region COMMANDS

        private DelegateCommand _Other;
        public DelegateCommand OtherCommand =>
            _Other ?? (_Other = new DelegateCommand(ExecuteOtherCommand));

        private DelegateCommand _Finish;
        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(ExecuteFinishCommand));
        #endregion

        #region VIEW MODEL PROPERTIES

        /// <summary>
        /// Rejected task number.
        /// </summary>
        public int TaskNumber
        {
            get { return _taskNumber; }
            set { SetProperty(ref _taskNumber, value); }
        }

        /// <summary>
        /// Where the rejected task  came from.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { SetProperty(ref _origin, value); }
        }

        /// <summary>
        /// Where the rejected task went
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }
        #endregion

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Models.Activity Rejected = parameters["RejectedActivity"] as Models.Activity;
            cameFrom = (string)parameters["ComesFrom"] ;
            TaskNumber = Rejected.Id;
        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.QueryAssigned"/> view for continue working in assigned tasks.
        /// </summary>
        private async void ExecuteOtherCommand()
        {
            if ( cameFrom == "Executed")
                await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/QueryExecuted");
            if ( cameFrom == "Assigned")
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
