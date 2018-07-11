using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace TaskMobile.Models
{
    /// <summary>
    /// Partial class that implements the <see cref="ObservableCollection{T}"/> class.
    /// </summary>
    public partial class Task : ObservableCollection<TaskDetail>, INotifyPropertyChanged
    {
        // Backup variable for storing TaskDetailViewModel objects
        private ObservableCollection<TaskDetail> Details = new ObservableCollection<TaskDetail>();
        private bool _expanded;

        public Task()
        {
        }

        /// <summary>
        /// Initializes details list and add the passed detail.
        /// </summary>
        /// <param name="Detail">Detail to add.</param>
        public Task(TaskDetail Detail)
        {
            Details.Add(Detail);
        }


       /// <summary>
       /// Specify the button to show when task details are available.
       /// </summary>
        public string StateIcon
        {
            get
            {
                return Expanded ? UserControls.Icon.ChevronUp : UserControls.Icon.ChevronDown;
            }
        }


        /// <summary>
        /// When true, show the activities details. When false, hide the details.
        /// </summary>
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
                        foreach (TaskDetail detailToAdd in Details)
                        {
                            base.Add(detailToAdd); // add detail  to the principal  ObservableColletion
                        }
                    }
                    else
                        this.Clear();
                }
            }
        }

        /// <summary>
        /// Add one <see cref="TaskDetail"/> to the backup field that store the task details.
        /// </summary>
        /// <param name="detailToAdd">Entity to add in the collection</param>
        public new void Add(Models.TaskDetail detailToAdd)
        {
            Details.Add(detailToAdd);
        }

        /// <summary>
        /// Add a set of <see cref="TaskDetail"/> to the backup fild that store the task details.
        /// </summary>
        /// <param name="collectionToAdd">Details to add</param>
        public void Add(IEnumerable<Models.TaskDetail> collectionToAdd)
        {
            foreach (var detail in collectionToAdd)
            {
                Details.Add(detail);
            }
        }

        /// <summary>
        /// Clear the backup field that store the task details.
        /// </summary>
        public new void Clear()
        {
            Details.Clear();
            base.Clear();
        }
        
    }
}
