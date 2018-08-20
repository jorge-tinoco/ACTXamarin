using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskMobile.Models
{
    /// <summary>
    /// Represent a single detail for a <see cref="Models.Task"/> entity.
    /// </summary>
    public partial class TaskDetail
    {
        private int _TaskNumber;
        private string _WorkOrder;
        private string _Lot;
        private string _SapCode;
        private int _Pieces;
        private double _Tons;
        private string _StockType;
        private string _origin;
        private string _destination;
        /// <summary>
        /// Task to which this detail belongs
        /// </summary>
        /// <remarks>Numeric with  9 positions</remarks>
        public int TaskNumber
        {
            get { return _TaskNumber; }
            set { _TaskNumber = value; }
        }

        /// <summary>
        /// Detail work order
        /// </summary>
        /// <remarks>Alphanumeric with 9 positions</remarks>
        public string WorkOrder
        {
            get { return _WorkOrder; }
            set { _WorkOrder = value; }
        }

        /// <summary>
        /// Represents the lot number of this detail task.
        /// </summary>
        /// <remarks>Alphanumeric with 5 positions</remarks>
        public string Lot
        {
            get { return _Lot; }
            set { _Lot = value; }
        }

        /// <summary>
        /// Assigned sap code.
        /// </summary>
        /// <remarks>Alphanumeric with 18 characters</remarks>
        public string SapCode
        {
            get { return _SapCode; }
            set { _SapCode = value; }
        }

        /// <summary>
        /// Amount of pieces.
        /// </summary>
        /// <remarks>Numeric with 7 positions</remarks>
        public int Pieces
        {
            get { return _Pieces; }
            set { _Pieces = value; }
        }

        /// <summary>
        /// Amount of tons.
        /// </summary>
        /// <remarks>Numeric with N9.3 format.</remarks>
        public double Tons
        {
            get { return _Tons; }
            set { _Tons = value; }
        }

        /// <summary>
        /// Detail stock type.
        /// </summary>
        /// <remarks>Alphanumeric with 15 characters</remarks>
        public string StockType
        {
            get { return _StockType; }
            set { _StockType = value; }
        }

        /// <summary>
        /// Origin location.
        /// </summary>
        public string Origin
        {
            get { return _origin; }
            set { _origin = value; }
        }

        /// <summary>
        /// Destination Location.
        /// </summary>
        public string Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }



    }
}
