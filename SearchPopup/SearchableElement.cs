using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SearchPopup
{
    public partial class SearchableElement : ObservableObject
    {
        [ObservableProperty]
        public string elementText = "";

        [ObservableProperty]
        public bool isFavorite = false;
    }
}
