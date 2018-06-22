using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class FinishDetails : ContentPage
    {
        public FinishDetails()
        {
            InitializeComponent();
            SizeChanged += FinishDetails_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void FinishDetails_SizeChanged(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
            //FinishContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);

        }
    }
}
