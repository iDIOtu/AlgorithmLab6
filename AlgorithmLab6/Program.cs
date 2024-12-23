using System.Collections;
using System;

namespace AlgorithmLab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //RunFunctionalTestOpenAddressingTable();
            //RunTestOpenAddressingTable();
        }

        static void RunTestOpenAddressingTable()
        {
            int size = 10000;
            Console.WriteLine("Открытая адресация. Тест 1: заполнение на 1/2 "); 
            
            OpenAddressingHashTable<string, string> linear = FillOATableWithStrings(size/2, "linear");
            OpenAddressingHashTable<string, string> quadratic = FillOATableWithStrings(size / 2, "quadratic");
            OpenAddressingHashTable<string, string> Double = FillOATableWithStrings(size / 2, "double");
            OpenAddressingHashTable<string, string> cuckoo = FillOATableWithStrings(size / 2, "cuckoo");
            OpenAddressingHashTable<string, string> random = FillOATableWithStrings(size / 2, "random");

            Console.WriteLine("Исследование " + " Размер " + " Количество записей " + " Максимальный кластер");
            Console.WriteLine("Линейное:    " + " " + linear.GetSize() + "         " + linear.GetCount() + "                   " + linear.MaxClusterLength());
            Console.WriteLine("Квадратичное:" + " " + quadratic.GetSize() + "         " + quadratic.GetCount() + "                   " + quadratic.MaxClusterLength());
            Console.WriteLine("Двойное:     " + " " + Double.GetSize() + "         " + Double.GetCount() + "                   " + Double.MaxClusterLength());
            Console.WriteLine("Кукушкой:    " + " " + cuckoo.GetSize() + "         " + cuckoo.GetCount() + "                   " + cuckoo.MaxClusterLength());
            Console.WriteLine("Случайное:   " + " " + random.GetSize() + "         " + random.GetCount() + "                   " + random.MaxClusterLength());


            Console.WriteLine("Открытая адресация. Тест 2: заполнение полностью ");

            linear = FillOATableWithStrings(size, "linear");
            quadratic = FillOATableWithStrings(size, "quadratic");
            Double = FillOATableWithStrings(size, "double");
            cuckoo = FillOATableWithStrings(size, "cuckoo");
            random = FillOATableWithStrings(size, "random");

            Console.WriteLine("Исследование " + " Размер " + " Количество записей " + " Максимальный кластер");
            Console.WriteLine("Линейное:    " + " " + linear.GetSize() + "         " + linear.GetCount() + "                   " + linear.MaxClusterLength());
            Console.WriteLine("Квадратичное:" + " " + quadratic.GetSize() + "         " + quadratic.GetCount() + "                   " + quadratic.MaxClusterLength());
            Console.WriteLine("Двойное:     " + " " + Double.GetSize() + "         " + Double.GetCount() + "                   " + Double.MaxClusterLength());
            Console.WriteLine("Кукушкой:    " + " " + cuckoo.GetSize() + "         " + cuckoo.GetCount() + "                   " + cuckoo.MaxClusterLength());
            Console.WriteLine("Случайное:   " + " " + random.GetSize() + "         " + random.GetCount() + "                   " + random.MaxClusterLength());
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
