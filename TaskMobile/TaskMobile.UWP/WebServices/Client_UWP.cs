using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using TaskMobile.WebServices;
using TaskMobile.UWP.WebServices;
using TaskMobile.WebServices.Entities.TMAP;

[assembly: Dependency(typeof(Client_UWP))]
namespace TaskMobile.UWP.WebServices
{
    public class Client_UWP : IClient
    {
        public string LOGGING_URL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TMAP_URL { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TMAP_KEY_PROXY { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public string UserDomain => throw new NotImplementedException();

        public string UserName => throw new NotImplementedException();

        public Authentication Authentication => throw new NotImplementedException();

        public int ExpirationTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Get<T>(string route, Action<T> callbackSuccess, Action<object> callbackError)
        {
            throw new NotImplementedException();
        }

        public void InitTMAP(Action<AuthResponse> callbackSuccess, Action<AuthResponse> callbackError)
        {
            throw new NotImplementedException();
        }

        public void Post<T>(string route, string data, Action<T> callbackSuccess, Action<object> callbackError)
        {
            throw new NotImplementedException();
        }

        public void SetCredentials(string domain, string user, string password)
        {
            throw new NotImplementedException();
        }
    }
}
