using SearchPopup;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SearchPopupDemo
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<SearchableElement> fruits { get; } =
        new ObservableCollection<SearchableElement>
        {
            new SearchableElement { ElementText = "Apple", IsFavorite = false },
            new SearchableElement { ElementText = "Cherry", IsFavorite = true }
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            fruitSearchPopup.containedPopup.PlacementTarget = (UIElement)sender;
            fruitSearchPopup.containedPopup.IsOpen = true;
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (fruitSearchPopup.containedPopup.IsKeyboardFocusWithin == true)
                return; // do no close when textbox lost focus into the popup
            fruitSearchPopup.containedPopup.IsOpen = false;
        }
    }
}