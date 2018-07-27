using Android.Graphics;
using TaskMobile.Droid.CustomRenderers;
using TaskMobile.UserControls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SquareButton), typeof(SquareButtonRenderer))]
namespace TaskMobile.Droid.CustomRenderers
{
    public class SquareButtonRenderer: ButtonRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {

            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                Control.Typeface = Typeface.CreateFromAsset(Forms.Context.Assets,
                    FontAwesome.FontAwesomeName + ".ttf");
            }
        }
    }
}