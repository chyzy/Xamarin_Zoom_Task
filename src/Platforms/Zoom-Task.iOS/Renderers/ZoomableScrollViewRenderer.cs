﻿using System;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Zoom_Task.Core.Controls;
using Zoom_Task.iOS.Renderers;

[assembly: ExportRenderer(typeof(ZoomableScrollView), typeof(ZoomableScrollViewRenderer))]
namespace Zoom_Task.iOS.Renderers
{
    public class ZoomableScrollViewRenderer : ScrollViewRenderer
    {
        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            var element = e.NewElement as ZoomableScrollView;
            if (element != null)
            {
                MaximumZoomScale = (nfloat)element.MaximumZoom;
                MinimumZoomScale = (nfloat)element.MinimumZoom;
                MultipleTouchEnabled = true;
                ShowsVerticalScrollIndicator =
                    ShowsHorizontalScrollIndicator = false;

                ViewForZoomingInScrollView = v => v.Subviews[0];
                DidZoom += (sender, args) =>
                {
                    CenterContent();
                };
            }
        }

        public override void LayoutSubviews()
        {
            base.LayoutSubviews();

            var view = Subviews[0];
            if (Element != null)
            {
                ContentSize = view.Frame.Size;
            }
        }

        public override bool TouchesShouldCancelInContentView(UIView view)
        {
            //NOTE: We need zooming to be allowed when you happen to tap a UIButton. Overriding this method, fixes our case.
            return true;
        }

        private void CenterContent()
        {
            nfloat top = 0, left = 0;

            if (ContentSize.Width < Bounds.Size.Width)
            {
                left = (Bounds.Size.Width - ContentSize.Width) * 0.5f;
            }
            if (ContentSize.Height < Bounds.Size.Height)
            {
                top = (Bounds.Size.Height - ContentSize.Height) * 0.5f;
            }
            ContentInset = new UIEdgeInsets(top, left, top, left);
        }
    }
}

