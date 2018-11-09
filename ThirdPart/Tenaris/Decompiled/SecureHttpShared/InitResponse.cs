// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.InitResponse
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

namespace SecureHttpShared
{
  public enum InitResponse
  {
    OK,
    CertificateError,
    InvalidCredentials,
    NoNetworkConnection,
    UserDoNotHaveTMAPAccessRights,
    MaxBadLogonReached,
    RequestTimeout,
  }
}
