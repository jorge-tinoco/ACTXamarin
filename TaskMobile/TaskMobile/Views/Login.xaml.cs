using Xamarin.Forms;

namespace TaskMobile.Views
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
            SizeChanged += Login_SizeChanged;
        }

        /// <summary>
        /// Change the page container for according device dimensions.
        /// </summary>
        private void Login_SizeChanged(object sender, System.EventArgs e)
        {
            LoginContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize );
        }
    }
}
