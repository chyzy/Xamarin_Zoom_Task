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

        public DocumentViewPage()
        {
            InitializeComponent();
            BindingContext = new DocumentViewPageViewModel();
        }

        void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            Button button = sender as Button;
            _signatureImageSource =  button.CommandParameter as Image;
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
    }
}