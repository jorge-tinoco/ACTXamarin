using Prism.Mvvm;
using Prism.Navigation;

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
        protected INavigationService _navigationService;

        public BaseViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        private string _Driver;
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

        private string _Vehicle;
        /// <summary>
        /// Represents current Vehicle. (Selected by the driver/user)
        /// </summary>
        public string Vehicle
        {
            get { return _Vehicle; }
            set
            {
                _Vehicle = value;
                SetProperty(ref _Vehicle, value);
            }
        }
    }
}
