namespace EntityFramework.Extensions.Tests.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;

    public static class DbModelExtensions
    {
        public static IDictionary<string, object> TableAnnotation(this DbModel model, Type entityType)
        {
            var entityModel = model.ConceptualModel.EntityTypes.FirstOrDefault(x => x.Name == entityType.Name);
            var configuration = entityModel?.MetadataProperties.FirstOrDefault(x => x.Name == "Configuration")?.Value;

            var annotations = configuration?.GetType().GetField("_annotations", BindingFlags.NonPublic | BindingFlags.Instance)
                ?.GetValue(configuration) as IDictionary<string, object>;

            return annotations;
        }
    }
}