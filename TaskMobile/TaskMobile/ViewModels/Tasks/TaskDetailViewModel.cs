using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels.Tasks
{
    /// <summary>
    /// Represents the <see cref="Models.TaskDetail"/> model.
    /// </summary>
    /// <remarks>
    /// For exposing public properties of a model and some other properties that are useful for presenting , we need a ViewModel for each Model.
    /// </remarks>
    public class TaskDetailViewModel
    {
        private Models.TaskDetail _detail;

        public TaskDetailViewModel(Models.TaskDetail detail)
        {
            this._detail = detail;
        }


        public string Origin { get { return _detail.Origin; } }
        public string Destination { get { return _detail.Destination; } }
        public string Lot { get { return _detail.Lot; } }
        public string SapCode { get { return _detail.SapCode; } }
        public string WorkOrder { get { return _detail.WorkOrder; } }
        public int Pieces { get { return _detail.Pieces; } }
        public double Tons { get { return _detail.Tons; } }
    }
}
