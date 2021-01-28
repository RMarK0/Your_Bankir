using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
using static Ваш_БанкирЪ.FirstStartupPage;

namespace Ваш_БанкирЪ
{
    /// <summary>
    /// Обеспечивает зависящее от конкретного приложения поведение, дополняющее класс Application по умолчанию.
    /// </summary>
    sealed partial class App : Application
    {
        private DispatcherTimer timer;
        private int secondsCounted;
        private bool isFirstStartup = false;

        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;

            timer = new DispatcherTimer() { Interval = new TimeSpan(0,0,1) };
            timer.Tick += TimerOnTick;
            timer.Start();
        }

        private void TimerOnTick(object sender, object e)
        {
            secondsCounted++;
            if (isFirstStartup)
            {
                if (secondsCounted == 3)
                {
                    startupPage.TextFadingOut();
                }

                if (secondsCounted == 4)
                {
                    startupPage.ChangeHeaderText("Мы проводим первую настройку приложения");
                    startupPage.TextFadingIn();
                }
                
                if (secondsCounted == 7)
                {
                    startupPage.TextFadingOut();
                }

                if (secondsCounted == 8)
                {
                    startupPage.ChangeHeaderText("Сейчас вы создадите вашу учетную запись");
                    startupPage.TextFadingIn();
                }

                if (secondsCounted == 11)
                {
                    startupPage.TextFadingOut();
                }

                if (secondsCounted == 12)
                {
                    startupPage.ChangeHeaderText("Приступаем");
                    startupPage.TextFadingIn();
                }

                if (secondsCounted == 16)
                    rootFrame.Navigate(typeof(CreateNewUserPage));
            }
        }

        public static int CurrentSum;
        public static int TotalExpenses;
        public static int TotalIncomes;
        public static int ThisMonthExpenses;

        public static FinancialChangeList FinancialChangesList;
        public static TargetList TargetsList;
        public static List<Client> ExistingClientsList;
        public static Client ActiveClient;

        private static readonly StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
        private static StorageFile usersFile;
        private static StorageFile changesFile;
        private static StorageFile targetsFile;

        internal static string changesPath = storageFolder.Path + "/ChangesData.xml";
        internal static string usersPath = storageFolder.Path + "/LogPassDB.xml";
        internal static string targetsPath = storageFolder.Path + "/TargetsData.xml";

        internal static Version versionInfo = typeof(App).GetTypeInfo().Assembly.GetName().Version;

        private Frame rootFrame;

        private async void InitializeFiles()
        {
            usersFile = await storageFolder.CreateFileAsync("LogPassDB.xml", CreationCollisionOption.OpenIfExists);
            changesFile = await storageFolder.CreateFileAsync("ChangesData.xml", CreationCollisionOption.OpenIfExists);
            targetsFile = await storageFolder.CreateFileAsync("TargetsData.xml", CreationCollisionOption.OpenIfExists);
            string text = await FileIO.ReadTextAsync(usersFile);
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
                }

                text = await FileIO.ReadTextAsync(changesFile);
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

                text = await FileIO.ReadTextAsync(targetsFile);
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

        public static void InitializeData()
        {
            // Метод, осущ. ввод в массив FinancialChangesList и TargetList данных из XML файла
            // Данные ни в коем случае не должны добавляться до того, как данные из XML файла заполнят массив
            XmlDocument targetXmlDocument = new XmlDocument();
            XmlDocument changesXmlDocument = new XmlDocument();
            targetXmlDocument.Load(App.targetsPath);
            changesXmlDocument.Load(App.changesPath);

            XmlElement targetRoot = targetXmlDocument.DocumentElement;
            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            App.FinancialChangesList = new FinancialChangeList(200); // сделать не массив, а List
            App.TargetsList = new TargetList(15);                    // аналогично и тут

            int ChangesXML = 0;
            int TargetsXML = 0;

            foreach (XmlNode Target in targetRoot)
                TargetsXML++;

            if (App.TargetsList.Capacity > TargetsXML)
            {
                string name = "";
                int fullSum = -1;
                string comment = "";
                int currentSum = -1;
                long dateAdded = -1;
                string clientID = "";

                foreach (XmlNode Target in targetRoot)
                {
                    foreach (XmlNode targetChildNode in Target.ChildNodes)
                    {
                        switch (targetChildNode.Name)
                        {
                            case ("name"):
                                name = targetChildNode.InnerText;
                                break;
                            case ("fullSum"):
                                fullSum = Convert.ToInt32(targetChildNode.InnerText);
                                break;
                            case ("comment"):
                                comment = targetChildNode.InnerText;
                                break;
                            case ("currentSum"):
                                currentSum = Convert.ToInt32(targetChildNode.InnerText);
                                break;
                            case ("date"):
                                dateAdded = Convert.ToInt64(targetChildNode.InnerText);
                                break;
                            case ("clientID"):
                                clientID = targetChildNode.InnerText;
                                break;
                        }
                    }
                    App.TargetsList.AddTarget(name, fullSum, comment, dateAdded, currentSum, clientID);
                }
            }

            foreach (XmlNode Change in changesRoot)
                ChangesXML++;

            if (App.FinancialChangesList.Capacity > ChangesXML)
            {
                long date = -1;
                int sum = -1;
                string category = "";
                string clientID = "";
                bool isIncome = false;
                string comment = "";

                foreach (XmlNode Change in changesRoot)
                {
                    foreach (XmlNode ChangeChildNode in Change.ChildNodes)
                    {
                        switch (ChangeChildNode.Name)
                        {
                            case ("date"):
                                date = Convert.ToInt64(ChangeChildNode.InnerText);
                                break;
                            case ("sum"):
                                sum = Convert.ToInt32(ChangeChildNode.InnerText);
                                break;
                            case ("category"):
                                category = ChangeChildNode.InnerText;
                                break;
                            case ("clientID"):
                                clientID = ChangeChildNode.InnerText;
                                break;
                            case ("isIncome"):
                                isIncome = Convert.ToBoolean(ChangeChildNode.InnerText);
                                break;
                            case ("comment"):
                                comment = ChangeChildNode.InnerText;
                                break;
                        }
                    }
                    App.FinancialChangesList.AddFinancialChange(sum, isIncome, comment, category, date, clientID);
                }
            }

            XmlDocument usersXmlDocument = new XmlDocument();
            usersXmlDocument.Load(App.usersPath);
            XmlElement root = usersXmlDocument.DocumentElement;
            ExistingClientsList = new List<Client>();

            foreach (XmlNode user in root)
            {
                string generation = "";
                string id = "";
                string name = "";
                foreach (XmlNode userChildNode in user.ChildNodes)
                {
                    switch (userChildNode.Name)
                    {
                        case ("generation"):
                            generation = userChildNode.InnerText;
                            break;
                        case ("id"):
                            id = userChildNode.InnerText;
                            break;
                        case ("name"):
                            name = userChildNode.InnerText;
                            break;
                    }
                }
                Client temp = new Client(name, id, generation);
                App.ExistingClientsList.Add(temp);
            }

        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e)
        {
            await Task.Run(() => InitializeFiles());
            
            rootFrame = Window.Current.Content as Frame;
            ApplicationView.PreferredLaunchViewSize = new Size(900, 720);
            ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            ApplicationView.GetForCurrentView().SetPreferredMinSize(new Size(500, 500));

            // Не повторяйте инициализацию приложения, если в окне уже имеется содержимое,
            // только обеспечьте активность окна
            if (rootFrame == null)
            {
                InitializeData();
                // Создание фрейма, который станет контекстом навигации, и переход к первой странице
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: Загрузить состояние из ранее приостановленного приложения
                }

                if (e.PreviousExecutionState == ApplicationExecutionState.NotRunning)
                {
                    isFirstStartup = true;
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
                    if (e.PreviousExecutionState != ApplicationExecutionState.NotRunning)
                    {

                        rootFrame.Navigate(typeof(LoginPage), e.Arguments);
                    }
                    else if (e.PreviousExecutionState == ApplicationExecutionState.NotRunning && FirstStart())
                    {
                        startupPage = new FirstStartupPage();
                        rootFrame.Navigate(typeof(FirstStartupPage));
                    }
                }
                // Обеспечение активности текущего окна
                Window.Current.Activate();
            }
        }

        private bool FirstStart()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(usersPath);
            XmlElement root = doc.DocumentElement;

            if (root != null)
                foreach (XmlNode node in root)
                {
                    if (node != null)
                        return false;
                }
            return true;
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
