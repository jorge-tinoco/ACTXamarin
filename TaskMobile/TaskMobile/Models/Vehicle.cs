using SQLite;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represents vehicle entity.
    /// </summary>
    /// <remarks>
    /// This model also represents "vehicle" table in SQLite database.
    /// </remarks>
    [Table("vehicle")]
    public class Vehicle
    {
        private string _Identifier;
        /// <summary>
        /// Vehicle unique identifier.
        /// </summary>
        [PrimaryKey, MaxLength(6), Column("id"), NotNull]
        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }


        private int _Plate;
        /// <summary>
        /// Vehicle plate number.
        /// </summary>
        [ MaxLength(8), Column("plate")]
        public int Plate
        {
            get { return _Plate; }
            set { _Plate = value; }
        }

        private string _Descriptioon;
        /// <summary>
        /// Brief vehicle description.
        /// </summary>
        /// <remarks>Maximyn 60 characters.</remarks>
        [MaxLength(60), Column("description")]
        public string Description
        {
            get { return _Descriptioon; }
            set { _Descriptioon = value; }
        }

    }
}
