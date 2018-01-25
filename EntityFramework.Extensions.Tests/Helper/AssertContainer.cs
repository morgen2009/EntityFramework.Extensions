namespace EntityFramework.Extensions.Tests.Helper
{
    public class AssertContainer<T>
    {
        public T Value { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertContainer{T}"/> class.
        /// </summary>
        public AssertContainer(T value)
        {
            this.Value = value;
        }
    }
}