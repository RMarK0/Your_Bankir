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
        private DateTime date;
        private int sum;
        private Client client;
        private bool isIncome;
        private string comment;

        public FinancialChange(int sum, bool isIncome, string comment)
        {
            this.comment = comment;
            this.sum = sum;
            this.isIncome = isIncome;

            client = LoginPage.ActiveClient;
            date = DateTime.Now;
        }

        public FinancialChange(int sum, bool isIncome)
        {
            this.sum = sum;
            this.isIncome = isIncome;

            comment = "Комментарий отсутствует";
            client = LoginPage.ActiveClient;
            date = DateTime.Now;
        }
    }

    public class Target
    {
        public string Name; // вручную
        public int FullSum; // вручную
        public string Comment; // вручную

        public int CurrentSum; // автоматически
        public DateTime DateAdded; // автоматически
        public Client ClientAdded; // автоматически

        public Target(string name, int fullSum, string comment)
        {
            Name = name;
            FullSum = fullSum;
            Comment = comment;

            DateAdded = DateTime.Now;
            CurrentSum = 0;
            ClientAdded = LoginPage.ActiveClient;
        }

        public Target(string name, int fullSum)
        {
            Name = name;
            FullSum = fullSum;
            Comment = "Комментарий отсутствует";

            DateAdded = DateTime.Now;
            CurrentSum = 0;
            ClientAdded = LoginPage.ActiveClient;
        }
    }

    public class TargetList : IEnumerable
    {
        Target[] TargetsList = new Target[15];

        public bool IsFull;
        public void AddTarget(string name, int fullSum, string comment)
        {
            IsFull = true;
            for (int i = 0; i < TargetsList.Length; i++)
            {
                if (TargetsList[i] == null)
                {
                    TargetsList[i] = new Target(name, fullSum, comment);
                    IsFull = false;
                }
            }
        }

        public void AddTarget(string name, int fullSum)
        {
            IsFull = true;
            for (int i = 0; i < TargetsList.Length; i++)
            {
                if (TargetsList[i] == null)
                {
                    TargetsList[i] = new Target(name, fullSum);
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
                temp[i2] = TargetsList[i];
                i2++;
                i++;
            }
            i++;
            for (; i < TargetsList.Length; i++)
            {
                temp[i2] = TargetsList[i];
                i2++;
            }
            temp[i2] = null;
            TargetsList = temp;
        }

        public IEnumerator GetEnumerator()
        {
            return TargetsList.GetEnumerator();
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