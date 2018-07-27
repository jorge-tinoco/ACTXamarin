using Xamarin.Forms;

namespace TaskMobile.UserControls
{
    public class SquareButton: Button
    {
        //Parameterless constructor for XAML
        public SquareButton()
        {
            FontFamily = FontAwesome.FontAwesomeName;
        }

        public SquareButton(string fontAwesomeLabel = null)
        {
            FontFamily = FontAwesome.FontAwesomeName;
            Text = fontAwesomeLabel;
        }
    }
}
