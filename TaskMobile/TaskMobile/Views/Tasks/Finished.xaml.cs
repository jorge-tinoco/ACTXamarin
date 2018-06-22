using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class Finished : ContentPage
    {
        public Finished()
        {
            InitializeComponent();
            SizeChanged += Finished_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void Finished_SizeChanged(object sender, System.EventArgs e)
        {
            FinishContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
