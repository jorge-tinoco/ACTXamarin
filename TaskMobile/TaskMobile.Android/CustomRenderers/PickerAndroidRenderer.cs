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
                var Small = Device.GetNamedSize(NamedSize.Small, typeof(Picker));
                var Medium = Device.GetNamedSize(NamedSize.Medium, typeof(Picker));
                var Large = Device.GetNamedSize(NamedSize.Large, typeof(Picker));
                var Micro = Device.GetNamedSize(NamedSize.Micro, typeof(Picker));
                var Default = Device.GetNamedSize(NamedSize.Default, typeof(Picker));
                switch (Device.Idiom)
                {
                    case TargetIdiom.Unsupported:
                        Control.TextSize = (float)Medium;
                        break;
                    case TargetIdiom.Phone:
                        Control.TextSize = 14;
                        break;
                    case TargetIdiom.Tablet:
                        Control.TextSize = 22;
                        break;
                    case TargetIdiom.Desktop:
                        Control.TextSize = 28;
                        break;
                    default:
                        Control.TextSize = (float)Medium;
                        break;
                }
            }
        }
    }
}