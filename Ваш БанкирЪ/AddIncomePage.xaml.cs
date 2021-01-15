using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
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
using WinRTXamlToolkit.Controls;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class AddIncomePage : Page
    {
        public static AddIncomePage IncomePage;

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
                Text = $"{sum:### ### ###} ₽",
                Margin = new Thickness(-10, 0, 0, 0)
            };

            Grid infoGrid = new Grid();

            Flyout infoFlyout = new Flyout { Content = infoGrid };

            Button infoOkButton = new Button { Content = "Закрыть" };
            infoOkButton.Click += (sender, args) => infoFlyout.Hide();
            infoOkButton.VerticalAlignment = VerticalAlignment.Bottom;

            TextBlock infoTextBlock = new TextBlock
            {
                Text = String.Format(
                    $"{FunctionClass.GetClientFromID(clientID)} в {changeDate.ToShortDateString()}: \n" +
                    $"{comment}"),
                TextWrapping = TextWrapping.WrapWholeWords,
                MaxWidth = 300,
                MaxHeight = 400,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(0, 0, 0, 50)
            };

            infoGrid.Children.Add(infoOkButton);
            infoGrid.Children.Add(infoTextBlock);

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
        

        public AddIncomePage()
        {
            IncomePage = this;
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
            if (IncomeSumTextBox.Text.Trim() != "" && IncomeSumTextBox.Text.Length < 10)
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

                    UpdateHistory();
                    App.TotalIncomes += sum;
                    App.CurrentSum += sum;
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
                IncomeErrorText.Text = "Введите корректную сумму";
                IncomeErrorFlyout.ShowAt(IncomeAddButton);
            }
        }

        private void ErrorFlyout_Click(object sender, RoutedEventArgs e)
        {
            IncomeErrorFlyout.Hide();
        }
    }
}
