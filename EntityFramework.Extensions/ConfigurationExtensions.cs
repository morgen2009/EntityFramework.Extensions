namespace EntityFramework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using System.Data.Entity.ModelConfiguration.Configuration;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;

    public static class ConfigurationExtensions
    {
        public static TriggerAnnotation HasTrigger<T>(this EntityTypeConfiguration<T> configuration, string name) where T : class
        {
            var annotation = new TriggerAnnotation(name);

            configuration.HasTableAnnotation(TriggerAnnotation.AnnotationName, annotation);

            return annotation;
        }

        public static MultipleTriggerAnnotation HasTriggers<T>(this EntityTypeConfiguration<T> configuration, params TriggerAnnotation[] triggers) where T : class
        {
            var annotation = new MultipleTriggerAnnotation(triggers);

            configuration.HasTableAnnotation(TriggerAnnotation.AnnotationName, annotation);

            return annotation;
        }

        public static PrimitivePropertyConfiguration HasValues<T>(this PrimitivePropertyConfiguration configuration, string name, params T[] values)
        {
            var valuesInt = values.Length == 0 ? Enum.GetValues(typeof(T)).Cast<int>().ToArray() : values.Cast<int>().ToArray();

            var annotation = new ValueConstraintAnnotation(name, valuesInt);

            configuration.HasColumnAnnotation(ValueConstraintAnnotation.AnnotationName, annotation);

            return configuration;
        }
    }
}