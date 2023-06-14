using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Zoom_Task
{
    public partial class App : Application
    {
        public static bool IsEntryFocused { get; set; }

        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());
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
