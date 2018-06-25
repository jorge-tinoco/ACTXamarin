using Xamarin.Forms;

namespace TaskMobile.Views.Vehicle
{
    public partial class Change : ContentPage
    {
        public Change()
        {
            InitializeComponent();
            SizeChanged += Change_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void Change_SizeChanged(object sender, System.EventArgs e)
        {
            ChangeContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
