namespace EntityFramework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration;
    using EntityFramework.Extensions.Annotations;

    public abstract class ViewTypeConfiguration<TEntityType> : EntityTypeConfiguration<TEntityType> where TEntityType : class
    {
        private readonly ICollection<string> viewCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewTypeConfiguration{TEntityType}"/> class.
        /// </summary>
        protected ViewTypeConfiguration(ICollection<string> viewCollection)
        {
            this.viewCollection = viewCollection;
        }

        private new EntityTypeConfiguration<TEntityType> ToTable(string tableName)
        {
            throw new InvalidOperationException();
        }

        private new EntityTypeConfiguration<TEntityType> ToTable(string tableName, string schema)
        {
            throw new InvalidOperationException();
        }

        public ViewAnnotation ToView(string viewName)
        {
            this.viewCollection.Add(viewName);

            base.ToTable(viewName);

            var annotation = new ViewAnnotation();

            this.HasTableAnnotation(ViewAnnotation.AnnotationName, annotation);

            return annotation;
        }
    }
}