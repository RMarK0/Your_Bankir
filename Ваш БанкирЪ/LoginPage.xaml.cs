using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.ServiceModel.Dispatcher;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
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
    public sealed partial class LoginPage : Page
    {
        private bool InitializeUser(string login, string password, ref string ID)
        {
            XmlReader logPassReader = XmlReader.Create("data/LogPassDB.xml");
            XmlDocument logPassDocument = new XmlDocument();
            logPassDocument.Load(logPassReader);

            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            string inputPasswordHash = stringBuilder.ToString();

            XmlElement logPassRoot = logPassDocument.DocumentElement;
            foreach (XmlNode User in logPassRoot)
            {
                if (User.Attributes.Count > 0)
                {
                    XmlNode loginNode = User.Attributes.GetNamedItem("login");
                    if (loginNode.Value == login)
                    {
                        foreach (XmlNode userChildNode in User.ChildNodes)
                        {
                            if (userChildNode.Name == "passMD5" && userChildNode.InnerText == inputPasswordHash)
                            {
                                foreach (XmlNode childNode in User.ChildNodes)
                                {
                                    if (childNode.Name == "ID" && childNode != null)
                                        ID = childNode.InnerText;
                                }
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }


        private void InitializeData()
        {
            // Метод, осущ. ввод в массив FinancialChangesList данных из XML файла
            // Данные ни в коем случае не должны добавляться до того, как данные из XML файла заполнят массив

        }

        public LoginPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size { Height = 720, Width = 1280 };
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            this.InitializeComponent();
        }

        private void Grid_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                loginButton_Clicked(sender, e);
            }
        }

        public static FinancialChangeList FinancialChangesList;
        public static Client ActiveClient;
        private void loginButton_Clicked(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;
            string ID = null;

            if (InitializeUser(login, password, ref ID))
            {
                ActiveClient = new Client(login, ID); // ОБЯЗАТЕЛЬНО СДЕЛАТЬ ПАРСЕР ИЗ XML
                InitializeData();
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
    }
}
