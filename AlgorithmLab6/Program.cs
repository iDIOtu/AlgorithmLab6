using System.Collections;
using System;
using System.Diagnostics;

namespace AlgorithmLab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RunFunctionalTestOpenAddressingTable();
            RunTestOpenAddressingTable();
        }

        static void RunTestOpenAddressingTable()
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
            OpenAddressingHashTable<string, string> t = new OpenAddressingHashTable<string, string>("cuckoo");
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

        static public OpenAddressingHashTable<int, int> FillOATable(int Size) { return FillOATable(Size, "quadratic"); }
        static public OpenAddressingHashTable<int, int> FillOATable(int Size, string probingMethod)
        {
            OpenAddressingHashTable<int, int> hashTable = new OpenAddressingHashTable<int, int>(probingMethod);
            Random random = new Random();
            for (int i = 0; i < Size; i++)
            {
                int value = random.Next(10000);
                hashTable.Add(i, value);
            }
            return hashTable;
        }

        static public OpenAddressingHashTable<string, string> FillOATableWithStrings(int Size) { return FillOATableWithStrings(Size, "quadratic"); }
        static public OpenAddressingHashTable<string, string> FillOATableWithStrings(int Size, string probingMethod)
        {
            OpenAddressingHashTable<string, string> hashTable = new OpenAddressingHashTable<string, string>(probingMethod);
            for (int i = 0; i < Size; i++)
            {
                hashTable.Add(i.ToString(), i.ToString());
            }
            return hashTable;
        }

        static public OpenAddressingHashTable<int, int> TableOverflow(int Size)
        {
            return FillOATable(Size + 10000);
        }
    }
}
