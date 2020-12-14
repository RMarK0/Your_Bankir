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
        public long date { get; private set; }
        public int sum { get; private set; }
        public string category { get; private set; }
        public string clientID { get; private set; }
        public bool isIncome { get; private set; }
        public string comment { get; private set; }

        public FinancialChange(int sum, bool isIncome, string comment, string category)
        {
            this.comment = comment;
            this.sum = sum;
            this.isIncome = isIncome;
            this.category = category;

            clientID = App.ActiveClient.ID;
            date = DateTime.Now.ToBinary();
        }

        public FinancialChange(int sum, bool isIncome, string category)
        {
            this.sum = sum;
            this.isIncome = isIncome;
            this.category = category;

            comment = "Комментарий отсутствует";
            clientID = App.ActiveClient.ID;
            date = DateTime.Now.ToBinary();
        }

        public FinancialChange(int sum, bool isIncome, string comment, string category, long date,
            string clientID)
        {
            this.comment = comment;
            this.sum = sum;
            this.isIncome = isIncome;
            this.category = category;
            this.clientID = clientID;
            this.date = date;
        }
    }

    public class FinancialChangeList : IEnumerable
    {
        private FinancialChange[] _financialChangesList;
        public int Capacity { get; }
        public int Count { get; private set; }

        public FinancialChangeList(int capacity)
        {
            Capacity = capacity;
            Count = 0;
            _financialChangesList = new FinancialChange[Capacity];
        }

        public void AddFinancialChange(int sum, bool isIncome, string comment, string category, long date,
            string clientID)
        {
            if (Count < Capacity)
            {
                _financialChangesList[Count] = new FinancialChange(sum, isIncome, comment, category, date, clientID);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_financialChangesList is full, can't add new change to array");
            }
        }

        public void AddFinancialChange(int sum, bool isIncome, string comment, string category)
        {
            if (Count < Capacity)
            {
                _financialChangesList[Count] = new FinancialChange(sum, isIncome, comment, category);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_financialChangesList is full, can't add new change to array");
            }
        }

        public void AddFinancialChange(int sum, bool isIncome, string category)
        {
            if (Count < Capacity)
            {
                _financialChangesList[Count] = new FinancialChange(sum, isIncome, category);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_financialChangesList is full, can't add new change to array");
            }
        }

        public void DeleteFinancialChange(int index)
        {
            

            FinancialChange[] temp = new FinancialChange[15];
            int i = 0;
            int i2 = 0;
            while (i < index)
            {
                temp[i2] = _financialChangesList[i];
                i2++;
                i++;
            }
            i++;
            for (; i < _financialChangesList.Length; i++)
            {
                temp[i2] = _financialChangesList[i];
                i2++;
            }
            temp[i2] = null;
            _financialChangesList = temp;
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

        public int CurrentSum { get; private set; } 
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
        private Target[] _targetsList;
        public int Capacity { get; }
        public int Count { get; private set; }

        public TargetList(int capacity)
        {
            Count = 0;
            Capacity = capacity;
            _targetsList = new Target[Capacity];
        }

        public void AddTarget(string name, int fullSum, string comment, long dateAdded, int currentSum, string clientID)
        {
            if (Count < Capacity)
            {
                _targetsList[Count] = new Target(name, fullSum, comment, dateAdded, currentSum, clientID);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_targetsList is full and new target can't be added");
            }
        }

        public void AddTarget(string name, int fullSum, string comment)
        {
            if (Count < Capacity)
            {
                _targetsList[Count] = new Target(name, fullSum, comment);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_targetsList is full and new target can't be added");
            }
        }

        public void AddTarget(string name, int fullSum)
        {
            if (Count < Capacity)
            {
                _targetsList[Count] = new Target(name, fullSum);
                Count++;
            }
            else
            {
                throw new StackOverflowException("_targetsList is full and new target can't be added");
            }
        }

        public void DeleteTarget(int index)
        {
            

            Target[] temp = new Target[Capacity];
            int i = 0;
            int i2 = 0;
            while (i < index)
            {
                temp[i2] = _targetsList[i];
                i2++;
                i++;
            }
            i++;
            for (; i < _targetsList.Length; i++)
            {
                temp[i2] = _targetsList[i];
                i2++;
            }
            temp[i2] = null;
            _targetsList = temp;
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

        public Client(string name, string ID)
        {
            Name = name;
            this.ID = ID;
            if (ID == "1" || ID == "4")
            {
                Generation = "Young";
            }
            else if (ID == "2" || ID == "3")
            {
                Generation = "Adult";
            }
            else if (ID == "5" || ID == "6")
            {
                Generation = "Old";
            }
            else
            {
                throw new NotImplementedException($"No assigned generations for ID {this.ID}");
            }
        }
    }
}