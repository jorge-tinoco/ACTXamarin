using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represents supported mills for this application.
    /// </summary>
    public class Mill
    {
        private string _key;
        /// <summary>
        /// Mill identifier
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }


        private string _value;
        /// <summary>
        /// Mill name.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

    }
}
