using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using Xamarin.Forms;

namespace Zoom_Task.Core.ViewModels
{
    public class MainPageViewModel : BindableObject
    {
        public MainPageViewModel()
        {
            DocumentPages = new ObservableCollection<DocumentPageViewModel>()
            {
                new DocumentPageViewModel(),
                new DocumentPageViewModel(),
                new DocumentPageViewModel(),
                new DocumentPageViewModel()
            };     
        }

        private ObservableCollection<DocumentPageViewModel> _documentPages;
        public ObservableCollection<DocumentPageViewModel> DocumentPages
        {
            get { return _documentPages; }
            set { _documentPages = value; }
        }
    }
}
