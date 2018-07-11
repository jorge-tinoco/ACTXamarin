namespace TaskMobile.Models
{
    /// <summary>
    /// Represents activity entity associated to a <see cref="Models.Task"/>.
    /// </summary>
    public class Activity
    {
        private int _id;
        private int _order;
        private string _status;
        private string _name;
        private string _alias;
        private string _number;

        /// <summary>
        /// Activity identifier;
        /// </summary>
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// Work order
        /// </summary>
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }

        /// <summary>
        /// Activity status.
        /// </summary>
        public string Status
        {
            get { return _status; }
            set
            {
                switch (value)
                {
                    case "E":
                        _status = "Ejecutada";
                        break;
                    case "R":
                        _status = "Rechazada";
                        break;
                    case "A":
                        _status = "Asignada";
                        break;
                    default:
                        _status = "No reconocido";
                        break;
                }
            }
        }

        /// <summary>
        /// Activity name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Vehicle alias.
        /// </summary>
        public string VehicleAlias
        {
            get { return _alias; }
            set { _alias = value; }
        }

        /// <summary>
        /// Vehicle number.
        /// </summary>
        public string VehicleNumber
        {
            get { return _number; }
            set { _number = value; }
        }
    }
}
