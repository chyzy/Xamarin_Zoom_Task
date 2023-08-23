using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Zoom_Task.Core.ViewModels;

namespace Zoom_Task.Core
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DocumentViewPage : ContentPage
    {
        public DocumentViewPage()
        {
            InitializeComponent();
            BindingContext = new DocumentViewPageViewModel();
        }
    }
}