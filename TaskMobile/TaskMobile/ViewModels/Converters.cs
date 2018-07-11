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
            return new Models.TaskDetail {
                Lot = entityToConvert.LOTE,
                Pieces = entityToConvert.PIECES,
                SapCode = entityToConvert.SYMBOL,
                StockType = "",
                TaskNumber = 0,
                Tons = entityToConvert.TONS,
                WorkOrder = entityToConvert.OP,
                Origin = entityToConvert.ORIG_LOCATION,
                Destination = entityToConvert.DEST_LOCATION };
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
            return new Models.TaskDetail {
                Lot = entityToConvert.LOTE,
                Pieces = entityToConvert.PIECES,
                SapCode = entityToConvert.SYMBOL,
                StockType = stockType,
                TaskNumber = taskNumber,
                Tons = entityToConvert.TONS,
                WorkOrder = entityToConvert.OP,
                Origin = entityToConvert.ORIG_LOCATION,
                Destination = entityToConvert.DEST_LOCATION };
        }

        /// <summary>
        /// Translate <see cref="WebServices.Entities.Tasks.Task"/> to <see cref="Models.Task"/>
        /// </summary>
        /// <param name="entityToConvert">Entity that contains the model to translate.</param>
        /// <returns>Entity converted</returns>
        internal static Models.Task Task(WebServices.Entities.Tasks.Task entityToConvert)
        {
            return new Models.Task()
            {
                Number = entityToConvert.ID,
                Type = entityToConvert.OPERATION_NAME,
                Origin = entityToConvert.ORIGIN_DEPOSIT,
                Destination = entityToConvert.DESTINY_DEPOSIT,
                Remission = entityToConvert.REMISSION,
                DateTask = entityToConvert.CREATED_DATE };
        }

        /// <summary>
        /// Translate <see cref="WebServices.Entities.Tasks.Activity"/> to a local model (<see cref="Models.Activity"/> ).
        /// </summary>
        /// <param name="activityToConvert">Web service entity to convert.</param>
        /// <returns>Local model translated.</returns>
        internal static Models.Activity Activity(WebServices.Entities.Tasks.Activity activityToConvert)
        {
            return new Models.Activity()
            {
                Id = activityToConvert.ID,
                Name = activityToConvert.TASKTYPE_NAME,
                Order = activityToConvert.ORDER,
                VehicleAlias = activityToConvert.VEHICLETYPE_ALIAS,
                VehicleNumber = activityToConvert.VEHICLE_NUMBER,
                Status = activityToConvert.STATUS  };
        }
    }
}
