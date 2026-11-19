namespace FrontierDispatch.Core
{
    public class Entry<T>
    {
        public Entry(T value, int priority)
        {
            Value = value;
            Priority = priority;
        }

        public T Value { get; }
        public int Priority { get; }
        
    }
}