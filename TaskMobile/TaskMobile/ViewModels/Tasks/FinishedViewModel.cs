using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishedViewModel : BaseViewModel, INavigatingAware
    {
        private int _TaskNumber;
        private DelegateCommand _Other;
        private DelegateCommand _Finish;

        public FinishedViewModel(INavigationService navigationService, IPageDialogService dialogService) 
            : base(navigationService, dialogService)
        {

        }

        #region COMMANDS

        public DelegateCommand OtherCommand =>
            _Other ?? (_Other = new DelegateCommand(ExecuteOtherCommand));

        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(ExecuteFinishCommand));
        #endregion

        #region VIEW MODEL PROPERTIES

        /// <summary>
        /// Executed task number.
        /// </summary>
        public int TaskNumber
        {
            get { return _TaskNumber; }
            set { SetProperty(ref _TaskNumber, value); }
        }
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
            Models.Activity Finished = parameters["ActivityFinished"] as Models.Activity;
            TaskNumber = Finished.Id;
        }
    }
}
