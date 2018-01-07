namespace EntityFramework.Extensions.Generator.Sql.View
{
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;

    public class ViewSqlGenerator : IViewSqlGenerator
    {
        private static string AlterViewSql(string viewName, string viewBody)
        {
            return $"ALTER VIEW [{viewName}] AS {viewBody}";
        }

        private static string RenameSql(string name, string nameNew)
        {
            return $"EXEC sp_rename N'{name}', N'{nameNew}', N'VIEW'";
        }

        private static string DropViewSql(string viewName)
        {
            return $"DROP VIEW [{viewName}]";
        }

        private static string CreateViewSql(string viewName, string viewBody)
        {
            return $"CREATE VIEW [{viewName}] AS {viewBody}";
        }

        /// <param name="viewName"></param>
        /// <param name="viewAnnotation"></param>
        /// <inheritdoc />
        public MigrationOperation CreateView(string viewName, ViewAnnotation viewAnnotation)
        {
            return new InverseSqlOperation(CreateViewSql(viewName, viewAnnotation.Body), DropViewSql(viewName));
        }

        /// <inheritdoc />
        public MigrationOperation AlterView(string viewName, ViewAnnotation oldAnnotation, ViewAnnotation newAnnotation)
        {
            return new InverseSqlOperation(AlterViewSql(viewName, newAnnotation.Body), AlterViewSql(viewName, oldAnnotation.Body));
        }

        /// <inheritdoc />
        public MigrationOperation RenameView(string name, string newName)
        {
            return new InverseSqlOperation(RenameSql(name, newName), RenameSql(newName, name));
        }

        /// <inheritdoc />
        public MigrationOperation DropView(string viewName, ViewAnnotation annotation)
        {
            return new InverseSqlOperation(DropViewSql(viewName), CreateViewSql(viewName, annotation.Body));
        }
    }
}