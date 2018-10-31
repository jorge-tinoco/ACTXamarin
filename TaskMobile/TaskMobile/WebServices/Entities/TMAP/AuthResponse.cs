
namespace TaskMobile.WebServices.Entities.TMAP
{
    /// <summary>
    /// TMAP authentication response.
    /// </summary>
    public class AuthResponse
    {
        public AuthResponse(TmapResponse response)
        {
            Response = response;
            MustLogin = false;
        }

        public AuthResponse(TmapResponse response, bool mustLogin)
        {
            Response = response;
            MustLogin = mustLogin;
        }

        public TmapResponse Response { get; }

        public bool MustLogin { get; }
    }
}
