using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskMobile.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Prism.Mvvm;
using Prism.Commands;
using Prism.Navigation;

namespace TaskMobile.ViewModels
{
    /// <summary>
    /// View model representing <see cref="Views.Tasks.Assigned"/> view.
    /// </summary>
    public class TaskViewModel : BindableBase
    {
        INavigationService _navigationService;
       
        public DelegateCommand<object>ToDetailCommand { get; private set; }

        public TaskViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            WebServices.SOAP.TaskClient TaskWsClient = new WebServices.SOAP.TaskClient();
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster"; 
            AssignedTasks = TaskWsClient.GetAssignedTasks().ToList();
            ToDetailCommand = new DelegateCommand<object>(GoToAction);
        }

        private async  void GoToAction(object selectedTask)
        {
            NavigationParameters Parameters = new NavigationParameters();
            Parameters.Add("SelectedTask", selectedTask);
            await _navigationService.NavigateAsync("AssignedToExecuted", Parameters);
        }


        private string _Driver;
        /// <summary>
        /// Current driver.
        /// </summary>
        public string Driver
        {
            get { return _Driver; }
            set {
                SetProperty(ref _Driver, value);
            }
        }

        private string _Vehicle;
        /// <summary>
        /// Represents current Vehicle.
        /// </summary>
        public string Vehicle
        {
            get { return _Vehicle; }
            set { _Vehicle = value;
                SetProperty(ref _Vehicle, value);
            }
        }

        private List<Models.Task> _AssignedTasks;
        /// <summary>
        /// Current pending/assigned  tasks.
        /// </summary>
        public List<Models.Task> AssignedTasks
        {
            get { return _AssignedTasks; }
            set {
                SetProperty(ref _AssignedTasks, value);

            }
        }
    }
}
