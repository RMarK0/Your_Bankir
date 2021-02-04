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
using static Ваш_БанкирЪ.App;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddTargetSelectPage : Page
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

        public void UpdateSums()
        {
            string currentBalance = "0 ₽";
            string lastMonthExpenses = "0 ₽";
            if (ThisMonthExpenses != 0)
                lastMonthExpenses = $"{ThisMonthExpenses:### ### ###} ₽";
            if (CurrentSum != 0)
                currentBalance = $"{CurrentSum:### ### ###} ₽";

            BalanceSumTextBox.Text = currentBalance;
            PrevMonthExpensesSumTextBox.Text = lastMonthExpenses;
        }

        public AddTargetSelectPage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
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
            FunctionClass.InitializeFinances(ref App.CurrentSum, ref App.TotalExpenses, ref App.TotalIncomes, ref App.ThisMonthExpenses);
            UpdateSums();
        }

        private void AddTargetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTargetPage));
        }

        private void EditTargetButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(ActiveTargetsPage));
        }
    }
}
