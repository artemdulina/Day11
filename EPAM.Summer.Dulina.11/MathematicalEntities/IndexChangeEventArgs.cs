namespace MathematicalEntities
{
    public sealed class IndexChangeEventArgs<T>
    {
        public int I { get; }
        public int J { get; }
        public T OldValue { get; }
        public T NewValue { get; }

        public IndexChangeEventArgs(int i, int j, T oldValue, T newValue)
        {
            I = i;
            J = j;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
