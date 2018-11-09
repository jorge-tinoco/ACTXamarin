// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.Credentials
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using System;

namespace SecureHttpShared
{
    public static class Credentials
    {
        public static string ProxyUrl { get; set; }

        public static string KeyProxy { get; set; }

        public static string Domain { get; set; }

        public static string User { get; set; }

        public static string Password { get; set; }

        public static string WsUser { get; set; }

        public static string WsPassword { get; set; }

        public static int AuthenticationExpirationTime { get; set; }

        internal static ICredentialsStorage CredentialsStorage { get; set; }

        private static bool Authenticated { get; set; }

        private static DateTime? LastUserRequest { get; set; }

        internal static bool SendAirWatchError { get; set; }

        internal static string AirWatchError { get; set; }

        internal static void SetAuthenticated()
        {
            Credentials.Authenticated = true;
            if (Credentials.CredentialsStorage == null)
                return;
            Credentials.CredentialsStorage.StoreCredentials(Credentials.ProxyUrl, Credentials.KeyProxy, Credentials.Domain, Credentials.User, Credentials.Password);
        }

        internal static void Logout()
        {
            Credentials.LastUserRequest = new DateTime?();
            Credentials.Authenticated = false;
            Credentials.ProxyUrl = string.Empty;
            Credentials.KeyProxy = string.Empty;
            Credentials.Domain = string.Empty;
            Credentials.User = string.Empty;
            Credentials.Password = string.Empty;
            if (Credentials.CredentialsStorage == null)
                return;
            Credentials.CredentialsStorage.RemoveStoredCredentials();
        }

        internal static void UpdateLastUserRequest()
        {
            Credentials.LastUserRequest = new DateTime?(DateTime.Now);
        }

        public static bool LoadStoredCredentials()
        {
            if (Credentials.CredentialsStorage != null)
                return Credentials.CredentialsStorage.LoadStoredCredentials();
            return false;
        }

        internal static bool IsAuthenticated()
        {
            if (!Credentials.LastUserRequest.HasValue || DateTime.Now.Subtract(Credentials.LastUserRequest.Value).TotalSeconds <= (Credentials.AuthenticationExpirationTime > 0 ? (double)Credentials.AuthenticationExpirationTime : (double)SecureHttpConstants.DefaultAuthenticationExpirationTime))
                return Credentials.Authenticated;
            return false;
        }
    }
}
