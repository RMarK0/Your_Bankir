﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// Документацию по шаблону элемента "Пустая страница" см. по адресу https://go.microsoft.com/fwlink/?LinkId=234238

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Пустая страница, которую можно использовать саму по себе или для перехода внутри фрейма.
    /// </summary>
    public sealed partial class FirstStartupPage : Page
    {
        public static FirstStartupPage startupPage;

        public void TextFadingIn()
        {
            TextFadeIn.Begin();
        }
        public void TextFadingOut()
        {
            TextFadeOut.Begin();
        }

        public Image StartupGridImage = new Image()
        {
            Source = new BitmapImage(new Uri("ms-appx:///Assets/background-filler-gif.gif")),
            Stretch = Stretch.Fill
        };

        public void ChangeHeaderText(string text)
        {
            HeaderTextBlock.Text = text;
        }

        public FirstStartupPage()
        {
            this.InitializeComponent();
            startupPage = this;

            ((Storyboard)Resources["GradientAnimation"]).Begin();
        }
    }
}
