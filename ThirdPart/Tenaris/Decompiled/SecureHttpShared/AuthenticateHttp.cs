// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.AuthenticateHttp
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Reflection;

namespace SecureHttpShared
{
    internal class AuthenticateHttp : SecureHttp
    {
        protected static readonly object syncLock = new object();
        private PublicKey _publicKey;

        internal AuthenticateHttp()
        {
            ThreadPool.QueueUserWorkItem((WaitCallback)(o => Logger.InitializeLogger(this.GetApplicationName())));
        }

        internal AuthenticateHttp(Context context)
        {
            this._context = context;
            ThreadPool.QueueUserWorkItem((WaitCallback)(o => Logger.InitializeLogger(this.GetApplicationName())));
        }

        public override void Init(string logUrl, string appName, string appVersion, bool debug, Action<InitResponse> callbackSuccess, Action<InitResponse> callbackError)
        {
            this._callbackSuccess = callbackSuccess;
            this._callbackError = callbackError;
            this._debug = debug;
            this._logUrl = logUrl;
            this._appName = appName;
            this._appVersion = appVersion;
            this._initReady = false;
            if (!this.IsAuthenticated())
            {
                if (string.IsNullOrEmpty(Credentials.Domain) || string.IsNullOrEmpty(Credentials.User) || string.IsNullOrEmpty(Credentials.Password))
                    this._callbackError(InitResponse.InvalidCredentials);
                else if (!this.NetworkConnectionAvailable())
                    this._callbackError(InitResponse.NoNetworkConnection);
                else
                    ThreadPool.QueueUserWorkItem((WaitCallback)(o => this.AuthenticateInit()));
            }
            else
            {
                this._initReady = true;
                Logger.LogInfoToTLS((SecureHttp)this, "AuthenticateInit", "User successfully authenticated using credentials");
                this._callbackSuccess(InitResponse.OK);
            }
        }

        public override void Logout()
        {
            Credentials.Logout();
        }

        public override string GetUser()
        {
            return Credentials.User;
        }

        public override void Get(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            if (!this.ValidUrl(requestUriString, callbackError))
                return;
            string serviceUrl = this.GetServiceUrl(requestUriString);
            if (additionalHeaders == null)
                additionalHeaders = new List<KeyValuePair<string, string>>();
            this.AddAuthenticationHeaders(additionalHeaders);
            base.Get(serviceUrl, additionalHeaders, contentType, callbackSuccess, callbackError);
        }

        public override void Post(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            if (!this.ValidUrl(requestUriString, callbackError))
                return;
            string serviceUrl = this.GetServiceUrl(requestUriString);
            if (additionalHeaders == null)
                additionalHeaders = new List<KeyValuePair<string, string>>();
            this.AddAuthenticationHeaders(additionalHeaders);
            base.Post(serviceUrl, data, additionalHeaders, contentType, callbackSuccess, callbackError);
        }

        public override void UploadFile(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError)
        {
            if (!this.ValidUrl(requestUriString, callbackError))
                return;
            string serviceUrl = this.GetServiceUrl(requestUriString);
            if (additionalHeaders == null)
                additionalHeaders = new List<KeyValuePair<string, string>>();
            this.AddAuthenticationHeaders(additionalHeaders);
            base.UploadFile(serviceUrl, additionalHeaders, contentType, formData, callbackSuccess, callbackError);
        }

        public override void DownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            if (!this.ValidUrl(requestUriString, callbackError))
                return;
            if (additionalHeaders == null)
                additionalHeaders = new List<KeyValuePair<string, string>>();
            this.AddAuthenticationHeaders(additionalHeaders);
            base.DownloadFile(requestUriString, filePath, fileName, additionalHeaders, contentType, callbackSuccess, callbackError);
        }

        public override bool IsAuthenticated()
        {
            return Credentials.IsAuthenticated();
        }

        protected virtual void SetAuthenticated()
        {
            Credentials.SetAuthenticated();
        }

        private void SendAirWatchErrorToTLS()
        {
            if (!Credentials.SendAirWatchError)
                return;
            Logger.LogExceptionToTLS((SecureHttp)this, "AirWatchInit", Credentials.AirWatchError);
            Credentials.SendAirWatchError = false;
            Credentials.AirWatchError = string.Empty;
        }

        private bool ValidUrl(string requestUriString, Action<string> callbackError)
        {
            if (string.IsNullOrEmpty(requestUriString))
            {
                callbackError("Request Url can't be empty");
                return false;
            }
            if (!string.IsNullOrEmpty(Credentials.ProxyUrl))
                return true;
            callbackError("Proxy Url can't be empty");
            return false;
        }

        private void AuthenticateInit()
        {
            lock (AuthenticateHttp.syncLock)
            {
                TimeTracker timeTracker = new TimeTracker();
                TrackAction trackAction1 = timeTracker.StartTrackAction("Get Certificate");
                this.GetStoredCertificate("");

                if (this._certificate == null)
                {
                    //ReadWsCredentials();
                }
                 
                trackAction1.Stop();
                TrackAction trackAction2 = timeTracker.StartTrackAction("Get Public Key");
                this.SetPublicKey();
                trackAction2.Stop();
                if (!this.IsKeyReady)
                    this._callbackError(InitResponse.CertificateError);
                else
                    this.EncryptCredentials(Credentials.Domain, Credentials.User, Credentials.Password, this._publicKey.Exponent, this._publicKey.Modulus, timeTracker);
                this._initReady = true;
            }
        }

        private void SetPublicKey()
        {
            this._publicKey = (PublicKey)null;
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri((Credentials.ProxyUrl.EndsWith("/") ? Credentials.ProxyUrl : Credentials.ProxyUrl + "/") + SecureHttpConstants.WS_KEY));
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "GetPublicKey AutenticateHTTP", "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                //Credenciales de TMG
                var wsCredentials = GetWsBasicAuthenticationHeaders();
                foreach (var header in wsCredentials)
                {
                    httpWebRequest.Headers.Add(header.Key, header.Value);
                }
                httpWebRequest.Method = "GET";
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    try
                    {
                        this._publicKey = JsonConvert.DeserializeObject<PublicKey>(streamReader.ReadToEnd());
                    }
                    finally
                    {
                        ((IDisposable)streamReader)?.Dispose();
                    }
                }
                Logger.LogDebug(this._appName, this._appVersion, "GetPublicKey AutenticateHTTP", "OK");
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, "GetPublicKey AutenticateHTTP", "Error: " + ex.Message);
                Logger.LogExceptionToTLS((SecureHttp)this, "GetPublicKey AutenticateHTTP", ex.Message);
            }
        }

        private void EncryptCredentials(string domain, string user, string password, string exponent, string modulus, TimeTracker timeTracker)
        {
            string str = "";
            try
            {
                TrackAction trackAction = timeTracker.StartTrackAction("Validate Credentials");
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(new Uri((Credentials.ProxyUrl.EndsWith("/") ? Credentials.ProxyUrl : Credentials.ProxyUrl + "/") + SecureHttpConstants.WS_VALID));
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                else
                    Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Using no certificates");
                httpWebRequest.Method = "GET";
                List<KeyValuePair<string, string>> additionalHeaders = new List<KeyValuePair<string, string>>();
                this.AddAuthenticationHeaders(additionalHeaders);
                List<KeyValuePair<string, string>>.Enumerator enumerator = additionalHeaders.GetEnumerator();
                try
                {
                    while (enumerator.MoveNext())
                    {
                        KeyValuePair<string, string> current = enumerator.Current;
                        httpWebRequest.Headers.Add(current.Key, current.Value);
                    }
                }
                finally
                {
                    enumerator.Dispose();
                }
                HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    StreamReader streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                    try
                    {
                        str = streamReader.ReadToEnd();
                    }
                    finally
                    {
                        ((IDisposable)streamReader)?.Dispose();
                    }
                    this.SetAuthenticated();
                    this.SendAirWatchErrorToTLS();
                    trackAction.Stop();
                    Logger.LogInfoToTLS((SecureHttp)this, "AuthenticateInit", "User successfully authenticated using credentials. " + timeTracker.GetTrackedInfo());
                    this._callbackSuccess(InitResponse.OK);
                }
                else
                {
                    this.Logout();
                    trackAction.Stop();
                    this._callbackError(InitResponse.InvalidCredentials);
                }
                Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Result VALID: " + str);
            }
            catch (WebException ex)
            {
                HttpWebResponse httpWebResponse = (HttpWebResponse)null;
                // ISSUE: type reference
                if (Type.Equals(ex.Response.GetType(), typeof(HttpWebResponse)))
                    httpWebResponse = (HttpWebResponse)ex.Response;
                if (httpWebResponse != null)
                {
                    if (httpWebResponse.StatusCode == HttpStatusCode.Forbidden || httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Result VALID: Invalid credentials");
                        this.Logout();
                        this._callbackError(InitResponse.InvalidCredentials);
                    }
                    else if (httpWebResponse.StatusCode == HttpStatusCode.RequestTimeout)
                        this._callbackError(InitResponse.InvalidCredentials);
                    else if (httpWebResponse.StatusCode.ToString() == "998")
                        this._callbackError(InitResponse.MaxBadLogonReached);
                    else if (httpWebResponse.StatusCode.ToString() == "999")
                    {
                        this._callbackError(InitResponse.UserDoNotHaveTMAPAccessRights);
                    }
                    else
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Result VALID: " + ex.Message);
                        Logger.LogExceptionToTLS((SecureHttp)this, "EncryptCredentials AutenticateHTTP", ex.Message);
                        this._callbackError(InitResponse.CertificateError);
                    }
                }
                else
                {
                    Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Result VALID: " + ex.Message);
                    Logger.LogExceptionToTLS((SecureHttp)this, "EncryptCredentials AutenticateHTTP", ex.Message);
                    this._callbackError(InitResponse.CertificateError);
                }
            }
            catch (Exception ex)
            {
                Logger.LogDebug(this._appName, this._appVersion, "EncryptCredentials AutenticateHTTP", "Result VALID: " + ex.Message);
                Logger.LogExceptionToTLS((SecureHttp)this, "EncryptCredentials AutenticateHTTP", ex.Message);
                this._callbackError(InitResponse.CertificateError);
            }
        }

        private string GetServiceUrl(string requestUriString)
        {
            string str1 = requestUriString.Replace(Credentials.ProxyUrl, "");
            string str2 = Credentials.ProxyUrl.EndsWith("/") ? Credentials.ProxyUrl : Credentials.ProxyUrl + "/";
            int num1 = str1.IndexOf("?");
            int num2 = str1.IndexOf("/");
            string str3;
            if (num2 != -1 && (num1 == -1 || num2 < num1))
            {
                str3 = string.Format("{0}{1}/{2}/{3}", new object[4]
                {
          (object) str2,
          (object) SecureHttpConstants.WS_PROXY,
          (object) Credentials.KeyProxy,
          (object) str1
                });
            }
            else
            {
                if (num1 != -1)
                    str1 = ((object)new StringBuilder(str1)
                    {
                        [num1] = '&'
                    }).ToString();
                str3 = string.Format("{0}{1}?app={2}&route={3}", new object[4]
                {
          (object) str2,
          (object) SecureHttpConstants.WS_PROXY,
          (object) Credentials.KeyProxy,
          (object) str1
                });
            }
            return str3;
        }

        private void AddAuthenticationHeaders(List<KeyValuePair<string, string>> additionalHeaders)
        {
             
            try
            {
                //Credenciales de TMAP 
                RSACryptoServiceProvider cryptoServiceProvider = new RSACryptoServiceProvider();
                ((AsymmetricAlgorithm)cryptoServiceProvider).FromXmlString(string.Format("<RSAKeyValue><Modulus>{1}</Modulus><Exponent>{0}</Exponent></RSAKeyValue>", (object)this._publicKey.Exponent, (object)this._publicKey.Modulus));
                byte[] inArray = cryptoServiceProvider.Encrypt(Encoding.UTF8.GetBytes(string.Format("{0}\\{1}:{2}", (object)Credentials.Domain, (object)Credentials.User, (object)Credentials.Password)), false);
                if (additionalHeaders == null)
                    additionalHeaders = new List<KeyValuePair<string, string>>();
                additionalHeaders.Add(new KeyValuePair<string, string>("X-TNSMobile-ProxyAuth", Convert.ToBase64String(inArray)));


                //Credenciales de TMG
                var wsCredentials = GetWsBasicAuthenticationHeaders();
                additionalHeaders.AddRange(wsCredentials);
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, nameof(AddAuthenticationHeaders), "Error adding authentication headers for TMAP: " + ex.Message);
            }

        }

        private void ReadWsCredentials() {

            //TMG Credentials
            try
            {
#if __IOS__
            var resourcePrefix = "TaskMobile.iOS.";
#endif
#if __ANDROID__
                var resourcePrefix = "TaskMobile.Android.";
#endif
                // string resourceNamce = resourcePrefix + "Configuration/encrypted_file.txt";
                string resourceNamce =  "encrypted_file.txt";

                var assembly = GetType().GetTypeInfo().Assembly;
                Stream stream = assembly.GetManifestResourceStream(resourceNamce);
 
                byte[] currentConfig = ReadFully(stream);
                byte[] file = Encryptor.DecryptFile(currentConfig);
                Dictionary<String, Object> configuration = BytesToJson(file);
              
                object _user;
                object _password;

              
                configuration.TryGetValue("SERVICE_USER", out _user);
                configuration.TryGetValue("SERVICE_USER_PWD", out _password);
                 
                Credentials.WsUser = Convert.ToString(_user);
                Credentials.WsPassword = Convert.ToString(_password) ;

            
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, nameof(AddAuthenticationHeaders), "Error adding authentication headers for TMG: " + ex.Message);
            }
        }

        private static List<KeyValuePair<string, string>> GetWsBasicAuthenticationHeaders()
        {
            var keyAuthenticationCloud = HttpRequestHeader.Authorization.ToString();
            var valueAuthenticationCloud = "Basic " + Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(string.Format("{0}:{1}",
                Credentials.WsUser, Credentials.WsPassword)));

            return new List<KeyValuePair<string, string>>()
                    {
                           new KeyValuePair<string, string>(keyAuthenticationCloud, valueAuthenticationCloud)
                    };
        }


        public static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }

        }

        private Dictionary<String, Object> BytesToJson(byte[] arrayInJson)
        {
            try
            {
                var reader = new StreamReader(new MemoryStream(arrayInJson), Encoding.Default);
                string content = reader.ReadToEnd();
                var values = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);
                if (values == null)
                    throw new Exception("No se ha podido recuperar los datos del array de bytes");
                return values;
            }
            catch (Exception ex)
            {
                Android.Util.Log.Error("WTT", ex.Message);
                throw ex;
            }

        }
 
        private bool IsKeyReady
        {
            get
            {
                return this._publicKey != null && this._publicKey.Modulus != null && this._publicKey.Exponent != null;
            }
        }
    }
}
