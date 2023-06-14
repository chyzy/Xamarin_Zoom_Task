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

        void Entry_Focused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Entry Focused");
            App.IsEntryFocused = true;
        }

        void Entry_Unfocused(System.Object sender, Xamarin.Forms.FocusEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Entry Unfocused");
            App.IsEntryFocused = false;
        }
    }
}

