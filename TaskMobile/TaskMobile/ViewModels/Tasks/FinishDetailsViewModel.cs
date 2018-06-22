using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    public class FinishDetailsViewModel : BaseViewModel, INavigatingAware
    {
        public FinishDetailsViewModel(INavigationService navigationService) : base(navigationService)
        {

        }


        #region  VIEW MODEL PROPERTIES

        private List<Models.TaskDetail> _Details;
        /// <summary>
        /// Current finished  tasks details.
        /// </summary>
        public List<Models.TaskDetail> Details
        {
            get { return _Details; }
            set
            {
                SetProperty(ref _Details, value);
            }
        }

        private bool _isRefreshing = false;
        /// <summary>
        /// Flag for stablish when the list view is refreshing.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set
            {
                SetProperty(ref _isRefreshing, value);
            }
        }

        #endregion
        #region  COMMANDS
        //Command implementations goes here.
        private DelegateCommand _Finish;
        public DelegateCommand FinishCommand =>
            _Finish ?? (_Finish = new DelegateCommand(ExecuteFinishCommand));

        #endregion


        /// <summary>
        /// Refresh detail  task list.
        /// </summary>
        /// <returns></returns>
        private async Task RefreshData()
        {
            await Task.Delay(1000);
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
            Models.Task Selected = parameters["TaskWithDetail"] as Models.Task;
            Details = Selected.Details.ToList();
        }
    }
}
