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
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class CreateNewUserPage : Page
    {
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Visible;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            SystemNavigationManager.GetForCurrentView().BackRequested -= OnBackRequested;
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                AppViewBackButtonVisibility.Disabled;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
            e.Handled = true;
        }

        public CreateNewUserPage()
        {
            this.InitializeComponent();
        }

        private void CreateUserFlyoutButton_OnClick(object sender, RoutedEventArgs e)
        {
            CreateUserFlyout.Hide();
        }

        private void CreateUserButton_OnClick(object sender, RoutedEventArgs e)
        {
            Client newClient;
            string name;
            int id = 0;
            string generation;
            string password;
            bool userExists = false;

            if (UsernameTextBox.Text.Trim() != "")
            {
                foreach (Client client in App.ExistingClientsList)
                {
                    if (client != null)
                    {
                        if (client.Name == UsernameTextBox.Text.Trim())
                        {
                            userExists = true;
                            id++;
                        }

                    }
                }

                if (!userExists)
                {
                    name = UsernameTextBox.Text.Trim();
                    if (PasswordTextBox.Password.Length < 4 || PasswordTextBox.Password.Length > 24 ||
                        RepeatPasswordTextBox.Password.Length < 4 || RepeatPasswordTextBox.Password.Length > 24)
                    {
                        CreateUserFlyoutTextBlock.Text = "Пароль должен содержать от 4 до 24 символов";
                        CreateUserFlyout.ShowAt(CreateUserButton);
                    }
                    else
                    {
                        if (PasswordTextBox.Password == RepeatPasswordTextBox.Password)
                        {
                            password = PasswordTextBox.Password;
                            if (UserGenerationComboBox.SelectedItem != null)
                            {
                                generation = UserGenerationComboBox.SelectedItem.ToString();

                                newClient = new Client(name, id.ToString(), generation);
                                App.ExistingClientsList.Add(newClient);
                                
                                FunctionClass.AddToXml(newClient, password);
                                CreateUserFlyoutTextBlock.Text = "Пользователь успешно создан";
                                CreateUserFlyout.ShowAt(CreateUserButton);

                                UsernameTextBox.Text = String.Empty;
                                PasswordTextBox.Password = String.Empty;
                                RepeatPasswordTextBox.Password = String.Empty;
                                UserGenerationComboBox.SelectedItem = null;
                            }
                            else
                            {
                                CreateUserFlyoutTextBlock.Text = "Выберите поколение";
                                CreateUserFlyout.ShowAt(CreateUserButton);
                            }
                        }
                        else
                        {
                            CreateUserFlyoutTextBlock.Text = "Пароли должны совпадать";
                            CreateUserFlyout.ShowAt(CreateUserButton);
                        }
                    }
                }
                else
                {
                    CreateUserFlyoutTextBlock.Text = "Пользователь с таким именем уже существует";
                    CreateUserFlyout.ShowAt(CreateUserButton);
                }
            }
            else
            {
                CreateUserFlyoutTextBlock.Text = "Поле имени пользователя пустое";
                CreateUserFlyout.ShowAt(CreateUserButton);
            }

            
        }
    }
}
