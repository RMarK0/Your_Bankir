using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ваш_БанкирЪ
{
    public static class FunctionClass
    {
        public static void InitializeData()
        {
            // Метод, осущ. ввод в массив FinancialChangesList и TargetList данных из XML файла
            // Данные ни в коем случае не должны добавляться до того, как данные из XML файла заполнят массив

            XmlReader targetReader = XmlReader.Create("data/TargetsData.xml");
            XmlReader changesReader = XmlReader.Create("data/ChangesData.xml");
            XmlDocument targetXmlDocument = new XmlDocument();
            XmlDocument changesXmlDocument = new XmlDocument();
            targetXmlDocument.Load(targetReader);
            changesXmlDocument.Load(changesReader);

            XmlElement targetRoot = targetXmlDocument.DocumentElement;
            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            App.FinancialChangesList = new FinancialChangeList(200); // сделать настройку Capacity из настроек
            App.TargetsList = new TargetList(15);                    // аналогично и тут
            
            int ChangesXML = 0;
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
                XmlReader targetReader = XmlReader.Create("data/TargetsData.xml");
                XmlDocument targetXmlDocument = new XmlDocument();
                targetXmlDocument.Load(targetReader);
                XmlElement targetRoot = targetXmlDocument.DocumentElement;

                XmlElement targetElement = targetXmlDocument.CreateElement("target");
                XmlElement nameElement = targetXmlDocument.CreateElement("name");
                XmlElement fullSumElement = targetXmlDocument.CreateElement("fullSum");
                XmlElement commentElement = targetXmlDocument.CreateElement("comment");
                XmlElement currentSumElement = targetXmlDocument.CreateElement("currentSum");
                XmlElement dateElement = targetXmlDocument.CreateElement("date");
                XmlElement clientIDElement = targetXmlDocument.CreateElement("clientID");

                XmlText nameText = targetXmlDocument.CreateTextNode(obj.Name);
                XmlText fullSumText = targetXmlDocument.CreateTextNode(obj.FullSum.ToString());
                XmlText commentText = targetXmlDocument.CreateTextNode(obj.Comment);
                XmlText currentSumText = targetXmlDocument.CreateTextNode(obj.CurrentSum.ToString());
                XmlText dateText = targetXmlDocument.CreateTextNode(obj.DateAdded.ToString());
                XmlText clientIDText = targetXmlDocument.CreateTextNode(obj.ClientID);

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
            targetXmlDocument.Save("data/TargetsData.xml");

                targetReader.Close();
        }

        internal static void AddToXML(FinancialChange obj)
        {
            XmlReader changesReader = XmlReader.Create("data/ChangesData.xml");
            XmlDocument changesXmlDocument = new XmlDocument();
            changesXmlDocument.Load(changesReader);
            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            XmlElement changesElement = changesXmlDocument.CreateElement("target");
            XmlElement dateElement = changesXmlDocument.CreateElement("date");
            XmlElement sumElement = changesXmlDocument.CreateElement("sum");
            XmlElement categoryElement = changesXmlDocument.CreateElement("category");
            XmlElement commentElement = changesXmlDocument.CreateElement("comment");
            XmlElement isIncomeElement = changesXmlDocument.CreateElement("isIncome");
            XmlElement clientIDElement = changesXmlDocument.CreateElement("clientID");

            XmlText dateText = changesXmlDocument.CreateTextNode(obj.date.ToString());
            XmlText sumText = changesXmlDocument.CreateTextNode(obj.sum.ToString());
            XmlText categoryText = changesXmlDocument.CreateTextNode(obj.category);
            XmlText commentText = changesXmlDocument.CreateTextNode(obj.comment);
            XmlText isIncomeText = changesXmlDocument.CreateTextNode(obj.isIncome.ToString());
            XmlText clientIDText = changesXmlDocument.CreateTextNode(obj.clientID);

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
            changesXmlDocument.Save("data/ChangesData.xml");

            changesReader.Close();
        }

        internal static void DeleteFromXML(int index, Target obj)
        {
            XmlReader targetReader = XmlReader.Create("data/TargetsData.xml");
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
            XmlReader changesReader = XmlReader.Create("data/ChangesData.xml");
            XmlDocument changesXmlDocument = new XmlDocument();
            changesXmlDocument.Load(changesReader);

            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            if (changesRoot != null) changesRoot.ChildNodes[index].RemoveAll();
            else throw new NullReferenceException("changesRoot was null");
            changesRoot.RemoveChild(changesRoot.ChildNodes[index]);

            changesReader.Close();
        }
    }
}
