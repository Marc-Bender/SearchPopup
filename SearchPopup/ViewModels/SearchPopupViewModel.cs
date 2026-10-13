using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace SearchPopup.ViewModels
{
    public partial class SearchPopupViewModel : ObservableObject
    {
        public ObservableCollection<SearchableElement> PossibleElements { get; set; } = new();
        public ObservableCollection<SearchableElement> ShownElements { get; set; } = new();
        [ObservableProperty]
        private string searchText = "";
        [ObservableProperty]
        private SearchableElement selectedElement;

        partial void OnSearchTextChanged(string value) => RegenerateShowedElements();
        public void RegenerateShowedElements()
        {
            string[] GenerateFuzzyComponents(string word)
            {
                string[] components = [];
                for (int i = 0; i < word.Length; i++)
                {
                    for (int j = 0; j < word.Length - i - 1; j++) // -1 to ensure that the component is never just simply echoed... this is to allow for handling of whole text matches different to substring matches
                    {
                        components.Append(word.Substring(i, j));
                    }
                }

                return components;
            }


            // show elements based on the following categorization
            // favorites first -- they always get displayed
            // elements that contain one of the words searched for in whole
            // elements that contain any substring of the search term at any position
            ShownElements.Clear();
            var toBeShownElements = new ObservableCollection<SearchableElement>();

            if(SearchText == "")
            {
                foreach (var element in PossibleElements.OrderBy(x => x.IsFavorite == false))
                {
                    ShownElements.Add(element);
                }
            }
            else
            {
                var favorites = new ObservableCollection<SearchableElement>(PossibleElements.Where(x => x.IsFavorite == true).ToList());
                foreach (var favorite in favorites)
                {
                    toBeShownElements.Add(favorite);
                }

                var words = SearchText.ToLower().Split(" "); // to lower to allow for more permissive matching (eg. searching for "ap" in "Apple", "Pineapple", "Grape" should return all 3)

                foreach (var word in words)
                {
                    if (word == "")
                    {
                        continue; // to avoid duplicates otherwise doing something like "app " (with the space in the end) would cause all elements matching "app" (w/o space) to be in the shownelements + all possibly elements on top of that which is erroneous
                    }
                    var wholeWordMatches = PossibleElements.Where(x => x.ElementText.ToLower().Contains(word) && x.IsFavorite == false); // must also check for isFavorite == false here to avoid duplicate entries!
                    foreach (var match in wholeWordMatches)
                    {
                        toBeShownElements.Add(match);
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
                    var matchesThisComponent = PossibleElements.Where(x => x.ElementText.ToLower().Contains(component) && x.IsFavorite == false); // must also check for isFavorite == false here to avoid duplicate entries!
                    if (matchesThisComponent.Count() != 0)
                    {
                        foreach (var match in matchesThisComponent)
                        {
                            toBeShownElements.Add(match);
                        }
                    }
                }
                foreach (var element in toBeShownElements.OrderBy(x => x.IsFavorite == false))
                {
                    ShownElements.Add(element);
                }
            }
        }
    }
}
