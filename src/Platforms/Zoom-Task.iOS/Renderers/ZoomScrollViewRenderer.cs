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
            DidZoom += ZoomScrollViewRenderer_DidZoom;
            ZoomingEnded += ZoomScrollViewRenderer_ZoomingEnded;
        }

        private void ZoomScrollViewRenderer_ZoomingEnded(object sender, ZoomingEndedEventArgs e)
        {
            if(_ZoomScrollView != null)
            {
                System.Diagnostics.Debug.WriteLine($"ZoomingEnded : {Zooming}, ZoomScale : {ZoomScale}, CurrentZoomScale : {_ZoomScrollView.CurrentZoomScale}");
                _ZoomScrollView.CurrentZoomScale = (float)ZoomScale;
            }
        }

        private void ZoomScrollViewRenderer_DidZoom(object sender, System.EventArgs e)
        {
            if(_ZoomScrollView != null)
            {
                System.Diagnostics.Debug.WriteLine($"Zooming : {Zooming}, ZoomScale : {ZoomScale}, CurrentZoomScale : {_ZoomScrollView.CurrentZoomScale}");
                _ZoomScrollView.CurrentZoomScale = (float)ZoomScale;
            }
        }

        private void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ZoomScrollView.MinimumZoomScale) ||
                args.PropertyName == nameof(ZoomScrollView.MaximumZoomScale))
            {
                UpdateMinMaxScale();
            }
            else if(args.PropertyName == nameof(ZoomScrollView.CurrentZoomScale))
            {
                UpdateCurrentZoomScale();
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

        private void UpdateCurrentZoomScale()
        {
            if (_ZoomScrollView != null && _ZoomScrollView.CurrentZoomScale != ZoomScale)
            {   
                SetZoomScale(_ZoomScrollView.CurrentZoomScale, false);
            }
        }
    }
}