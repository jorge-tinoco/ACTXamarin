using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class Request<T> where T : new()
    {
        public Request()
        {
            if (MessageBody == null)
                MessageBody = new T();
        }
        public MessageHeader MessageHeader { get; set; }
        public T MessageBody { get; set; }

    }
}
