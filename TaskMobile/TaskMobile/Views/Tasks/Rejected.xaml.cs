using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class Rejected : ContentPage
    {
        public Rejected()
        {
            InitializeComponent();
            SizeChanged += Rejected_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void Rejected_SizeChanged(object sender, System.EventArgs e)
        {
            RejectedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize );
        }
    }
}
