namespace EntityFramework.Extensions.Tests.Helper
{
    public class AssertContainer<T>
    {
        public T Value { get; }

        public AssertContainer(T value)
        {
            this.Value = value;
        }
    }
}