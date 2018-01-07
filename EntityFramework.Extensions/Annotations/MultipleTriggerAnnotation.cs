namespace EntityFramework.Extensions.Annotations
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Linq;

    public class MultipleTriggerAnnotation : IMergeableAnnotation
    {
        public IList<TriggerAnnotation> Triggers { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultipleTriggerAnnotation"/> class.
        /// </summary>
        public MultipleTriggerAnnotation()
        {
            this.Triggers = new List<TriggerAnnotation>();
        }

        public MultipleTriggerAnnotation(IEnumerable<TriggerAnnotation> annotations)
        {
            this.Triggers = new List<TriggerAnnotation>(annotations);
        }

        /// <inheritdoc />
        public CompatibilityResult IsCompatibleWith(object other)
        {
            if (!(other is TriggerAnnotation) && !(other is MultipleTriggerAnnotation))
            {
                return new CompatibilityResult(false, $"Target class is not inherited from {typeof(TriggerAnnotation).Name}");
            }

            return new CompatibilityResult(true, null);
        }

        /// <inheritdoc />
        public object MergeWith(object other)
        {
            var merge =  other is MultipleTriggerAnnotation ? (MultipleTriggerAnnotation)other : other as TriggerAnnotation;

            return new MultipleTriggerAnnotation(this.Triggers.Concat(merge.Triggers));
        }

        public static implicit operator MultipleTriggerAnnotation(TriggerAnnotation annotation)
        {
            return annotation == null ? null : new MultipleTriggerAnnotation(new []{ annotation });
        }
    }
}