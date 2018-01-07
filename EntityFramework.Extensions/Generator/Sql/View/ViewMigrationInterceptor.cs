namespace EntityFramework.Extensions.Generator.Sql.View
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;

    public class ViewMigrationInterceptor : IMigrationOperationInterceptor
    {
        private readonly IViewSqlGenerator sqlGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewMigrationInterceptor"/> class.
        /// </summary>
        public ViewMigrationInterceptor(IViewSqlGenerator sqlGenerator)
        {
            this.sqlGenerator = sqlGenerator;
        }
        /// <inheritdoc />
        public IEnumerable<MigrationOperation> Process(IEnumerable<MigrationOperation> operations)
        {
            foreach (var operation in operations)
            {
                if ((operation as CreateTableOperation)?.Annotations.ContainsKey(ViewAnnotation.AnnotationName) == true)
                {
                    var operation1 = operation as CreateTableOperation;
                    yield return this.sqlGenerator.CreateView(operation1.Name, (ViewAnnotation)operation1.Annotations[ViewAnnotation.AnnotationName]);
                }
                else if ((operation as AlterTableOperation)?.Annotations.ContainsKey(ViewAnnotation.AnnotationName) == true)
                {
                    var operation1 = operation as AlterTableOperation;
                    yield return this.sqlGenerator.AlterView(operation1.Name, (ViewAnnotation)operation1.Annotations[ViewAnnotation.AnnotationName].OldValue, (ViewAnnotation)operation1.Annotations[ViewAnnotation.AnnotationName].NewValue);
                }
                else if (operation is RenameTableOperation)
                {
                    yield return this.sqlGenerator.RenameView((operation as RenameTableOperation).Name, (operation as RenameTableOperation).NewName);
                }
                else if ((operation as DropTableOperation)?.RemovedAnnotations.ContainsKey(ViewAnnotation.AnnotationName) == true)
                {
                    var operation1 = operation as DropTableOperation;
                    yield return this.sqlGenerator.DropView(operation1.Name, (ViewAnnotation)operation1.RemovedAnnotations[ViewAnnotation.AnnotationName]);
                }
                else
                {
                    yield return operation;
                }
            }
        }
    }
}