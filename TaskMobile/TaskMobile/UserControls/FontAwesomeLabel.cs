﻿using Xamarin.Forms;

namespace TaskMobile.UserControls
{
    
    /// <summary>
    /// Class for using FontAwesome in XAML namespaces.
    /// </summary>
    public class FontAwesomeLabel : Label
    {
        //Parameterless constructor for XAML
        public FontAwesomeLabel()
        {
            FontFamily = FontAwesome.FontAwesomeName;
        }

        public FontAwesomeLabel(string fontAwesomeLabel = null)
        {
            FontFamily = FontAwesome.FontAwesomeName;
            Text = fontAwesomeLabel;
        }
    }

   
}
