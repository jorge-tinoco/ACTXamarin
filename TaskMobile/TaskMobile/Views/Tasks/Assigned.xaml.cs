using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TaskMobile.Views.Tasks
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Assigned : ContentPage
    {
        public Assigned()
        {
            InitializeComponent();
            SizeChanged += Assigned_SizeChanged;
        }

        /// <summary>
        /// Set the relative layout according Device Height.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Assigned_SizeChanged(object sender, EventArgs e)
        {
            AssignedContainer.HeightRequest = (this.Height - Utilities.Screen.FooterSize);
        }
    }
}
