using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using TaskMobile.WebServices;
using TaskMobile.WebServices.Entities;
using TaskMobile.WebServices.Entities.Common;
using Xamarin.Forms;

namespace TaskMobile.ViewModels
{
    public class LoginViewModel : BaseViewModel, INavigatingAware
    {

        private string _user;
        private string _password;
        private bool _isBusy = true;
        private Models.Language _lang;
        private Models.Mill _mill;
        private IEnumerable<Models.Language> _available;
        private IEnumerable<Models.Mill> _mills;
        private DelegateCommand _login;

        public LoginViewModel(INavigationService navigationService, IPageDialogService dialog, IClient client)
            : base(navigationService, dialog, client)
        {
        }



        #region COMMANDS
        public DelegateCommand LoginCommand =>
            _login ?? (_login = new DelegateCommand(ExecuteLoginCommand));
        #endregion


        /// <summary>
        /// Supported languages for this application.
        /// </summary>
        public IEnumerable<Models.Language> AvailableLanguages
        {
            get { return _available; }
            set { SetProperty(ref _available, value); }
        }

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

        /// <summary>
        /// Supported mills for this application.
        /// </summary>
        public IEnumerable<Models.Mill> SupportedMills
        {
            get { return _mills; }
            set { SetProperty(ref _mills, value); }
        }


        /// <summary>
        /// Selected mill.
        /// </summary>
        public Models.Mill Mill
        {
            get { return _mill; }
            set { SetProperty(ref _mill, value); }
        }

        /// <summary>
        /// Specified user.
        /// </summary>
        public string User
        {
            get { return _user; }
            set
            {
                SetProperty(ref _user, value);
            }
        }

        /// <summary>
        /// Given password.
        /// </summary>
        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set { SetProperty(ref _isBusy, value); }
        }


        /// <summary>
        /// Go to the application.
        /// </summary>
        private async void ExecuteLoginCommand()
        {
            IsBusy = true;
            var domain = "";
            var user = "";
            if (User == null)
            {
                await _dialogService.DisplayAlertAsync("Atención", "Ingrese un nombre se usuario", "Lo haré");
                IsBusy = false;
                return;
            }
            string[] splited = User.Split('\\');
            if (splited.Count() > 1)
            {
                domain = splited[0];
                user = splited[1];
            }
            else
            {
                domain = "TAMSA";
                user = User;
            }
            WebService.SetCredentials(domain, user, Password);
            WebService.InitTMAP(TMAPresponse, TMAPresponse);
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
            IsBusy = false;
        }

        private void TMAPresponse(WebServices.Entities.TMAP.AuthResponse response)
        {
            Device.BeginInvokeOnMainThread(async () => {
                IsBusy = false;
                switch (response.Response)
                {
                    case WebServices.Entities.TMAP.TmapResponse.Ok:
                        await _navigationService.NavigateAsync("TaskMobile:///MainPage");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.CertificateError:
                        await _dialogService.DisplayAlertAsync("Error", "Error de certificado", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.InvalidCredentials:
                        await _dialogService.DisplayAlertAsync("Error", "Credenciales inválidas.", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.NoNetworkConnection:
                        await _dialogService.DisplayAlertAsync("Error", "Error de red.", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.UserDoNotHaveTmapAccessRights:
                        await _dialogService.DisplayAlertAsync("Error", "No tienes permisos en TMP.", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.MaxBadLogonReached:
                        await _dialogService.DisplayAlertAsync("Error", "Error de tiempo máximo de inicio de sesión.", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.RequestTimeout:
                        await _dialogService.DisplayAlertAsync("Error", "Error de request.", "Ok");
                        break;
                    case WebServices.Entities.TMAP.TmapResponse.Unknow:
                        await _dialogService.DisplayAlertAsync("Error", "Error desconocido.", "Ok");
                        break;
                    default:
                        await _dialogService.DisplayAlertAsync("Error", "Error no catalogado.", "Ok");
                        break;
                }
            });
        }


    }
}
