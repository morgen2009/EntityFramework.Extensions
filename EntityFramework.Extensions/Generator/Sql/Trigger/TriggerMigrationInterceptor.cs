namespace EntityFramework.Extensions.Generator.Sql.Trigger
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;

    public class TriggerMigrationInterceptor : IMigrationOperationInterceptor
    {
        private readonly ITriggerSqlGenerator sqlGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TriggerMigrationInterceptor"/> class.
        /// </summary>
        public TriggerMigrationInterceptor(ITriggerSqlGenerator sqlGenerator)
        {
            this.sqlGenerator = sqlGenerator;
        }

        /// <inheritdoc />
        public IEnumerable<MigrationOperation> Process(IEnumerable<MigrationOperation> operations)
        {
            foreach (var operation in operations)
            {
                if ((operation as CreateTableOperation)?.Annotations.ContainsKey(TriggerAnnotation.AnnotationName) == true)
                {
                    yield return operation;
                    var operation1 = operation as CreateTableOperation;
                    yield return this.sqlGenerator.CreateTrigger(
                        operation1.Name,
                        operation1.Annotations[TriggerAnnotation.AnnotationName] as TriggerAnnotation);
                }
                else if ((operation as AlterTableOperation)?.Annotations.ContainsKey(TriggerAnnotation.AnnotationName) == true)
                {
                    yield return operation;
                    var operation1 = operation as AlterTableOperation;
                    yield return this.sqlGenerator.DropTrigger(
                        operation1.Name,
                        operation1.Annotations[TriggerAnnotation.AnnotationName].OldValue as TriggerAnnotation);
                    yield return this.sqlGenerator.DropTrigger(
                        operation1.Name,
                        operation1.Annotations[TriggerAnnotation.AnnotationName].NewValue as TriggerAnnotation);
                }
                else if ((operation as DropTableOperation)?.RemovedAnnotations.ContainsKey(TriggerAnnotation.AnnotationName) == true)
                {
                    yield return operation;
                    var operation1 = operation as DropTableOperation;
                    yield return this.sqlGenerator.DropTrigger(
                        operation1.Name,
                        operation1.RemovedAnnotations[TriggerAnnotation.AnnotationName] as TriggerAnnotation);
                }
                else
                {
                    yield return operation;
                }
            }
        }
    }
}