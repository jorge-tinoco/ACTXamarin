using System.Xml.Serialization;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    ///  This class represents the <see cref="Common.Request{T}.MessageBody"/>.
    /// </summary>
    public class DetailsRequest
    {
        /// <summary>
        /// Task id that contains the details that will be shown.
        /// </summary>
        [XmlElement(ElementName = "taskId")]
        public int TaskId { get; set; }
    }
}
