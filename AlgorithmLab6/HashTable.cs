namespace AlgorithmLab6
{
    public abstract class HashTable<T1, T2>
    {
        abstract public bool Find(T1 key, out T2 value);
        abstract public void Add(T1 key, T2 value);
        abstract public void Remove(T1 key);
    }
}
