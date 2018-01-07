namespace EntityFramework.Extensions.Generator.Sql.View
{
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;

    public interface IViewSqlGenerator
    {
        MigrationOperation CreateView(string viewName, ViewAnnotation viewAnnotation);
        MigrationOperation AlterView(string viewName, ViewAnnotation oldAnnotation, ViewAnnotation newAnnotation);
        MigrationOperation RenameView(string name, string newName);
        MigrationOperation DropView(string viewName, ViewAnnotation annotation);
    }
}