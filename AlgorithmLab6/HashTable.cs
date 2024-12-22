namespace AlgorithmLab6
{
    public interface IHashTable<T1, T2>
    {
        void Find(T1 key, T2 value);
        T2 Add(T1 key);
        void Remove(T1 key);
    }
}
