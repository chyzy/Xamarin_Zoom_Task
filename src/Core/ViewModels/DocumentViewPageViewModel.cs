using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Zoom_Task.Core.ViewModels
{
    public class DocumentViewPageViewModel
    {
        public DocumentViewPageViewModel()
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
