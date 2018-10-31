namespace TaskMobile.Models
{
    /// <summary>
    /// Represents activity entity associated to a <see cref="Models.Task"/>.
    /// </summary>
    public class Activity
    {
        private string _status;

        /// <summary>
        /// Activity identifier;
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Work order
        /// </summary>
        public int Order { get; set; }

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
                    case "P":
                        _status = "Pendiente";
                        break;
                    case "E":
                        _status = "Ejecutada";
                        break;
                    case "R":
                        _status = "Rechazada";
                        break;
                    case "A":
                        _status = "Asignada";
                        break;
                    case "F":
                        _status = "Finalizada";
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
        public string Name { get; set; }

        /// <summary>
        /// Vehicle alias.
        /// </summary>
        public string VehicleAlias { get; set; }

        /// <summary>
        /// Vehicle number.
        /// </summary>
        public string VehicleNumber { get; set; }
    }
}
