using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ваш_БанкирЪ
{
    public static class FunctionClass
    {
        public static void InitializeUserDB()
        {
            XmlDocument document = new XmlDocument();
            document.Load(App.usersPath);
            XmlElement root = document.DocumentElement;

            string loginStr, passMD5Str, IDStr;

            for (int i = 1; i < 7; i++)
            {
                IDStr = i.ToString();
                switch (i)
                {
                    case (1):
                        loginStr = "Dmitry";
                        passMD5Str = "1364475428ad82dda84cbf2dce41e909";
                        break;
                    case (2):
                        loginStr = "Zhanna";
                        passMD5Str = "325ccfad88a9025cbb704ed0f12fa602";
                        break;
                    case (3):
                        loginStr = "Andrey";
                        passMD5Str = "3fbb16dead2a8acac3fe81e6dcc7b3c2";
                        break;
                    case (4):
                        loginStr = "Taya";
                        passMD5Str = "9627d171eff9e712f370d981d8cca894";
                        break;
                    case (5):
                        loginStr = "Leonid";
                        passMD5Str = "0a7083c14b732d58416ecaf9b082cd97";
                        break;
                    case (6):
                        loginStr = "Galina";
                        passMD5Str = "bc6c62b9d688cdf9e2e8fd3747fe8272";
                        break;
                    default:
                        throw new Exception("No available name for this ID");
                }

                XmlElement user = document.CreateElement("user");
                XmlAttribute login = document.CreateAttribute("login");
                XmlElement passMD5 = document.CreateElement("passMD5");
                XmlElement ID = document.CreateElement("ID");

                XmlText IDText = document.CreateTextNode(IDStr);
                XmlText loginText = document.CreateTextNode(loginStr);
                XmlText passText = document.CreateTextNode(passMD5Str);

                login.AppendChild(loginText);
                passMD5.AppendChild(passText);
                ID.AppendChild(IDText);
                user.Attributes.Append(login);
                user.AppendChild(passMD5);
                user.AppendChild(ID);
                root?.AppendChild(user);
                document.Save(App.usersPath);
            }
        }

        public static void InitializeData()
        {
            // Метод, осущ. ввод в массив FinancialChangesList и TargetList данных из XML файла
            // Данные ни в коем случае не должны добавляться до того, как данные из XML файла заполнят массив

            XmlReader targetReader = XmlReader.Create(App.targetsPath);
            XmlReader changesReader = XmlReader.Create(App.changesPath);
            XmlDocument targetXmlDocument = new XmlDocument();
            XmlDocument changesXmlDocument = new XmlDocument();
            targetXmlDocument.Load(targetReader);
            changesXmlDocument.Load(changesReader);

            XmlElement targetRoot = targetXmlDocument.DocumentElement;
            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            App.FinancialChangesList = new FinancialChangeList(200); // сделать настройку Capacity из настроек
            App.TargetsList = new TargetList(15);                    // аналогично и тут
            
            int ChangesXML = 0; // сделать проверку на соотв. массиву
            int TargetsXML = 0;

            foreach (XmlNode Target in targetRoot)
            {
                string name = "";
                int fullSum = -1;
                string comment = "";
                int currentSum = -1;
                long dateAdded = -1;
                string clientId = ""; 

                foreach (XmlNode TargetChildNode in Target.ChildNodes)
                {
                    switch (TargetChildNode.Name)
                    {
                        case ("name"):
                            name = TargetChildNode.InnerText;
                            break;
                        case ("fullSum"):
                            fullSum = Convert.ToInt32(TargetChildNode.InnerText);
                            break;
                        case ("comment"):
                            comment = TargetChildNode.InnerText;
                            break;
                        case ("currentSum"):
                            currentSum = Convert.ToInt32(TargetChildNode.InnerText);
                            break;
                        case ("date"):
                            dateAdded = Convert.ToInt64(TargetChildNode.InnerText);
                            break;
                        case ("clientID"):
                            clientId = TargetChildNode.InnerText;
                            break;
                    }
                }
                if (name != "" && fullSum != -1 && comment != "" && currentSum != -1 && dateAdded != -1 && clientId != "")
                    App.TargetsList.AddTarget(name, fullSum, comment, dateAdded, currentSum, clientId);
            }

            foreach (XmlNode Change in changesRoot)
            {
                long date = -1;
                int sum = -1;
                string category = "";
                string clientID = "";
                bool isIncome = false;
                string comment = "";

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
                if (date != -1 && sum != -1 && category != "" && clientID != "" && comment != "")
                    App.FinancialChangesList.AddFinancialChange(sum, isIncome, comment, category, date, clientID);
            }

            targetReader.Close();
            changesReader.Close();
        }

        internal static void AddToXML(Target obj)
        {
            var targetXmlDocument = new XmlDocument();
            targetXmlDocument.Load(App.targetsPath);
            var targetRoot = targetXmlDocument.DocumentElement;

            var targetElement = targetXmlDocument.CreateElement("target");
            var nameElement = targetXmlDocument.CreateElement("name");
            var fullSumElement = targetXmlDocument.CreateElement("fullSum");
            var commentElement = targetXmlDocument.CreateElement("comment");
            var currentSumElement = targetXmlDocument.CreateElement("currentSum");
            var dateElement = targetXmlDocument.CreateElement("date");
            var clientIDElement = targetXmlDocument.CreateElement("clientID");

            var nameText = targetXmlDocument.CreateTextNode(obj.Name);
            var fullSumText = targetXmlDocument.CreateTextNode(obj.FullSum.ToString());
            var commentText = targetXmlDocument.CreateTextNode(obj.Comment);
            var currentSumText = targetXmlDocument.CreateTextNode(obj.CurrentSum.ToString());
            var dateText = targetXmlDocument.CreateTextNode(obj.DateAdded.ToString());
            var clientIDText = targetXmlDocument.CreateTextNode(obj.ClientID);

            nameElement.AppendChild(nameText);
            fullSumElement.AppendChild(fullSumText);
            commentElement.AppendChild(commentText);
            currentSumElement.AppendChild(currentSumText);
            dateElement.AppendChild(dateText);
            clientIDElement.AppendChild(clientIDText);

            targetElement.AppendChild(nameElement);
            targetElement.AppendChild(fullSumElement);
            targetElement.AppendChild(commentElement);
            targetElement.AppendChild(currentSumElement);
            targetElement.AppendChild(dateElement);
            targetElement.AppendChild(clientIDElement);

            if (targetRoot != null) targetRoot.AppendChild(targetElement);
            else throw new NullReferenceException("targetRoot was null");
            targetXmlDocument.Save(App.targetsPath);
        }

        internal static void AddToXML(FinancialChange obj)
        {
            // XmlReader changesReader = XmlReader.Create("data/ChangesData.xml");
            XmlDocument changesXmlDocument = new XmlDocument();
            changesXmlDocument.Load(App.changesPath);
            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            XmlElement changesElement = changesXmlDocument.CreateElement("change");
            XmlElement dateElement = changesXmlDocument.CreateElement("date");
            XmlElement sumElement = changesXmlDocument.CreateElement("sum");
            XmlElement categoryElement = changesXmlDocument.CreateElement("category");
            XmlElement commentElement = changesXmlDocument.CreateElement("comment");
            XmlElement isIncomeElement = changesXmlDocument.CreateElement("isIncome");
            XmlElement clientIDElement = changesXmlDocument.CreateElement("clientID");

            XmlText dateText = changesXmlDocument.CreateTextNode(obj.Date.ToString());
            XmlText sumText = changesXmlDocument.CreateTextNode(obj.Sum.ToString());
            XmlText categoryText = changesXmlDocument.CreateTextNode(obj.Category);
            XmlText commentText = changesXmlDocument.CreateTextNode(obj.Comment);
            XmlText isIncomeText = changesXmlDocument.CreateTextNode(obj.IsIncome.ToString());
            XmlText clientIDText = changesXmlDocument.CreateTextNode(obj.ClientId);

            dateElement.AppendChild(dateText);
            sumElement.AppendChild(sumText);
            categoryElement.AppendChild(categoryText);
            commentElement.AppendChild(commentText);
            isIncomeElement.AppendChild(isIncomeText);
            clientIDElement.AppendChild(clientIDText);

            changesElement.AppendChild(dateElement);
            changesElement.AppendChild(sumElement);
            changesElement.AppendChild(categoryElement);
            changesElement.AppendChild(commentElement);
            changesElement.AppendChild(isIncomeElement);
            changesElement.AppendChild(clientIDElement);

            if (changesRoot != null) changesRoot.AppendChild(changesElement);
            else throw new NullReferenceException("changesRoot was null");
            
            changesXmlDocument.Save(App.changesPath);

            //changesReader.Close();
        }

        internal static void DeleteFromXML(int index, Target obj)
        {
            XmlReader targetReader = XmlReader.Create(App.targetsPath);
            XmlDocument targetXmlDocument = new XmlDocument();
            targetXmlDocument.Load(targetReader);

            XmlElement targetRoot = targetXmlDocument.DocumentElement;

            if (targetRoot!= null) targetRoot.ChildNodes[index].RemoveAll();
            else throw new NullReferenceException("targetRoot was null");
            targetRoot.RemoveChild(targetRoot.ChildNodes[index]);

            targetReader.Close();
        }

        internal static void DeleteFromXML(int index, FinancialChange obj)
        {
            XmlReader changesReader = XmlReader.Create(App.changesPath);
            XmlDocument changesXmlDocument = new XmlDocument();
            changesXmlDocument.Load(changesReader);

            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            if (changesRoot != null) changesRoot.ChildNodes[index].RemoveAll();
            else throw new NullReferenceException("changesRoot was null");
            changesRoot.RemoveChild(changesRoot.ChildNodes[index]);

            changesReader.Close();
        }

        internal static string GetClientFromID(string clientID)
        {
            string output;

            XmlReader clientReader = XmlReader.Create(App.usersPath);
            XmlDocument clientDocument = new XmlDocument();
            clientDocument.Load(clientReader);

            XmlElement clientRoot = clientDocument.DocumentElement;
            string xPath = String.Format($"user[ID='{clientID}']");

            XmlNode clientNode = clientRoot.SelectSingleNode(xPath);
            if (clientNode != null)
            {
                if (clientNode.Attributes != null)
                {
                    XmlNode attribute = clientNode.Attributes.GetNamedItem("login");
                    output = attribute.Value;
                    return output;
                }
                else
                {
                    throw new Exception($"No attributes for client with ID {clientID}");
                }
            }
            else
            {
                throw new Exception("Client node with provided ID wasn't found");
            }
        }
    }
}
