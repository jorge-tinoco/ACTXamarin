using System.Collections.Generic;

namespace TaskMobile.WebServices.Entities
{
    public class VehicleResponse
    {
        public VehicleResponse()
        {
            VehicleListResult = new List<VehicleListResult>();
        }
        public List<VehicleListResult> VehicleListResult { get; set; }
    }
}
