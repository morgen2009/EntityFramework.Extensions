namespace EntityFramework.Extensions.Generator.CSharpCode
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Utilities;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Commons;

    public class TriggerAnnotationCodeGenerator : AnnotationCodeGenerator
    {
        /// <inheritdoc />
        public override IEnumerable<string> GetExtraNamespaces(IEnumerable<string> annotationNames)
        {
            if (!annotationNames.Contains(TriggerAnnotation.AnnotationName))
            {
                yield break;
            }

            yield return typeof(TriggerTypeEnum).Namespace;
            yield return typeof(TriggerAnnotation).Namespace;
        }

        /// <inheritdoc />
        public override void Generate(string annotationName, object annotation, IndentedTextWriter writer)
        {
            if (annotationName != TriggerAnnotation.AnnotationName)
            {
                return;
            }

            var triggerAnnotation = annotation as MultipleTriggerAnnotation;
            var annotation1 = triggerAnnotation ?? (TriggerAnnotation) annotation;

            if (annotation1 == null)
            {
                return;
            }

            if (annotation1.Triggers.Count == 0)
            {
                writer.WriteLine("null");
                return;
            }

            writer.WriteLine("new [] { ");
            writer.Indent++;

            var count = annotation1.Triggers.Count;
            var delimiter = ",";
            foreach (var trigger in annotation1.Triggers)
            {
                if (count == 0)
                {
                    delimiter = "";
                }

                var events = Enum.GetValues(typeof(TriggerEventEnum)).Cast<TriggerEventEnum>()
                    .Where(x => trigger.TriggerEvents.HasFlag(x)).ToArray();

                var triggerEventsText = string.Join(", ", events.Select(x => $"{typeof(TriggerEventEnum).Name}.{x}"));
                var scriptText = trigger.Body.ToBase64();

                writer.WriteLine($"new TriggerAnnotation(\"{trigger.Name}\")");
                writer.Indent++;
                writer.WriteLine($".{trigger.TriggerType}({triggerEventsText})");
                writer.WriteLine($".HasBodyEncoded(\"{scriptText}\"){delimiter}");

                writer.Indent--;
                count--;
            }

            writer.Indent++;
            writer.WriteLine("}");

            writer.Indent--;
        }
    }
}