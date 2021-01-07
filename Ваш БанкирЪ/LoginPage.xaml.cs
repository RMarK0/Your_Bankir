using System;
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
        private bool InitializeUser(string login, string password, ref string ID)
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

        public LoginPage()
        {
            ApplicationView.PreferredLaunchViewSize = new Size {Height = 720, Width = 1280};
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;

            InitializeComponent();
            
            versionTextBox.Text = versionInfo.ToString();
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

            if (InitializeUser(login, password, ref ID))
            {
                ActiveClient = new Client(login, ID);
                FunctionClass.InitializeData();
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
    }
}
