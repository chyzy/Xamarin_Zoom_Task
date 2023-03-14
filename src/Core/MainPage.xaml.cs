using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            BindingContext = new MainPageViewModel();
        }

        protected override void OnAppearing()
        {
            DisplayAlert("Hi! Thank you for accepting the task.",
                "The goal is to modify the MainView scrollview so it supports pinch and pan gestures." +
                "You can modify the ZoomableScrollView.cs or create custom native renderes for that (Android and iOS)." +
                " The reference point is how the other apps (ex. Adobe Acrobat, Chrome etc.) render and zoom PDF.",
                "Good luck :)");
        }
    }
}
