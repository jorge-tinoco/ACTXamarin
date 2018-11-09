using Android.Content;
using AndroidSecureHTTP;
using Newtonsoft.Json;
using SecureHttpShared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TaskMobile.Droid.WebServices;
using TaskMobile.WebServices;
using TaskMobile.WebServices.Entities.TMAP;
using Xamarin.Forms;

[assembly: Dependency(typeof(Client_Android))]
namespace TaskMobile.Droid.WebServices
{
    public class Client_Android : IClient
    {
        private string _logging ;
        private string _tmap ;
        private string _tmapProxy ;
        private readonly string _appName ;
        private readonly string _appVersion;
        private readonly string _user;     // service user
        private readonly string _password; // service user's  password
        private readonly Context _context;
        private int _expiration; // In seconds
        /// <summary>
        /// Unique TMAP service client for making TMAP requests.
        /// </summary>
        private SecureHttp _secureClient;

        public Client_Android()
        {
            object tmap,key, expiration, log, user, password;
            var crypto = new Utilities.Crypto();
            var file = new Utilities.File();
            var appInfo = new Utilities.App();
            // TODO: set the correct encrypted files and make sure settings are correct.
            crypto.EncryptFile("config_tl.txt");
            string environment = file.ReadFromAssets("current_config.txt");
            //var currentConfig = string.Format("Configuration/config_{0}.txt", environment);
            var currentConfig = string.Concat("Configuration/", "encrypted_file.txt");
            Dictionary<string,object> configuration =  crypto.DecryptFile(currentConfig);
            configuration.TryGetValue("TMAP_PROXY_URL", out tmap);
            configuration.TryGetValue("TMAP_KEY_PROXY", out key);
            configuration.TryGetValue("EXPIRABLE_CREDENTIALS_TIME", out expiration);
            configuration.TryGetValue("LOGGER_URL", out log);
            configuration.TryGetValue("SERVICE_USER", out user);
            configuration.TryGetValue("SERVICE_USER_PWD", out password);
            
            _tmap = (string)tmap;
            _tmapProxy = (string)key;
            _user = (string)user;
            _password = (string)password;
            if( expiration != null)
                int.TryParse(  expiration.ToString(), out _expiration);
            _logging = (string)log;
            _context = Android.App.Application.Context;
            //_appName = AppInfo.AppName;
            _appVersion = appInfo.VersionName;
            _appName = "WTT";
        }


        /// <summary>
        /// TMAP logging URL.
        /// </summary>
        public string LOGGING_URL
        {
            get { return _logging; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("You must specify a valid URL");
                _logging = value;
            }
        }

        /// <summary>
        /// TMAP url.
        /// </summary>
        public string TMAP_URL
        {
            get { return _tmap; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Please specify a valid URL");
                bool endWithSlash = value.EndsWith("/");
                if (endWithSlash)
                    _tmap = value.Remove(value.Count() - 1);
                else
                    _tmap = value;
            }
        }

        /// <summary>
        /// Parameter set in TMAP url. Example <see cref="TMAP_URL"/>/<see cref="TMAP_KEY_PROXY"/>.
        /// </summary>
        public string TMAP_KEY_PROXY
        {
            get { return _tmapProxy; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new Exception("Proxy must be specified");
                _tmapProxy = value;
            }
        }

        /// <summary>
        /// User's domain.
        /// </summary>
        public string UserDomain
        {
            get { return Credentials.Domain; }
        }

        /// <summary>
        /// Username used in TMAP.
        /// </summary>
        public string UserName
        {
            get { return Credentials.User; }
        }
        
        /// <summary>
        /// Authentication type .
        /// </summary>
        public Authentication Authentication
        {
            get
            {
                string domainUser = string.Concat(Credentials.Domain, Credentials.User);
                if (string.IsNullOrEmpty(domainUser))
                    return Authentication.Anonymous;
                else
                    if (ExpirationTime > 0)
                        return Authentication.CredentialsWithExpiration;
                    else
                        return Authentication.Credentials;
            }
        }

        /// <summary>
        /// Credentials expiration time. In seconds.
        /// </summary>
        public int ExpirationTime
        {
            get { return _expiration; }
            set
            {
                if (value < 0)
                    throw new Exception("Expiration must be 0 or greater");
                else
                {
                    _expiration = value;
                    Credentials.AuthenticationExpirationTime = value;
                }
            }
        }

        /// <summary>
        /// Close session.
        /// </summary>
        public void LogOut()
        {
            _secureClient.Logout();
        }

        /// <summary>
        /// Set user credentials.
        /// </summary>
        /// <param name="domain">User's domain.</param>
        /// <param name="user">User name.</param>
        /// <param name="password">Password.</param>
        public void SetCredentials(string domain, string user, string password)
        {
            Credentials.Domain = domain;
            Credentials.User = user;
            Credentials.Password = password;
            Credentials.WsUser = _user;
            Credentials.WsPassword = _password;
        }

        /// <summary>
        /// Init TMAP web service client implementation.
        /// </summary>
        /// <param name="callbackSuccess">TMAP success response.</param>
        /// <param name="callbackError">TMAP error response.</param>
        public void InitTMAP(Action<AuthResponse> callbackSuccess, Action<AuthResponse> callbackError)
        {
            Credentials.ProxyUrl = TMAP_URL;
            Credentials.KeyProxy = TMAP_KEY_PROXY;
            Credentials.AuthenticationExpirationTime = ExpirationTime;
            SecureHttp tmapHttpClient = GetSecureHttp(Authentication);
          
            tmapHttpClient.Init(LOGGING_URL, _appName, _appVersion,
                (success) =>
                {
                    _secureClient = tmapHttpClient;
                    callbackSuccess(new AuthResponse(TranslateResponse( success)));
                },
                (error) =>
                {
                    callbackError(new AuthResponse(TranslateResponse( error), true));
                });
        }

       /// <summary>
       /// Make a POST request to tenaris webservices(TMAP)
       /// </summary>
       /// <typeparam name="T"></typeparam>
       /// <param name="route">TMAP route.</param>
       /// <param name="data">Data for send to TMAP.</param>
       /// <param name="success">TMAP success callback.</param>
       /// <param name="error">TMAP error callback.</param>
        public void Post<T>(string route, string data, Action<T> success, Action<object> error)
       {
            //TODO: make sure if necessary include Authentication header in this part.
            var credentials = $"{_user}:{_password}";
            var byteArray = Encoding.UTF8.GetBytes(credentials);
            var base64 = Convert.ToBase64String(byteArray);
            var header = new KeyValuePair<string, string>("Basic",base64);
            var headers = new List<KeyValuePair<string, string>> {header};
           _secureClient.Post(route, data,
                    response =>
                    {
                        var settings = new JsonSerializerSettings
                        {
                            //Error = this.ResponseErrorHandler,
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        var deserialized = JsonConvert.DeserializeObject<T>(response, settings);
                        success(deserialized);
                    },
                    errorResponse =>
                    {
                        error(errorResponse);
                    });
        }

        public void Get<T>(string route, Action<T> callbackSuccess, Action<object> callbackError)
        {
            throw new NotImplementedException("Get method  has not been implemented in Android.");
        }
        /// <summary>
        /// Return an web service client objet  based in authentication type. 
        /// </summary>
        /// <param name="authType">Authentication type.</param>
        /// <returns>Web service client.</returns>
        private SecureHttp GetSecureHttp(Authentication authType)
        {
            switch (authType)
            {
                case Authentication.Credentials:
                    return SecureHttpFactory.GetAuthenticationRequester(_context);
                case Authentication.CredentialsWithExpiration:
                    return SecureHttpFactory.GetExpirableAuthenticationRequester(_context);
                case Authentication.Anonymous:
                    return SecureHttpFactory.GetAnonymousAuthenticationRequester(_context);
                default:
                    throw new ArgumentException("Invalid authentication type", "authenticationType");
            }
        }

        private TmapResponse TranslateResponse(InitResponse response)
        {
            switch (response)
            {
                case InitResponse.OK:
                    return TmapResponse.Ok;
                case InitResponse.CertificateError:
                    return TmapResponse.CertificateError;
                case InitResponse.InvalidCredentials:
                    return TmapResponse.InvalidCredentials;
                case InitResponse.NoNetworkConnection:
                    return TmapResponse.NoNetworkConnection;
                case InitResponse.UserDoNotHaveTMAPAccessRights:
                    return TmapResponse.UserDoNotHaveTmapAccessRights;
                case InitResponse.MaxBadLogonReached:
                    return TmapResponse.MaxBadLogonReached;
                case InitResponse.RequestTimeout:
                    return TmapResponse.RequestTimeout;
                default:
                    return TmapResponse.Unknow;
            }
        }
    }
}