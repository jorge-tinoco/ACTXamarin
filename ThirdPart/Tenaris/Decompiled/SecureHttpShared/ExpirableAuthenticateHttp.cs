// Decompiled with JetBrains decompiler
// Type: SecureHttpShared.ExpirableAuthenticateHttp
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using System;
using System.Collections.Generic;

namespace SecureHttpShared
{
  internal class ExpirableAuthenticateHttp : AuthenticateHttp
  {
    internal ExpirableAuthenticateHttp()
    {
    }

    internal ExpirableAuthenticateHttp(Context context)
      : base(context)
    {
    }

    protected override void SetAuthenticated()
    {
      base.SetAuthenticated();
      Credentials.UpdateLastUserRequest();
    }

    public override void Get(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
    {
      if (this.IsAuthenticated())
      {
        Credentials.UpdateLastUserRequest();
        base.Get(requestUriString, additionalHeaders, contentType, callbackSuccess, callbackError);
      }
      else
        callbackError(SecureHttpConstants.AuthenticationExpiredError);
    }

    public override void Post(string requestUriString, string data, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
    {
      if (this.IsAuthenticated())
      {
        Credentials.UpdateLastUserRequest();
        base.Post(requestUriString, data, additionalHeaders, contentType, callbackSuccess, callbackError);
      }
      else
        callbackError(SecureHttpConstants.AuthenticationExpiredError);
    }

    public override void UploadFile(string requestUriString, List<KeyValuePair<string, string>> additionalHeaders, string contentType, byte[] formData, Action<string> callbackSuccess, Action<string> callbackError)
    {
      if (this.IsAuthenticated())
      {
        Credentials.UpdateLastUserRequest();
        base.UploadFile(requestUriString, additionalHeaders, contentType, formData, callbackSuccess, callbackError);
      }
      else
        callbackError(SecureHttpConstants.AuthenticationExpiredError);
    }

    public override void DownloadFile(string requestUriString, string filePath, string fileName, List<KeyValuePair<string, string>> additionalHeaders, string contentType, Action<string> callbackSuccess, Action<string> callbackError)
    {
      if (this.IsAuthenticated())
      {
        Credentials.UpdateLastUserRequest();
        base.DownloadFile(requestUriString, filePath, fileName, additionalHeaders, contentType, callbackSuccess, callbackError);
      }
      else
        callbackError(SecureHttpConstants.AuthenticationExpiredError);
    }
  }
}
