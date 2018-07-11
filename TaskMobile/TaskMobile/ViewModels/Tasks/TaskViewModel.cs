using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// Represents the <see cref="Models.Task"/> model.
    /// </summary>
    /// <remarks>
    /// For exposing public properties of a model and some other properties that are useful for presenting , we need a ViewModel for each Model.
    /// </remarks>
    public class TaskViewModel: ObservableCollection<TaskDetailViewModel>, INotifyPropertyChanged
    {
        private Models.Task _task;

        // It's a backup variable for storing TaskDetailViewModel objects
        private ObservableCollection<TaskDetailViewModel> Details = new ObservableCollection<TaskDetailViewModel>();

        public TaskViewModel(Models.Task task, bool expanded = false)
        {
            this._task = task;
            this._expanded = expanded;
            foreach (Models.TaskDetail Detail in task.Details)
            {
                Details.Add(new TaskDetailViewModel(Detail));
            }
            if (expanded)
            {
                foreach (TaskDetailViewModel DetailViewModel in Details)
                {
                    this.Add(DetailViewModel);
                }
            }
        }

        public void Add(Models.TaskDetail detailToAdd)
        {
            TaskDetailViewModel DetailViewModel = new TaskDetailViewModel(detailToAdd);
            Details.Add(DetailViewModel);
        }

        public void Add(IEnumerable<Models.TaskDetail> detailsToAdd)
        {
            foreach (var detailToAdd in detailsToAdd)
            {
                TaskDetailViewModel DetailViewModel = new TaskDetailViewModel(detailToAdd);
                Details.Add(DetailViewModel);
            }
        }

        public new void Clear()
        {
            Details.Clear();
            base.Clear();
        }

        public int Number { get { return _task.Number; } }
        public string Type { get { return _task.Type; } }
        public string Origin { get { return _task.Origin; } }
        public string Destination { get { return _task.Destination; } }
        public string Remission { get { return _task.Remission; } }
        public char Status { get { return _task.Status; } }

       
        public DateTime DateTask {
            get
            {
                return _task.DateTask;
            }
        }
        


        public string StateIcon { get { return Expanded ? UserControls.Icon.ChevronUp : UserControls.Icon.ChevronDown; } }

        private bool _expanded;

        public bool Expanded
        {
            get { return _expanded; }
            set
            {
                if (_expanded != value)
                {
                    _expanded = value;
                    OnPropertyChanged(new PropertyChangedEventArgs("Expanded"));
                    OnPropertyChanged(new PropertyChangedEventArgs("StateIcon"));
                    if (_expanded)
                    {
                        foreach (var detailToAdd in Details)
                        {
                            this.Add(detailToAdd);
                        }
                    }
                    else
                        this.Clear();
                }
            }
        }

    }
}
