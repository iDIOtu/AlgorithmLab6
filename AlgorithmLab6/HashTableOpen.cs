using System;
using System.Collections.Generic;
using System.Linq;

namespace AlgorithmLab6
{
    public class HashTableOpen<T1, T2> : HashTable<T1, T2>
    {
        private int Size = 10000;
        private (T1 key, T2 value)?[] table;
        private int count;
        private string probingMethod = "quadratic";
        private string hashFunction = "multiplication";

        public HashTableOpen()
        {
            table = new (T1, T2)?[Size];
            count = 0;
        }
        public HashTableOpen(string probingMethod)
        {
            table = new (T1, T2)?[Size];
            count = 0;
            if (IsProbingMethod(probingMethod)) this.probingMethod = probingMethod;
            else Console.WriteLine("Метод не найден");
        }

        public HashTableOpen(string probingMethod, string hashFunction)
        {
            table = new (T1, T2)?[Size];
            count = 0;
            if (IsProbingMethod(probingMethod)) this.probingMethod = probingMethod;
            else Console.WriteLine("Метод не найден");
            if (IsHashFunction(hashFunction)) this.hashFunction = hashFunction;
            else Console.WriteLine("Хэшфункция не найдена");
        }

        public HashTableOpen(int NewSize)
        {
            Size = NewSize;
            table = new (T1, T2)?[Size];
            count = 0;
        }

        public override void Add(T1 key, T2 value)
        {
            if (probingMethod == "cuckoo")
            {
                AddCuckoo(key, value);
                return;
            }
            if (count >= Size * 0.95)
            {
                Size *= 2;
                Array.Resize(ref table, Size);
            }

            int i = 0;
            int index;
            int hash = Hash(key, i);
            do
            {
                index = ProbingMethodSwitch(hash, i);
                if (index == -2) //ресайз для рандома
                {
                    Size *= 2;
                    Array.Resize(ref table, Size);
                    index = ProbingMethodSwitch(hash, i);
                }
                if (!table[index].HasValue)
                {
                    table[index] = (key, value);
                    count++;
                    return;
                }

                i++;
            } while (true);
        }

        public override bool Find(T1 key, out T2 value)
        {
            if (probingMethod == "random") return FindRandomly(key, out value);
            int i = 0;
            int hash = Hash(key, i);
            int index;
            do
            {
                index = ProbingMethodSwitch(hash, i);
                if (index == -1) return FindCuckoo(key, out value);

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

        public override void Remove(T1 key)
        {
            if (probingMethod == "random")
            {
                if (!RemoveRandomly(key)) Console.WriteLine("Элемент не найден"); return;
            }
            int i = 0;
            int hash = Hash(key, i);
            int index;
            do
            {
                index = ProbingMethodSwitch(hash, i);
                if (index == -1)
                {
                    if (!RemoveCuckoo(key)) Console.WriteLine("Элемент не найден"); return;
                }
                if (!table[index].HasValue)
                {
                    Console.WriteLine("Элемент не найден"); return;
                }

                if (table[index].Value.key.Equals(key))
                {
                    table[index] = null;
                    count--;
                    return;
                }

                i++;
            } while (true);
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
                case "multiplication2":
                    return HashPrimeMultiplication(key);
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

        private int HashPrimeMultiplication(T1 key)
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

            return Math.Abs(hash) % Size;
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

        private int RandomProbing(int key, int i)
        {
            if (i >= Size)
            {
                return -2;
            }
            Random random = new Random();
            return (key + random.Next(0, Size)) % Size;
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
                case "random":
                    index = RandomProbing(hash, i);
                    break;
                default:
                    throw new ArgumentException("Invalid probing method");
            }

            return index;
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

        private bool FindRandomly(T1 key, out T2 value)
        {
            int i = 0;
            int hash = Hash(key, i);
            while (i < Size)
            {
                int index = RandomProbing(hash, i);
                if (!table[index].HasValue) continue;
                if (table[index].Value.key.Equals(key))
                {
                    value = table[index].Value.value;
                    return true;
                }
            }
            value = default;
            return false;
        }

        private bool RemoveCuckoo(T1 key)
        {
            int i = 0;
            int index = HashMultiplication(key, 0);
            while (true)
            {
                if (i >= MaxAttempts) index = HashMultiplication(index, i);
                if (index >= Size) index %= Size;
                if (!table[index].HasValue)
                {
                    return false; // Элемент не найден
                }
                if (table[index].Value.key.Equals(key))
                {
                    table[index] = null;
                    count--;
                    return true;
                }
                // Пробуем вторую хеш-функцию
                index = HashDivision(key, i);
                i++;
            }
        }

        private bool RemoveRandomly(T1 key)
        {
            int i = 0;
            int hash = Hash(key, i);
            while (i < Size)
            {
                int index = RandomProbing(hash, i);
                if (!table[index].HasValue)
                {
                    continue;
                }
                if (table[index].Value.key.Equals(key))
                {
                    table[index] = null;
                    count--;
                    return true;
                }
            }
            return false;
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
            if (probingMethod == "linear" || probingMethod == "quadratic" || probingMethod == "double" || probingMethod == "cuckoo" || probingMethod == "random") return true;
            else return false;
        }

        private bool IsHashFunction(string hashFunction)
        {
            if (hashFunction == "division" || hashFunction == "multiplication" || hashFunction == "multiplication2") return true;
            else return false;
        }
    }
}
