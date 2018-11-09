// Decompiled with JetBrains decompiler
// Type: AndroidSecureHTTP.AndroidCredentialsStorage
// Assembly: AndroidSecureHTTP, Version=1.8.6.0, Culture=neutral, PublicKeyToken=null
// MVID: 9176A26D-3D63-4AA8-9ADD-EA7425EBC381
// Assembly location: C:\opt\Sources\Tenaris.ILO.Mobile\ThirdPart\AndroidSecureHTTP.dll

using Android.Content;
using SecureHttpShared;

namespace AndroidSecureHTTP
{
  internal class AndroidCredentialsStorage : ICredentialsStorage
  {
    private const string PreferencesName = "AndroidCredentialsStorage";
    private readonly Context _context;

    public AndroidCredentialsStorage(Context context)
    {
      this._context = context;
    }

    public void StoreCredentials(string proxyUrl, string keyProxy, string domain, string user, string password)
    {
      ISharedPreferencesEditor preferencesEditor = this._context.GetSharedPreferences(nameof (AndroidCredentialsStorage), (FileCreationMode) 0).Edit();
      preferencesEditor.PutString(SecureHttpConstants.CredentialsProxyUrl, proxyUrl);
      preferencesEditor.PutString(SecureHttpConstants.CredentialsKeyProxy, keyProxy);
      preferencesEditor.PutString(SecureHttpConstants.CredentialsDomain, domain);
      preferencesEditor.PutString(SecureHttpConstants.CredentialsUser, user);
      preferencesEditor.PutString(SecureHttpConstants.CredentialsPassword, password);
      preferencesEditor.Commit();
    }

    public void RemoveStoredCredentials()
    {
      ISharedPreferencesEditor preferencesEditor = this._context.GetSharedPreferences(nameof (AndroidCredentialsStorage), (FileCreationMode) 0).Edit();
      preferencesEditor.Remove(SecureHttpConstants.CredentialsProxyUrl);
      preferencesEditor.Remove(SecureHttpConstants.CredentialsKeyProxy);
      preferencesEditor.Remove(SecureHttpConstants.CredentialsDomain);
      preferencesEditor.Remove(SecureHttpConstants.CredentialsUser);
      preferencesEditor.Remove(SecureHttpConstants.CredentialsPassword);
      preferencesEditor.Commit();
    }

    public bool LoadStoredCredentials()
    {
      ISharedPreferences sharedPreferences = this._context.GetSharedPreferences(nameof (AndroidCredentialsStorage), (FileCreationMode) 0);
      if (!sharedPreferences.Contains(SecureHttpConstants.CredentialsProxyUrl) || !sharedPreferences.Contains(SecureHttpConstants.CredentialsKeyProxy) || (!sharedPreferences.Contains(SecureHttpConstants.CredentialsDomain) || !sharedPreferences.Contains(SecureHttpConstants.CredentialsUser)) || !sharedPreferences.Contains(SecureHttpConstants.CredentialsPassword))
        return false;
      Credentials.ProxyUrl = sharedPreferences.GetString(SecureHttpConstants.CredentialsProxyUrl, "");
      Credentials.KeyProxy = sharedPreferences.GetString(SecureHttpConstants.CredentialsKeyProxy, "");
      Credentials.Domain = sharedPreferences.GetString(SecureHttpConstants.CredentialsDomain, "");
      Credentials.User = sharedPreferences.GetString(SecureHttpConstants.CredentialsUser, "");
      Credentials.Password = sharedPreferences.GetString(SecureHttpConstants.CredentialsPassword, "");
      return true;
    }
  }
}
