using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
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
using Windows.UI.Xaml.Navigation;
using static Ваш_БанкирЪ.App;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class PasswordChangePage : Page
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

        private static bool ComparePassword(string password, string login, string ID)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            var inputPasswordHash = stringBuilder.ToString();

            var logPassDocument = new XmlDocument();
            logPassDocument.Load(usersPath);
            var logPassRoot = logPassDocument.DocumentElement;
            foreach (XmlNode User in logPassRoot)
                if (User.Attributes.Count > 0)
                {
                    var loginNode = User.Attributes.GetNamedItem("login");
                    if (loginNode.Value == login)
                        foreach (XmlNode userChildNode in User.ChildNodes)
                            if (userChildNode.Name == "passMD5" && userChildNode.InnerText == inputPasswordHash)
                            {
                                foreach (XmlNode childNode in User.ChildNodes)
                                    if (childNode.Name == "ID")
                                        ID = childNode.InnerText;
                                return true;
                            }
                }
            return false;
        }

        public PasswordChangePage()
        {
            this.InitializeComponent();
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        private void ChangePasswordButton_OnClick(object sender, RoutedEventArgs e)
        {
            string login = ActiveClient.Name;
            string ID = ActiveClient.ID;

            if (OldPasswordBox.Password != "")
            {
                var password = OldPasswordBox.Password;
                if (ComparePassword(password, login, ID))
                {
                    if (NewPasswordBox.Password == RepeatPasswordBox.Password)
                    {
                        if (NewPasswordBox.Password.Length >= 8 && NewPasswordBox.Password.Length <= 24)
                        {
                            var newPassword = NewPasswordBox.Password;

                            var md5Hasher = MD5.Create();
                            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(newPassword));

                            XmlDocument xDoc = new XmlDocument();
                            xDoc.Load(usersPath);
                            XmlElement xRoot = xDoc.DocumentElement;

                            XmlNode userNode = xRoot?.SelectSingleNode($"user[ID={ID}]");
                            if (userNode != null)
                                foreach (XmlNode node in userNode.ChildNodes)
                                {
                                    if (node.Name == "passMD5")
                                    {
                                        node.InnerText = data.ToString();
                                    }
                                }

                            FlyoutTextBlock.Text = "Пароль успешно изменен";
                            ChangePasswordButtonFlyout.ShowAt(ChangePasswordButton);
                        }
                        else
                        {
                            FlyoutTextBlock.Text = "Новый пароль должен содержать 8-24 символов";
                            ChangePasswordButtonFlyout.ShowAt(ChangePasswordButton);
                        }
                    }
                    else
                    {
                        FlyoutTextBlock.Text = "Новые пароли не совпадают";
                        ChangePasswordButtonFlyout.ShowAt(ChangePasswordButton);
                    }
                }
                else
                {
                    FlyoutTextBlock.Text = "Старый пароль введет неверно";
                    ChangePasswordButtonFlyout.ShowAt(ChangePasswordButton);
                }
            }
            else
            {
                FlyoutTextBlock.Text = "Введите старый пароль";
                ChangePasswordButtonFlyout.ShowAt(ChangePasswordButton);
            }
        }

        private void FlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            ChangePasswordButtonFlyout.Hide();
        }
    }
}
