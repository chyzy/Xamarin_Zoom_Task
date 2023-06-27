using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task.Core
{	
	public partial class PanGesturePage : ContentPage
	{	
		public PanGesturePage ()
		{
			InitializeComponent ();
            BindingContext = new MainPageViewModel();
        }
	}
}

