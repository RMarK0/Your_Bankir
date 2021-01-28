﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.Xml.Linq;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static Ваш_БанкирЪ.App;

namespace Ваш_БанкирЪ
{
    public sealed partial class LoginPage : Page
    {
        public static LoginPage loginPage;

        private bool InitializeUser(string login, string password, ref string ID, ref string generation)
        {
            bool isCorrect = false;

            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            var inputPasswordHash = stringBuilder.ToString();

            var logPassDocument = new XmlDocument();
            logPassDocument.Load(usersPath);
            var logPassRoot = logPassDocument.DocumentElement;
            foreach (XmlNode user in logPassRoot)
            {
                if (user.HasChildNodes)
                {
                    foreach (XmlNode userChildNode in user.ChildNodes)
                    {
                        switch (userChildNode.Name)
                        {
                            case ("login"):
                                if (userChildNode.InnerText == login)
                                {
                                    foreach (XmlNode child in user.ChildNodes)
                                    {
                                        if (child.Name == "passMD5" && child.InnerText == inputPasswordHash)
                                        {
                                            isCorrect = true;
                                        }
                                    }
                                }
                                break;
                            case ("generation"):
                                generation = userChildNode.InnerText;
                                break;
                            case ("ID"):
                                ID = userChildNode.InnerText;
                                break;
                        }
                    }
                }
            }

            if (isCorrect)
                return true;
            return false;
        }

        public LoginPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size {Height = 720, Width = 1280};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            InitializeComponent();
            loginPage = this;
            versionTextBlock.Text = String.Format($"{versionInfo} © Dmitry Rybalko");



        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter) loginButton_Clicked(sender, e);
        }

        private void loginButton_Clicked(object sender, RoutedEventArgs e)
        {
            var login = LoginTextBox.Text;
            var password = PasswordBox.Password;
            string ID = null;
            string generation = "";

            if (InitializeUser(login, password, ref ID, ref generation))
            {
                ActiveClient = new Client(login, ID, generation);
                FunctionClass.InitializeFinances(ref CurrentSum, ref TotalExpenses, ref TotalIncomes,
                    ref ThisMonthExpenses);
                Frame.Navigate(typeof(MainMenuPage));
            }
            else
            {
                PasswordErrorFlyout.ShowAt(LoginButton);
            }
        }

        private void PasswordErrorButton_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordErrorFlyout.Hide();
        }

        private void CreateNewUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CreateNewUserPage));
        }
    }
}
