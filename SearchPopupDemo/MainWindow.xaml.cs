using SearchPopup;
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
            new SearchableElement { ElementText = "Cherry", IsFavorite = true },
            new SearchableElement { ElementText = "Pear", IsFavorite = false },
            new SearchableElement { ElementText = "Orange", IsFavorite = false },
            new SearchableElement { ElementText = "Pineapple", IsFavorite = false},
            new SearchableElement { ElementText = "Strawberry", IsFavorite = false},
            new SearchableElement { ElementText = "Grape", IsFavorite = false},
            new SearchableElement { ElementText = "Lemon", IsFavorite = false},
        };

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                fruitSearchPopup.containedPopup.PlacementTarget = textBox;
                fruitSearchPopup.containedPopup.Placement = PlacementMode.Relative;
                fruitSearchPopup.containedPopup.HorizontalOffset = 0;
                fruitSearchPopup.containedPopup.VerticalOffset = textBox.ActualHeight;
                fruitSearchPopup.containedPopup.IsOpen = true;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (fruitSearchPopup.containedPopup.IsKeyboardFocusWithin == true)
                return; // do no close when textbox lost focus into the popup
            fruitSearchPopup.containedPopup.IsOpen = false;
        }

        private void Window_LocationChanged(object sender, EventArgs e) => Window_MovedOrResized(sender, e);

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e) => Window_MovedOrResized(sender, e);

        private void Window_MovedOrResized(object sender, EventArgs e)
        {
            fruitSearchPopup.RefreshLocation();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(
                    (sender is TextBox textBox)
                 && (fruitSearchPopup is not null) // this may happen upon program start ...
              )
            {
                fruitSearchPopup.RegenerateShowedElements(textBox.Text);
            }
        }
    }
}