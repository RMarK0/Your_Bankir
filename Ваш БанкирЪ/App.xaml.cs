using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Инициализирует одноэлементный объект приложения.  Это первая выполняемая строка разрабатываемого
        /// кода; поэтому она является логическим эквивалентом main() или WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        /// <summary>
        /// Вызывается при обычном запуске приложения пользователем.  Будут использоваться другие точки входа,
        /// например, если приложение запускается для открытия конкретного файла.
        /// </summary>
        /// <param name="e">Сведения о запросе и обработке запуска.</param>

        public static FinancialChangeList FinancialChangesList;
        public static TargetList TargetsList;
        public static Client ActiveClient;

        private static readonly StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private static StorageFile usersFile;
        private static StorageFile changesFile;
        private static StorageFile targetsFile;

        internal static string changesPath = storageFolder.Path + "/ChangesData.xml";
        internal static string usersPath = storageFolder.Path + "/LogPassDB.xml";
        internal static string targetsPath = storageFolder.Path + "/TargetsData.xml";
        // TODO: Сделать инициализацию XML заново...

        private async void InitializeFiles()
        {
            usersFile = await storageFolder.CreateFileAsync("LogPassDB.xml", CreationCollisionOption.OpenIfExists);
            changesFile = await storageFolder.CreateFileAsync("ChangesData.xml", CreationCollisionOption.OpenIfExists);
            targetsFile = await storageFolder.CreateFileAsync("TargetsData.xml", CreationCollisionOption.OpenIfExists);


            string text = await Windows.Storage.FileIO.ReadTextAsync(usersFile);
            if (text.Trim() == "")
            {
                XmlWriterSettings settings = new XmlWriterSettings {Indent = true};
                
                string path = storageFolder.Path + "/LogPassDB.xml"; 
                XmlWriter writer = XmlWriter.Create(path, settings); 
                writer.WriteStartDocument();
                writer.WriteStartElement("users");
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();

                FunctionClass.InitializeUserDB();
            }

            text = await Windows.Storage.FileIO.ReadTextAsync(changesFile);
            if (text.Trim() == "")
            {
                string path = storageFolder.Path + "/ChangesData.xml";
                XmlWriter writer = XmlWriter.Create(path);
                writer.WriteStartDocument();
                writer.WriteStartElement("changes");
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }

            text = await Windows.Storage.FileIO.ReadTextAsync(targetsFile);
            if (text.Trim() == "")
            {
                string path = storageFolder.Path + "/TargetsData.xml"; 
                XmlWriter writer = XmlWriter.Create(path);
                writer.WriteStartDocument();
                writer.WriteStartElement("targets");
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
            }

        }

        protected override void OnLaunched(LaunchActivatedEventArgs e)
        {
            InitializeFiles();
            Frame rootFrame = Window.Current.Content as Frame;
            ApplicationView.PreferredLaunchViewSize = new Size(900, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                // Размещение фрейма в текущем окне
                Window.Current.Content = rootFrame;
            }

            if (e.PrelaunchActivated == false)
            {
                if (rootFrame.Content == null)
                {
                    // Если стек навигации не восстанавливается для перехода к первой странице,
                    // настройка новой страницы путем передачи необходимой информации в качестве параметра
                    // параметр
                    rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        /// <summary>
        /// Вызывается в случае сбоя навигации на определенную страницу
        /// </summary>
        /// <param name="sender">Фрейм, для которого произошел сбой навигации</param>
        /// <param name="e">Сведения о сбое навигации</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Вызывается при приостановке выполнения приложения.  Состояние приложения сохраняется
        /// без учета информации о том, будет ли оно завершено или возобновлено с неизменным
        /// содержимым памяти.
        /// </summary>
        /// <param name="sender">Источник запроса приостановки.</param>
        /// <param name="e">Сведения о запросе приостановки.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Сохранить состояние приложения и остановить все фоновые операции
            deferral.Complete();
        }
    }
}
