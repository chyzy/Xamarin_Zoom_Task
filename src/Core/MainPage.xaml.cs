using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Zoom_Task.Core;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async void ExampleScrollPage(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ZoomableScrollViewPage());
        }

        private async void ExampleScrollPage2(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new ZoomScrollViewPage());
        }

        private async void PanGesturePage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PanGesturePage());
        }
    }
}
