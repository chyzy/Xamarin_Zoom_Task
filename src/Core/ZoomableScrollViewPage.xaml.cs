using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task.Core
{	
	public partial class ZoomableScrollViewPage : ContentPage
	{	
		public ZoomableScrollViewPage ()
		{
			InitializeComponent ();
            BindingContext = new MainPageViewModel();
        }
	}
}

