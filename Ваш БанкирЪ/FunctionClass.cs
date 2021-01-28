using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Ваш_БанкирЪ
{
    public static class FunctionClass
    {
        internal static void AddToXml(Client obj, string password)
        {
            var md5Hasher = MD5.Create();
            var data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(password));

            var stringBuilder = new StringBuilder();
            for (var i = 0; i < data.Length; i++)
                stringBuilder.Append(data[i].ToString("x2"));
            var passwordHash = stringBuilder.ToString();

            var usersXmlDocument = new XmlDocument();
            usersXmlDocument.Load(App.usersPath);
            var usersRoot = usersXmlDocument.DocumentElement;

            XmlElement userElement = usersXmlDocument.CreateElement("user");
            XmlElement loginElement = usersXmlDocument.CreateElement("login");
            XmlElement passwordElement = usersXmlDocument.CreateElement("passMD5");
            XmlElement generationElement = usersXmlDocument.CreateElement("generation");
            XmlElement idElement = usersXmlDocument.CreateElement("ID");

            XmlText loginText = usersXmlDocument.CreateTextNode(obj.Name);
            XmlText passwordText = usersXmlDocument.CreateTextNode(passwordHash);
            XmlText generationText = usersXmlDocument.CreateTextNode(obj.Generation);
            XmlText idText = usersXmlDocument.CreateTextNode(obj.ID);

            loginElement.AppendChild(loginText);
            passwordElement.AppendChild(passwordText);
            generationElement.AppendChild(generationText);
            idElement.AppendChild(idText);

            userElement.AppendChild(loginElement);
            userElement.AppendChild(passwordElement);
            userElement.AppendChild(generationElement);
            userElement.AppendChild(idElement);

            if (usersRoot != null) usersRoot.AppendChild(userElement);
            else throw new NullReferenceException("targetRoot was null");
            usersXmlDocument.Save(App.usersPath);
        }

        internal static void AddToXml(Target obj)
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

        internal static void AddToXml(FinancialChange obj)
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
            XmlDocument targetXmlDocument = new XmlDocument();
            targetXmlDocument.Load(App.targetsPath);

            XmlElement targetRoot = targetXmlDocument.DocumentElement;

            if (targetRoot!= null) targetRoot.ChildNodes[index].RemoveAll();
            else throw new NullReferenceException("targetRoot was null");
            targetRoot.RemoveChild(targetRoot.ChildNodes[index]);

            targetXmlDocument.Save(App.targetsPath);
        }

        internal static void DeleteFromXML(int index, FinancialChange obj)
        {
            XmlDocument changesXmlDocument = new XmlDocument();
            changesXmlDocument.Load(App.changesPath);

            XmlElement changesRoot = changesXmlDocument.DocumentElement;

            if (changesRoot != null) changesRoot.ChildNodes[index].RemoveAll();
            else throw new NullReferenceException("changesRoot was null");
            changesRoot.RemoveChild(changesRoot.ChildNodes[index]);

            changesXmlDocument.Save(App.changesPath);
        }

        internal static string GetClientFromID(string clientID)
        {
            XmlReader clientReader = XmlReader.Create(App.usersPath);
            XmlDocument clientDocument = new XmlDocument();
            clientDocument.Load(clientReader);

            XmlElement clientRoot = clientDocument.DocumentElement;
            string xPath = string.Format($"user[ID='{clientID}']");

            XmlNode clientNode = clientRoot.SelectSingleNode(xPath);
            if (clientNode != null)
            {
                if (clientNode.Attributes != null)
                {
                    XmlNode attribute = clientNode.Attributes.GetNamedItem("login");
                    var output = attribute.Value;
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

        internal static void InitializeFinances(ref int CurrentSum, ref int TotalExpenses, ref int TotalIncomes, ref int ThisMonthExpenses)
        {
            foreach (FinancialChange change in App.FinancialChangesList)
            {
                if (change != null)
                {
                    if (change.IsIncome)
                    {
                        TotalIncomes += change.Sum;
                    }
                    else
                    {
                        TotalExpenses += change.Sum;
                    }

                    DateTime temp = DateTime.FromBinary(change.Date);
                    if (!change.IsIncome && temp.Month == DateTime.Now.Month)
                    {
                        ThisMonthExpenses += change.Sum;
                    }
                }
            }
            CurrentSum = TotalIncomes - TotalExpenses;
        }
    }
}
