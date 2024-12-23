using System.Collections;
using System;

namespace AlgorithmLab6
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        static void RunTestOpenAddressingTable()
        {
            OpenAddressingHashTable<string, string> hashTable = new OpenAddressingHashTable<string, string>();
            // Вставка элементов
            hashTable.Add("2", "1");
            hashTable.Add("2", "2");
            hashTable.Add("3", "3");

            // Поиск элементов
            Console.WriteLine(hashTable.Find("2", out string foundedValue)); // True
            Console.WriteLine(foundedValue); // 1
            Console.WriteLine(hashTable.Find("4", out foundedValue)); // False
            Console.WriteLine(foundedValue); // 404 -not found c:

            // Удаление элемента
            hashTable.Remove("2");
            Console.WriteLine(hashTable.Find("2", out foundedValue)); // False
            Console.WriteLine(foundedValue); // not found

            OpenAddressingHashTable<int, int> filledOATable = FillOATable(10000);
            Console.WriteLine(filledOATable.Find(0, out int FirstValue)); // True
            Console.WriteLine(filledOATable.Find(9999, out int LastValue)); // True       
            Console.WriteLine("Key 0: " + FirstValue + " | Key 9999: " + LastValue + " | Count: " + filledOATable.GetCount());

            Console.WriteLine("Overflow: ");
            filledOATable = TableOverflow(filledOATable.GetSize()); // 20000 elements
            Console.WriteLine(filledOATable.Find(0, out FirstValue)); // True
            Console.WriteLine(filledOATable.Find(9999, out LastValue)); // True       
            Console.WriteLine(filledOATable.Find(19999, out int expandedLastValue)); // True  
            Console.WriteLine("Key 0: " + FirstValue + " | Key 9999: " + LastValue + " | Key 19999: " + LastValue + " | Count: " + filledOATable.GetCount());
            OpenAddressingHashTable<string, string> stringOATable = FillOATableWithStrings(10000);
            Console.WriteLine(stringOATable.GetSize());
            Console.WriteLine(stringOATable.GetCount());
            Console.WriteLine(stringOATable.MaxClusterLength());
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
