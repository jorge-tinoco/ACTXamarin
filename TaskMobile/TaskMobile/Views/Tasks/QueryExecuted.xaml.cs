using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class QueryExecuted : ContentPage
    {
        public QueryExecuted()
        {
            InitializeComponent();
            SizeChanged += QueryExecuted_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void QueryExecuted_SizeChanged(object sender, System.EventArgs e)
        {
            ExecutedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
