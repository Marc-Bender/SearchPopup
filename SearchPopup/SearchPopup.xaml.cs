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
        public SearchPopup()
        {
            InitializeComponent();
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
    }

}
