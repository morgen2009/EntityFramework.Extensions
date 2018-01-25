namespace EntityFramework.Extensions.Generator.Sql.View
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using EntityFramework.Extensions.Annotations;

    public static class DictionaryExtensions
    {
        public static ViewAnnotation View(this IDictionary<string, object> annotations)
        {
            return annotations[ViewAnnotation.AnnotationName] as ViewAnnotation;
        }

        public static ViewAnnotation NewView(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations[ViewAnnotation.AnnotationName].NewValue as ViewAnnotation;
        }

        public static ViewAnnotation OldView(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations[ViewAnnotation.AnnotationName].OldValue as ViewAnnotation;
        }

        public static bool HasView(this IDictionary<string, object> annotations)
        {
            return annotations.ContainsKey(ViewAnnotation.AnnotationName);
        }

        public static bool HasView(this IDictionary<string, AnnotationValues> annotations)
        {
            return annotations.ContainsKey(ViewAnnotation.AnnotationName);
        }
    }
}