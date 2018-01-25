namespace EntityFramework.Extensions.Generator.Sql.View
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;

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
                var createTableOperation = operation as CreateTableOperation;
                if (createTableOperation != null && createTableOperation.Annotations.HasView())
                {
                    yield return this.sqlGenerator.CreateView(createTableOperation.Name, createTableOperation.Annotations.View());
                    continue;
                }

                var alterTableOperation = operation as AlterTableOperation;
                if (alterTableOperation != null && alterTableOperation.Annotations.HasView())
                {
                    yield return this.sqlGenerator.AlterView(alterTableOperation.Name, alterTableOperation.Annotations.OldView(), alterTableOperation.Annotations.NewView());
                    continue;
                }

                if (operation is RenameTableOperation)
                {
                    yield return this.sqlGenerator.RenameView((operation as RenameTableOperation).Name, (operation as RenameTableOperation).NewName);
                    continue;
                }

                var dropTableOperation = operation as DropTableOperation;
                if (dropTableOperation != null && dropTableOperation.RemovedAnnotations.HasView())
                {
                    yield return this.sqlGenerator.DropView(dropTableOperation.Name, dropTableOperation.RemovedAnnotations.View());
                    continue;
                }

                yield return operation;
            }
        }
    }
}