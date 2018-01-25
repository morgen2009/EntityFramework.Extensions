namespace EntityFramework.Extensions.Generator.Sql.Trigger
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;

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
                var createTableOperation = operation as CreateTableOperation;
                if (createTableOperation != null)
                {
                    yield return operation;

                    if (createTableOperation.Annotations.HasTrigger())
                    {
                        yield return this.sqlGenerator.CreateTrigger(createTableOperation.Name, createTableOperation.Annotations.Trigger());
                    }

                    continue;
                }

                var alterTableOperation = operation as AlterTableOperation;
                if (alterTableOperation != null)
                {
                    if (alterTableOperation.Annotations.HasTrigger())
                    {
                        yield return this.sqlGenerator.DropTrigger(alterTableOperation.Name, alterTableOperation.Annotations.OldTrigger());
                        yield return alterTableOperation;
                        yield return this.sqlGenerator.CreateTrigger(alterTableOperation.Name, alterTableOperation.Annotations.NewTrigger());
                        continue;
                    }
                    yield return alterTableOperation;
                    continue;
                }

                var dropTableOperation = (DropTableOperation) operation;
                if (dropTableOperation != null)
                {
                    if (dropTableOperation.RemovedAnnotations.HasTrigger())
                    {
                        yield return this.sqlGenerator.DropTrigger(dropTableOperation.Name, dropTableOperation.RemovedAnnotations.Trigger());
                    }
                    yield return operation;
                    continue;
                }

                yield return operation;
            }
        }
    }
}