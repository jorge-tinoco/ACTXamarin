using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.WebServices.REST
{
    public class Vehicles
    {
        /// <summary>
        /// TMAP route.
        /// </summary>
        private string _route;
        /// <summary>
        /// Data send in TMAP requests.
        /// </summary>
        private string _data;
        /// <summary>
        /// Backfield for web service client.
        /// </summary>
        private readonly IClient _service;


        public Vehicles(IClient webService)
        {
            _service = webService;
        }

        /// <summary>
        /// Return existing vehicles in one specific tenaris system.
        /// </summary>
        /// <param name="success">Success callback.</param>
        /// <param name="error">Error callback.</param>
        /// <param name="system">Tenaris system where the web service will seek.</param>
        public void All( Action<IEnumerable<Models.Vehicle>> success, Action<object> error, string system = "ACT")
        {
            _route = "GetVehicles";
            IEnumerable<Models.Vehicle> allVehicles;
            var request = new Request<Vehicle>
            {
                MessageBody =
                {
                    SystemId = system,
                    User = "DUMMY"
                }
            };
            _data = JsonConvert.SerializeObject(request);
            _service.Post<Response<VehicleResponse>>(_route, _data,
                response =>
                {
                      
                    double code = response.MessageLog.ProcessingResultCode;
                    List<VehicleListResult> vehicles = response.MessageBody.VehicleListResult;
                    if (code.Equals(0) && vehicles.Any())
                    {
                        allVehicles = vehicles.Select(ViewModels.Converters.Vehicle);
                        success(allVehicles);
                    }
                    else
                    {
                        error(new Exceptions.WttException("No se encontraron vehículos"));
                    }
                }, error);
        }
    }
}
