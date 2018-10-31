namespace TaskMobile.WebServices.Entities.TMAP
{
    /// <summary>
    /// Possible Tenaris TMAP responses.
    /// </summary>
    public enum TmapResponse
    {
        Ok,
        CertificateError,
        InvalidCredentials,
        NoNetworkConnection,
        UserDoNotHaveTmapAccessRights,
        MaxBadLogonReached,
        RequestTimeout,
        Unknow
    }
}
