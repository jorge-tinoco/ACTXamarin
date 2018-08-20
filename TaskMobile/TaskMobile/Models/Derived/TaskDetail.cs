
namespace TaskMobile.Models
{
    /// <summary>
    /// Complementary class with aditional attributes for using in the views.
    /// </summary>
    /// <remarks>
    /// Name space must be <see cref="TaskMobile.Models"/> in order to use this complementary class with principal class.
    /// </remarks>
    public partial class TaskDetail
    {
        private string _piecesText;
        private string _tonsText;
        private string _textStyle;
        private bool _rowIsHeader = false;

        /// <summary>
        /// Used for store Pieces header in grids, and  also the <see cref="Models.TaskDetail.Pieces"/> attribute.
        /// </summary>
        public string PiecesText
        {
            get { return _piecesText; }
            set { _piecesText = value; }
        }

        /// <summary>
        /// Used for store Tons header in grids, and  also the <see cref="Models.TaskDetail.Tons"/> attribute.
        /// </summary>
        public string TonsText
        {
            get { return _tonsText; }
            set { _tonsText = value; }
        }

        /// <summary>
        /// Style used in  the view. Property useful only when task details headers are used.
        /// </summary>
        public string TextStyle
        {
            get { return _textStyle; }
            set { _textStyle = value; }
        }


        /// <summary>
        /// Says if the current item is row header. 
        /// </summary>
        public bool RowIsHeader
        {
            get { return _rowIsHeader; }
            set { _rowIsHeader = value; }
        }


    }
}
