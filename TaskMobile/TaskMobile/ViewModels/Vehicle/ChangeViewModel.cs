using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.Models;
using TaskMobile.WebServices.REST;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.ViewModels.Vehicle
{
    public class ChangeViewModel : BaseViewModel, INavigatingAware
    {
        IPageDialogService _dialogService;
        public ChangeViewModel(INavigationService navigationService, IPageDialogService dialogService) : base(navigationService)
        {
            _dialogService = dialogService;
        }

        private IEnumerable<Models.Vehicle> _vehicles;
        /// <summary>
        /// Available vehicles for working. 
        /// </summary>
        public IEnumerable<Models.Vehicle> AvailableVehicles
        {
            get { return _vehicles; }
            set { SetProperty(ref _vehicles, value); }
        }


        private Models.Vehicle _veh;
        /// <summary>
        /// Current selected vehicle.
        /// </summary>
        public new Models.Vehicle Vehicle
        {
            get { return _veh; }
            set {
                SetProperty(ref _veh, value);
            }
        }

        private bool _IsBusy = false;
        /// <summary>
        /// Determine if the webservices is loading.
        /// </summary>
        public bool IsBusy
        {
            get { return _IsBusy; }
            set { SetProperty(ref _IsBusy, value); }
        }
        #region COMMANDS
        // Command implementations goes here.
        private DelegateCommand _select;
        public DelegateCommand SelectCommand =>
            _select ?? (_select = new DelegateCommand(ExecuteSelectCommand));
        #endregion

        /// <summary>
        /// Add selected vehicle to database and navigate to <see cref="MainPage"/>.
        /// </summary>
        private async void ExecuteSelectCommand()
        {
            await App.SettingsInDb.SetVehicle(Vehicle);
            await _dialogService.DisplayAlertAsync("Mensaje", "Haz seleccionado el vehículo: "+Vehicle.Description, "Bien");
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                Client RESTClient = new Client (WebServices.URL.GetVehicles);
                Request<WebServices.Entities.Vehicle> Requests = new Request<WebServices.Entities.Vehicle>();
                Requests.MessageBody.SystemId = "ACT";
                Requests.MessageBody.User = "Tinoco";
                var Response = await RESTClient.Post<Response<VehicleResponse>>(Requests);
                AvailableVehicles = Response.MessageBody.VehicleListResult.Select(vehicle => ConvertToModel(vehicle)).ToList();
                IsBusy = false;
            }
            catch (Exception e)
            {
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al consultar los vehículos", "Aceptar");
            }
        }


        private Models.Vehicle ConvertToModel(VehicleListResult entityToConvert)
        {
            string VehicleIdentifier = entityToConvert != null ? entityToConvert.VEHICLEID.ToString() : "";
            return new Models.Vehicle { Identifier = VehicleIdentifier, Description = entityToConvert.BRAND, Plate = entityToConvert.UNITNUMBER };
        }
    }
}
