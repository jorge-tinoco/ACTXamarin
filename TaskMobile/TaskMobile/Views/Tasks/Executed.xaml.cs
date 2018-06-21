using System;
using Xamarin.Forms;

namespace TaskMobile.Views.Tasks
{
    public partial class Executed : ContentPage
    {
        public Executed()
        {
            InitializeComponent();
            SizeChanged += Executed_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        private void Executed_SizeChanged(object sender, EventArgs e)
        {
            ExecutedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize );
        }
    }
}
