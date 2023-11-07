using System;
using System.ComponentModel;
using Android.Content;
using Android.Graphics;
using Android.Views;
using Android.Views.InputMethods;
using Com.Otaliastudios.Zoom;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Zoom_Task.Core.Controls;
using Zoom_Task.Droid.Renderers;
using AndroidView = Android.Views.View;
using FormsPlatform = Xamarin.Forms.Platform.Android.Platform;

[assembly: ExportRenderer(typeof(ZoomScrollView), typeof(ZoomScrollViewRenderer))]
namespace Zoom_Task.Droid.Renderers
{
    public class ZoomScrollViewRenderer : ViewRenderer<ZoomScrollView, ZoomLayout>, ZoomEngine.IListener
    {
        private ZoomLayout _zoomLayout;
        private AndroidView _content;
        private VisualElementTracker _contentTracker;
        private InputMethodManager _inputMethodManager;

        private double ScrolledXPosition = 0.0;
        private double ScrolledYPosition = 0.0;
        private double LastZoomedScale = 0.0;

        private ZoomScrollView _ZoomScrollView => Element as ZoomScrollView;

        public ZoomScrollViewRenderer(Context context) : base(context)
        {
            AutoPackage = false;
            _inputMethodManager = (InputMethodManager)context.GetSystemService(Context.InputMethodService);            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ZoomScrollView> args)
        {
            System.Diagnostics.Debug.WriteLine($"OnElementChanged : {args.NewElement}");
            if (args.OldElement != null)
            {
                UnhookScrollToRequestListener(args.OldElement);
            }

            base.OnElementChanged(args);

            if (args.NewElement != null)
            {
                _zoomLayout = GetOrCreateZoomLayout();
                _zoomLayout.SetHasClickableChildren(true);
                _zoomLayout.SetTransformation(ZoomLayout.InterfaceConsts.TransformationNone, (int)(GravityFlags.Top | GravityFlags.Left));
                //_zoomLayout.SetSmallerPolicy(ZoomLayout.InterfaceConsts.SmallerPolicyFromTransformation);
                SetNativeControl(_zoomLayout);

                _content = CreateScrollViewContent();
                if (_content != null)
                {
                    _zoomLayout.AddView(_content);
                }

                UpdateMinMaxScale();
                UpdateScrollbars();
                HookScrollToRequestListener(args.NewElement);
            }
        }

        public void OnIdle(ZoomEngine engine)
        {
            Element.SendScrollFinished();  
        }

        public void OnUpdate(ZoomEngine engine, Matrix transform)
        {
            double panX = Context.FromPixels(engine.PanX);
            double panY = Context.FromPixels(engine.PanY);

            //System.Diagnostics.Debug.WriteLine($"ZoomScrollView OnUpdate : {panX}, {panY}, ZoomLayout.Zoom =  {_zoomLayout.Zoom}, LastZoomedScale : {LastZoomedScale}");
            if (_ZoomScrollView.ScrollX == panX && _ZoomScrollView.ScrollY == panY)
                return;

            ScrolledXPosition = panX;
            ScrolledYPosition = panY;
            LastZoomedScale = _zoomLayout.Zoom;
            _ZoomScrollView.CurrentZoomScale = _zoomLayout.Zoom;

            Element.SetScrolledPosition(ScrolledXPosition, ScrolledYPosition);
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnElementPropertyChanged(sender, args);

           //System.Diagnostics.Debug.WriteLine($"ZoomScrollView : OnElementPropertyChanged : {args.PropertyName}, Zoom : {_zoomLayout.Zoom}, Zoom Condition : {_zoomLayout.Zoom > Element.MinimumZoomScale}");

            if (args.PropertyName == nameof(ZoomScrollView.MinimumZoomScale) ||
                args.PropertyName == nameof(ZoomScrollView.MaximumZoomScale))
            {
                UpdateMinMaxScale();
            }
            else if (args.PropertyName == nameof(ZoomScrollView.CurrentZoomScale))
            {
                UpdateCurrentZoomScale();
            }

            _zoomLayout.SetOverScrollHorizontal(_zoomLayout.Zoom > Element.MinimumZoomScale);
        }

        private void OnScrollRequested(object sender, ScrollToRequestedEventArgs args)
        {
            float toX = Context.ToPixels(args.ScrollX);
            float toY = Context.ToPixels(args.ScrollY);
           
            if (args.Mode == ScrollToMode.Element)
            {
                Xamarin.Forms.Point itemPosition = Element.GetScrollPositionForElement(args.Element as VisualElement, args.Position);

                toX = Context.ToPixels(itemPosition.X);
                toY = Context.ToPixels(itemPosition.Y);
            }

            _zoomLayout.PanTo(toX, toY, args.ShouldAnimate);
        }

        private ZoomLayout GetOrCreateZoomLayout()
        {
            ZoomLayout result = GetChildOfType<ZoomLayout>() ?? new ZoomLayout(Context);

            result.RemoveAllViews();
            result.OverScrollMode = OverScrollMode.IfContentScrolls;
            result.SetOverScrollHorizontal(false);
            result.SetOverPinchable(false);
            return result;
        }

        private TView GetChildOfType<TView>() where TView : AndroidView
        {
            for (int i = 0; i < ChildCount; ++i)
            {
                AndroidView child = GetChildAt(i);

                if (child is TView)
                {
                    return child as TView;
                }
            }

            return null;
        }

        private AndroidView CreateScrollViewContent()
        {
            Xamarin.Forms.View content = _ZoomScrollView.Content;

            if (content != null)
            {
                IVisualElementRenderer renderer = FormsPlatform.GetRenderer(content);

                if (renderer == null)
                {
                    renderer = FormsPlatform.CreateRendererWithContext(content, Context);
                    FormsPlatform.SetRenderer(content, renderer);
                }

                if (renderer.View.Parent != null)
                {
                    renderer.View.RemoveFromParent();
                }

                _contentTracker = new VisualElementTracker(renderer);
                _contentTracker.UpdateLayout();

                return renderer.View;
            }

            return null;
        }

        private void HookScrollToRequestListener(ZoomScrollView view)
        {
            _zoomLayout.Engine.AddListener(this);
            view.ScrollToRequested += OnScrollRequested;            
        }

        private void UnhookScrollToRequestListener(ZoomScrollView view)
        {
            _zoomLayout.Engine.RemoveListener(this);
            view.ScrollToRequested -= OnScrollRequested;
        }

        protected override void OnLayout(bool changed, int l, int t, int r, int b)
        {
            bool isKeyboardFocused = false;
            try
            {
                isKeyboardFocused = _inputMethodManager.IsAcceptingText;
                System.Diagnostics.Debug.WriteLine($"OnLayout : isKeyboardFocused : {isKeyboardFocused}");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception while checking for input : {ex.Message}");
            }

            if(!isKeyboardFocused)
            {
                base.OnLayout(changed, l, t, r, b);
                _contentTracker?.UpdateLayout();
            }

            System.Diagnostics.Debug.WriteLine($"OnLayout : Setting ScrolledPosition : {ScrolledXPosition}, {ScrolledYPosition}");
            Element.SetScrolledPosition(ScrolledXPosition, ScrolledYPosition);
        }

        private void UpdateMinMaxScale()
        {
            System.Diagnostics.Debug.WriteLine("Update MinMax Scale");
            if (_zoomLayout != null && _ZoomScrollView != null)
            {
                _zoomLayout.SetMinZoom(_ZoomScrollView.MinimumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
                _zoomLayout.SetMaxZoom(_ZoomScrollView.MaximumZoomScale, ZoomLayout.InterfaceConsts.TypeZoom);
            }
        }

        private void UpdateCurrentZoomScale()
        {
            if (_zoomLayout != null && _ZoomScrollView != null && _ZoomScrollView.CurrentZoomScale != _zoomLayout.Zoom)
            {
                System.Diagnostics.Debug.WriteLine($"Set Zoom to : {_ZoomScrollView.CurrentZoomScale}, From : {_zoomLayout.Zoom}");
                _zoomLayout.ZoomTo(_ZoomScrollView.CurrentZoomScale, false);
            }
        }

        private void UpdateScrollbars()
        {
            System.Diagnostics.Debug.WriteLine("Update Scrollbars");
            if (_zoomLayout != null && _ZoomScrollView != null)
            {
                _zoomLayout.SetHorizontalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Horizontal ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
                _zoomLayout.SetVerticalPanEnabled(_ZoomScrollView.Orientation == ScrollOrientation.Vertical ||
                    _ZoomScrollView.Orientation == ScrollOrientation.Both);
            }
        }
    }
}

