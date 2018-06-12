using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.SOAP
{

    internal class TaskClient
    {

        /// <summary>
        /// Use for getting all assigned tasks
        /// </summary>
        /// <returns>Collection of <see cref="Models.Task"/> assigned.</returns>
        internal ICollection<Models.Task> GetAssignedTasks()
        {
            IList<Models.Task> Assigned = new List<Models.Task>();
            Assigned.Add(new Models.Task {  Number =23, Origin = "ALCO", Destination = "FAT2", Remission = 13345, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task {  Number =23, Origin = "ALCO", Destination ="PRE2", Remission = 13346, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination ="FAT1", Remission = 13347, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination ="FAT1", Remission = 13348, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination ="FAT1", Remission = 13349, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination ="FAT1", Remission = 13350, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination = "FAT1", Remission = 13351, Type ="Movimiento de Material", DateTask = DateTime.Now});
            return Assigned;
        }
    }
}
