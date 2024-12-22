using System.Collections;
using System;

namespace AlgorithmLab6
{
    internal class Program
    {
        static void Main(string[] args)
        {
            OpenAddressingHashTable<string, string> hashTable = new OpenAddressingHashTable<string, string>();
            // Вставка элементов
            hashTable.Insert("2", "1");
            hashTable.Insert("2", "2");
            hashTable.Insert("3", "3");

            // Поиск элементов
            Console.WriteLine(hashTable.Find("2", out string foundedValue)); // True
            Console.WriteLine(hashTable.Find("4", out foundedValue)); // False

            // Удаление элемента
            hashTable.Remove("2");
            Console.WriteLine(hashTable.Find("2", out foundedValue)); // False
        }
    }
}
