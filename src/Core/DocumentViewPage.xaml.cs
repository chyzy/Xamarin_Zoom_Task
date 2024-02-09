using SignaturePad.Forms;
using System.IO;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoom_Task.Core.Controls;
using Zoom_Task.Core.ViewModels; 

namespace Zoom_Task.Core
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentViewPage : ContentPage
    {
        private readonly DocumentViewPageViewModel _documentViewPageViewModel;
        private Image _signatureImageSource;
        private double _lastScrollXPos = -1;
        private double _lastScrollYPos = -1;
        private double _zoomedInScrollXPos = -1;
        private double _zoomedInScrollYPos = -1;
        private float _lastZoomedValue = -1;
        private float _fieldZoomFactor = 0.5f;
        private bool _isZoomingInProgress = false;

        public DocumentViewPage()
        {
            InitializeComponent();
            _documentViewPageViewModel = new DocumentViewPageViewModel();
            BindingContext = _documentViewPageViewModel;
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            _signatureImageSource = button.CommandParameter as Image;
            System.Diagnostics.Debug.WriteLine($"Button Clicked..., Command Parameter  {_signatureImageSource}");
            signatureView.Clear();
            SLSingatureView.IsVisible = true;
            SLSingatureView.ScaleTo(1, 250, Easing.Linear);
        }

        async void BtnAcceptSignature_Clicked(System.Object sender, System.EventArgs e)
        {
            await SLSingatureView.ScaleTo(0, 250, Easing.Linear);
            Stream bitmap = await signatureView.GetImageStreamAsync(SignatureImageFormat.Png);
            _signatureImageSource.Source = ImageSource.FromStream(() => bitmap);
            SLSingatureView.IsVisible = false;
        }

        async void BtnCancelSignature_Clicked(System.Object sender, System.EventArgs e)
        {
            await SLSingatureView.ScaleTo(0, 250, Easing.Linear);
            SLSingatureView.IsVisible = false;
        }

        async void ScaleView_ZoomedIn(System.Object sender, System.EventArgs e)
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;

            double mainScreenWidth = displayInfo.Width / displayInfo.Density;
            double mainScreenHeight = displayInfo.Height / displayInfo.Density;

            ScaleView scaleView = sender as ScaleView;
            AbsoluteLayout parentLayout = (AbsoluteLayout)scaleView.Parent;
            DocumentPageViewModel documentPageViewModel = parentLayout.BindingContext as DocumentPageViewModel;

            int index = 0;
            if (documentPageViewModel != null)
                index = _documentViewPageViewModel.DocumentPages.IndexOf(documentPageViewModel);

            Rectangle rectangle = scaleView.Bounds;
            Rectangle parentRectangle = parentLayout.Bounds;

            _lastScrollXPos = MainScrollView.ScrollX;
            _lastScrollYPos = MainScrollView.ScrollY;
            _lastZoomedValue = MainScrollView.CurrentZoomScale;

            double pageXPadding = 0, pageYPadding = 0;
            if (mainScreenWidth > parentRectangle.Width)
                pageXPadding = (mainScreenWidth - parentRectangle.Width) / 2;

            if (mainScreenHeight > parentRectangle.Height)
                pageYPadding = (mainScreenHeight - parentRectangle.Height) / 2;
            else
                pageYPadding = (mainScreenHeight - rectangle.Height) / 2;

            int padding = Device.Idiom == TargetIdiom.Tablet ? Device.RuntimePlatform == Device.Android ? 16 : 40 : Device.RuntimePlatform == Device.Android ? 16 : 20;
            double yPos = index * parentRectangle.Height;
            yPos += scaleView.Y;

            //Calculate Zoom Factor Based on size
            double zoomFactor = (mainScreenWidth - (padding * 2)) / rectangle.Width;
            _fieldZoomFactor = (float)zoomFactor;

            System.Diagnostics.Debug.WriteLine($"ScrollX : {MainScrollView.ScrollX}, {rectangle.X}, Width : {rectangle.Width}, Parent.X : {parentRectangle.X}, {parentRectangle.Width} ScrollY : {MainScrollView.ScrollY}, {rectangle.Y}, Height : {rectangle.Height}, Parent.Y : {parentRectangle.Y}, {parentRectangle.Height}, DeviceInfo  : Width : {mainScreenWidth}, Height : {mainScreenHeight}, ZoomFactor : {zoomFactor}, {MainScrollView.CurrentZoomScale}");

            MainScrollView.CurrentZoomScale = _fieldZoomFactor;  

            if (Device.RuntimePlatform == Device.Android)
            {
                // Check Whether Zoomed field in scrolled to Left side.
                if (MainScrollView.ScrollX < 0 && (MainScrollView.ScrollX < (rectangle.X * -1) || MainScrollView.ScrollX > (rectangle.X * -1)))
                    _zoomedInScrollXPos = (rectangle.X + padding) * -1;
                else
                    _zoomedInScrollXPos = 0;

                double tScrollY = 0;
                if (Device.Idiom == TargetIdiom.Tablet)
                    tScrollY = index > 0 ? yPos + pageYPadding - padding : yPos - (pageYPadding + padding);
                else
                    tScrollY = yPos - (pageYPadding + padding * 2);

                _zoomedInScrollYPos = tScrollY * -1;

                //MainScrollView.ScrollY < (scaleView.Height * -1) ?  MainScrollView.ScrollY + (scaleView.Height * ((_fieldZoomFactor - 1)/2) * -1) : 0;                
                await MainScrollView.ScrollToAsync(_zoomedInScrollXPos, _zoomedInScrollYPos, true);
            }
            else
            {
                double tScrollX = Device.Idiom == TargetIdiom.Tablet ? ((rectangle.X + pageXPadding) * _fieldZoomFactor) - padding : ((rectangle.X + pageXPadding) * _fieldZoomFactor) + padding;                

                double tScrollY = 0;
                if (Device.Idiom == TargetIdiom.Tablet)
                    tScrollY = index > 0 ? (yPos * _fieldZoomFactor) + pageYPadding - (padding * _fieldZoomFactor) : (yPos * _fieldZoomFactor) - (pageYPadding + (padding * _fieldZoomFactor));
                else
                    tScrollY = (yPos * _fieldZoomFactor) - (pageYPadding - padding * 2);

                _zoomedInScrollXPos = tScrollX;
                _zoomedInScrollYPos = tScrollY;

                await MainScrollView.ScrollToAsync(_zoomedInScrollXPos, _zoomedInScrollYPos, true);
            }

            System.Diagnostics.Debug.WriteLine($"Scrolled To : {_zoomedInScrollXPos}, {_zoomedInScrollYPos}, ZoomFactor : {_fieldZoomFactor}");
            _isZoomingInProgress = true;
        }

        async void ScaleView_ZoomedOut(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ScaleView ZoomedOut... ");
            _isZoomingInProgress = false;

            MainScrollView.CurrentZoomScale = _lastZoomedValue;
            await MainScrollView.ScrollToAsync(_lastScrollXPos, _lastScrollYPos, true);

            _lastScrollXPos = -1;
            _lastScrollYPos = -1;
            _lastZoomedValue = -1;

            _zoomedInScrollXPos = -1;
            _zoomedInScrollYPos = -1;
        }

        void MainScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Scrolled : {e.ScrollX}, {e.ScrollY}, ZoomFactor : {MainScrollView.CurrentZoomScale}");
            if (_isZoomingInProgress && (_zoomedInScrollXPos != -1 || _zoomedInScrollYPos != -1))
            {
                if (MainScrollView.ScrollX != _zoomedInScrollXPos || MainScrollView.ScrollY != _zoomedInScrollYPos)
                    MainScrollView.ScrollToAsync(_zoomedInScrollXPos, _zoomedInScrollYPos, false);

                if (MainScrollView.CurrentZoomScale != _fieldZoomFactor)
                    MainScrollView.CurrentZoomScale = _fieldZoomFactor;
            }
        }
    }
}