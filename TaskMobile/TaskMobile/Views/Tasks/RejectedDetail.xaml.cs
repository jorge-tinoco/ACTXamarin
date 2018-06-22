using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class RejectedDetail : ContentPage
    {
        public RejectedDetail()
        {
            InitializeComponent();
            SizeChanged += RejectedDetail_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void RejectedDetail_SizeChanged(object sender, System.EventArgs e)
        {
            RejectedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
