using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;
using WinRTXamlToolkit.Controls;

namespace Ваш_БанкирЪ
{
    internal class FinancialChange
    {
        public long Date { get; private set; }
        public int Sum { get; private set; }
        public string Category { get; private set; }
        public string ClientId { get; private set; }
        public bool IsIncome { get; private set; }
        public string Comment { get; private set; }

        public FinancialChange(int sum, bool isIncome, string comment, string category)
        {
            this.Comment = comment;
            this.Sum = sum;
            this.IsIncome = isIncome;
            this.Category = category;

            ClientId = App.ActiveClient.ID;
            Date = DateTime.Now.ToBinary();
        }

        public FinancialChange(int sum, bool isIncome, string category)
        {
            this.Sum = sum;
            this.IsIncome = isIncome;
            this.Category = category;

            Comment = "Комментарий отсутствует";
            ClientId = App.ActiveClient.ID;
            Date = DateTime.Now.ToBinary();
        }

        public FinancialChange(int sum, bool isIncome, string comment, string category, long date,
            string clientID)
        {
            this.Comment = comment;
            this.Sum = sum;
            this.IsIncome = isIncome;
            this.Category = category;
            this.ClientId = clientID;
            this.Date = date;
        }
    }

    public class FinancialChangeList : IEnumerable
    {
        private readonly List<FinancialChange> _financialChangesList;
        public int Count { get; private set; }

        public FinancialChangeList()
        {
            Count = 0;
            _financialChangesList = new List<FinancialChange>();
        }

        public void AddFinancialChange(int sum, bool isIncome, string comment, string category, long date,
            string clientID)
        {
            FinancialChange item = new FinancialChange(sum, isIncome, comment, category, date, clientID);
            _financialChangesList.Add(item);
            Count++;
        }

        public void AddFinancialChange(int sum, bool isIncome, string comment, string category)
        {
            FinancialChange item = new FinancialChange(sum, isIncome, comment, category);
            _financialChangesList.Add(item);
            FunctionClass.AddToXml(item);
            Count++;
        }

        public void AddFinancialChange(int sum, bool isIncome, string category)
        {
            FinancialChange item = new FinancialChange(sum, isIncome, category);
            _financialChangesList.Add(item);
            FunctionClass.AddToXml(item);
            Count++;
        }

        public void DeleteFinancialChange(int index)
        {
            _financialChangesList.RemoveAt(index);
            FunctionClass.DeleteFromXml(index, new FinancialChange(1, false, ""));
            Count--;
        }

        public IEnumerator GetEnumerator()
        {
            return _financialChangesList.GetEnumerator();
        }
    }

    internal class Target
    {
        public string Name { get; private set; }
        public int FullSum { get; private set; }
        public string Comment { get; private set; }

        public int CurrentSum { get; internal set; } 
        public long DateAdded { get; private set; } 
        public string ClientID { get; private set; } 

        public Target(string name, int fullSum, string comment)
        {
            Name = name;
            FullSum = fullSum;
            Comment = comment;

            DateAdded = DateTime.Now.ToBinary();
            CurrentSum = 0;
            ClientID = App.ActiveClient.ID;
        }

        public Target(string name, int fullSum)
        {
            Name = name;
            FullSum = fullSum;
            Comment = "Комментарий отсутствует";

            DateAdded = DateTime.Now.ToBinary();
            CurrentSum = 0;
            ClientID = App.ActiveClient.ID;
        }

        public Target(string name, int fullSum, string comment, long dateAdded, int currentSum, string clientID)
        {
            Name = name;
            FullSum = fullSum;
            Comment = comment;
            DateAdded = dateAdded;
            CurrentSum = currentSum;
            ClientID = clientID;
        }
    }

    public class TargetList : IEnumerable
    {
        private readonly List<Target> _targetsList;
        public int Count { get; private set; }

        public TargetList()
        {
            Count = 0;
            _targetsList = new List<Target>();
        }

        public void AddTarget(string name, int fullSum, string comment, long dateAdded, int currentSum, string clientID)
        {
            Target item = new Target(name, fullSum, comment, dateAdded, currentSum, clientID);
            _targetsList.Add(item);
            Count++;
        }

        public void AddTarget(string name, int fullSum, string comment)
        {
            Target item = new Target(name, fullSum, comment);
            _targetsList.Add(item);
            FunctionClass.AddToXml(item);
            Count++;
        }

        public void AddTarget(string name, int fullSum)
        {
            Target item = new Target(name, fullSum);
            _targetsList.Add(item);
            FunctionClass.AddToXml(item);
            Count++;
        }

        public void DeleteTarget(int index)
        {
            _targetsList.RemoveAt(index);
            FunctionClass.DeleteFromXml(index, new Target("", 1));
            Count--;
        }

        public IEnumerator GetEnumerator()
        {
            return _targetsList.GetEnumerator();
        }
    }


    public class Client
    {
        public string Generation { get; }
        public string ID { get; }
        public string Name { get; }

        public Client(string name, string id, string generation)
        {
            Generation = generation;
            Name = name;
            ID = id;
        }
    }
}