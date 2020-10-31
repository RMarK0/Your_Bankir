using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
        
        public MainPage()
        {
            this.InitializeComponent();
        }

        private void loginButton_Clicked(object sender, RoutedEventArgs e)
        {
            XmlDocument xDoc = new XmlDocument();
            xDoc.LoadXml();

            MD5 md5hasher = MD5.Create();
            string Password = PasswordBox.Password;
            byte[] data = md5hasher.ComputeHash(Encoding.UTF8.GetBytes(Password));
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            string passwordHash = stringBuilder.ToString();
            

            this.Frame.Navigate(typeof(MainMenuPage));
        }

        
    }
}
