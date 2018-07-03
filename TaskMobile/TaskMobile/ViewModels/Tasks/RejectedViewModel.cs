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
        #region Properties
        private int _taskNumber;
        /// <summary>
        /// Rejected task number.
        /// </summary>
        public int TaskNumber
        {
            get { return _taskNumber; }
            set { SetProperty(ref _taskNumber, value); }
        }
        private string _origin;
        /// <summary>
        /// Where the rejected task  came from.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { SetProperty(ref _origin, value); }
        }

        private string _destination;
        /// <summary>
        /// Where the rejected task went
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { SetProperty(ref _destination, value); }
        }
        #endregion

        #region COMMANDS

        private DelegateCommand _Other;
        public DelegateCommand OtherCommand =>
            _Other ?? (_Other = new DelegateCommand(ExecuteOtherCommand));

        private DelegateCommand _Finish;
        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(ExecuteFinishCommand));
        #endregion


        public RejectedViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.Assigned"/> view for continue working in assigned tasks.
        /// </summary>
        private async void ExecuteOtherCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/AssignedTasks");
        }

        /// <summary>
        /// Go to main page.
        /// </summary>
        private async void ExecuteFinishCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        public void OnNavigatingTo(NavigationParameters parameters)
        {
            Models.TaskDetail Rejected = parameters["CanceledTask"] as Models.TaskDetail;
            TaskNumber = Rejected.TaskNumber;
            this.Origin = Rejected.Origin;
            this.Destination = Rejected.Destination;
        }
    }
}
