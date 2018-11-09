// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.ICredentialsStorage
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

namespace SecureHttpShared
{
  public interface ICredentialsStorage
  {
    void StoreCredentials(string proxyUrl, string keyProxy, string domain, string user, string password);

    void RemoveStoredCredentials();

    bool LoadStoredCredentials();
  }
}
