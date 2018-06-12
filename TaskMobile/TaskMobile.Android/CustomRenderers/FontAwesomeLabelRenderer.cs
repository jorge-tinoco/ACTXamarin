using Android.Graphics;
using TaskMobile.Droid.CustomRenderers;
using TaskMobile.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof( FontAwesomeLabel), typeof(FontAwesomeLabelRenderer))]
namespace TaskMobile.Droid.CustomRenderers
{
    public class FontAwesomeLabelRenderer : LabelRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets,
                    FontAwesomeLabel.FontAwesomeName + ".ttf");
            }
        }
    }
}