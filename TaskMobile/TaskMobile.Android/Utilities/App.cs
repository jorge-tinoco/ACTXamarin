using Android.Content;
using Android.Content.PM;

namespace TaskMobile.Droid.Utilities
{
    /// <summary>
    /// Useful application data.
    /// </summary>
    internal class App
    {
        static Context _context;
        private PackageInfo _info;

        public App()
        {
            _context= global::Android.App.Application.Context;
            PackageManager manager = _context.PackageManager;
            _info = manager.GetPackageInfo(_context.PackageName, 0);
        }

        /// <summary>
        /// Application name.
        /// </summary>
        internal string AppName
        {
            get { return _context.GetString(Resource.String.ApplicationName); }
        }

        /// <summary>
        /// Application version name. Example 1.3.2
        /// </summary>
        internal string VersionName
        {
            get { return _info.VersionName; }
        }

        /// <summary>
        /// Same as android VersionCode set in android properties.
        /// </summary>
        internal int BuildVersion
        {
            get{ return _info.VersionCode; }
        }
    }
}