using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.Core;
using Windows.Devices.Input;
using Windows.Gaming.Input;
using Windows.System;
using Windows.System.Profile;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Automation;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using muxc = Microsoft.UI.Xaml.Controls;
using Lit_Uno.Pages;


// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Lit_Uno
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public static MainPage Current;
        public static Frame RootFrame = null;
        public VirtualKey ArrowKey;
        public DeviceType DeviceFamily { get; set; }
        private bool _isGamePadConnected;
        public bool _isKeyboardConnected;
        private Microsoft.UI.Xaml.Controls.NavigationViewItem _allControlsMenuItem;
        private Microsoft.UI.Xaml.Controls.NavigationViewItem _newControlsMenuItem;

        public bool IsFocusSupported
        {
            get
            {
                return DeviceFamily == DeviceType.Xbox || _isGamePadConnected || _isKeyboardConnected;
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

            NavigationView.PaneDisplayMode = muxc.NavigationViewPaneDisplayMode.Left;

            //_navHelper = new RootFrameNavigationHelper(rootFrame, NavigationViewControl);

            SetDeviceFamily();
           
            Current = this;
            RootFrame = rootFrame;

            this.GotFocus += (object sender, RoutedEventArgs e) =>
            {
                // helpful for debugging focus problems w/ keyboard & gamepad
                if (FocusManager.GetFocusedElement() is FrameworkElement focus)
                {
                    //Debug.WriteLine("got focus: " + focus.Name + " (" + focus.GetType().ToString() + ")");
                }
            };
            AddNavigationMenuItems();


#if NETFX_CORE
            //CoreApplication.GetCurrentView().TitleBar.LayoutMetricsChanged += (s, e) => UpdateAppTitle(s);
            _isKeyboardConnected = Convert.ToBoolean(new KeyboardCapabilities().KeyboardPresent);
#endif
        }

        private void AddNavigationMenuItems()
        {
            var subItem = new muxc.NavigationViewItem { Content = "Level1", Tag = "SamplePage1", Icon = GetIcon("\xe734") };
            var item = new muxc.NavigationViewItem() { Content = "Home", Tag = "HomePage", Icon = GetIcon("ms-appx:///Assets/Hub.png"), DataContext = subItem };
            // var subItem = new muxc.NavigationViewItem() { Content = "Home", Tag = "home" };
            item.MenuItems.Add(subItem);

            NavigationView.MenuItems.Add(item);
        }

        private static IconElement GetIcon(string imagePath)
        {
            //https://docs.microsoft.com/en-us/windows/uwp/design/style/segoe-ui-symbol-font glyphs
            return imagePath.ToLowerInvariant().EndsWith(".png") ?
                        (IconElement)new BitmapIcon() { UriSource = new Uri(imagePath, UriKind.RelativeOrAbsolute), ShowAsMonochrome = false } :
                        (IconElement)new FontIcon()
                        {
                            FontFamily = new FontFamily("Segoe MDL2 Assets"),
                            Glyph = imagePath
                        };
        }

        private void OnNewControlsMenuItemLoaded(object sender, RoutedEventArgs e)
        {
            if (IsFocusSupported)
            {
                _newControlsMenuItem.Focus(FocusState.Keyboard);
            }
            _newControlsMenuItem.IsSelected = true;
        }

        private void SetDeviceFamily()
        {
#if NETFX_CORE
            var familyName = AnalyticsInfo.VersionInfo.DeviceFamily;

            if (!Enum.TryParse(familyName.Replace("Windows.", string.Empty), out DeviceType parsedDeviceType))
            {
                parsedDeviceType = DeviceType.Other;
            }

            DeviceFamily = parsedDeviceType;
#endif
        }

        public enum DeviceType
        {
            Desktop,
            Mobile,
            Other,
            Xbox
        }

        private void NavigationView_SelectionChanged(Microsoft.UI.Xaml.Controls.NavigationView sender, Microsoft.UI.Xaml.Controls.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                rootFrame.Navigate(typeof(SettingsPage));
            }
            else
            {
                var selectedItem = (Microsoft.UI.Xaml.Controls.NavigationViewItem)args.SelectedItem;
                if (selectedItem != null)
                {
                    string selectedItemTag = ((string)selectedItem.Tag);
                    sender.Header = "Lit_Uno.Pages " + selectedItemTag.Substring(selectedItemTag.Length - 1);
                    string pageName = "Lit_Uno.Pages." + selectedItemTag;
                    Type pageType = Type.GetType(pageName);
                    rootFrame.Navigate(pageType);
                }
            }
        }
        private void controlsSearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {

        }

        private void controlsSearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {

        }

        private void KeyboardAccelerator_Invoked(KeyboardAccelerator sender, KeyboardAcceleratorInvokedEventArgs args)
        {

        }
    }
}
