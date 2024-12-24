using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmLab6
{
    public class HashTableChains<T1, T2> : HashTable<T1, T2>
    {
        public const int TableSize = 1000;
        public LinkedList<KeyValuePair<T1, T2>>[] table;
        public HashMethod hashMethod;

        public HashTableChains(HashMethod method)
        {
            hashMethod = method;
            table = new LinkedList<KeyValuePair<T1, T2>>[TableSize];
            for (int i = 0; i < TableSize; i++)
            {
                table[i] = new LinkedList<KeyValuePair<T1, T2>>();
            }
        }

        public override void Add(T1 key, T2 value)
        {
            int index = GetHash(key);
            var chain = table[index];
            foreach (var pair in chain)
            {
                if (EqualityComparer<T1>.Default.Equals(pair.Key, key))
                {
                    Console.WriteLine("Уже существует такой ключ, сообщение снизу неправда.");
                    return;
                }
            }
            chain.AddLast(new KeyValuePair<T1, T2>(key, value));
        }

        public override bool Find(T1 key, out T2 value)
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

        public override void Remove(T1 key)
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
            Console.WriteLine("ключ не найден, сообщение снизу неправда");
            return;
        }

        private int GetHash(T1 key)
        {
            switch (hashMethod)
            {
                case HashMethod.Division:
                    return HashMethodDivision(key);
                case HashMethod.Multiplication:
                    return HashMethodMultiplication(key);
                case HashMethod.PrimeXor:
                    return HashMethodPrimeXor(key);
                case HashMethod.Polynomial:
                    return PolynomialHashMethod(key);
                case HashMethod.BitShift:
                    return BitShiftHashMethod(key);
                case HashMethod.Default:
                default:
                    return Math.Abs(key.GetHashCode() % TableSize);
            }
        }

        private int HashMethodDivision(T1 key)
        {
            string input = key.ToString();
            int hash = 0;

            foreach (char c in input)
                hash = hash * 128 + c;

            return Math.Abs(hash % TableSize);
        }

        private int HashMethodMultiplication(T1 key)
        {
            const double A = 0.6180339887; // золотое сечение
            string input = key.ToString();
            int hash = 0;

            foreach (char c in input)
                hash = hash * 128 + c;

            double fractionalPart = (hash * A) % 1;
            int result = (int)(fractionalPart * TableSize);

            return Math.Abs(result == 0 ? 1 : result);
        }

        private int HashMethodPrimeXor(T1 key)
        {
            const int prime = 31; // простое число
            string input = key.ToString();
            int hash = 0;

            foreach (char c in input)
                hash = (hash * prime) ^ c;

            return Math.Abs(hash % TableSize);
        }

        private int PolynomialHashMethod(T1 key)
        {
            const int prime = 16777619; // Большое простое число
            int hash = 0;
            string input = key.ToString();

            for (int i = 0; i < input.Length; i++)
                hash += (int)(input[i] * Math.Pow(prime, i)); // Используем полиномиальную формулу

            return Math.Abs(hash % TableSize);
        }

        private int BitShiftHashMethod(T1 key)
        {
            int hash = 0;
            string input = key.ToString();

            foreach (char c in input)
            {
                hash = (hash << 5) - hash; // Битовый сдвиг влево на 5 и вычитание
                hash += c; // Добавляем значение символа
            }

            return Math.Abs(hash % TableSize);
        }

        public List<int> GetChainLengths()
        {
            return table.Select(x => x?.Count ?? 0).ToList();
        }
    }

}
