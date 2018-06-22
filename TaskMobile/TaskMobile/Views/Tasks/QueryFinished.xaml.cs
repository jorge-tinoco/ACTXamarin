using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class QueryFinished : ContentPage
    {
        public QueryFinished()
        {
            InitializeComponent();
            SizeChanged += QueryFinished_SizeChanged;
        }
        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void QueryFinished_SizeChanged(object sender, System.EventArgs e)
        {
            FinishedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
