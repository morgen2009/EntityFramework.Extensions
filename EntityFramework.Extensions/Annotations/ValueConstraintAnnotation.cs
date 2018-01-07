namespace EntityFramework.Extensions.Annotations
{
    public class ValueConstraintAnnotation
    {
        public const string AnnotationName = "check_constraint";

        public string Name { get; private set; }

        public int[] Values { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueConstraintAnnotation"/> class.
        /// </summary>
        public ValueConstraintAnnotation(string name, int[] values)
        {
            this.Values = values;
            this.Name = name;
        }

        public ValueConstraintAnnotation HasName(string name)
        {
            this.Name = name;

            return this;
        }
    }
}