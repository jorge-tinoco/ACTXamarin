using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represents the current driver/user.
    /// </summary>
    /// <remarks>
    /// This model also represents "vehicle" table in SQLite database.
    /// </remarks>
    [Table("driver")]
    public class Driver
    {
        private int _Identifier;
        /// <summary>
        /// Unique driver identifier.
        /// </summary>
        [AutoIncrement, PrimaryKey, MaxLength(6), Column("id"), NotNull]
        public int Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }


        private string _User;
        /// <summary>
        /// User driver. This  data comes from AD(Active Directory).
        /// </summary>
        [ MaxLength(8), Column("user")]
        public string User
        {
            get { return _User; }
            set { _User = value; }
        }


        private string _Name;
        /// <summary>
        /// Driver name.
        /// </summary>
        [MaxLength(60), Column("name")]
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

    }
}
