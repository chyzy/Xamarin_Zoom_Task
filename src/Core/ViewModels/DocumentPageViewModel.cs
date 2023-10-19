using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Zoom_Task.Core.ViewModels
{
    public class DocumentPageViewModel : BindableObject
    {
        public DocumentPageViewModel()
        {
            DocumentBackgroundImageSource = ImageSource.FromResource("Zoom_Task.Core.Images.Document_A4.jpg");
        }

        public ImageSource _documentBackgroundImageSource;
        public ImageSource DocumentBackgroundImageSource
        {
            get => _documentBackgroundImageSource;
            set { _documentBackgroundImageSource = value; }
        }

        public ImageSource _signature;
        public ImageSource Signature
        {
            get => _signature;
            set { _signature = value; }
        }
    }
}
