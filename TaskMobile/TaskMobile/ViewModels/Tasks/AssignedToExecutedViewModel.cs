using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TaskMobile.ViewModels.Tasks
{
   /// <summary>
   /// View model representing <see cref="Views.Tasks.AssignedToExecuted"/> view.
   /// </summary>
    public class AssignedToExecutedViewModel : BaseViewModel, INavigationAware
    {
        INavigationService _navigationService;

        /// <summary>
        /// Constructor that implements navigation service.
        /// </summary>
        /// <param name="navigationService">Navigation service.</param>
        public AssignedToExecutedViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            Driver = "Jorge Tinocos";
            Vehicle = "Hyster";
        }

        void INavigatedAware.OnNavigatedFrom(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executed when navigated from other page. In this case, the request comes from <see cref="ViewModels.TaskViewModel"/>
        /// </summary>
        /// <param name="parameters">Selected <see cref="Models.Task"/> passed as parameter.</param>
        void INavigatedAware.OnNavigatedTo(NavigationParameters parameters)
        {
            Models.Task Selected = parameters["SelectedTask"] as Models.Task;
            Details = Selected.Details.ToList();
        }

        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
           
        }
        

        private List<Models.TaskDetail> _Details;
        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public List<Models.TaskDetail> Details
        {
            get { return _Details; }
            set
            {
                SetProperty(ref _Details, value);
            }
        }
    }
}
