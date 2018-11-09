// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.SecureHttpConstants
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

namespace SecureHttpShared
{
  public class SecureHttpConstants
  {
    internal static readonly string WS_KEY = "key";
    internal static readonly string WS_VALID = "Valid";
    internal static readonly string WS_PROXY = "proxy";
    internal static readonly string CertificateFolder = "Certificates";
    internal static readonly string CertificateFileName = "userCert.crt";
    internal static readonly string PasswordFileName = "userCert.txt";
    internal static readonly string URL_AIRWATCH = "https://ds259.awmdm.com/DeviceServices";
    internal static readonly string AirWatchCertificateKey = "AirWatchCertificateData";
    internal static readonly string AirWatchPasswordKey = "AirWatchCertificatePassword";
    internal static readonly string CredentialsProxyUrl = nameof (CredentialsProxyUrl);
    internal static readonly string CredentialsKeyProxy = nameof (CredentialsKeyProxy);
    internal static readonly string CredentialsDomain = nameof (CredentialsDomain);
    internal static readonly string CredentialsUser = nameof (CredentialsUser);
    internal static readonly string CredentialsPassword = nameof (CredentialsPassword);
    internal static readonly int DefaultAuthenticationExpirationTime = 300;
    internal static readonly int AirWatchTimeout = 30;
    internal static readonly int RequestTimeout = 80000;
    internal static readonly int DownloadTimeout = 80000;
    internal static readonly string AirWatchCertificateValidationUrl = "https://tpns.mobile.tenaris.com/TPNS/TPNSService.svc/isalive";
    internal static readonly string AirWatchRevokedCertificateError = "The certificate is revoked";
    public static readonly string AuthenticationExpiredError = nameof (AuthenticationExpiredError);
    public static readonly string NetworkConnectionError = "Device is not connected to any network";
  }
}
