using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class ExecutedToFinish : ContentPage
    {
        public ExecutedToFinish()
        {
            InitializeComponent();
            SizeChanged += ExecutedToFinish_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void ExecutedToFinish_SizeChanged(object sender, System.EventArgs e)
        {
            //AssignedToExecutedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize );
        }
    }
}
