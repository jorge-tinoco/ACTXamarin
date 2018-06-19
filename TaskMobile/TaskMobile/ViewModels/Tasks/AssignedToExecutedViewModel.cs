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
    public class AssignedToExecutedViewModel : BindableBase, INavigationAware
    {
        INavigationService _navigationService;

        private int _SelectedTask;
        public int SelectedTask
        {
            get { return _SelectedTask; }
            set { SetProperty(ref _SelectedTask, value); }
        }
        
        public AssignedToExecutedViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        void INavigatedAware.OnNavigatedFrom(NavigationParameters parameters)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Executed when navigated from other page. In this case, the request comes from <see cref="ViewModels.TaskViewModel"/>
        /// </summary>
        /// <param name="parameters">Passed parameters.</param>
        void INavigatedAware.OnNavigatedTo(NavigationParameters parameters)
        {
            
            Models.Task Selected = parameters["SelectedTask"] as Models.Task;
            SelectedTask = Selected.Number;
        }

        void INavigatingAware.OnNavigatingTo(NavigationParameters parameters)
        {
           
        }
    }
}
