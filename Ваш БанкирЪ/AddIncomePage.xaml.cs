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
using Windows.UI.Xaml.Media.Imaging;
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

        private void InitializeChanges()
        {
            foreach (FinancialChange change in App.FinancialChangesList)
            {
                if (change != null)
                {
                    long date = change.date;
                    int sum = change.sum;
                    bool isIncome = change.isIncome;
                    DateTime changeDate = DateTime.FromBinary(date);

                    string header;

                    if (isIncome)
                    {
                        header = String.Format($"Доход от {changeDate.ToShortDateString()}");
                    }
                    else
                    {
                        header = String.Format($"Расход от {changeDate.ToShortDateString()}");
                    }

                    Grid changeGrid = new Grid();
                    changeGrid.Height = 70;
                    changeGrid.Margin = new Thickness(0, 0, 0, 20);

                    TextBlock headerTextBlock = new TextBlock();
                    headerTextBlock.VerticalAlignment = VerticalAlignment.Top;
                    headerTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    headerTextBlock.FontSize = 25;
                    headerTextBlock.Text = header;

                    TextBlock sumTextBlock = new TextBlock();
                    sumTextBlock.VerticalAlignment = VerticalAlignment.Bottom;
                    sumTextBlock.HorizontalAlignment = HorizontalAlignment.Left;
                    sumTextBlock.FontSize = 25;
                    sumTextBlock.Text = String.Format("{0:### ### ### ### ###} ₽", sum.ToString());

                    Button deleteButton = new Button();
                    deleteButton.VerticalAlignment = VerticalAlignment.Center;
                    deleteButton.HorizontalAlignment = HorizontalAlignment.Right;
                    deleteButton.Height = 70;
                    deleteButton.Width = 70;

                    Image deleteButtonIcon = new Image();
                    deleteButtonIcon.Source = new BitmapImage(new Uri("ms-appx:///Assets/icons8-удалить-96.png"));
                    deleteButton.Content = deleteButtonIcon;

                    changeGrid.Children.Add(headerTextBlock);
                    changeGrid.Children.Add(sumTextBlock);
                    changeGrid.Children.Add(deleteButton);
                    ChangesHistory.Children.Add(changeGrid);
                }
            }
        }

        public AddIncomePage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            InitializeChanges();
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
                        App.FinancialChangesList.AddFinancialChange(sum, true, comment, category);
                    }
                    else
                    {
                        App.FinancialChangesList.AddFinancialChange(sum, true, category);
                    }

                    IncomeErrorText.Text = "Доход успешно добавлен";
                    IncomeSumTextBox.Text = "";
                    IncomeCommentsTextBox.Text = "";
                    IncomeCategoryComboBox.SelectedItem = null;
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
