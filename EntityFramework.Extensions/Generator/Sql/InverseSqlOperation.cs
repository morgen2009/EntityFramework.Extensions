namespace EntityFramework.Extensions.Generator.Sql
{
    using System.Data.Entity.Migrations.Model;

    public class InverseSqlOperation : SqlOperation
    {
        public string InverseSql { get; }

        /// <inheritdoc />
        public InverseSqlOperation(string sql, string inverseSql) : base(sql)
        {
            this.InverseSql = inverseSql;
        }

        /// <inheritdoc />
        public override MigrationOperation Inverse => new InverseSqlOperation(this.InverseSql, this.Sql);
    }
}