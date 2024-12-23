using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Policy;

namespace AlgorithmLab6
{
    internal class OpenAddressingHashTable<T1, T2> : IHashTable<T1, T2> where T1 : IComparable
    {
        private int Size = 10000;
        private (T1 key, T2 value)?[] table;
        private int count;
        private string probingMethod = "quadratic";
        private string hashFunction = "division";

        public OpenAddressingHashTable()
        {
            table = new (T1, T2)?[Size];
            count = 0;
        }
        public OpenAddressingHashTable(string probingMethod)
        {
            table = new (T1, T2)?[Size];
            count = 0;
            if (IsProbingMethod(probingMethod)) this.probingMethod = probingMethod;
            else Console.WriteLine("Метод не найден");
        }

        public OpenAddressingHashTable(string probingMethod, string hashFunction)
        {
            table = new (T1, T2)?[Size];
            count = 0;
            if (IsProbingMethod(probingMethod)) this.probingMethod = probingMethod;
            else Console.WriteLine("Метод не найден");
            if (IsHashFunction(hashFunction)) this.hashFunction = hashFunction;
            else Console.WriteLine("Хэшфункция не найдена");
        }

        public OpenAddressingHashTable(int NewSize)
        {
            Size = NewSize;
            table = new (T1, T2)?[Size];
            count = 0;
        }

        public bool SetProbingMethod(string probingMethod) 
        {
            if (IsProbingMethod(probingMethod))
            {
                this.probingMethod = probingMethod;
                return true;
            }
            else return false;
        }
        public string GetProbingMethod() { return probingMethod; }

        public bool SetHashFunction(string hashFunction)
        {
            if (IsHashFunction(hashFunction))
            {
                this.hashFunction = hashFunction;
                return true;
            }
            else return false;
        }
        public string GetHashFunction() { return hashFunction; }
        public int GetCount() {  return count; }
        public int GetSize() { return Size; }
        public void SetSize(int Size) 
        { 
            Array.Resize(ref table, Size); 
            this.Size = Size;
        }


        // Метод хеширования
        private int Hash(int key)
        {
            return key % Size; 
        }

        private int Hash(T1 key, int i)
        {
            //return Math.Abs(key.GetHashCode() % Size);  

            switch (hashFunction)
            {
                case "multiplication":
                    return HashMultiplication(key, i);
                case "division":
                    return HashDivision(key, i);
                default:
                    throw new ArgumentException("Invalid probing method");
            }
        }
        private int HashMultiplication(int key, int i)
        {
            double fractionalPart = (key * 0.6180339887) % 1;// Умножаем на константу и извлекаем дробную часть
            return (int)(fractionalPart * Size + i);// Умножаем дробную часть на размер таблицы и округляем
        }
        private int HashMultiplication(T1 key, int i)
        {
            int keyHash = key.GetHashCode();// Преобразуем строку в числовое значение
            double fractionalPart = (keyHash * 0.6180339887) % 1;// Умножаем на константу и извлекаем дробную часть
            return (int)Math.Abs(fractionalPart * Size + i);// Умножаем дробную часть на размер таблицы и округляем
        }

        private int HashDivision(int key, int i)
        {
            return Math.Abs(key) % 31 + 1; // Разделим на размер таблицы, или на простое число (хз пока)
        }
        private int HashDivision(T1 key, int i)
        {
            int hash = key.GetHashCode(); // Получаем хэш-код объекта
            return Math.Abs(hash) % Size + 1; 
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
        public void Add(T1 key, T2 value)
        {
            if ( probingMethod == "cuckoo")
            {
                AddCuckoo(key, value);
                return;
            }
            if (count >= Size * 0.95)
            {
                Size *= 2;
                Array.Resize(ref table, Size); ;
            }

            int i = 0;
            int index;
            int hash = Hash(key, i);
            do
            {
                index = ProbingMethodSwitch(hash, i);
                if (!table[index].HasValue ) // 
                {
                    table[index] = (key, value);
                    count++;
                    return;
                }

                i++;
            } while (true);
        }

        private readonly int MaxAttempts = 30;
        private void AddCuckoo(T1 key, T2 value)
        {
            if (count >= Size)
            {
                Console.WriteLine("Таблица переполнена. Добавление методом кукушки невозможно"); // У кукушки  коэффициент загрузки меньше 50 %
                //Size *= 2;
                //Array.Resize(ref table, Size); 
            }

            int i = 0;
            int index = HashMultiplication(key, i);
            
            while (count < Size)
            {
                if (i >= MaxAttempts) index = HashMultiplication(index, i);
                if (index >= Size) index %= Size;
                if (!table[index].HasValue) // Если ячейка пустая
                {
                    table[index] = (key, value);
                    count++;
                    return;
                }
                else // Ячейка занята
                {
                    T1 tempKey = table[index].Value.key;
                    T2 tempValue = table[index].Value.value;
                    table[index] = (key, value); // Помещаем ключ в ячейку
                    key = tempKey; // Перемещаем старый ключ
                    value = tempValue; // ... и значение
                    index = HashDivision(key, i); // Используем вторую хеш-функцию
                }
                i++;
            }
        }
        private int ProbingMethodSwitch(int hash, int i)
        {
            int index;

            switch (probingMethod)
            {
                case "cuckoo":
                    index = -1; // У кукушки всё по-своему
                    break;
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

            return index;
        }
        public T2 Add(T1 key)
        {
            throw new NotImplementedException();
        }

        public bool Find(T1 key)
        {
            return Find(key, out T2 value);
        }

        public bool Find(T1 key, out T2 value) // public bool Search(T1 key, out T2 value)
        {
            int i = 0;
            int hash = Hash(key, i);
            int index;
            do
            {
                index = ProbingMethodSwitch(hash, i);
                if (index == -1)
                { 
                    return FindCuckoo(key, out value);  
                }
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

        private bool FindCuckoo(T1 key, out T2 value) 
        {
            int i = 0;
            int index = HashMultiplication(key, 0);
            while (true) 
            {
                if (i >= MaxAttempts) index = HashMultiplication(index, i);
                if (index >= Size) index %= Size;
                if (!table[index].HasValue)
                {
                    value = default;
                    return false; // Элемент не найден
                }
                if (table[index].Value.key.Equals(key))
                {
                    value = table[index].Value.value;
                    return true;
                }
                // Пробуем вторую хеш-функцию
                index = HashDivision(key, i);
                i++;
            }

        }

        public void Remove(T1 key)
        {
            int i = 0;
            int hash = Hash(key, i);
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

        private bool IsProbingMethod(string probingMethod)
        {
            if (probingMethod == "linear" || probingMethod == "quadratic" || probingMethod == "double" || probingMethod == "cuckoo") return true;
            else return false;
        }

        private bool IsHashFunction(string hashFunction)
        {
            if (hashFunction == "division" || hashFunction == "multiplication" ) return true;
            else return false;
        }
    }
}
