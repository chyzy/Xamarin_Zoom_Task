﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoom_Task.Core;

namespace Zoom_Task
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new ZoomScrollViewPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
