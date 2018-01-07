namespace EntityFramework.Extensions.Generator.Sql
{
    using System.Data.Entity.Migrations.Model;

    public class InverseSqlOperation : SqlOperation
    {
        private readonly string inverseSql;

        /// <inheritdoc />
        public InverseSqlOperation(string sql, string inverseSql) : base(sql)
        {
            this.inverseSql = inverseSql;
        }

        /// <inheritdoc />
        public override MigrationOperation Inverse => new InverseSqlOperation(this.inverseSql, this.Sql);
    }
}