using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Zoom_Task.Core.Controls
{
    public class ZoomableScrollView : ScrollView
    {
        //If its possible implement pinch/pan logic here. If not it should be implemented in native renderers.
        //You can base on https://github.com/LuckyDucko/ZoomView.Forms/tree/master/ZoomView.Forms if you wish.

        public static readonly BindableProperty MaximumZoomProperty = BindableProperty.Create(nameof(MaximumZoom), typeof(double), typeof(ZoomableScrollView), 1d);

        public double MaximumZoom
        {
            get { return (double)GetValue(MaximumZoomProperty); }
            set { SetValue(MaximumZoomProperty, value); }
        }

        public static readonly BindableProperty MinimumZoomProperty = BindableProperty.Create(nameof(MinimumZoom), typeof(double), typeof(ZoomableScrollView), 1d);

        public double MinimumZoom
        {
            get { return (double)GetValue(MinimumZoomProperty); }
            set { SetValue(MinimumZoomProperty, value); }
        }
    }
}
