using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchPopup
{
    /// <summary>
    /// Interaction logic for SearchPopup.xaml
    /// </summary>
    public partial class SearchPopup : UserControl
    {
        public Popup containedPopup => this.popupControl;
        public ObservableCollection<SearchableElement> ShownElements { get; set; } = new();
        public SearchPopup()
        {
            InitializeComponent();
            RegenerateShowedElements("");
        }

        private void popupControl_LostFocus(object sender, RoutedEventArgs e)
        {
            containedPopup.IsOpen = false;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(
                    (sender is Image img)
                 && (img.DataContext is SearchableElement element)
              )
            {
                element.IsFavorite = !element.IsFavorite;
            }
        }

        public void RefreshLocation()
        {
            if (containedPopup.IsOpen == true)
            {
                // repositioning the popup only happens on isopen toggle ...
                containedPopup.IsOpen = false;
                containedPopup.IsOpen = true;
            }
        }

        public string[] GenerateFuzzyComponents(string fuzzySearchText)
        {
            string[] components = [];
            for (int i = 0; i<fuzzySearchText.Length; i++)
            {
                for (int j = 0; j<fuzzySearchText.Length - i - 1; j++) // -1 to ensure that the component is never just simply echoed... this is to allow for handling of whole text matches different to substring matches
                {
                    components.Append(fuzzySearchText.Substring(i, j));
                }
            }

            return components;
        }
        public void RegenerateShowedElements(string fuzzySearchText)
        {
            if (DataContext is ObservableCollection<SearchableElement> elements)
            {
                // show elements based on the following categorization
                // favorites first -- they always get displayed
                // elements that contain one of the words searched for in whole
                // elements that contain any substring of the search term at any position
                ShownElements.Clear();
                var favorites = new ObservableCollection<SearchableElement>(elements.Where(x => x.IsFavorite == true).ToList());
                foreach(var favorite in favorites)
                {
                    ShownElements.Add(favorite);
                }

                var words = fuzzySearchText.ToLower().Split(" "); // to lower to allow for more permissive matching (eg. searching for "ap" in "Apple", "Pineapple", "Grape" should return all 3)
                
                foreach (var word in words)
                {
                    var wholeWordMatches = elements.Where(x => x.ElementText.ToLower().Contains(word) && x.IsFavorite == false); // must also check for isFavorite == false here to avoid duplicate entries!
                    foreach(var match in wholeWordMatches)
                    {
                        ShownElements.Add(match);
                    }
                }

                string[] fuzzySearchComponents = [];

                foreach (var word in words)
                {
                    foreach (var component in GenerateFuzzyComponents(word))
                    {
                        fuzzySearchComponents.Append(component);
                    }
                }

                foreach (var component in fuzzySearchComponents)
                {
                    var matchesThisComponent = elements.Where(x => x.ElementText.ToLower().Contains(component) && x.IsFavorite == false); // must also check for isFavorite == false here to avoid duplicate entries!
                    if (matchesThisComponent.Count() != 0)
                    {
                        foreach(var match in matchesThisComponent)
                        {
                            ShownElements.Add(match);
                        }
                    }
                }                
            }

        }

        private void popupControl_Opened(object sender, EventArgs e) => RegenerateShowedElements("");
    }

}
