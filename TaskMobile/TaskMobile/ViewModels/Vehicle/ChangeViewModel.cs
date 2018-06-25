using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using TaskMobile.Models;

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
        public new Models.Vehicle Vehicle
        {
            get { return _veh; }
            set { SetProperty(ref _veh, value); }
        }

        #region COMMANDS
        // Command implementations goes here.
        private DelegateCommand _select;
        public DelegateCommand SelectCommand =>
            _select ?? (_select = new DelegateCommand(ExecuteSelectCommand));
        #endregion


        private async void ExecuteSelectCommand()
        {
            await _dialogService.DisplayAlertAsync("Mensaje", "Haz seleccionado el vehículo "+Vehicle.Description, "Bien");
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            WebServices.SOAP.VehicleClient VehicleWS = new WebServices.SOAP.VehicleClient();
            AvailableVehicles = await VehicleWS.FreeToUse();
        }
    }
}
