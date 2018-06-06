﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using TaskMobile.DB;

namespace TaskMobile
{
    public partial class App : Application
    {
        public static MasterDetailPage MasterD { get; set; }
        public static EmployeeREPO EmployeeRepo;

        public App()
        {
            InitializeComponent();
            MainPage = new TaskMobile.MainPage(); 
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
