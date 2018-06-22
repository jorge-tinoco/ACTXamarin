using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Application  supported languages.
    /// </summary>
    public class Language
    {
        private string _key;
        /// <summary>
        /// Language identifier.
        /// </summary>
        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _value;
        /// <summary>
        /// Language description.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _shortName;
        /// <summary>
        /// Language short name.
        /// </summary>
        public string ShortName
        {
            get { return _shortName; }
            set { _shortName = value; }
        }

    }
}
