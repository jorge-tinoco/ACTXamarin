using System;
using System.IO;
using Xamarin.Forms;

namespace TaskMobile.Droid.Utilities
{
    internal class File
    {
        /// <summary>
        /// Read a file from android asset and return the content.
        /// </summary>
        /// <param name="name">File name to read.</param>
        /// <returns>File content in string format.</returns>
        internal string ReadFromAssets(string name)
        {
            Stream stream;
            string content = "";
            try
            {
                stream = Forms.Context.Assets.Open(name);
                using (StreamReader sr = new StreamReader(stream))
                {
                    content = sr.ReadToEnd();
                    stream.Close();
                }
                return content;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}