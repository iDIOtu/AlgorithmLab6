namespace AlgorithmLab6
{
    public interface IHashTable<T1, T2>
    {
        bool Find(T1 key, out T2 value);
        void Add(T1 key, T2 value);
        void Remove(T1 key);
    }
}
