using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using TaskMobile.WebServices;
using Xamarin.Forms;

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
    ///         void YourClass(INavigationService navigation):base( navigation)
    ///         {
    ///             ...
    ///         }
    ///         ...
    ///     }
    ///     </code>
    /// </example>
    public class BaseViewModel : BindableBase
    {
        protected readonly INavigationService _navigationService;
        protected readonly IPageDialogService _dialogService;
        protected readonly IClient WebService;
        private bool _isRefreshing = true;
        private string _driver;
        private string _vehicle;
        private Models.Vehicle _curentVehicle;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public BaseViewModel(INavigationService navigationService, IPageDialogService dialogService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
        }

        public BaseViewModel(INavigationService navigationService, IClient webService)
        {
            _navigationService = navigationService;
            WebService = webService;
        }

        public BaseViewModel(INavigationService navigationService, IPageDialogService dialogService, IClient webService)
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            WebService = webService;
        }

        /// <summary>
        /// Current driver.
        /// </summary>
        public string Driver
        {
            get { return _driver; }
            set
            {
                SetProperty(ref _driver, value);
            }
        }

        /// <summary>
        /// Represents current Vehicle. (Selected by the driver/user)
        /// </summary>
        public string Vehicle
        {
            get { return _vehicle; }
            set
            {
                SetProperty(ref _vehicle, value);
            }
        }

        /// <summary>
        /// Vehicle identifier(int format).
        /// </summary>
        protected int VehicleId => _curentVehicle.Identifier != null ? int.Parse(_curentVehicle.Identifier) : 0;

        /// <summary>
        /// Model that represents the current vehicle.
        /// </summary>
        public Models.Vehicle CurrentVehicle
        {
            get { return _curentVehicle; }
            set { SetProperty(ref _curentVehicle, value); }
        }

        /// <summary>
        /// Says when the view model is busy.
        /// </summary>
        public bool IsRefreshing
        {
            get { return _isRefreshing; }
            set { SetProperty(ref _isRefreshing, value); }
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
                await _navigationService.NavigateAsync("TaskMobile:///MainPage/NavigationPage/ChangeVehicle");
            }
            else
                Vehicle = CurrentVehicle.NameToShow;
        }

        /// <summary>
        /// Executed when some problem occurs in web services comsumption.
        /// </summary>
        /// <param name="error">Web service error.</param>
        protected virtual void OnWebServiceError(object error)
        {
            IsRefreshing = false;
            string errorMessage = string.Empty;
            string title = "Error en consumo de servicio";
            if (error is string)
                errorMessage = (string)error;
            if (error is Exception)
                errorMessage = error.ToString();
            if (error is Exceptions.WttException)
            {
                var casted = (Exceptions.WttException) error;
                switch (casted.Severity)
                {
                    case Exceptions.Severity.None:
                        title = "Información";
                        break;
                    case Exceptions.Severity.Low:
                        title = "Atención";
                        break;
                    case Exceptions.Severity.Medium:
                        title = "Alerta";
                        break;
                    case Exceptions.Severity.High:
                        title = "Error";
                        break;
                    case Exceptions.Severity.Intense:
                        title = "Peligro";
                        break;
                    default:
                        title = "Mensaje NO identificado";
                        break;
                }
                errorMessage = casted.Message;
            }
            App.LogToDb.Error(errorMessage);
            Device.BeginInvokeOnMainThread(async () => {
                await _dialogService.DisplayAlertAsync(title, errorMessage, "Entiendo");
                if (errorMessage == "AuthenticationExpiredError")
                    await _navigationService.NavigateAsync("TaskMobile:///LoginPage");
            });
        }
    }
}
