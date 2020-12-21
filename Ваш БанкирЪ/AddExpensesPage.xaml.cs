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
    public sealed partial class AddExpensesPage : Page
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

        private void CreateHistoryNode(long date, int sum, bool isIncome, string comment, string clientID, string category)
        {
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

            Grid changeGrid = new Grid { Height = 70, Margin = new Thickness(0, 0, 0, 20) };

            TextBlock headerTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 25,
                Text = header
            };

            TextBlock sumTextBlock = new TextBlock
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontSize = 25,
                Text = $"{sum:### ### ### ### ### ###} ₽",
                Margin = new Thickness(-30, 0, 0, 0)
            };

            StackPanel infoStackPanel = new StackPanel();

            Flyout infoFlyout = new Flyout { Content = infoStackPanel };

            Button infoOkButton = new Button { Content = "Закрыть" };
            infoOkButton.Click += (sender, args) => infoFlyout.Hide();
            infoOkButton.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock infoTextBlock = new TextBlock
            {
                Text = String.Format(
                    $"{FunctionClass.GetClientFromID(clientID)} в {changeDate.ToShortDateString()}: \n " +
                    $"{comment}"),
                TextWrapping = TextWrapping.WrapWholeWords,
                MaxWidth = 300,
                MaxHeight = 400,
                VerticalAlignment = VerticalAlignment.Top
            };

            infoStackPanel.Children.Add(infoOkButton);
            infoStackPanel.Children.Add(infoTextBlock);

            Image infoButtonIcon = new Image
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icons8-информация-96.png"))
            };

            Button infoButton = new Button
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
                Height = 70,
                Width = 70,
                Content = infoButtonIcon,
                Flyout = infoFlyout
            };
            infoButton.Click += (sender, args) => infoFlyout.ShowAt(infoButton);

            changeGrid.Children.Add(headerTextBlock);
            changeGrid.Children.Add(sumTextBlock);
            changeGrid.Children.Add(infoButton);
            ChangesHistory.Children.Add(changeGrid);
        }

        private void UpdateHistory()
        {
            FinancialChange temp;
            long date = Int64.MinValue;
            int sum = Int32.MinValue;
            bool isIncome = false;
            string comment = "";
            string clientID = "";
            string category = "";

            foreach (FinancialChange change in App.FinancialChangesList)
            {
                if (change != null)
                {
                    date = change.Date;
                    sum = change.Sum;
                    isIncome = change.IsIncome;
                    comment = change.Comment;
                    clientID = change.ClientId;
                    category = change.Category;
                }
            }
            temp = new FinancialChange(sum, isIncome, comment, category, date, clientID);
            CreateHistoryNode(temp.Date, temp.Sum, temp.IsIncome, temp.Comment, temp.ClientId, temp.Category);

        }

        private void InitializeChanges()
        {
            foreach (FinancialChange change in App.FinancialChangesList)
            {
                if (change != null)
                {
                    long date = change.Date;
                    int sum = change.Sum;
                    bool isIncome = change.IsIncome;
                    string comment = change.Comment;
                    string clientID = change.ClientId;
                    string category = change.Category;

                    CreateHistoryNode(date, sum, isIncome, comment, clientID, category);
                }
            }

        }

        public AddExpensesPage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            InitializeChanges();
        }

        private void ExpensesSum_OnBeforeTextChanging(TextBox sender, TextBoxBeforeTextChangingEventArgs args)
        {
            args.Cancel = args.NewText.Any(c => !char.IsDigit(c));
        }

        private void ExpensesAddButton_OnClick(object sender, RoutedEventArgs e)
        {
            string comment = ExpensesCommentsTextBox.Text;
            if (ExpensesSumTextBox.Text.Trim() != "" && Convert.ToInt32(ExpensesSumTextBox.Text) < int.MaxValue)
            {
                int sum = Convert.ToInt32(ExpensesSumTextBox.Text);

                if (ExpensesCategoryComboBox.SelectedItem != null)
                {
                    string category = ExpensesCategoryComboBox.SelectedItem.ToString();
                    if (comment.Trim() != "")
                    {
                        App.FinancialChangesList.AddFinancialChange(sum, false, comment, category);
                    }
                    else
                    {
                        App.FinancialChangesList.AddFinancialChange(sum, false, category);
                    }

                    UpdateHistory();
                    ExpenseErrorText.Text = "Расход успешно добавлен";
                    ExpensesSumTextBox.Text = "";
                    ExpensesCommentsTextBox.Text = "";
                    ExpensesCategoryComboBox.SelectedItem = null;
                }
                else
                {
                    ExpenseErrorText.Text = "Категория не выбрана";
                    ExpenseErrorFlyout.ShowAt(ExpensesAddButton);
                }
            }
            else
            {
                ExpenseErrorText.Text = "Введите корректную сумму";
                ExpenseErrorFlyout.ShowAt(ExpensesAddButton);
            }
        }

        private void ExpenseErrorButton_OnClick(object sender, RoutedEventArgs e)
        {
            ExpenseErrorFlyout.Hide();
        }
    }
}
