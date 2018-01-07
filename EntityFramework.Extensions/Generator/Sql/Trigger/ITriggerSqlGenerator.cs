namespace EntityFramework.Extensions.Generator.Sql.Trigger
{
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;

    public interface ITriggerSqlGenerator
    {
        MigrationOperation CreateTrigger(string tableName, TriggerAnnotation triggerAnnotation);

        MigrationOperation DropTrigger(string tableName, TriggerAnnotation triggerAnnotation);
    }
}