using SignaturePad.Forms;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoom_Task.Core.Controls;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task.Core
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentViewPage : ContentPage
    {
        private Image _signatureImageSource;
        private double _lastScrollXPos = -1;
        private double _lastScrollYPos = -1;
        private double _zoomedInScrollXPos = -1;
        private double _zoomedInScrollYPos = -1;
        private float _lastZoomedValue = -1;
        private float _fieldZoomFactor = 1.8f;
        private bool _isZoomingInProgress = false;

        public DocumentViewPage()
        {
            InitializeComponent();
            BindingContext = new DocumentViewPageViewModel();
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
            ScaleView scaleView = sender as ScaleView;
            Rectangle rectangle = scaleView.Bounds;
            _lastScrollXPos = MainScrollView.ScrollX;
            _lastScrollYPos = MainScrollView.ScrollY;
            _lastZoomedValue = MainScrollView.CurrentZoomScale;
            
            MainScrollView.CurrentZoomScale = _fieldZoomFactor;
            int padding = Device.RuntimePlatform == Device.Android ? 16 : 70;
            if(Device.RuntimePlatform == Device.Android)
            {
                _zoomedInScrollXPos = (rectangle.X + padding) * -1;
                _zoomedInScrollYPos = MainScrollView.ScrollY + (scaleView.Height * (_fieldZoomFactor - 1) * -1);
                await MainScrollView.ScrollToAsync(_zoomedInScrollXPos, _zoomedInScrollYPos, true);
            }
            else
            {
                _zoomedInScrollXPos = _lastScrollXPos + rectangle.X + padding;
                _zoomedInScrollYPos = MainScrollView.ScrollY + (scaleView.Height * (_fieldZoomFactor - 1));
                await MainScrollView.ScrollToAsync(_zoomedInScrollXPos, _zoomedInScrollYPos, true);
            }

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