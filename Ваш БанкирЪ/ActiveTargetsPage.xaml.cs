using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
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
using static Ваш_БанкирЪ.App;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    internal class TargetNode
    {
        public string Name { get; internal set; }
        public int FullSum { get; internal set; }
        public string Comment { get; internal set; }
        public int CurrentSum { get; internal set; }
        public long DateAdded { get; internal set; }
        public string ClientID { get; internal set; }

        public Grid targetGrid;
        private ProgressBar progressBar;
        private TextBlock headerTextBlock;
        private Button editButton;
        private Button infoButton;
        private Flyout infoFlyout;
        private Flyout editFlyout;
        internal Flyout deleteButtonFlyout;

        private Image infoIcon;
        private Image editIcon;

        private Grid infoFlyoutGrid;
        private Grid editFlyoutGrid;

        private Button infoFlyoutCloseButton;
        private Button editFlyoutCloseButton;
        private Button flyoutDeleteTargetButton;
        private Button flyoutSaveChangesButton;

        private TextBlock infoFlyoutTextBlock;
        private TextBlock editFlyoutTextBlock;

        private TextBox editFlyoutTextBox;

        internal void CreateTargetNode(string name, int fullSum, string comment, int currentSum, long dateAdded,
            string clientId)
        {
            Name = name;
            DateAdded = dateAdded;
            CurrentSum = currentSum;
            ClientID = clientId;
            FullSum = fullSum;
            Comment = comment;

            double doubleCurrentSum = Convert.ToDouble(CurrentSum);
            double doubleFullSum = Convert.ToDouble(FullSum);

            DateTime _dateAdded = DateTime.FromBinary(DateAdded);

            targetGrid = new Grid()
            {
                Height = 80,
                Margin = new Thickness(0, 0, 0, 30)
            };

            progressBar = new ProgressBar()
            {
                Width = 270,
                Height = 35,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                Value = (doubleCurrentSum / doubleFullSum) * 100
            };

            headerTextBlock = new TextBlock()
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 30,
                MaxWidth = 270,
                Text = Name
            };

            editIcon = new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icons8-редактировать-96.png"))
            };

            infoIcon = new Image()
            {
                Source = new BitmapImage(new Uri("ms-appx:///Assets/icons8-информация-96.png"))
            };

            infoButton = new Button()
            {
                Width = 60,
                Height = 60,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 0, 70, 0),
                Content = infoIcon
            };

            deleteButtonFlyout = new Flyout();
            deleteButtonFlyout.Content = new TextBlock()
            {
                Text = "Цель успешно удалена. Обновите страницу чтобы изменения вступили в силу.",
                TextWrapping = TextWrapping.WrapWholeWords,
                MaxWidth = 300
            };

            editButton = new Button()
            {
                Width = 60,
                Height = 60,
                HorizontalAlignment = HorizontalAlignment.Right,
                Content = editIcon
            };

            infoFlyout = new Flyout();
            editFlyout = new Flyout();

            infoFlyoutGrid = new Grid()
            {
                MaxWidth = 400
            };
            editFlyoutGrid = new Grid()
            {
                Height = 140,
                MaxWidth = 400
            };

            infoFlyoutCloseButton = new Button()
            {
                Content = "Закрыть",
                VerticalAlignment = VerticalAlignment.Bottom
            };

            infoFlyoutTextBlock = new TextBlock()
            {
                Text = $"Автор цели: {FunctionClass.GetClientFromID(ClientID)}\n" +
                       $"Дата: {_dateAdded.ToShortDateString()}\n" +
                       $"Сумма: {CurrentSum}/{FullSum}\n" +
                       $"Описание: {Comment}",
                Margin = new Thickness(0,0,0,50),
                TextWrapping = TextWrapping.WrapWholeWords
            };

            infoFlyoutGrid.Children.Add(infoFlyoutCloseButton);
            infoFlyoutGrid.Children.Add(infoFlyoutTextBlock);
            infoFlyout.Content = infoFlyoutGrid;

            infoButton.Click += (sender, args) => infoFlyout.ShowAt(infoButton);
            infoFlyoutCloseButton.Click += (sender, args) => infoFlyout.Hide();

            targetGrid.Children.Add(progressBar);
            targetGrid.Children.Add(editButton);
            targetGrid.Children.Add(infoButton);
            targetGrid.Children.Add(headerTextBlock);

            editFlyoutTextBox = new TextBox()
            {
                Width = 250,
                Height = 40,
                PlaceholderText = "Введите новое значение суммы...",
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            editFlyoutTextBox.BeforeTextChanging += (sender, args) =>
            {
                args.Cancel = args.NewText.Any(c => !Char.IsDigit(c));
                if (editFlyoutTextBox.Text.Length > 8)
                {
                    args.Cancel = args.NewText.Any();
                }
            };
            flyoutDeleteTargetButton = new Button()
            {
                Width = 120,
                Content = "Удалить цель",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Right
            };
            flyoutSaveChangesButton = new Button()
            {
                Width = 120,
                Content = "Сохранить",
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Left
            };
            editFlyoutTextBlock = new TextBlock()
            {
                Text = "Введите новую сумму",
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Center
            };
            editFlyoutGrid.Children.Add(editFlyoutTextBlock);
            editFlyoutGrid.Children.Add(editFlyoutTextBox);
            editFlyoutGrid.Children.Add(flyoutDeleteTargetButton);
            editFlyoutGrid.Children.Add(flyoutSaveChangesButton);
            editFlyout.Content = editFlyoutGrid;
            editButton.Flyout = editFlyout;
            editButton.Click += (sender, args) => editFlyout.ShowAt(editButton);

            flyoutSaveChangesButton.Click += (sender, args) => EditTarget(Convert.ToInt32(editFlyoutTextBox.Text), this);
            flyoutDeleteTargetButton.Click += (sender, args) => DeleteTarget(this);


        }

        public void ClearFlyoutTextBox()
        {
            editFlyoutTextBox.Text = "";
        }

        internal void DeleteTarget(TargetNode targetNode)
        {
            int index = 0;
            foreach (Target target in App.TargetsList)
            {
                if (target != null)
                {
                    if (target.Name == targetNode.Name && target.FullSum == targetNode.FullSum &&
                        target.Comment == targetNode.Comment &&
                        target.CurrentSum == targetNode.CurrentSum && target.DateAdded == targetNode.DateAdded &&
                        target.ClientID == targetNode.ClientID)
                    {
                        App.TargetsList.DeleteTarget(index);
                    }
                    else
                        index++;
                }
            }
            targetNode.deleteButtonFlyout.ShowAt(targetNode.flyoutDeleteTargetButton);
        }

        internal static void EditTarget(int newSum, TargetNode targetNode)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(App.targetsPath);
            XmlElement xRoot = xDoc.DocumentElement;

            XmlNode _targetNode = xRoot?.SelectSingleNode($"target[name='{targetNode.Name}' and fullSum='{targetNode.FullSum}'" +
                                                          $" and comment='{targetNode.Comment}' and currentSum='{targetNode.CurrentSum}'" +
                                                          $" and date='{targetNode.DateAdded}' and clientID='{targetNode.ClientID}']");
            if (_targetNode != null)
            {
                foreach (Target target in App.TargetsList)
                {
                    if (target != null)
                    {
                        if (target.Name == targetNode.Name && target.FullSum == targetNode.FullSum &&
                          target.Comment == targetNode.Comment &&
                          target.CurrentSum == targetNode.CurrentSum && target.DateAdded == targetNode.DateAdded &&
                          target.ClientID == targetNode.ClientID)
                        target.CurrentSum = newSum;
                    }
                }
                targetNode.CurrentSum = newSum;
                foreach (XmlNode childNode in _targetNode.ChildNodes)
                {
                    if (childNode != null)
                    {
                        if (childNode.Name == "currentSum")
                            childNode.InnerText = newSum.ToString();
                        targetNode.ClearFlyoutTextBox();
                    }
                }
            }
            xDoc.Save(App.targetsPath);
        }
    }

    public sealed partial class ActiveTargetsPage : Page
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

        public void UpdateTargets()
        {
            TargetViewerStackPanel.Children.Clear();

            string Name = null;
            int FullSum = Int32.MinValue;
            string Comment = null;
            int CurrentSum = Int32.MinValue;
            long DateAdded = Int64.MinValue;
            string ClientID = null;

            foreach (Target target in App.TargetsList)
            {
                if (target != null)
                {
                    Name = target.Name;
                    FullSum = target.FullSum;
                    Comment = target.Comment;
                    CurrentSum = target.CurrentSum;
                    DateAdded = target.DateAdded;
                    ClientID = target.ClientID;

                    TargetNode newNode = new TargetNode();
                    newNode.CreateTargetNode(Name, FullSum, Comment, CurrentSum, DateAdded, ClientID);
                    TargetViewerStackPanel.Children.Add(newNode.targetGrid);
                }
            }
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

        public ActiveTargetsPage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            UpdateTargets();
            UpdateSums();
        }
    }
}
