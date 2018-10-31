using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;
using TaskMobile.WebServices;
using Xamarin.Forms;

namespace TaskMobile.ViewModels.Vehicle
{
    public class ChangeViewModel : BaseViewModel, INavigatingAware
    {
        private IEnumerable<Models.Vehicle> _vehicles;
        private Models.Vehicle _veh;
        private bool _isBusy ;
        private readonly WebServices.REST.Vehicles _service;

        public ChangeViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient client) 
            : base(navigationService, dialogService, client)
        {
            _service = new WebServices.REST.Vehicles(client);
        }

        #region COMMANDS
        // Command implementations goes here.
        private DelegateCommand _select;
        public DelegateCommand SelectCommand =>
            _select ?? (_select = new DelegateCommand(ExecuteSelectCommand));
        #endregion

        /// <summary>
        /// Available vehicles for working. 
        /// </summary>
        public IEnumerable<Models.Vehicle> AvailableVehicles
        {
            get { return _vehicles; }
            set { SetProperty(ref _vehicles, value); }
        }

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

        /// <summary>
        /// Determine if the webservices is loading.
        /// </summary>
        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                _service.All(
                    vehicles =>
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            AvailableVehicles = vehicles.ToList();
                            IsBusy = false;
                        });
                    }, OnWebServiceError);
            }
            catch (Exception e)
            {
                if (AvailableVehicles == null)
                    AvailableVehicles = new List<Models.Vehicle>() { new Models.Vehicle() { Plate = 1, Description = "Test", Identifier = "s3" }, new Models.Vehicle() { Plate = 2, Description = "Test 4", Identifier = "s4" } };
                App.LogToDb.Error(e);
                await _dialogService.DisplayAlertAsync("Error", "Ha ocurrido un error al consultar los vehículos", "Aceptar");
            }
        }

        /// <summary>
        /// Add selected vehicle to database and navigate to <see cref="MainPage"/>.
        /// </summary>
        private async void ExecuteSelectCommand()
        {
            if (Vehicle == null)
                await _dialogService.DisplayAlertAsync("Un momento", "Primero selecciona un vehículo de la lista ", "Lo haré");
            else
            {
                await App.SettingsInDb.SetVehicle(Vehicle);
                await _dialogService.DisplayAlertAsync("Mensaje", "Haz seleccionado el vehículo: " + Vehicle.NameToShow, "Bien");
                await _navigationService.NavigateAsync("TaskMobile:///MainPage");
            }
        }
    }
}
