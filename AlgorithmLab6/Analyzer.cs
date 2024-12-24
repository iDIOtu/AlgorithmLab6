using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlgorithmLab6
{
    public class Analyzer
    {
        private static Random random = new Random();

        // Метод для генерации случайной строки заданной длины
        private static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            char[] stringChars = new char[length];
            for (int i = 0; i < length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new string(stringChars);
        }

        public static List<KeyValuePair<string, string>> GenerateKeyValuePairList(int numberOfEntries)
        {
            var keyValuePairList = new List<KeyValuePair<string, string>>();
            var existingKeys = new HashSet<string>(); // Для отслеживания существующих ключей

            while (keyValuePairList.Count < numberOfEntries)
            {
                string key = GenerateRandomString(10); // Генерация случайного ключа длиной 10 символов
                string value = GenerateRandomString(20); // Генерация случайного значения длиной 20 символов

                // Проверяем, существует ли ключ
                if (!existingKeys.Contains(key))
                {
                    existingKeys.Add(key);
                    keyValuePairList.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return keyValuePairList;
        }

        static public void ChainedAnalyze()
        {
            var pairs = GenerateKeyValuePairList(100000);
            Console.WriteLine($"Начало тестирования:\n");

            double bestLoadFactor = double.MaxValue;
            int bestMaxChainLength = int.MaxValue;
            int bestMinChainLength = int.MaxValue;
            HashMethod bestMethod = HashMethod.Default;

            foreach (HashMethod method in Enum.GetValues(typeof(HashMethod)))
            {
                Console.WriteLine($"Анализ для метода хеширования: {method}");
                var HashTable = new HashTableChains<string, string>(method);

                foreach (var pair in pairs)
                    HashTable.Add(pair.Key, pair.Value);

                var chainLengths = HashTable.GetChainLengths();
                int totalcount = chainLengths.Count(x => x > 0);
                int maxChainLength = chainLengths.Max();
                int minChainLength = chainLengths.Where(x => x > 0).DefaultIfEmpty(0).Min();

                double loadFactor = (double)totalcount / HashTableChains<string, string>.TableSize * 100;

                Console.WriteLine($"Коэффициент заполнения: {loadFactor}");
                Console.WriteLine($"Максимальная длина цепочки: {maxChainLength}");
                Console.WriteLine($"Минимальная длина цепочки: {minChainLength}");
                Console.WriteLine();

                if (maxChainLength < bestMaxChainLength || (maxChainLength == bestMaxChainLength && minChainLength < bestMinChainLength))
                {
                    bestLoadFactor = loadFactor;
                    bestMaxChainLength = maxChainLength;
                    bestMinChainLength = minChainLength;
                    bestMethod = method;
                }
            }

            Console.WriteLine($"Лучший метод хеширования: {bestMethod}");
            Console.WriteLine($"Причина: Этот метод обеспечивает наименьшую длину цепочек {bestMaxChainLength} и лучший коэффициент заполнения {bestLoadFactor}, что свидетельствует о более равномерном распределении ключей и меньших затратах на операции поиска.\n");
        }

        internal static void OpenAnalyze()
        {
            var pairs = GenerateKeyValuePairList(10000);
            Console.WriteLine($"Начало тестирования:\n");
            var probingsList = new List<string>() { "linear", "quadratic", "double", "cuckoo", "Fibonacci"/*, "random"*/ };
            var hashList = new List<string>() { "division", "multiplication", "multiplication2" };

            int BestMaxCluster = int.MaxValue;
            string bestHash = "default";
            string bestProb = "default";

            foreach (var hash in hashList)
            {
                foreach (var prob in probingsList)
                {
                    Console.WriteLine($"Анализ для метода хеширования: {hash} и стратегия пробирования: {prob}");
                    var HashTable = new HashTableOpen<string, string>(prob, hash);

                    foreach (var pair in pairs)
                        HashTable.Add(pair.Key, pair.Value);

                    int maxClusternLength = HashTable.MaxClusterLength();

                    Console.WriteLine($"Максимальная длина кластера: {maxClusternLength}");
                    Console.WriteLine();

                    if (maxClusternLength < BestMaxCluster)
                    {
                        BestMaxCluster = maxClusternLength;
                        bestHash = hash;
                        bestProb = prob;
                    }
                }
                
            }

            Console.WriteLine($"Лучший метод хеширования: {bestHash} и стратегия пробирования: {bestProb}");
            Console.WriteLine($"Причина: Этот метод со стратегией cобеспечивает наименьшую длину кластера {BestMaxCluster}\n");
        }
    }
}
