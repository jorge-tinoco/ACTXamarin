using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TaskMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel, INavigatingAware
    {
        private IEnumerable<Models.Language> _available;
        /// <summary>
        /// Supported languages for this application.
        /// </summary>
        public IEnumerable<Models.Language> AvailableLanguages
        {
            get { return _available; }
            set { SetProperty(ref _available, value); }
        }

        private Models.Language _lang;
        /// <summary>
        /// Selected language.
        /// </summary>
        public Models.Language Language
        {
            get { return _lang; }
            set
            {
                SetProperty(ref _lang, value);
            }
        }

        private IEnumerable<Models.Mill> _mills;
        /// <summary>
        /// Supported mills for this application.
        /// </summary>
        public IEnumerable<Models.Mill> SupportedMills
        {
            get { return _mills; }
            set { SetProperty(ref _mills, value); }
        }


        private Models.Mill _mill;
        /// <summary>
        /// Selected mill.
        /// </summary>
        public Models.Mill Mill
        {
            get { return _mill; }
            set { SetProperty(ref _mill, value); }
        }

        private string _user;
        /// <summary>
        /// Specified user.
        /// </summary>
        public string User
        {
            get { return _user; }
            set {
                SetProperty(ref _user, value);
            }
        }

        private string _password;
        /// <summary>
        /// Given password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }


        #region COMMANDS
        private DelegateCommand _login;
        public DelegateCommand LoginCommand =>
            _login ?? (_login = new DelegateCommand(ExecuteLoginCommand, CanExecuteLoginCommand));
        #endregion

        public  LoginViewModel(INavigationService navigationService):base(navigationService)
        {
            
        }

        /// <summary>
        /// Go to the application.
        /// </summary>
        private async void ExecuteLoginCommand()
        {
            await _navigationService.NavigateAsync("TaskMobile:///MainPage");
        }

        /// <summary>
        /// TO DO: validate user with the tenaris webservices
        /// </summary>
        /// <returns></returns>
        bool CanExecuteLoginCommand()
        {
            return User == Password;
        }

        /// <summary>
        /// Loading values before the view appears.
        /// </summary>
        public async void OnNavigatingTo(NavigationParameters parameters)
        {
            DB.LanguagesREPO Languages = new DB.LanguagesREPO();
            DB.MillsREPO Mills = new DB.MillsREPO();
            AvailableLanguages = await Languages.SupportedLanguages();
            SupportedMills = await Mills.SupportedMills();
        }
    }
}
