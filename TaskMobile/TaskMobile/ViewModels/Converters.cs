using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels
{
    /// <summary>
    /// Convert external Models for a local designed model  in  <see cref="Models"/> 
    /// </summary>
    internal static class Converters
    {
        /// <summary>
        /// Translate <see cref="WebServices.Entities.Tasks.Detail"/> to <see cref="Models.TaskDetail"/>
        /// </summary>
        /// <param name="entityToConvert">Entity that contains the model to translate.</param>
        /// <returns>Entity converted</returns>
        private static Models.TaskDetail TaskDetail(WebServices.Entities.Tasks.Detail entityToConvert)
        {
            return new Models.TaskDetail { Lot = entityToConvert.LOTE, Pieces = entityToConvert.PIECES, SapCode = entityToConvert.SYMBOL, StockType = "", TaskNumber = 0, Tons = entityToConvert.TONS, WorkOrder = entityToConvert.OP, Origin = entityToConvert.ORIG_LOCATION, Destination = entityToConvert.DEST_LOCATION };
        }

        /// <summary>
        /// Translate <see cref="WebServices.Entities.Tasks.Detail"/> to <see cref="Models.TaskDetail"/>
        /// </summary>
        /// <param name="taskNumber">Task number for detail to translate.</param>
        /// <param name="stockType">Stock type for detail to translate.</param>
        /// <param name="entityToConvert">Entity that contains the model to translate.</param>
        /// <returns>Entity converted</returns>
        internal static  Models.TaskDetail TaskDetail(WebServices.Entities.Tasks.Detail entityToConvert,int taskNumber,  string stockType)
        {
            return new Models.TaskDetail { Lot = entityToConvert.LOTE, Pieces = entityToConvert.PIECES, SapCode = entityToConvert.SYMBOL, StockType = stockType, TaskNumber = taskNumber, Tons = entityToConvert.TONS, WorkOrder = entityToConvert.OP, Origin = entityToConvert.ORIG_LOCATION, Destination = entityToConvert.DEST_LOCATION };
        }

        /// <summary>
        /// Translate <see cref="WebServices.Entities.Tasks.Task"/> to <see cref="Models.Task"/>
        /// </summary>
        /// <param name="entityToConvert">Entity that contains the model to translate.</param>
        /// <returns>Entity converted</returns>
        internal static Models.Task Task(WebServices.Entities.Tasks.Task entityToConvert)
        {
            var Details = entityToConvert.DETAILS.Select(detail => 
                                                        Converters.TaskDetail(detail, entityToConvert.ID , entityToConvert.TYPE_STOCK ));
            Models.Task Converted = new Models.Task(Details);
            Converted.Number = entityToConvert.ID;
            Converted.Type = entityToConvert.OPERATION_NAME;
            Converted.Origin = entityToConvert.ORIGIN_DEPOSIT;
            Converted.Destination = entityToConvert.DESTINY_DEPOSIT;
            Converted.Remission = entityToConvert.REMISSION;
            Converted.DateTask = entityToConvert.CREATED_DATE;
            return Converted;
        }
    }
}
