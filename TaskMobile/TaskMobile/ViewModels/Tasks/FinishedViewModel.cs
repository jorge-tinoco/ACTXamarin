using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishedViewModel : BaseViewModel, INavigatingAware
    {
        public FinishedViewModel(INavigationService navigationService) : base(navigationService)
        {

        }

        #region VIEW MODEL PROPERTIES
        private int _TaskNumber;
        /// <summary>
        /// Executed task number.
        /// </summary>
        public int TaskNumber
        {
            get { return _TaskNumber; }
            set { SetProperty(ref _TaskNumber, value); }
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

        /// <summary>
        /// Navigate to <see cref="Views.Tasks.Assigned"/> view for continue working in assigned tasks.
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
            Models.TaskDetail Executed = parameters["FinishedTask"] as Models.TaskDetail;
            TaskNumber = Executed.TaskNumber;
        }
    }
}
