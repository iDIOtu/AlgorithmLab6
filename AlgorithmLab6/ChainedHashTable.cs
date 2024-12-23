using System;
using System.Collections.Generic;

namespace AlgorithmLab6
{
    public class ChainedHashTable<T1, T2> : IHashTable<T1, T2>
    {
        private const int TableSize = 1000;
        private LinkedList<KeyValuePair<T1, T2>>[] table;

        public ChainedHashTable()
        {
            table = new LinkedList<KeyValuePair<T1, T2>>[TableSize];
            for (int i = 0; i < TableSize; i++)
            {
                table[i] = new LinkedList<KeyValuePair<T1, T2>>();
            }
        }

        private int GetHash(T1 key)
        {
            return Math.Abs(key.GetHashCode()) % TableSize; // метод деления
        }
        private int GetHashByMultiplication(T1 key)
        {
            double A = 0.6180339887; // золотое сечение
            int hash = Math.Abs(key.GetHashCode());
            double fractionalPart = (hash * A) % 1;
            return (int)(fractionalPart * TableSize);
        }
        private int GetHashPrimeMultiplication(T1 key)
        {
            const int prime = 16777619; // простое число
            int hash = 0;

            if (key is string strKey)
            {
                foreach (char c in strKey)
                {
                    hash = (hash * prime) ^ c; // примитивный XOR с умножением
                }
            }
            else
            {
                hash = key.GetHashCode();
            }

            return Math.Abs(hash) % TableSize;
        }

        public void Add(T1 key, T2 value)
        {
            int index = GetHash(key);
            var chain = table[index];
            foreach (var pair in chain)
            {
                if (EqualityComparer<T1>.Default.Equals(pair.Key, key))
                {
                    throw new ArgumentException("Ключ не найден.");
                }
            }
            chain.AddLast(new KeyValuePair<T1, T2>(key, value));
        }

        public bool Find(T1 key, out T2 value)
        {
            int index = GetHash(key);
            var chain = table[index];
            foreach (var pair in chain)
            {
                if (EqualityComparer<T1>.Default.Equals(pair.Key, key))
                {
                    value = pair.Value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        public void Remove(T1 key)
        {
            int index = GetHash(key);
            var chain = table[index];
            var node = chain.First;
            while (node != null)
            {
                if (EqualityComparer<T1>.Default.Equals(node.Value.Key, key))
                {
                    chain.Remove(node);
                    return;
                }
                node = node.Next;
            }
            throw new KeyNotFoundException("Ключ не найден.");
        }

        public void Analyze() // подсчет коэффициента заполнения, длину самой длинной и самой короткой цепочки в ячейках хеш-таблицы
        {
            int totalElements = 0;
            int maxChainLength = 0;
            int minChainLength = int.MaxValue;

            foreach (var chain in table)
            {
                int chainLength = chain.Count;
                totalElements += chainLength;
                if (chainLength > maxChainLength)
                    maxChainLength = chainLength;
                if (chainLength < minChainLength && chainLength > 0)
                    minChainLength = chainLength;
            }

            double loadFactor = (double)totalElements / TableSize;

            Console.WriteLine($"Коэффициент заполнения: {loadFactor:F2}");
            Console.WriteLine($"Максимальная длина цепочки: {maxChainLength}");
            Console.WriteLine($"Минимальная длина цепочки: {minChainLength}");
        }
        public void GenerateAndInsertElements(int numberOfElements, int keyRange) // генераци 100000 элементов
        {
            if (typeof(T1) != typeof(int))
                throw new InvalidOperationException("This method only supports T1 as int.");

            HashSet<int> uniqueKeys = new HashSet<int>();
            Random random = new Random();
            while (uniqueKeys.Count < numberOfElements)
            {
                uniqueKeys.Add(random.Next(1, keyRange));
            }

            foreach (int key in uniqueKeys)
            {
                Add((T1)(object)key, (T2)Convert.ChangeType($"Value{key}", typeof(T2))); // вставка элементов в таблицу
            }
        }
    }

}
