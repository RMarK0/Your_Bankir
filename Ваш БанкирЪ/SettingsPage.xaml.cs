using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.Navigate(typeof(MainMenuPage));
            e.Handled = true;
        }


        public SettingsPage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            AboutAppFlyoutTextBlock.Text = String.Format($"Версия {App.versionInfo} Pre-release © Dmitry Rybalko 2020");

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(
                "Windows.UI.Xaml.Media.XamlCompositionBrushBase"))
            {
                this.Background = new AcrylicBrush()
                {
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    TintOpacity = 0.9,
                    TintColor = Color.FromArgb(255, 0, 0, 0),
                    Opacity = 1
                };
            }
        }

        private void NavigateToChangePassword_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(PasswordChangePage));
        }

        private void AboutAppButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutAppFlyout.ShowAt(AboutAppButton);
        }

        private void FlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            AboutAppFlyout.Hide();
        }
    }
}
