using CommunityToolkit.Mvvm.ComponentModel;
using SearchPopup.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace SearchPopup
{
    public partial class SearchPopup : UserControl
    {
        public Popup containedPopup => this.popupControl;
        public SearchPopup()
        {
            InitializeComponent();

            // to prevent that the popup stays open when the app loses focus -- which would cause the popup to be on top of any other apps,
            // this can not be avoided otherwise as staysopen must remain true to ensure that the popup opens reliably at all and dynamically modifying the stayopen member also did not yield the expected results
            Loaded += (_, _) =>
            {
                var window = Window.GetWindow(this);
                if (window != null)
                {
                    window.Deactivated += (_, _) => containedPopup.IsOpen = false;
                }
            };
        }



        private void popupControl_LostFocus(object sender, RoutedEventArgs e)
        {
            if (popupControl.IsKeyboardFocusWithin == false)
            {
                popupControl.IsOpen = false;
            }
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
            if (popupControl.IsOpen == true)
            {
                // repositioning the popup only happens on isopen toggle ...
                popupControl.IsOpen = false;
                popupControl.IsOpen = true;
            }
        }

        private void popupControl_Opened(object sender, EventArgs e)
        {
            if (DataContext is SearchPopupViewModel vm)
            {
                vm.SearchText = "";
                searchTextField.Focus();
                vm.RegenerateShowedElements();
            }
        }

        private void TextBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (
                    (sender is Label label)
                 && (label.DataContext is SearchableElement element)
                 && (DataContext is SearchPopupViewModel vm)
               )
            {
                vm.SelectedElement = element;
                popupControl.IsOpen = false;
            }
        }
    }

}
