using System;

namespace AlgorithmLab6
{
    internal class OpenAddressingHashTable<T1, T2> : IHashTable<T1, T2> where T1 : IComparable
    {
        private const int Size = 10000;
        private (T1 key, T2 value)?[] table;
        private int count;

        public OpenAddressingHashTable()
        {
            table = new (T1, T2)?[Size];
            count = 0;
        }

        // Метод хеширования
        private int Hash(int key)
        {
            return key % Size; 
        }

        private int GenericHash(T1 key)
        {
            return Math.Abs(key.GetHashCode()) % Size;
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
        public void Insert(T1 key, T2 value, string probingMethod = "linear")
        {
            if (count >= Size)
            {
                throw new InvalidOperationException("Hash table is full");
            }

            int i = 0;
            int index;
            int hash = GenericHash(key);
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

        // Поиск элемента
        //public bool Search(int key)
        //{
        //    int i = 0;
        //    int index;
        //    do
        //    {
        //        index = LinearProbing(key, i); // Можно выбрать любой метод

        //        if (table[index] == null)
        //        {
        //            return false; // Элемент не найден
        //        }

        //        if (table[index] == key)
        //        {
        //            return true; // Элемент найден
        //        }

        //        i++;
        //    } while (true);
        //}

        // Удаление элемента
        //public void Delete(int key)
        //{
        //    int i = 0;
        //    int index;
        //    do
        //    {
        //        index = LinearProbing(key, i); // Можно выбрать любой метод

        //        if (table[index] == null)
        //        {
        //            throw new InvalidOperationException("Element not found");
        //        }

        //        if (table[index] == key)
        //        {
        //            table[index] = -1; // Помечаем элемент как удалённый
        //            count--;
        //            return;
        //        }

        //        i++;
        //    } while (true);
        //}
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
            int hash = GenericHash(key);
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
            int hash = GenericHash(key);
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
    }
}
