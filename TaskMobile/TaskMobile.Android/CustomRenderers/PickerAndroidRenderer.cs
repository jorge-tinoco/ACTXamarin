using Android.Graphics;
using TaskMobile.Droid.CustomRenderers;
using TaskMobile.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(Picker), typeof(PickerAndroidRenderer))]
namespace TaskMobile.Droid.CustomRenderers
{
    public class PickerAndroidRenderer:PickerRenderer
    {
        /// <summary>
        /// Created for changing picker font size.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
        {
            base.OnElementChanged(e);

            if (e.OldElement != null || e.NewElement != null)
            {
                if ( Device.Idiom == TargetIdiom.Phone )
                    Control.TextSize *= 0.48f;
                else
                    Control.TextSize *= 0.95f;

            }
        }
    }
}