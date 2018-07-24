using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Prism.Unity;
using Microsoft.Practices.Unity;

namespace TaskMobile.Droid
{
    [Activity(Label = "TaskMobile", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Landscape)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new TaskMobile.App(new AndroidPlatformInitializer()));
            // Comment or uncomment this part for using GORILA SDK
            //LoadApplication(UXDivers.Gorilla.Droid.Player.CreateApplication(
            //    this,
            //    new UXDivers.Gorilla.Config("Good Gorilla")
            //      // Register FontAwesome
            //      .RegisterAssemblyFromType<TaskMobile.UserControls.FontAwesomeLabel>()
            //      // Register circle buttons
            //      .RegisterAssemblyFromType<TaskMobile.UserControls.CircleButton>()
            //      // Register gradient colors
            //      .RegisterAssemblyFromType<TaskMobile.UserControls.GradientColor>()
            //      //Register Prism assemblies
            //      .RegisterAssemblyFromType<Prism.IActiveAware>()
            //      .RegisterAssemblyFromType<Prism.Mvvm.BindableBase>()
            //      .RegisterAssemblyFromType<Prism.Navigation.INavigatedAware>()
            //      .RegisterAssemblyFromType<Prism.Unity.PrismApplication>()
            //    ));



        }
        public class AndroidPlatformInitializer : IPlatformInitializer
        {
            public void RegisterTypes(IUnityContainer container)
            {

            }
        }
    }
}

