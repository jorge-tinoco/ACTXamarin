using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.WebServices.Entities.Common
{
    public class LogItem
    {
        public string ErrorCode { set; get; }

        public string ErrorDescription { set; get; }

        public double SeverityCode { set; get; }

        public LogItem()
        {
            ErrorCode = string.Empty;
            ErrorDescription = string.Empty;
            SeverityCode = 0;
        }

        public LogItem(string mCodigo,
                        string mDescripcion,
                        int mOrigen)
        {
            ErrorCode = mCodigo;
            ErrorDescription = mDescripcion;
            SeverityCode = mOrigen;
        }
    }
}
