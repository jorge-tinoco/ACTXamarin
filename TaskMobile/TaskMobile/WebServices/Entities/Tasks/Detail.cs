using System.Collections.Generic;

namespace TaskMobile.WebServices.Entities.Tasks
{
    public class Detail
    {
        public int ID { get; set; }
        public string OP { get; set; }
        public string CVP { get; set; }
        public string LOTE { get; set; }
        public string LINGED { get; set; }
        public string MARKET { get; set; }
        public double MIN_RANGE { get; set; }
        public double MAX_RANGE { get; set; }
        public string STEEL_GRADE { get; set; }
        public string DIAMETER_INCHE { get; set; }
        public string DIAMETER_MM { get; set; }
        public int PIECES { get; set; }
        public double METERS { get; set; }
        public double TONS { get; set; }
        public string PROCESSING_SATE { get; set; }
        public string ORIG_LOCATION { get; set; }
        public string DEST_LOCATION { get; set; }
        public string SYMBOL { get; set; }
    }
}
