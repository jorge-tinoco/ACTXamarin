using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;

namespace TaskMobile.ViewModels
{
    /// <summary>
    /// Task mobile custom common base class for using on all ViewModels.
    /// </summary>
    /// <example>
    ///     <code>
    ///     public YourClass: BaseViewModel
    ///     {
    ///         ...
    ///         void YourClass(INavigationService navigationService):base( navigationService)
    ///         {
    ///             ...
    ///         }
    ///         ...
    ///     }
    ///     </code>
    /// </example>
    public class BaseViewModel : BindableBase
    {
        private string _Driver;
        private string _Vehicle;
        private Models.Vehicle _curentVehicle;
        protected INavigationService _navigationService;
        protected IPageDialogService _dialogService;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public BaseViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        /// <summary>
        /// Current driver.
        /// </summary>
        public string Driver
        {
            get { return _Driver; }
            set
            {
                SetProperty(ref _Driver, value);
            }
        }

        /// <summary>
        /// Represents current Vehicle. (Selected by the driver/user)
        /// </summary>
        public string Vehicle
        {
            get { return _Vehicle; }
            set
            {
                SetProperty(ref _Vehicle, value);
            }
        }

        /// <summary>
        /// Model that represents the current vehicle.
        /// </summary>
        public Models.Vehicle CurrentVehicle
        {
            get { return _curentVehicle; }
            set { SetProperty(ref _curentVehicle, value); }
        }

        /// <summary>
        /// Check if the vehicle has been set by the user.
        /// </summary>
        protected async Task  CheckVehicle()
        {
            CurrentVehicle = await App.SettingsInDb.CurrentVehicle();
            if (CurrentVehicle == null)
            {
                if (_dialogService != null)
                    await _dialogService.DisplayAlertAsync("Oops", "Un minuto, el vehículo no ha sido configurado. Configura el vehículo", "Entiendo");
                else
                    await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/ChangeVehicle");
            }
            else
                Vehicle = CurrentVehicle.NameToShow;
        }
    }
}
