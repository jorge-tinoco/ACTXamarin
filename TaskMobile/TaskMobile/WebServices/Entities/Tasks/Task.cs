using System;
using System.Collections.Generic;

namespace TaskMobile.WebServices.Entities.Tasks
{
    public class Task
    {
        public int ID { get; set; }
        public string OPERATION_NAME { get; set; }
        public string TYPE_STOCK { get; set; }
        public string STATUS { get; set; }
        public string ORIGIN_LOCATION { get; set; }
        public string OP { get; set; }
        public string UAC { get; set; }
        public string CVP { get; set; }
        public string REMISSION { get; set; }
        public int ORIGIN_GEOREFERENCE { get; set; }
        public string ORIGIN_DEPOSIT { get; set; }
        public string ORIGIN_NAME { get; set; }
        public int DESTINY_GEOREFERENCE { get; set; }
        public string DESTINY_DEPOSIT { get; set; }
        public string DESTINY_NAME { get; set; }
        public double T_TONS { get; set; }
        public int T_PIECES { get; set; }
        public string FLAT_VEHICLE { get; set; }
        public int FLAGSYSTEM { get; set; }
        public DateTime CREATED_DATE { get; set; }
    }
}
