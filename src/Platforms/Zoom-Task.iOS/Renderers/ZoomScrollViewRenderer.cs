using System.ComponentModel;
using System.Linq;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Zoom_Task.Core.Controls;
using Zoom_Task.iOS.Renderers;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]

namespace Zoom_Task.iOS.Renderers
{
    public class ZoomScrollViewRenderer : ScrollViewRenderer
    {
        private ZoomScrollView _ZoomScrollView => Element as ZoomScrollView;

        protected override void OnElementChanged(VisualElementChangedEventArgs args)
        {
            if (args.OldElement != null)
            {
                args.OldElement.PropertyChanged -= OnElementPropertyChanged;
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                args.NewElement.PropertyChanged += OnElementPropertyChanged;
            }

            ViewForZoomingInScrollView = GetZoomSubView;
            UpdateMinMaxScale();
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ZoomScrollView.MinimumZoomScale) ||
                args.PropertyName == nameof(ZoomScrollView.MaximumZoomScale))
            {
                UpdateMinMaxScale();
            }
        }

        private UIView GetZoomSubView(UIScrollView scrollView)
        {
            return scrollView.Subviews?.FirstOrDefault();
        }

        private void UpdateMinMaxScale()
        {
            if (_ZoomScrollView != null)
            {
                MinimumZoomScale = _ZoomScrollView.MinimumZoomScale;
                MaximumZoomScale = _ZoomScrollView.MaximumZoomScale;
            }
        }
    }
}

