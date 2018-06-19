using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represents the current driver/user.
    /// </summary>
    public class Driver
    {
        private int _Identifier;
        /// <summary>
        /// Unique driver identifier.
        /// </summary>
        public int Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }


        private string _User;
        /// <summary>
        /// User driver. This  data comes from AD(Active Directory).
        /// </summary>
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }


        private string _Name;
        /// <summary>
        /// Driver name.
        /// </summary>
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
}
