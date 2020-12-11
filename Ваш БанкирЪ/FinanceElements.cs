using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml;

namespace Ваш_БанкирЪ
{
    internal class FinancialChange
    {
        private long date;
        private int sum;
        private string category;
        private string clientID;
        private bool isIncome;
        private string comment;

        public FinancialChange(int sum, bool isIncome, string comment, string category)
        {
            this.comment = comment;
            this.sum = sum;
            this.isIncome = isIncome;
            this.category = category;

            clientID = LoginPage.ActiveClient.ID;
            date = DateTime.Now.ToBinary();
        }

        public FinancialChange(int sum, bool isIncome, string category)
        {
            this.sum = sum;
            this.isIncome = isIncome;
            this.category = category;

            comment = "Комментарий отсутствует";
            clientID = LoginPage.ActiveClient.ID;
            date = DateTime.Now.ToBinary();
        }
    }

    public class FinancialChangeList : IEnumerable
    {
        FinancialChange[] _financialChangesList = new FinancialChange[100];

        private bool _isFull;
        public void AddFinancialChange(int sum, bool isIncome, string comment, string category)
        {
            FinancialChange temp = new FinancialChange(sum, isIncome, comment, category);
            _isFull = true;
            for (int i = 0; i < _financialChangesList.Length; i++)
            {
                if (_financialChangesList[i] == null)
                {
                    _financialChangesList[i] = temp;
                    _isFull = false;
                }
            }
        }

        public void AddFinancialChange(int sum, bool isIncome, string category)
        {
            FinancialChange temp = new FinancialChange(sum, isIncome, category);
            _isFull = true;
            for (int i = 0; i < _financialChangesList.Length; i++)
            {
                if (_financialChangesList[i] == null)
                {
                    _financialChangesList[i] = temp;
                    _isFull = false;
                }
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
        }


        public IEnumerator GetEnumerator()
        {
            return _financialChangesList.GetEnumerator();
        }
    }

    public class Target
    {
        private string Name; // вручную
        private int FullSum; // вручную
        private string Comment; // вручную

        private int CurrentSum; // автоматически
        private long DateAdded; // автоматически
        private string ClientID; // автоматически

        public Target(string name, int fullSum, string comment)
        {
            Name = name;
            FullSum = fullSum;
            Comment = comment;

            DateAdded = DateTime.Now.ToBinary();
            CurrentSum = 0;
            ClientID = LoginPage.ActiveClient.ID;
        }

        public Target(string name, int fullSum)
        {
            Name = name;
            FullSum = fullSum;
            Comment = "Комментарий отсутствует";

            DateAdded = DateTime.Now.ToBinary();
            CurrentSum = 0;
            ClientID = LoginPage.ActiveClient.ID;
        }
    }

    public class TargetList : IEnumerable
    {
        Target[] _targetsList = new Target[15];

        public bool IsFull;
        public void AddTarget(string name, int fullSum, string comment)
        {
            IsFull = true;
            for (int i = 0; i < _targetsList.Length; i++)
            {
                if (_targetsList[i] == null)
                {
                    _targetsList[i] = new Target(name, fullSum, comment);
                    IsFull = false;
                }
            }
        }

        public void AddTarget(string name, int fullSum)
        {
            IsFull = true;
            for (int i = 0; i < _targetsList.Length; i++)
            {
                if (_targetsList[i] == null)
                {
                    _targetsList[i] = new Target(name, fullSum);
                    IsFull = false;
                }
            }
        }

        public void DeleteTarget(int index)
        {
            Target[] temp = new Target[15];
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

        public Client(string name, string ID) // ОБЯЗАТЕЛЬНО СДЕЛАТЬ ПАРСЕР ИЗ XML
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