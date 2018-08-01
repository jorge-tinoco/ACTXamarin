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
        private string _inventoryNumber;
        private int _Plate;
        private string _Descriptioon;

        /// <summary>
        /// Vehicle unique identifier.
        /// </summary>
        [PrimaryKey, MaxLength(6), Column("id"), NotNull]
        public string Identifier
        {
            get { return _Identifier; }
            set { _Identifier = value; }
        }


        /// <summary>
        /// Vehicle plate number.
        /// </summary>
        [ MaxLength(8), Column("plate")]
        public int Plate
        {
            get { return _Plate; }
            set { _Plate = value; }
        }

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

        /// <summary>
        /// Vehicle name showed in select combos. 
        /// </summary>
        [Ignore]
        public string NameToShow {
            get
            {
                if(Plate == 0  )
                    return InventoryNumber + " - "+Description;
                return Plate + " - "+Description; 
            }
        }


        /// <summary>
        /// Inventory number. Similar to <see cref="Plate"/>.
        /// </summary>
        [MaxLength(60), Column("inventory")]
        public string InventoryNumber
        {
            get { return _inventoryNumber; }
            set { _inventoryNumber = value; }
        }

    }
}
