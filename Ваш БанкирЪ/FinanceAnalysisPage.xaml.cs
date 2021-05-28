using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
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
using WinRTXamlToolkit.Controls.DataVisualization.Charting;
using static Ваш_БанкирЪ.App;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>

    public class ChartData
    {
        public string DataName { get; set; }
        public int DataValue { get; set; }
    }

    public sealed partial class FinanceAnalysisPage : Page
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

        public void InitializeCharts(Client client)
        {
            List<ChartData> ExpensesChartInfo = new List<ChartData>();
            List<ChartData> IncomesChartInfo = new List<ChartData>();

            foreach (FinancialChange change in FinancialChangesList)
            {
                DateTime changeDate = DateTime.FromBinary(change.Date);
                if (changeDate.Month == displayedDate.Month && changeDate.Year == displayedDate.Year)
                {
                    int SumValue = 0;
                    string Category = null;
                    bool categoryExists = false;

                    if (change.IsIncome)
                    {
                        foreach (ChartData node in IncomesChartInfo)
                        {
                            if (change.Category == node.DataName)
                            {
                                categoryExists = true;
                                break;
                            }
                            
                        }

                        if (!categoryExists)
                            Category = change.Category;

                        foreach (FinancialChange ch in FinancialChangesList)
                        {
                            if (client != null)
                            {
                                if (ch.Category == Category && ch.ClientId == client.ID)
                                    SumValue += ch.Sum;

                            }
                            else
                            {
                                if (ch.Category == Category)
                                    SumValue += ch.Sum;
                            }
                        }

                        if (Category != null)
                            IncomesChartInfo.Add(new ChartData {DataName = Category, DataValue = SumValue});
                    }
                    else
                    {
                        foreach (ChartData node in ExpensesChartInfo)
                        {
                            if (change.Category == node.DataName)
                            {
                                categoryExists = true;
                                break;
                            }
                        }

                        if (!categoryExists)
                            Category = change.Category;

                        foreach (FinancialChange ch in FinancialChangesList)
                        {
                            if (client != null)
                            {
                                if (ch.Category == Category && ch.ClientId == client.ID)
                                    SumValue += ch.Sum;

                            }
                            else
                            {
                                if (ch.Category == Category)
                                    SumValue += ch.Sum;
                            }
                        }
                        
                        if (Category != null)
                            ExpensesChartInfo.Add(new ChartData {DataName = Category, DataValue = SumValue});
                    }
                }
            }
            ((PieSeries) IncomesChart.Series[0]).ItemsSource = IncomesChartInfo;
            ((PieSeries) ExpensesChart.Series[0]).ItemsSource = ExpensesChartInfo;

            InitializeFinancialLists(ExpensesChartInfo, IncomesChartInfo);
        }

        public void InitializeFinancialLists(List<ChartData> expensesList, List<ChartData> incomesList)
        {
            foreach (ChartData item in expensesList)
            {
                //TextBlock textBlock

            }

            foreach (ChartData item in incomesList)
            {


            }
            //  TODO: сделать списки под диаграммами по категориям трат
        }

        public void InitializeDisplayedDate()
        {
            displayedDate = DateTime.Now.Date;
            DisplayedDateTextBlock.Text = String.Format($"{dateFormat.GetMonthName(displayedDate.Month)} {displayedDate.Year}");
        }

        public void IncrementDisplayedDate(object sender, RoutedEventArgs e)
        {
            displayedDate = displayedDate.AddMonths(1);
            DisplayedDateTextBlock.Text = String.Format($"{dateFormat.GetMonthName(displayedDate.Month)} {displayedDate.Year}");
            if (UserSwitch.IsOn)
                InitializeCharts(ActiveClient);
            else
                InitializeCharts(null);
        }

        public void DecrementDisplayedDate(object sender, RoutedEventArgs e)
        {
            displayedDate = displayedDate.AddMonths(-1);
            DisplayedDateTextBlock.Text = String.Format($"{dateFormat.GetMonthName(displayedDate.Month)} {displayedDate.Year}");
            if (UserSwitch.IsOn)
                InitializeCharts(ActiveClient);
            else
                InitializeCharts(null);
        }

        private readonly DateTimeFormatInfo dateFormat = new DateTimeFormatInfo();
        private DateTime displayedDate;
        public FinanceAnalysisPage()
        {
            this.InitializeComponent();
            InitializeDisplayedDate();
            InitializeCharts(null);
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;

            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent(
                "Windows.UI.Xaml.Media.XamlCompositionBrushBase"))
            {
                BottomFiller.Fill = new AcrylicBrush()
                {
                    BackgroundSource = AcrylicBackgroundSource.HostBackdrop,
                    TintOpacity = 0.75,
                    TintColor = Color.FromArgb(255, 0, 0, 0),
                    Opacity = 1
                };
            }
        }

        private void ToggleSwitch_OnToggled(object sender, RoutedEventArgs e)
        {
            if (UserSwitch.IsOn)
            {
                InitializeCharts(ActiveClient);
            }
            else
            {
                InitializeCharts(null);
            }
        }
    }
}
