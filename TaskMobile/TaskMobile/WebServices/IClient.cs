using System;
using System.Collections.Generic;
using TaskMobile.WebServices.Entities.TMAP;
using System.Threading.Tasks;

namespace TaskMobile.WebServices
{
    /// <summary>
    ///  Dependency injection interface for TMAP web services implementation.
    /// </summary>
    /// <remarks>
    ///     Used for make platform specific implementation for web services.
    /// </remarks>
    public interface IClient

    {
        string LOGGING_URL { get; set; }

        string TMAP_URL { get; set; }

        /// <summary>
        /// TMAP key for querying.
        /// </summary>
        /// <example>
        ///     APP-ACT
        /// </example>
        string TMAP_KEY_PROXY { get; set; }
        
        /// <summary>
        /// User domain stablished in TMAP client.
        /// </summary>
        string UserDomain { get; }

        /// <summary>
        /// User name stablished in TMAP client.
        /// </summary>
        string UserName { get; }

        /// <summary>
        /// Authentication type.
        /// </summary>
        Authentication Authentication { get; }

        /// <summary>
        /// Expiration time in seconds.
        /// </summary>
        int ExpirationTime { get; set; }

        void SetCredentials(string domain, string user, string password);

        void InitTMAP(Action<AuthResponse> callbackSuccess, Action<AuthResponse> callbackError);

        void Post<T>(string route, string data, Action<T> callbackSuccess, Action<object> callbackError);

        void Get<T>(string route, Action<T> callbackSuccess, Action<object> callbackError);
    }
}
