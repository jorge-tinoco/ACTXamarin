using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class AssignedToExecuted : ContentPage
    {
        public AssignedToExecuted()
        {
            InitializeComponent();
            SizeChanged += AssignedToExecuted_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void AssignedToExecuted_SizeChanged(object sender, System.EventArgs e)
        {
            AssignedToExecutedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize );
        }
    }
}
