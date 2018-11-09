// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.SecureHttp
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using Android.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

namespace SecureHttpShared
{
    public abstract class SecureHttp
    {
        protected string _appName = "";
        protected string _appVersion = "";
        protected string _logUrl;
        protected bool _debug;
        protected X509Certificate2 _certificate;
        protected bool _initReady;
        protected Action<InitResponse> _callbackSuccess;
        protected Action<InitResponse> _callbackError;
        protected string additionalLogInfo;
        protected Context _context;

        public void Init(string logUrl, string appName, string appVersion, Action<InitResponse> callbackSuccess, Action<InitResponse> callbackError)
        {
            this.Init(logUrl, appName, appVersion, false, callbackSuccess, callbackError);
        }

        public abstract void Init(string logUrl, string appName, string appVersion, bool debug, Action<InitResponse> callbackSuccess, Action<InitResponse> callbackError);

        public virtual bool HandleOpenUrl(string url, string sourceApplication)
        {
            return true;
        }

        protected void ValidateAirWatchCertificate(bool invokeCallback, bool sendInfoToTLS, bool storedCredentials, TimeTracker timeTracker)
        {
            TrackAction validateCert = timeTracker.StartTrackAction("Validate AW Certificate");
            this.Get(SecureHttpConstants.AirWatchCertificateValidationUrl, (Action<string>)(response =>
           {
               Logger.LogInfo(this._appName, this._appVersion, "AirWatchInit", "AirWatch SDK Successfully Started");
               if (sendInfoToTLS && timeTracker != null)
               {
                   validateCert.Stop();
                   if (storedCredentials)
                       Logger.LogInfoToTLS(this, "AirWatchInit", "User successfully authenticated using AirWatch stored credentials. " + timeTracker.GetTrackedInfo());
                   else if (invokeCallback)
                       Logger.LogInfoToTLS(this, "AirWatchInit", "User successfully authenticated using new AirWatch credentials. " + timeTracker.GetTrackedInfo());
                   else
                       Logger.LogInfoToTLS(this, "AirWatchInit", "New credentials obtained from AirWatch. " + timeTracker.GetTrackedInfo());
               }
               if (!invokeCallback)
                   return;
               this._callbackSuccess(InitResponse.OK);
           }), (Action<string>)(response =>
     {
               if (response.Contains(SecureHttpConstants.AirWatchRevokedCertificateError))
               {
                   this._certificate = (X509Certificate2)null;
                   Logger.LogError(this._appName, this._appVersion, "AirWatchInit", "AirWatch SDK Error, Certificate is Revoked");
                   Logger.LogExceptionToTLS(this, "AirWatchInit", "AirWatch SDK Error, Certificate is Revoked");
                   if (!invokeCallback)
                       return;
                   this._callbackError(InitResponse.CertificateError);
               }
               else
               {
                   Logger.LogInfo(this._appName, this._appVersion, "AirWatchInit", "AirWatch SDK Successfully Started");
                   if (sendInfoToTLS && timeTracker != null)
                   {
                       validateCert.Stop();
                       if (storedCredentials)
                           Logger.LogInfoToTLS(this, "AirWatchInit", "User successfully authenticated using AirWatch stored credentials. " + timeTracker.GetTrackedInfo());
                       else if (invokeCallback)
                           Logger.LogInfoToTLS(this, "AirWatchInit", "User successfully authenticated using new AirWatch credentials. " + timeTracker.GetTrackedInfo());
                       else
                           Logger.LogInfoToTLS(this, "AirWatchInit", "New credentials obtained from AirWatch. " + timeTracker.GetTrackedInfo());
                   }
                   if (!invokeCallback)
                       return;
                   this._callbackSuccess(InitResponse.OK);
               }
           }));
        }

        public virtual void Logout()
        {
        }

        public abstract bool IsAuthenticated();

        private void WaitInitReady()
        {
            while (!this._initReady)
                Thread.Sleep(500);
        }

        public virtual string GetUser()
        {
            string str = "";
            if (this._certificate != null)
            {
                try
                {
                    str = this._certificate.Subject.Split('=')[1];
                }
                catch (Exception ex)
                {
                    Logger.LogError(this._appName, this._appVersion, nameof(GetUser), ex.Message);
                }
            }
            return str;
        }

        public void Get(string requestUriString, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Get(requestUriString, (List<KeyValuePair<string, string>>)null, (string)null, callbackSuccess, callbackError);
        }

        public void GetSync(string requestUriString, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.GetSync(requestUriString, (List<KeyValuePair<string, string>>)null, (string)null, callbackSuccess, callbackError);
        }

        public void Get(string requestUriString, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Get(requestUriString, (List<KeyValuePair<string, string>>)null, contentType, callbackSuccess, callbackError);
        }

        public void GetSync(string requestUriString, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.GetSync(requestUriString, (List<KeyValuePair<string, string>>)null, contentType, callbackSuccess, callbackError);
        }

        public void Get(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Get(requestUriString, additionalHeaders, (string)null, callbackSuccess, callbackError);
        }

        public void GetSync(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.GetSync(requestUriString, additionalHeaders, (string)null, callbackSuccess, callbackError);
        }

        public virtual void Get(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Get(requestUriString, additionalHeaders, contentType, callbackSuccess, callbackError, false);
        }

        public virtual void GetSync(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.GetSync(requestUriString, additionalHeaders, contentType, callbackSuccess, callbackError, false);
        }

        public virtual void Get(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            if (this.NetworkConnectionAvailable())
                ThreadPool.QueueUserWorkItem((WaitCallback)(o => this.DoGet(requestUriString, additionalHeaders, contentType, callbackSuccess, callbackError, useCredentials)));
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        public virtual void GetSync(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            if (this.NetworkConnectionAvailable())
                this.DoGet(requestUriString, additionalHeaders, contentType, callbackSuccess, callbackError, useCredentials);
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        private void DoGet(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            this.WaitInitReady();
            string str = "";
            DateTime now1 = DateTime.Now;
            Logger.LogDebug(this._appName, this._appVersion, nameof(DoGet), "TmpStmp: " + (object)now1 + " Request URL: " + requestUriString);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Timeout = SecureHttpConstants.RequestTimeout;
                if (contentType == null)
                    httpWebRequest.ContentType = "application/json";
                else
                    httpWebRequest.ContentType = contentType;
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                else
                    Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "Using no certificates");
                if (additionalHeaders != null)
                {
                    foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "Additional Header: " + additionalHeader.Key + " = " + additionalHeader.Value);
                        httpWebRequest.Headers.Add(additionalHeader.Key, additionalHeader.Value);
                    }
                }
                if (useCredentials)
                    httpWebRequest.Credentials = (ICredentials)new NetworkCredential(Credentials.User, Credentials.Password);
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Logger.LogError(this._appName, this._appVersion, "DoGet " + requestUriString, "Error fetching data. Server returned status code: " + (object)response.StatusCode);
                    using (Stream responseStream = response.GetResponseStream())
                        str = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                }
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackSuccess(str);
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, "DoGet " + requestUriString, "Error: " + ex.Message);
                string message = ex.Message;
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoGet " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackError(message);
            }
        }

        public void Post(string requestUriString, string data, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Post(requestUriString, data, (List<KeyValuePair<string, string>>)null, (string)null, callbackSuccess, callbackError);
        }

        public void Post(string requestUriString, string data, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Post(requestUriString, data, (List<KeyValuePair<string, string>>)null, contentType, callbackSuccess, callbackError);
        }

        public void Post(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Post(requestUriString, data, additionalHeaders, (string)null, callbackSuccess, callbackError);
        }

        public virtual void Post(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.Post(requestUriString, data, additionalHeaders, contentType, callbackSuccess, callbackError, false);
        }

        public virtual void Post(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            if (this.NetworkConnectionAvailable())
                ThreadPool.QueueUserWorkItem((WaitCallback)(o => this.DoPost(requestUriString, data, additionalHeaders, contentType, callbackSuccess, callbackError, useCredentials)));
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        public virtual void PostSync(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, Action<string> callbackSuccess, Action<string> callbackError)
        {
            if (this.NetworkConnectionAvailable())
                this.DoPost(requestUriString, data, additionalHeaders, (string)null, callbackSuccess, callbackError, false);
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        private void DoPost(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            this.WaitInitReady();
            string str = "";
            DateTime now1 = DateTime.Now;
            Logger.LogDebug(this._appName, this._appVersion, nameof(DoPost), "TmpStmp: " + (object)now1 + " Request URL: " + requestUriString);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.Timeout = SecureHttpConstants.RequestTimeout;
                if (contentType == null)
                    httpWebRequest.ContentType = "application/json";
                else
                    httpWebRequest.ContentType = contentType;
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                else
                    Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "Using no certificates");
                if (additionalHeaders != null)
                {
                    foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "Additional Header: " + additionalHeader.Key + " = " + additionalHeader.Value);
                        httpWebRequest.Headers.Add(additionalHeader.Key, additionalHeader.Value);
                    }
                }
                if (useCredentials)
                    httpWebRequest.Credentials = (ICredentials)new NetworkCredential(Credentials.User, Credentials.Password);
                byte[] bytes = Encoding.UTF8.GetBytes(data);
                httpWebRequest.ContentLength = (long)bytes.Length;
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Logger.LogError(this._appName, this._appVersion, "DoPost " + requestUriString, "Error fetching data. Server returned status code: " + (object)response.StatusCode);
                    using (Stream responseStream = response.GetResponseStream())
                        str = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                }
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackSuccess(str);
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, "DoPost " + requestUriString, "Error: " + ex.Message);
                string message = ex.Message;
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoPost " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackError(message);
            }
        }

        public void DownloadFile(string requestUriString, string filePath, string fileName, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.DownloadFile(requestUriString, filePath, fileName, (List<KeyValuePair<string, string>>)null, (string)null, callbackSuccess, callbackError);
        }

        public void DownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.DownloadFile(requestUriString, filePath, fileName, additionalHeaders, (string)null, callbackSuccess, callbackError);
        }

        public void DownloadFile(string requestUriString, string filePath, string fileName, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.DownloadFile(requestUriString, filePath, fileName, (List<KeyValuePair<string, string>>)null, contentType, callbackSuccess, callbackError);
        }

        public virtual void DownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.DownloadFile(requestUriString, filePath, fileName, additionalHeaders, contentType, callbackSuccess, callbackError, false);
        }

        public virtual void DownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            if (this.NetworkConnectionAvailable())
                ThreadPool.QueueUserWorkItem((WaitCallback)(o => this.DoDownloadFile(requestUriString, filePath, fileName, additionalHeaders, contentType, callbackSuccess, callbackError, useCredentials)));
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        private void DoDownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            this.WaitInitReady();
            DateTime now1 = DateTime.Now;
            Logger.LogDebug(this._appName, this._appVersion, nameof(DoDownloadFile), "TmpStmp: " + (object)now1 + " Request URL: " + requestUriString);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Timeout = SecureHttpConstants.DownloadTimeout;
                if (contentType == null)
                    httpWebRequest.ContentType = "application/json";
                else
                    httpWebRequest.ContentType = contentType;
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                else
                    Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Using no certificates");
                if (additionalHeaders != null)
                {
                    foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Additional Header: " + additionalHeader.Key + " = " + additionalHeader.Value);
                        httpWebRequest.Headers.Add(additionalHeader.Key, additionalHeader.Value);
                    }
                }
                if (useCredentials)
                    httpWebRequest.Credentials = (ICredentials)new NetworkCredential(Credentials.User, Credentials.Password);
                using (HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Logger.LogError(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Error fetching data. Server returned status code: " + (object)response.StatusCode);
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        using (FileStream fileStream = System.IO.File.Create(Path.Combine(filePath, fileName)))
                        {
                            byte[] buffer = new byte[16384];
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                int count;
                                while ((count = responseStream.Read(buffer, 0, buffer.Length)) > 0)
                                    memoryStream.Write(buffer, 0, count);
                                byte[] array = memoryStream.ToArray();
                                fileStream.Write(array, 0, array.Length);
                            }
                        }
                    }
                }
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackSuccess("OK");
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Error: " + ex.Message);
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoDownloadFile " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackError(ex.Message);
            }
        }

        public void UploadFile(string requestUriString, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.UploadFile(requestUriString, (List<KeyValuePair<string, string>>)null, contentType, formData, callbackSuccess, callbackError);
        }

        public virtual void UploadFile(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError)
        {
            this.UploadFile(requestUriString, additionalHeaders, contentType, formData, callbackSuccess, callbackError, false);
        }

        public virtual void UploadFile(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            if (this.NetworkConnectionAvailable())
                ThreadPool.QueueUserWorkItem((WaitCallback)(o => this.DoUploadFile(requestUriString, additionalHeaders, contentType, formData, callbackSuccess, callbackError, useCredentials)));
            else
                callbackError(SecureHttpConstants.NetworkConnectionError);
        }

        private void DoUploadFile(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError, bool useCredentials)
        {
            this.WaitInitReady();
            string str = "";
            DateTime now1 = DateTime.Now;
            Logger.LogDebug(this._appName, this._appVersion, nameof(DoUploadFile), "TmpStmp: " + (object)now1 + " Request URL: " + requestUriString);
            try
            {
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(requestUriString);
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentType = contentType;
                httpWebRequest.ContentLength = (long)formData.Length;
                httpWebRequest.KeepAlive = false;
                if (this._certificate != null)
                {
                    httpWebRequest.ClientCertificates.Add((X509Certificate)this._certificate);
                    Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Using certificate: " + this._certificate.Issuer + " - " + this._certificate.Subject);
                }
                else
                    Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Using no certificates");
                if (additionalHeaders != null)
                {
                    foreach (KeyValuePair<string, string> additionalHeader in additionalHeaders)
                    {
                        Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Additional Header: " + additionalHeader.Key + " = " + additionalHeader.Value);
                        httpWebRequest.Headers.Add(additionalHeader.Key, additionalHeader.Value);
                    }
                }
                if (useCredentials)
                    httpWebRequest.Credentials = (ICredentials)new NetworkCredential(Credentials.User, Credentials.Password);
                using (Stream requestStream = httpWebRequest.GetRequestStream())
                {
                    requestStream.Write(formData, 0, formData.Length);
                    requestStream.Close();
                }
                using (HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse())
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                        Logger.LogError(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Error fetching data. Server returned status code: " + (object)response.StatusCode);
                    using (Stream responseStream = response.GetResponseStream())
                        str = new StreamReader(responseStream, Encoding.UTF8).ReadToEnd();
                }
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackSuccess(str);
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Error: " + ex.Message);
                string message = ex.Message;
                DateTime now2 = DateTime.Now;
                Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "TmpStmp: " + (object)now2 + " Response sent");
                Logger.LogDebug(this._appName, this._appVersion, "DoUploadFile " + requestUriString, "Request time span: " + this.DateDiff(now1, now2).ToString() + " ms");
                callbackError(message);
            }
        }

        protected void GetStoredCertificate(string path = "")
        {
            try
            {
                //T12725: Agosto/01/2018: Si no hay path para el certificado, colocarlo en null
                if (!string.IsNullOrEmpty(path))
                {
                    string storedPassword = this.GetStoredPassword(path);
                    this._certificate = new X509Certificate2(Encryptor.DecryptFile(this.ReadFile(SecureHttpConstants.CertificateFileName, path)), storedPassword);
                }
                else {
                    this._certificate = null;
                    
                }
            }
            catch (Exception ex)
            {
                this._certificate = (X509Certificate2)null;
                Logger.LogError(this._appName, this._appVersion, nameof(GetStoredCertificate), ex.Message);
            }
        }

        protected string GetStoredPassword(string path = "")
        {
            string str = "";
            byte[] fileBytes = this.ReadFile(SecureHttpConstants.PasswordFileName, path);
            if (fileBytes != null)
                str = Encryptor.DecryptString(fileBytes);
            return str;
        }

        protected byte[] ReadFile(string fileName, string path = "")
        {
            byte[] numArray = (byte[])null;
            try
            {
                numArray = this.ReadToEnd(!string.IsNullOrEmpty(path) ? (Stream)System.IO.File.OpenRead(Path.Combine(path, fileName)) : this._context.Assets.Open(fileName));
            }
            catch (Exception ex)
            {
                Logger.LogError(this._appName, this._appVersion, nameof(ReadFile), fileName + ": " + ex.Message);
            }
            return numArray;
        }

        protected byte[] ReadToEnd(Stream stream)
        {
            long num1 = 0;
            if (stream.CanSeek)
            {
                num1 = stream.Position;
                stream.Position = 0L;
            }
            try
            {
                byte[] buffer = new byte[4096];
                int length = 0;
                int num2;
                while ((num2 = stream.Read(buffer, length, buffer.Length - length)) > 0)
                {
                    length += num2;
                    if (length == buffer.Length)
                    {
                        int num3 = stream.ReadByte();
                        if (num3 != -1)
                        {
                            byte[] numArray = new byte[buffer.Length * 2];
                            Buffer.BlockCopy((Array)buffer, 0, (Array)numArray, 0, buffer.Length);
                            Buffer.SetByte((Array)numArray, length, (byte)num3);
                            buffer = numArray;
                            ++length;
                        }
                    }
                }
                byte[] numArray1 = buffer;
                if (buffer.Length != length)
                {
                    numArray1 = new byte[length];
                    Buffer.BlockCopy((Array)buffer, 0, (Array)numArray1, 0, length);
                }
                return numArray1;
            }
            finally
            {
                if (stream.CanSeek)
                    stream.Position = num1;
            }
        }

        public bool NetworkConnectionAvailable()
        {
            try
            {
                NetworkInfo activeNetworkInfo = ((ConnectivityManager)this._context.GetSystemService("connectivity")).ActiveNetworkInfo;
                return activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting;
            }
            catch (Exception)
            {
                return true;
            }
        }

        public AuthenticationType GetAuthenticationType()
        {
            if (this.GetType() == typeof(AuthenticateHttp))
                return AuthenticationType.Credentials;
            if (this.GetType() == typeof(ExpirableAuthenticateHttp))
                return AuthenticationType.CredentialsWithExpiration;
            return this.GetType() == typeof(AnonymousHttp) ? AuthenticationType.Anonymous : AuthenticationType.Airwatch;
        }

        protected void SaveAirWatchErrorForTLS(string function, string errorMessage)
        {
            if (this._certificate == null)
            {
                Credentials.SendAirWatchError = true;
                Credentials.AirWatchError = errorMessage;
            }
            else
            {
                Credentials.SendAirWatchError = false;
                Credentials.AirWatchError = string.Empty;
                Logger.LogExceptionToTLS(this, function, errorMessage);
            }
        }

        private double DateDiff(DateTime date1, DateTime date2)
        {
            return date2.Subtract(date1).TotalMilliseconds;
        }

        internal string GetCertificateSerialNumber()
        {
            if (this._certificate != null)
                return this._certificate.SerialNumber;
            return string.Empty;
        }

        internal string GetLogUrl()
        {
            return this._logUrl;
        }

        internal string GetLogApplicationName()
        {
            return this._appName;
        }

        internal string GetLogApplicationVersion()
        {
            return this._appVersion;
        }

        internal Context GetContext()
        {
            return this._context;
        }

        internal string GetApplicationName()
        {
            return this._context.PackageName;
        }
    }
}
