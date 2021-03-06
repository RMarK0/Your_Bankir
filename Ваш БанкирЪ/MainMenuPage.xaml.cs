﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
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

    public sealed partial class MainMenuPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Disabled;
        }
        
        public MainMenuPage()
        {
            this.InitializeComponent();
            ActiveClientTextBlock.Text =
                String.Format(
                    $"ID:{LoginPage.ActiveClient.ID} GEN:{LoginPage.ActiveClient.Generation} NAME:{LoginPage.ActiveClient.Name}");
        }

        public void AddIncomeButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddIncomePage));
        }

        public void AddExpensesButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddExpensesPage));
        }

        public void FinanceAnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FinanceAnalysisPage));
        }

        public void AddTargetButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AddTargetSelectPage));
        }

        public void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(SettingsPage));
        }

        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            //Session.Close(); или что-то типа этого
            Frame.Navigate(typeof(LoginPage));
        }
    }
}
