﻿using System.Collections;
using System;
using System.Diagnostics;

namespace AlgorithmLab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Mainview();
        }

        static void Mainview()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("(1) Create Chained Hash Table\n(2) Create Open Adressing Hash Table\n(3) Провести Тесты");
                while (true)
                {
                    string a = Console.ReadLine();

                    switch (a)
                    {
                        case "1":
                            CreateTable(true);
                            break;
                        case "2":
                            CreateTable(false);
                            break;
                        case "3":
                            Test();
                            break;
                        default:
                            Console.WriteLine("Введите корректное значение!");
                            break;

                    }
                }
            }
        }

        private static void CreateTable(bool flag)
        {
            if (flag)
            {
                Console.Clear();
                Console.WriteLine("(1) Division Method\n(2) Multiplication Method\n(3) Xor Method\n(4) Polynomial Method\n(5) BitShift Method\n(6) Default Method\n(7) Назад");
                var table = new HashTableChains<string, string>(HashMethod.Default);
                while (true)
                {
                    string a = Console.ReadLine();

                    switch (a)
                    {
                        case "1":
                            table.hashMethod = HashMethod.Division;
                            Actions(table);
                            break;
                        case "2":
                            table.hashMethod = HashMethod.Multiplication;
                            Actions(table);
                            break;
                        case "3":
                            table.hashMethod = HashMethod.PrimeXor;
                            Actions(table);
                            break;
                        case "4":
                            table.hashMethod = HashMethod.Polynomial;
                            Actions(table);
                            break;
                        case "5":
                            table.hashMethod = HashMethod.BitShift;
                            Actions(table);
                            break;
                        case "6":
                            table.hashMethod = HashMethod.Default;
                            Actions(table);
                            break;
                        case "7":
                            Mainview();
                            break;
                        default:
                            Console.WriteLine("Введите корректное значение!");
                            break;

                    }

                }
            }
            else
            {
                Console.Clear();
                Console.WriteLine("(1) Division Hash\n(2) Multiplication Hash\n(3) Xor Hash\n(4) Назад");
                while (true)
                {
                    string a = Console.ReadLine();

                    switch (a)
                    {
                        case "1":
                            Open("division");
                            break;
                        case "2":
                            Open("multiplication");
                            break;
                        case "3":
                            Open("multiplication2");
                            break;
                        case "4":
                            Mainview();
                            break;
                        default:
                            Console.WriteLine("Введите корректное значение!");
                            break;
                    }

                    
                }
            }
        }

        private static void Open(string hash)
        {
            Console.Clear();
            Console.WriteLine("(1) Linear probing\n(2) Quadratic Probing\n(3) Double Hash\n(4) Random Probing\n(5) Cuckoo\n(6) Fibonacci\n(7) Назад");
            var table = new HashTableOpen<string, string>();
            while (true)
            {
                string a = Console.ReadLine();

                switch (a)
                {
                    case "1":
                        table = new HashTableOpen<string, string>("linear", hash);
                        Actions(table);
                        break;
                    case "2":
                        table = new HashTableOpen<string, string>("quadratic", hash);
                        Actions(table);
                        break;
                    case "3":
                        table = new HashTableOpen<string, string>("double", hash);
                        Actions(table);
                        break;
                    case "4":
                        table = new HashTableOpen<string, string>("random", hash);
                        Actions(table);
                        break;
                    case "5":
                        table = new HashTableOpen<string, string>("cuckoo", hash);
                        Actions(table);
                        break;
                    case "6":
                        table = new HashTableOpen<string, string>("Fibonacci", hash);
                        Actions(table);
                        break;
                    case "7":
                        Mainview();
                        break;
                    default:
                        Console.WriteLine("Введите корректное значение!");
                        break;
                }
            }
        }

        private static void Actions(HashTable<string, string> table)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("(1) Find\n(2) Add\n(3) Remove\n(4) Назад");
                while (true)
                {
                    string a = Console.ReadLine();
                    string key;
                    string value;
                    switch (a)
                    {
                        
                        case "1":
                            Console.Write("Введите ключ: ");
                            key = Console.ReadLine();
                            if (table.Find(key, out value))
                                Console.Write($"Ваше значение: {value}\n\n");
                            else Console.WriteLine("Значение не найдено\n");
                            break;
                        case "2":
                            Console.Write("Введите ключ: ");
                            key = Console.ReadLine();
                            Console.Write("Введите значение: ");
                            value = Console.ReadLine();
                            table.Add(key, value);
                            Console.WriteLine("пара ключ значение добавлено\n");
                            break;
                        case "3":
                            Console.Write("Введите ключ: ");
                            key = Console.ReadLine();
                            table.Remove(key);
                            Console.WriteLine($"ключ: {key} удалён\n");
                            break;
                        case "4":
                            Mainview();
                            break;
                        default:
                            Console.WriteLine("Введите корректное значение!");
                            break;
                    }
                }
            }
        }

        private static void Test()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("(1) Chained Hash Table\n(2) Open Adressing Hash Table\n(3) Назад");
                while (true)
                {
                    string a = Console.ReadLine();

                    switch (a)
                    {
                        case "1":
                            Analyzer.ChainedAnalyze();
                            break;
                        case "2":
                            Analyzer.OpenAnalyze();
                            break;
                        case "3":
                            Mainview();
                            break;
                        default:
                            Console.WriteLine("Введите корректное значение!");
                            break;

                    }
                }
            }
        }

        /*static void RunTestOpenAddressingTable()
        {
            int size = 10000;
            Console.WriteLine("Открытая адресация. Тест 1: заполнение на 1/2 ");
            Stopwatch t1 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> linear = FillOATableWithStrings(size/2, "linear");
            t1.Stop(); Stopwatch t2 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> quadratic = FillOATableWithStrings(size / 2, "quadratic");
            t2.Stop(); Stopwatch t3 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> Double = FillOATableWithStrings(size / 2, "double");
            t3.Stop(); Stopwatch t4 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> cuckoo = FillOATableWithStrings(size / 2, "cuckoo");
            t4.Stop(); Stopwatch t5 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> Fibonacci = FillOATableWithStrings(size / 2, "Fibonacci");
            t5.Stop(); Stopwatch t6 = Stopwatch.StartNew();
            OpenAddressingHashTable<string, string> random = FillOATableWithStrings(size / 2, "random");
            Console.WriteLine("Открытая адресация. Тест 1: заполнение на 1/2 "); 
            
            HashTableOpen<string, string> linear = FillOATableWithStrings(size/2, "linear");
            HashTableOpen<string, string> quadratic = FillOATableWithStrings(size / 2, "quadratic");
            HashTableOpen<string, string> Double = FillOATableWithStrings(size / 2, "double");
            HashTableOpen<string, string> cuckoo = FillOATableWithStrings(size / 2, "cuckoo");
            HashTableOpen<string, string> random = FillOATableWithStrings(size / 2, "random");
            t5.Stop();

            Console.WriteLine("Исследование " + " Размер " + " Количество записей " + " Максимальный кластер" + " Максимальное число итераций для вставки " + " t ");
            Console.WriteLine("Линейное:    " + " " + linear.GetSize() + "         " + linear.GetCount() + "                       " + linear.MaxClusterLength() + "                   " + linear.GetMaxI() + "                    " + t1.Elapsed.TotalMilliseconds);
            Console.WriteLine("Квадратичное:" + " " + quadratic.GetSize() + "         " + quadratic.GetCount() + "                       " + quadratic.MaxClusterLength() + "                   " + quadratic.GetMaxI() + "                    " + t2.Elapsed.TotalMilliseconds);
            Console.WriteLine("Двойное:     " + " " + Double.GetSize() + "         " + Double.GetCount() + "                       " + Double.MaxClusterLength() + "                   " + Double.GetMaxI() + "                    " + t3.Elapsed.TotalMilliseconds);
            Console.WriteLine("Кукушкой:    " + " " + cuckoo.GetSize() + "         " + cuckoo.GetCount() + "                       " + cuckoo.MaxClusterLength() + "                   " + cuckoo.GetMaxI() + "                    " + t4.Elapsed.TotalMilliseconds);
            Console.WriteLine("Фибоначчи:   " + " " + Fibonacci.GetSize() + "         " + Fibonacci.GetCount() + "                       " + Fibonacci.MaxClusterLength() + "                   " + Fibonacci.GetMaxI() + "                    " + t5.Elapsed.TotalMilliseconds);
            Console.WriteLine("Случайное:   " + " " + random.GetSize() + "         " + random.GetCount() + "                       " + random.MaxClusterLength() + "                   " + random.GetMaxI() + "                    " + t6.Elapsed.TotalMilliseconds);

            Console.WriteLine("Открытая адресация. Тест 2: заполнение полностью ");
            t1.Restart();
            linear = FillOATableWithStrings(size, "linear"); t1.Stop(); t2.Restart(); 
            quadratic = FillOATableWithStrings(size, "quadratic"); t2.Stop();  t3.Restart();
            Double = FillOATableWithStrings(size, "double");t3.Stop(); t4.Restart();
            cuckoo = FillOATableWithStrings(size, "cuckoo"); t4.Stop(); t5.Restart();
            Fibonacci = FillOATableWithStrings(size, "Fibonacci"); t5.Stop(); t6.Restart();
            random = FillOATableWithStrings(size, "random"); 
            t6.Stop();

            Console.WriteLine("Исследование " + " Размер " + " Количество записей " + " Максимальный кластер" + " Максимальное число итераций для вставки " + " t ");
            Console.WriteLine("Линейное:    " + " " + linear.GetSize() + "         " + linear.GetCount() + "                       " + linear.MaxClusterLength() + "                   " + linear.GetMaxI() + "                    " + t1.Elapsed.TotalMilliseconds);
            Console.WriteLine("Квадратичное:" + " " + quadratic.GetSize() + "         " + quadratic.GetCount() + "                       " + quadratic.MaxClusterLength() + "                   " + quadratic.GetMaxI() + "                    " + t2.Elapsed.TotalMilliseconds);
            Console.WriteLine("Двойное:     " + " " + Double.GetSize() + "         " + Double.GetCount() + "                       " + Double.MaxClusterLength() + "                   " + Double.GetMaxI() + "                    " + t3.Elapsed.TotalMilliseconds);
            Console.WriteLine("Кукушкой:    " + " " + cuckoo.GetSize() + "         " + cuckoo.GetCount() + "                       " + cuckoo.MaxClusterLength() + "                   " + cuckoo.GetMaxI() + "                    " + t4.Elapsed.TotalMilliseconds);
            Console.WriteLine("Фибоначчи:   " + " " + Fibonacci.GetSize() + "         " + Fibonacci.GetCount() + "                       " + Fibonacci.MaxClusterLength() + "                   " + Fibonacci.GetMaxI() + "                    " + t5.Elapsed.TotalMilliseconds);
            Console.WriteLine("Случайное:   " + " " + random.GetSize() + "         " + random.GetCount() + "                       " + random.MaxClusterLength() + "                   " + random.GetMaxI() + "                    " + t6.Elapsed.TotalMilliseconds);
        }

        static void RunFunctionalTestOpenAddressingTable()
        {
            HashTableOpen<string, string> t = new HashTableOpen<string, string>("cuckoo");
            t.Add("1", "Добавлено с ключом 1");
            t.Add("2", "Добавлено с ключом 2");
            t.Add("3", "Добавлено с ключом 3");

            Console.WriteLine(t.GetCount());
            Console.WriteLine(t.Find("2", out string v));
            Console.WriteLine(v);
            t.Remove("2");
            Console.WriteLine(t.GetCount());
            Console.WriteLine(t.Find("2", out v));
            Console.WriteLine(v);

            for (int i = 0; i < 7000; i++)
            {
                t.Add(i.ToString(), i.ToString());
            }

            Console.WriteLine(t.GetCount());
            Console.WriteLine(t.Find("4951", out v));
            Console.WriteLine(v);
        }

        static public HashTableOpen<int, int> FillOATable(int Size) { return FillOATable(Size, "quadratic"); }
        static public HashTableOpen<int, int> FillOATable(int Size, string probingMethod)
        {
            HashTableOpen<int, int> hashTable = new HashTableOpen<int, int>(probingMethod);
            Random random = new Random();
            for (int i = 0; i < Size; i++)
            {
                int value = random.Next(10000);
                hashTable.Add(i, value);
            }
            return hashTable;
        }

        static public HashTableOpen<string, string> FillOATableWithStrings(int Size) { return FillOATableWithStrings(Size, "quadratic"); }
        static public HashTableOpen<string, string> FillOATableWithStrings(int Size, string probingMethod)
        {
            HashTableOpen<string, string> hashTable = new HashTableOpen<string, string>(probingMethod);
            for (int i = 0; i < Size; i++)
            {
                hashTable.Add(i.ToString(), i.ToString());
            }
            return hashTable;
        }

        static public HashTableOpen<int, int> TableOverflow(int Size)
        {
            return FillOATable(Size + 10000);
        }*/
    }
}
