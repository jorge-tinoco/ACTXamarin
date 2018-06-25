using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMobile.Models;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.SOAP
{
    /// <summary>
    /// Vehicle web service implementation.
    /// TODO: connect vehicle web services and implement web services actions.
    /// </summary>
    internal class VehicleClient
    {

        /// <summary>
        /// Get all availabe vehicles.
        /// </summary>
        /// <returns>Collection of free vehicles.</returns>
        internal Task<IEnumerable<Vehicle>> FreeToUse()
        {
            Task<IEnumerable<Vehicle>> task = new Task<IEnumerable<Vehicle>>(obj =>
            {
                Vehicle Vehicle = new Vehicle { Identifier = "A1", Description = "Vehículo nuevo" , Plate = 66323  };
                Vehicle[] vehicles = new Vehicle[] { Vehicle };
                return vehicles;
            }, 300);
            task.Start();
            return task;
        }
    }
}
