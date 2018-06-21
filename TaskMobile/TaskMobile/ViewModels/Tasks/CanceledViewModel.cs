using System;
using Prism.Navigation;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// Not used yet.
    /// </summary>
    public class CanceledViewModel : BaseViewModel, INavigatingAware
    {
        private int _TaskNumber;
        public int TaskNumber
        {
            get { return _TaskNumber; }
            set { SetProperty(ref _TaskNumber, value); }
        }
        public CanceledViewModel(INavigationService navigationService): base(navigationService)
        {

        }

        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
            Models.TaskDetail Canceled = parameters["CanceledTask"] as Models.TaskDetail;
            TaskNumber = Canceled.TaskNumber;
        }
    }
}
