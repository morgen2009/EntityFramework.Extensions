namespace EntityFramework.Extensions.Generator.Sql.Trigger
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using EntityFramework.Extensions.Annotations;

    public static class DictionaryExtensions
    {
        public static TriggerAnnotation Trigger(this IDictionary<string, object> annotations)
        {
            return annotations[TriggerAnnotation.AnnotationName] as TriggerAnnotation;
        }

        public static TriggerAnnotation NewTrigger(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations[TriggerAnnotation.AnnotationName].NewValue as TriggerAnnotation;
        }

        public static TriggerAnnotation OldTrigger(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations[TriggerAnnotation.AnnotationName].OldValue as TriggerAnnotation;
        }

        public static bool HasTrigger(this IDictionary<string, object> annotations)
        {
            return annotations.ContainsKey(TriggerAnnotation.AnnotationName);
        }

        public static bool HasTrigger(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations.ContainsKey(TriggerAnnotation.AnnotationName);
        }
    }
}