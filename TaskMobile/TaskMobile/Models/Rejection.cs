using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Reason for reject an  activity.
    /// </summary>
    [Table("rejections")]
    public class Rejection
    {
        private int _id;
        private int _number;
        private string _reason;

        public Rejection()
        {

        }

        public Rejection(string reason, int number = 0)
        {
            _reason = reason;
            _number = number;
        }

        /// <summary>
        /// Reason identifier
        /// </summary>
        [PrimaryKey, MaxLength(6), Column("id"), NotNull, AutoIncrement]
        public int Identifier
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Reason description.
        /// </summary>
        [MaxLength(60), Column("reason"), Indexed]
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }

        /// <summary>
        /// Custom number set by the user to identify the reasons. 
        /// </summary>
        [MaxLength(6), Column("number")]
        public int Number
        {
            get { return _number; }
            set { _number = value; }
        }

    }
}
