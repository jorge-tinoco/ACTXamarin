using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class Response<T> where T : new()
    {
        public Response()
        {
            MessageLog = new MessageLog();
            MessageBody = new T();
        }

        public T MessageBody { get; set; }
        public MessageLog MessageLog { get; set; }

    }
}
