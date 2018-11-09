// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.AnonymousHttp
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using System;

namespace SecureHttpShared
{
  public class AnonymousHttp : SecureHttp
  {
    internal AnonymousHttp()
    {
    }

    internal AnonymousHttp(Context context)
    {
      this._context = context;
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
      this.GetStoredCertificate("");
      this._initReady = true;
      callbackSuccess(InitResponse.OK);
    }

    public override bool IsAuthenticated()
    {
      return true;
    }

    public override string GetUser()
    {
      return string.Empty;
    }
  }
}
