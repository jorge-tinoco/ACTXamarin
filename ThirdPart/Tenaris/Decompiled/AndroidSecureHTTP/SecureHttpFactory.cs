// Decompiled with JetBrains decompiler
// Type: AndroidSecureHTTP.SecureHttpFactory
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using SecureHttpShared;

namespace AndroidSecureHTTP
{
  public class SecureHttpFactory
  {
    private static SecureHttp _authenticationRequester = (SecureHttp) null;
    private static SecureHttp _expirableAuthenticationRequester = (SecureHttp) null;
    private static SecureHttp _anonymousAuthenticationRequester = (SecureHttp) null;
    private static readonly object syncLock = new object();

    public static SecureHttp GetAuthenticationRequester(Context context)
    {
      lock (SecureHttpFactory.syncLock)
      {
        if (SecureHttpFactory._authenticationRequester == null)
        {
          SecureHttpFactory._authenticationRequester = (SecureHttp) new AuthenticateHttp(context);
          Credentials.CredentialsStorage = (ICredentialsStorage) new AndroidCredentialsStorage(context);
        }
        return SecureHttpFactory._authenticationRequester;
      }
    }

    public static SecureHttp GetExpirableAuthenticationRequester(Context context)
    {
      lock (SecureHttpFactory.syncLock)
      {
        if (SecureHttpFactory._expirableAuthenticationRequester == null)
          SecureHttpFactory._expirableAuthenticationRequester = (SecureHttp) new ExpirableAuthenticateHttp(context);
        return SecureHttpFactory._expirableAuthenticationRequester;
      }
    }

    public static SecureHttp GetAnonymousAuthenticationRequester(Context context)
    {
      lock (SecureHttpFactory.syncLock)
      {
        if (SecureHttpFactory._anonymousAuthenticationRequester == null)
          SecureHttpFactory._anonymousAuthenticationRequester = (SecureHttp) new AnonymousHttp(context);
        return SecureHttpFactory._anonymousAuthenticationRequester;
      }
    }
  }
}
