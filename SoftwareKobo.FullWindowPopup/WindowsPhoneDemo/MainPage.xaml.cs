using System.Reflection;
using System.Threading.Tasks;
using Windows.UI.Popups;
using SoftwareKobo;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=391641

namespace WindowsPhoneDemo
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            FullWindowPopup popup = new FullWindowPopup();
            popup.Background = new SolidColorBrush(Colors.Red);
            popup.Show();
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            FullWindowPopup popup = new FullWindowPopup();
            popup.Background = new SolidColorBrush(Colors.Red);
            popup.IsCloseOnBackPress = true;
            popup.Show();
        }

        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            FullWindowPopup popup = new FullWindowPopup();
            var child = (Grid)XamlReader.Load(
                 @"<Grid
    xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
    xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
    xmlns:mc=""http://schemas.openxmlformats.org/markup-compatibility/2006""
        Background=""Blue"" VerticalAlignment=""Top"">
        <StackPanel>
            <TextBlock Text=""Title"" FontSize=""18""/>
            <TextBlock Text=""Body"" FontSize=""15""/>
        </StackPanel>
    </Grid>"
                 );
            popup.Child = child;
            popup.Opened += Popup_Opened;
            popup.Closed += Popup_Closed;
            child.Tapped += async (s, args) =>
            {
                popup.Hide();
                txtMsg.Text = "点击了模拟 Toast。";
                await Task.Delay(3000);
                txtMsg.Text = string.Empty;
            };
            popup.Show();
        }

        private async void Popup_Closed(object sender, object e)
        {
            await StatusBar.GetForCurrentView().ShowAsync();
        }

        private async void Popup_Opened(object sender, object e)
        {
            await StatusBar.GetForCurrentView().HideAsync();
        }
    }
}
