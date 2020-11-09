using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



namespace Ваш_БанкирЪ
{
    public sealed partial class MainPage : Page
    {
        private bool CheckPasswordComplete(string login, string password)
        {
            XmlReader logPassReader = XmlReader.Create("G:\\Курсач\\Ваш БанкирЪ\\Ваш БанкирЪ\\bin\\x86\\Debug\\AppX\\LogPassDB.xml");
            XmlDocument logPassDocument = new XmlDocument();
            logPassDocument.Load(logPassReader);

            MD5 md5hasher = MD5.Create();
            byte[] data = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            string InputPasswordHash = stringBuilder.ToString();

            XmlElement logPassRoot = logPassDocument.DocumentElement;
            foreach (XmlNode User in logPassRoot)
            {
                if (User.Attributes.Count > 0)
                {
                    XmlNode loginNode = User.Attributes.GetNamedItem("login");
                    if (loginNode.Value == login)
                    {
                        foreach (XmlNode UserChildNode in User.ChildNodes)
                        {
                            if (UserChildNode.Name == "passMD5" && UserChildNode.InnerText == InputPasswordHash)
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        public MainPage()
        {
            this.InitializeComponent();
        }


        private void loginButton_Clicked(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            if (CheckPasswordComplete(login, password))
            {
                PasswordErrorFlyout.Hide();
                Frame.Navigate(typeof(MainMenuPage));
            }
            else
            {
                PasswordErrorFlyout.ShowAt((Button)LoginButton);
            }
        }


        private void PasswordErrorButton_OnClick(object sender, RoutedEventArgs e)
        {
            PasswordErrorFlyout.Hide();
        }
    }
}
