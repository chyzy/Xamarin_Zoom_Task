using SignaturePad.Forms;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task.Core
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentViewPage : ContentPage
    {
        private Image _signatureImageSource;
        private double _lastScrollXPos = -1;
        private double _lastScrollYPos = -1;
        private float _lastZoomedValue = -1;

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

        void ScaleView_ZoomedIn(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ScaleView ZoomedIn... ");
            _lastScrollXPos = MainScrollView.ScrollX;
            _lastScrollYPos = MainScrollView.ScrollY;
            _lastZoomedValue = MainScrollView.CurrentZoomScale;
        }

        void ScaleView_ZoomedOut(System.Object sender, System.EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("ScaleView ZoomedOut... ");
            _lastScrollXPos = -1;
            _lastScrollYPos = -1;
            _lastZoomedValue = -1;
        }

        void MainScrollView_Scrolled(System.Object sender, Xamarin.Forms.ScrolledEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"MainScrollView Scrolled : ScrollX : {e.ScrollX}. ScrollY : {e.ScrollY}, CurrentZoom : {MainScrollView.CurrentZoomScale}");
            if (_lastScrollXPos != -1 || _lastScrollYPos != -1)
            {
                if (MainScrollView.ScrollX != _lastScrollXPos || MainScrollView.ScrollY != _lastScrollYPos)
                    MainScrollView.ScrollToAsync(_lastScrollXPos, _lastScrollYPos, false);

                if (MainScrollView.CurrentZoomScale != _lastZoomedValue)
                    MainScrollView.CurrentZoomScale = _lastZoomedValue;
            }
        }
    }
}