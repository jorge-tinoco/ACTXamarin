using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using Prism.Services;
using TaskMobile.WebServices.Entities.Common;

namespace TaskMobile.WebServices.REST
{
    /// <summary>
    /// Client implementation for working with REST web service requests.
    /// </summary>
    public class Client
    {
        //public static SecureHttp Client = null
        private string WebServiceURL = @"https://reqres.in/api/users?page=2"; // Default(dummie) web service URL to query
        private string _route;
        private const string _tmapApp = "APP-ACT";
        private const string _iloBaseUrl = @"http://myyardtl-mx.tenaris.net/TaskManagerServices";
        protected IClient _secureClient;
        protected string _tmapAppName = "APP-ACT";
        protected string _tmapRoute = "GetVehicles";
        string _tmapUrl = "http://webapps2tl.tenaris.net/TenarisMobileAuthProxy";

        public Client()
        {

        }
        public Client(IClient clientService)
        {
            _secureClient = clientService;
        }

        public Client(string uri)
        {

        }


        /// <summary>
        /// TMAP app name.
        /// </summary>
        public string TMAPname
        {
            get { return _tmapApp; }
        }

        /// <summary>
        /// TMAP route.
        /// </summary>
        public string TMAPRoute
        {
            get { return _route; }
            set { _route = value; }
        }

        /// <summary>
        /// Generic method for consume REST web service,
        /// </summary>
        /// <remarks>
        /// TODO : deprecate this method. Instead, implement logic for using TMAP.
        /// </remarks>
        /// <typeparam name="T">Expected response object type.</typeparam>
        /// <returns>Response of <seealso cref="T"/> type.</returns>
        public async Task<T> Get<T>()
        {
            try
            {
                HttpClient Client = new HttpClient();
                var Response = await Client.GetAsync(WebServiceURL);
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var JSONstring = await Response.Content.ReadAsStringAsync();
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(JSONstring);
                }
                return default(T);
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message != null)
                    throw ex;
                else
                    throw new Exception("Hubo un error de red", ex.InnerException);
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        /// <summary>
        /// Call the generic class  <see cref="Get"/> for consuming REST web service.
        /// 
        /// </summary>
        /// <remarks>
        /// TODO : deprecate this method. Instead, implement logic for using TMAP.
        /// </remarks>
        /// <example>
        ///     <code>
        ///         <see cref="IClient"/> Client = new <seealso cref="REST.Client"/>();
        ///         Client.Get<T>(WebServiceURL);
        ///     </code>
        /// </example>
        /// <typeparam name="T">Generic of response type. </typeparam>
        /// <param name="URL">URL web services</param>
        /// <returns>Class of <seealso cref="T"/> type that contains returning object of requested web services.</returns>
        public async Task<T> Get<T>(string URL)
        {
            WebServiceURL = URL;
            if (string.IsNullOrEmpty(URL))
                throw new Exception("Es necesario definir el parámetro : URL");
            else
            {
                T Response = await Get<T>();
                return Response;
            }
        }

        /// <remarks>
        /// TODO : deprecate this method. Instead, implement logic for using TMAP.
        /// </remarks>
        public async Task<T> Post<T>(object request)
        {
            try
            {
                HttpClient Client = new HttpClient();
                Client.Timeout = new TimeSpan(0, 15, 0);
                Uri URL = new Uri(WebServiceURL, UriKind.Absolute);
                var ByteArray = Encoding.UTF8.GetBytes("T11092:trabajador0618");
                Client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(ByteArray));
                var Json = Newtonsoft.Json.JsonConvert.SerializeObject(request);
                var StringContent = new StringContent(Json, UnicodeEncoding.UTF8, "application/json");
                HttpResponseMessage Response = await Client.PostAsync(URL, StringContent);
                if (Response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    string JSONstring = await Response.Content.ReadAsStringAsync();
                    var Settings = new JsonSerializerSettings
                    {
                        Error = ResponseErrorHandler ,
                        NullValueHandling = NullValueHandling.Ignore
                    };
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(JSONstring, Settings);
                }
                return default(T);
            }
            catch (System.Net.WebException ex)
            {
                if (ex.Message != null)
                    throw ex;
                else
                    throw new Exception("Hubo un error de red", ex.InnerException);
            }
            catch (Exception e)
            {
                throw new Exception("Error al consumir el servicio: " + WebServiceURL, e);
            }
        }


        protected async Task<T> DoPost<T>(object request)
        {
            //string ServiceResponse = string.Empty;
            //string JsonData = JsonConvert.SerializeObject(request);
            //return await _secureClient.PostAsync<T>(_tmapRoute, JsonData,
            //                   response =>
            //                   {
            //                       var Settings = new JsonSerializerSettings
            //                       {
            //                           Error = ResponseErrorHandler,
            //                           NullValueHandling = NullValueHandling.Ignore
            //                       };
            //                       return JsonConvert.DeserializeObject<T>(response.ToString(), Settings);
            //                   },
            //                   error =>
            //                   {
            //                       //TO DO: Log the error
            //                       var errorcito = error;
            //                   });
            throw new NotImplementedException();
        }

        private void ResponseErrorHandler(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
    }
}
