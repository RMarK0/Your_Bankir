using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class AddTargetPage : Page
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
            Frame.Navigate(typeof(AddTargetSelectPage));
            e.Handled = true;
        }

        public AddTargetPage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }


        private void AddTargetButton_OnClick(object sender, RoutedEventArgs e)
        {
            string name;
            int sum;
            string comment;

            if (TargetNameTextBox.Text.Trim() != "" && TargetNameTextBox.Text.Length < 15)
            {
                name = TargetNameTextBox.Text.Trim();
                if (TargetSumTextBox.Text != "")
                {
                    sum = Convert.ToInt32(TargetSumTextBox.Text);

                    if (TargetCommsTextBox.Text.Trim() != "")
                    {
                        comment = TargetCommsTextBox.Text;
                        App.TargetsList.AddTarget(name, sum, comment);
                    }
                    else
                    {
                        App.TargetsList.AddTarget(name, sum);
                    }

                    TargetNameTextBox.Text = "";
                    TargetCommsTextBox.Text = "";
                    TargetSumTextBox.Text = "";

                    AddTargetFlyoutText.Text = "Цель успешно добавлена";
                    AddTargetFlyout.ShowAt(AddTargetButton);
                }
                else
                {
                    AddTargetFlyoutText.Text = "Введите корректную сумму";
                    AddTargetFlyout.ShowAt(AddTargetButton);
                }
            }
            else
            {
                AddTargetFlyoutText.Text = "Введите корректное название";
                AddTargetFlyout.ShowAt(AddTargetButton);
            }
        }

        private void TargetSumTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
            if (TargetSumTextBox.Text.Length > 8)
                args.Cancel = args.NewText.Any();
        }

        private void AddTargetFlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            AddTargetFlyout.Hide();
        }
    }
}
