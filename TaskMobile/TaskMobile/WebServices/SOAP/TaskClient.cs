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
        internal List<Models.Task> GetAssignedTasks()
        {
            List<Models.Task> Assigned = new List<Models.Task>();
            Models.TaskDetail GD = new Models.TaskDetail { WorkOrder = "232395", Lot = "4560", Pieces = 45, SapCode = "2300456", Tons = 34.062, StockType = "COPLE", TaskNumber = 234 };
            Assigned.Add(new Models.Task(GD) { Number = 234, Origin = "ALCO", Destination = "FAT2", Remission = 13345, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Assigned.Add(new Models.Task(GD) {  Number =235, Origin = "ALCO", Destination ="PRE2", Remission = 13346, Type ="Movimiento de Material", DateTask = DateTime.Now  });
            Assigned.Add(new Models.Task { Number = 300, Origin = "FAT2", Destination ="FAT1", Remission = 13347, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 345, Origin = "FAT2", Destination ="FAT1", Remission = 13348, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 299, Origin = "FAT2", Destination ="FAT1", Remission = 13349, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination ="FAT1", Remission = 13350, Type ="Movimiento de Material", DateTask = DateTime.Now});
            Assigned.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination = "FAT1", Remission = 13351, Type ="Movimiento de Material", DateTask = DateTime.Now});
            return Assigned;
        }

        /// <summary>
        /// Use for getting all executed tasks
        /// </summary>
        /// <returns>Collection of executed <see cref="Models.Task"/> .</returns>
        internal List<Models.Task> GetExecutedTasks()
        {
            List<Models.Task> Executed = new List<Models.Task>();
            Models.TaskDetail GD = new Models.TaskDetail { WorkOrder = "232395", Lot = "4560", Pieces = 45, SapCode = "2300456", Tons = 34.062, StockType = "COPLE", TaskNumber = 234 };
            Executed.Add(new Models.Task(GD) { Number = 234, Origin = "PRE2", Destination = "FAT2", Remission = 13345, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Executed.Add(new Models.Task(GD) { Number = 235, Origin = "FAT1", Destination = "PRE2", Remission = 13346, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Executed.Add(new Models.Task { Number = 300, Origin = "FAT3", Destination = "FAT1", Remission = 13347, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Executed.Add(new Models.Task { Number = 345, Origin = "FAT2", Destination = "FAT1", Remission = 13348, Type = "Movimiento de Coples", DateTask = DateTime.Now });
            Executed.Add(new Models.Task { Number = 299, Origin = "FAT1", Destination = "FAT1", Remission = 13349, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Executed.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination = "FAT1", Remission = 13350, Type = "Movimiento de TUBOS", DateTask = DateTime.Now });
            Executed.Add(new Models.Task { Number = 23, Origin = "FAT1", Destination = "FAT1", Remission = 13351, Type = "Movimiento de Material", DateTask = DateTime.Now });
            return Executed;
        }

        /// <summary>
        /// Get finished tasks from web service.
        /// </summary>
        /// <returns>Collection of current finished <see cref="Models.Task"/> .</returns>
        internal List<Models.Task> FinishedTasks()
        {
            List<Models.Task> Finished = new List<Models.Task>();
            Models.TaskDetail GD = new Models.TaskDetail { WorkOrder = "232395", Lot = "4560", Pieces = 45, SapCode = "2300456", Tons = 34.062, StockType = "COPLE", TaskNumber = 234 };
            Finished.Add(new Models.Task(GD) { Number = 234, Origin = "PRE2", Destination = "FAT3", Remission = 13345, Type = "Finalización de Material", DateTask = DateTime.Now });
            Finished.Add(new Models.Task(GD) { Number = 235, Origin = "ALCO", Destination = "ALCO", Remission = 13346, Type = "Encerrado de Material", DateTask = DateTime.Now });
            Finished.Add(new Models.Task { Number = 300, Origin = "FAT3", Destination = "FAT1", Remission = 13347, Type = "Fin de Material", DateTask = DateTime.Now });
            Finished.Add(new Models.Task { Number = 345, Origin = "FAT1", Destination = "FAT1", Remission = 13348, Type = "Test de Coples", DateTask = DateTime.Now });
            Finished.Add(new Models.Task { Number = 299, Origin = "ALCO", Destination = "FAT3", Remission = 13349, Type = "Movimiento de Material", DateTask = DateTime.Now });
            Finished.Add(new Models.Task { Number = 23, Origin = "FAT2", Destination = "FAT1", Remission = 13350, Type = "DUMMIE de TUBOS", DateTask = DateTime.Now });
            Finished.Add(new Models.Task { Number = 23, Origin = "ALCO", Destination = "FAT3", Remission = 13351, Type = "Movimiento de Material", DateTask = DateTime.Now });
            return Finished;
        }
    }
}
