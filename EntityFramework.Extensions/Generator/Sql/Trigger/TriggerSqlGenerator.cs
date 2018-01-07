namespace EntityFramework.Extensions.Generator.Sql.Trigger
{
    using System;
    using System.Data.Entity.Migrations.Model;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;

    public class TriggerSqlGenerator : ITriggerSqlGenerator
    {
        private static string GenerateDropSql(string name, TriggerAnnotation annotation)
        {
            return $"DROP TRIGGER IF EXISTS [{annotation.Name}]\nGO\n";
        }

        private static string GenerateCreateSql(string name, TriggerAnnotation annotation)
        {
            var typeText = FormatTriggerType(annotation.TriggerType);
            var eventText = FormatTriggerEvents(annotation.TriggerEvents);
            
            return $"CREATE TRIGGER [{annotation.Name}] ON {name} {typeText} {eventText} AS {annotation.Body}\nGO\n";
        }

        private static string FormatTriggerEvents(TriggerEventEnum events)
        {
            var items = Enum.GetValues(typeof(TriggerEventEnum)).Cast<TriggerEventEnum>().Where(x => events.HasFlag(x))
                .Select(x => x.ToString().ToUpper());

            return string.Join(",", items);
        }

        private static string FormatTriggerType(TriggerTypeEnum type)
        {
            return type == TriggerTypeEnum.After ? "AFTER" : "INSTEAD OF";
        }

        /// <inheritdoc />
        public MigrationOperation CreateTrigger(string tableName, TriggerAnnotation triggerAnnotation)
        {
            return new InverseSqlOperation(GenerateCreateSql(tableName, triggerAnnotation), GenerateDropSql(tableName, triggerAnnotation));
        }

        /// <inheritdoc />
        public MigrationOperation DropTrigger(string tableName, TriggerAnnotation triggerAnnotation)
        {
            return new InverseSqlOperation(GenerateDropSql(tableName, triggerAnnotation), GenerateCreateSql(tableName, triggerAnnotation));
        }
    }
}