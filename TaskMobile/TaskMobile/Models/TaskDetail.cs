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
    public class TaskDetail
    {
        private int _TaskNumber;
        /// <summary>
        /// Task to which this detail belongs
        /// </summary>
        /// <remarks>Numeric with  9 positions</remarks>
        public int TaskNumber
        {
            get { return _TaskNumber; }
            set { _TaskNumber = value; }
        }

        private string _WorkOrder;
        /// <summary>
        /// Detail work order
        /// </summary>
        /// <remarks>Alphanumeric with 9 positions</remarks>
        public string WorkOrder
        {
            get { return _WorkOrder; }
            set { _WorkOrder = value; }
        }


        private string _Lot;
        /// <summary>
        /// Represents the lot number of this detail task.
        /// </summary>
        /// <remarks>Alphanumeric with 5 positions</remarks>
        public string Lot
        {
            get { return _Lot; }
            set { _Lot = value; }
        }


        private string _SapCode;
        /// <summary>
        /// Assigned sap code.
        /// </summary>
        /// <remarks>Alphanumeric with 18 characters</remarks>
        public string SapCode
        {
            get { return _SapCode; }
            set { _SapCode = value; }
        }


        private int _Pieces;
        /// <summary>
        /// Amount of pieces.
        /// </summary>
        /// <remarks>Numeric with 7 positions</remarks>
        public int Pieces
        {
            get { return _Pieces; }
            set { _Pieces = value; }
        }

        private double _Tons;
        /// <summary>
        /// Amount of tons.
        /// </summary>
        /// <remarks>Numeric with N9.3 format.</remarks>
        public double Tons
        {
            get { return _Tons; }
            set { _Tons = value; }
        }

        private string _StockType;
        /// <summary>
        /// Detail stock type.
        /// </summary>
        /// <remarks>Alphanumeric with 15 characters</remarks>
        public string StockType
        {
            get { return _StockType; }
            set { _StockType = value; }
        }

    }
}
