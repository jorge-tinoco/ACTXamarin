namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    /// Activities body response.
    /// </summary>
    /// <remarks>
    /// Represents the web service response  of the REST API.
    ///     The properties of this class are the <see cref="Common.Response{T}.MessageBody"/>.
    /// </remarks>
    public class ActivityUpdResponse
    {
        /// <summary>
        /// Result for the last action executed on the task.
        /// </summary>
        public string Result { get; set; }
    }
}
