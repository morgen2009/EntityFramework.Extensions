namespace EntityFramework.Extensions.Annotations
{
    using System.Data.Entity.Infrastructure.Annotations;
    using System.IO;
    using System.Linq;
    using EntityFramework.Extensions.Commons;

    public class TriggerAnnotation : IMergeableAnnotation
    {
        public const string AnnotationName = "trigger";

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerAnnotation"/> class.
        /// </summary>
        public TriggerAnnotation(string triggerName)
        {
            this.Name = triggerName;
            this.TriggerType = TriggerTypeEnum.After;
            this.TriggerEvents = TriggerEventEnum.Update;
        }

        public string Body { get; set; }

        public TriggerTypeEnum TriggerType { get; set; }

        public TriggerEventEnum TriggerEvents { get; set; }

        public string Name { get; set; }

        public TriggerAnnotation After(params TriggerEventEnum[] types)
        {
            this.TriggerType = TriggerTypeEnum.After;
            this.TriggerEvents = (TriggerEventEnum)types.Aggregate(0, (a, x) => a | (int)x);

            return this;
        }

        public TriggerAnnotation InsteadOf(params TriggerEventEnum[] types)
        {
            this.TriggerType = TriggerTypeEnum.InsteadOf;
            this.TriggerEvents = (TriggerEventEnum)types.Aggregate(0, (a, x) => a | (int)x);

            return this;
        }

        public TriggerAnnotation HasBody(string script)
        {
            this.Body = script;

            return this;
        }

        public TriggerAnnotation HasBodyEncoded(string script)
        {
            this.Body = script.FromBase64();

            return this;
        }

        public TriggerAnnotation HasBody(Stream script)
        {
            this.Body = script.ReadAsString();

            return this;
        }

        /// <inheritdoc />
        public CompatibilityResult IsCompatibleWith(object other)
        {
            if (other is TriggerAnnotation)
            {
                return new CompatibilityResult(true, "ok");
            }

            var mergeableAnnotation = other as IMergeableAnnotation;
            return mergeableAnnotation != null
                ? mergeableAnnotation.IsCompatibleWith(this)
                : new CompatibilityResult(false, $"Target class is inherited from {typeof(TriggerAnnotation).Name} class");
        }

        /// <inheritdoc />
        public object MergeWith(object other)
        {
            var triggerAnnotation = other as TriggerAnnotation;
            if (triggerAnnotation == null)
            {
                return ((IMergeableAnnotation) other).MergeWith(this);
            }

            var result = new MultipleTriggerAnnotation(new []{ this });
            result.Triggers.Add(triggerAnnotation);

            return result;
        }
    }
}