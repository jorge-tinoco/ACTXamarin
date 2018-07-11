﻿using System.Xml.Serialization;

namespace TaskMobile.WebServices.Entities
{
    /// <summary>
    ///  This class represents the <see cref="Common.Request{T}.MessageBody"/>.
    /// </summary>
    public class ActivityRequest
    {
        /// <summary>
        /// Task identifier that contains the requested activies to show.
        /// </summary>
        [XmlElement(ElementName = "taskId")]
        public int TaskId { get; set; }
    }
}
