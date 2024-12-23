using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmLab6
{
    internal class OpenAddressingHashTable<T1, T2> : IHashTable<T1, T2> where T1 : IComparable
    {
        private int Size = 10000;
        private (T1 key, T2 value)?[] table;
        private int count;

        public OpenAddressingHashTable()
        {
            table = new (T1, T2)?[Size];
            count = 0;
        }

        public OpenAddressingHashTable(int NewSize)
        {
            Size = NewSize;
            table = new (T1, T2)?[Size];
            count = 0;
        }

        public int GetCount() {  return count; }
        public int GetSize() { return Size; }


        // Метод хеширования
        private int Hash(int key)
        {
            return key % Size; 
        }

        private int Hash(T1 key)
        {
            return Math.Abs(key.GetHashCode() % Size); // 
        }

        // Линейное исследование
        private int LinearProbing(int key, int i)
        {
            return (Hash(key) + i) % Size;
        }

        // Квадратичное исследование
        private int QuadraticProbing(int key, int i)
        {
            return (Hash(key) + i * i) % Size;
        }

        // Двойное хеширование
        private int DoubleHash(int key, int i)
        {
            int hash2 = 7 - (key % 7); // Вторичный хеш
            return (Hash(key) + i * hash2) % Size;
        }

        // Вставка элемента
        public void Add(T1 key, T2 value, string probingMethod)
        {
            if (count >= Size)
            {
                Size *= 2;
                Array.Resize(ref table, Size); ;
            }

            int i = 0;
            int index;
            int hash = Hash(key);
            do
            {
                switch (probingMethod)
                {
                    case "linear":
                        index = LinearProbing(hash, i);
                        break;
                    case "quadratic":
                        index = QuadraticProbing(hash, i);
                        break;
                    case "double":
                        index = DoubleHash(hash, i);
                        break;
                    default:
                        throw new ArgumentException("Invalid probing method");
                }

                if (!table[index].HasValue) // 
                {
                    table[index] = (key, value);
                    count++;
                    return;
                }

                i++;
            } while (true);
        }
        public T2 Add(T1 key)
        {
            throw new NotImplementedException();
        }

        public void Find(T1 key, T2 value)
        {
            throw new NotImplementedException();
        }

        public bool Find(T1 key, out T2 value) // public bool Search(T1 key, out T2 value)
        {
            int i = 0;
            int hash = Hash(key);
            int index;
            do
            {
                index = LinearProbing(hash, i);

                if (!table[index].HasValue)
                {
                    value = default;
                    return false; // Элемент не найден
                }

                if (table[index].Value.key.Equals(key))
                {
                    value = table[index].Value.value; // Элемент найден
                    return true;
                }

                i++;
            } while (true);
        }

        public void Remove(T1 key)
        {
            int i = 0;
            int hash = Hash(key);
            int index;
            do
            {
                index = LinearProbing(hash, i);

                if (!table[index].HasValue)
                {
                    throw new InvalidOperationException("Element not found");
                }

                if (table[index].Value.key.Equals(key))
                {
                    table[index] = null; // Удаляем элемент
                    count--;
                    return;
                }

                i++;
            } while (true);
        }

        public int MaxClusterLength() {  return GetClusterLengths().Max(); }
        public List<int> GetClusterLengths()
        {
            var clusters = new List<int>();
            int currentClusterLength = 0;

            foreach (var item in table)
            {
                if (item != null)
                {
                    currentClusterLength++;
                }
                else
                {
                    if (currentClusterLength > 0)
                    {
                        clusters.Add(currentClusterLength);
                        currentClusterLength = 0;
                    }
                }
            }

            if (currentClusterLength > 0)
            {
                clusters.Add(currentClusterLength);
            }

            return clusters;
        }
    }
}
