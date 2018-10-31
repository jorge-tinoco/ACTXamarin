using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TaskMobile.WebServices;
using TaskMobile.iOS.WebServices;
using TaskMobile.WebServices.Entities.TMAP;
using Newtonsoft.Json;
using System.Linq;

[assembly: Dependency(typeof(Client_iOS))]
namespace TaskMobile.iOS.WebServices
{
    class Client_iOS : IClient
    {
        string IClient.LOGGING_URL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IClient.TMAP_URL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IClient.TMAP_KEY_PROXY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        string IClient.UserDomain => throw new NotImplementedException();

        string IClient.UserName => throw new NotImplementedException();

        Authentication IClient.Authentication => throw new NotImplementedException();

        int IClient.ExpirationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void IClient.Get<T>(string route, Action<T> callbackSuccess, Action<object> callbackError)
        {
            throw new NotImplementedException();
        }

        void IClient.InitTMAP(Action<AuthResponse> callbackSuccess, Action<AuthResponse> callbackError)
        {
            throw new NotImplementedException();
        }

        void IClient.Post<T>(string route, string data, Action<T> callbackSuccess, Action<object> callbackError)
        {
            throw new NotImplementedException();
        }

        void IClient.SetCredentials(string domain, string user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
