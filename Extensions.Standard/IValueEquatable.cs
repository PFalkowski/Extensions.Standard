namespace Extensions.Standard
{
    public interface IValueEquatable<in T>
    {
        bool ValueEquals(T other);
    }
}
