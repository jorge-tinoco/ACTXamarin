using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskMobile.Models;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace TaskMobile.ViewModels
{
    public class TaskViewModel : BaseViewModel
    {
        public TaskViewModel()
        {
            WebServices.SOAP.TaskClient TaskWsClient = new WebServices.SOAP.TaskClient();
            Driver = "Jorge Tinoco";
            Vehicle = "Hyster";
            AssignedTasks = TaskWsClient.GetAssignedTasks().ToList();
        }


        private string _Driver;
        public string Driver
        {
            get { return _Driver; }
            set { _Driver = value;
                OnPropertyChanged("Driver");
            }
        }

        private string _Vehicle;
        public string Vehicle
        {
            get { return _Vehicle; }
            set { _Vehicle = value;
                OnPropertyChanged("Vehicle");
            }
        }

        private List<Models.Task> _AssignedTasks;

        public List<Models.Task> AssignedTasks
        {
            get { return _AssignedTasks; }
            set {
                _AssignedTasks = value;
                OnPropertyChanged("AssignedTasks");
            }
        }


      


     
    }
}
