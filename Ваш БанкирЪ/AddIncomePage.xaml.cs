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
    public sealed partial class AddIncomePage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Disabled;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.Navigate(typeof(MainMenuPage));
            e.Handled = true;
        }

        public AddIncomePage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void IncomeSumTextBox_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void IncomeAddButton_OnClick(object sender, RoutedEventArgs e)
        {
            string comment = IncomeCommentsTextBox.Text;
            if (IncomeSumTextBox.Text.Trim() != "")
            {
                int sum = Convert.ToInt32(IncomeSumTextBox.Text);
                if (IncomeCategoryComboBox.SelectedItem != null)
                {
                    string category = IncomeCategoryComboBox.SelectedItem.ToString();
                    if (comment.Trim() != "")
                    {
                        
                    }
                    else
                    {
                        
                    }




                    IncomeErrorText.Text = "Доход успешно добавлен";
                }
                else
                {
                    IncomeErrorText.Text = "Категория не выбрана";
                    IncomeErrorFlyout.ShowAt(IncomeAddButton);
                }
            }
            else
            {
                IncomeErrorText.Text = "Сумма не введена";
                IncomeErrorFlyout.ShowAt(IncomeAddButton);
            }
        }

        private void ErrorFlyout_Click(object sender, RoutedEventArgs e)
        {
            IncomeErrorFlyout.Hide();
        }
    }
}
