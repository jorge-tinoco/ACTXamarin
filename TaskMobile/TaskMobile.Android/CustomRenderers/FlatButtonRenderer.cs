using Xamarin.Forms.Platform.Android;
using Xamarin.Forms;
//using Android.Widget;
using TaskMobile.Droid.CustomRenderers;

[assembly: ExportRenderer(typeof(Button), typeof(  FlatButtonRenderer))]
namespace TaskMobile.Droid.CustomRenderers
{
    /// <summary>
    /// Created for enabling border radius and border color
    /// </summary>
    public class FlatButtonRenderer : ButtonRenderer
    {
        protected override void OnDraw(Android.Graphics.Canvas canvas)
        {
            base.OnDraw(canvas);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {
            base.OnElementChanged(e);
        }
    }
}
