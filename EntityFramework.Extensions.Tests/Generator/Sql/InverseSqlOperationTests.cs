namespace EntityFramework.Extensions.Tests.Generator.Sql
{
    using EntityFramework.Extensions.Generator.Sql;
    using NUnit.Framework;

    [TestFixture]
    public class InverseSqlOperationTests
    {
        [TestCase("sql 1", "sql 2")]
        public void InverseReturnSqlOperationWithRevertSql(string sql, string inverseSql)
        {
            var operation = new InverseSqlOperation(sql, inverseSql);

            var actual = operation.Inverse;

            Assert.IsInstanceOf<InverseSqlOperation>(actual);
            Assert.AreEqual(inverseSql, ((InverseSqlOperation) actual).Sql);
            Assert.AreEqual(sql, ((InverseSqlOperation) actual).InverseSql);
        }
    }
}