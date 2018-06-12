using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represents a single task.
    /// </summary>
    public class Task
    {
        private int _Number;
        /// <summary>
        /// Task number.
        /// </summary>
        public int Number
        {
            get { return _Number; }
            set { _Number = value; }
        }


        private string _Type;
        /// <summary>
        /// Task type.
        /// </summary>
        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }


        private int _Remission;
        /// <summary>
        /// Remission number assigned to this task.
        /// </summary>
        public int Remission
        {
            get { return _Remission; }
            set { _Remission = value; }
        }

        private string _Origin;
        /// <summary>
        /// Task origin.
        /// </summary>
        public string Origin
        {
            get { return _Origin; }
            set { _Origin = value; }
        }

        private string _Destination;
        /// <summary>
        /// Task destination.
        /// </summary>
        public string Destination
        {
            get { return _Destination; }
            set { _Destination = value; }
        }

        private DateTime _DateTask;
        /// <summary>
        /// Task date.
        /// </summary>
        public DateTime DateTask
        {
            get { return _DateTask; }
            set { _DateTask = value; }
        }

    }
}
