namespace EntityFramework.Extensions.Tests.Generator.Sql.Trigger
{
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator.Sql.Trigger;
    using NUnit.Framework;

    [TestFixture]
    public class TriggerSqlGeneratorTests
    {
        private TriggerSqlGenerator generator;

        [SetUp]
        public void SetUp()
        {
            this.generator = new TriggerSqlGenerator();
        }

        private static TestCaseData[] SourceData => new[]
        {
            new TestCaseData("TABLE1", "TR_TABLE1_UPDATE_LOG", TriggerEventEnum.Update, "BEGIN END",
                "CREATE TRIGGER [TR_TABLE1_UPDATE_LOG] ON TABLE1 AFTER UPDATE AS BEGIN END\nGO\n",
                "DROP TRIGGER IF EXISTS [TR_TABLE1_UPDATE_LOG]\nGO\n")
        };

        [TestCaseSource(nameof(SourceData))]
        public void CreateTriggerReturnSqlToCreateTriggerAndInverse(string tableName, string triggerName, TriggerEventEnum triggerEventEnum, string body, string expectedCreate, string expectedDrop)
        {
            var annotation = new TriggerAnnotation(triggerName).After(triggerEventEnum).HasBody(body);

            var migration = this.generator.CreateTrigger(tableName, annotation);

            Assert.IsInstanceOf<SqlOperation>(migration);
            Assert.AreEqual(expectedCreate, ((SqlOperation)migration).Sql);
            Assert.AreEqual(expectedDrop, (((SqlOperation)migration).Inverse as SqlOperation)?.Sql);
        }

        [TestCaseSource(nameof(SourceData))]
        public void DropTriggerReturnValidSqlToDropTriggerAndInverse(string tableName, string triggerName, TriggerEventEnum triggerEventEnum, string body, string expectedCreate, string expectedDrop)
        {
            var annotation = new TriggerAnnotation(triggerName).After(triggerEventEnum).HasBody(body);

            var migration = this.generator.DropTrigger(tableName, annotation);

            Assert.IsInstanceOf<SqlOperation>(migration);
            Assert.AreEqual(expectedDrop, ((SqlOperation)migration).Sql);
            Assert.AreEqual(expectedCreate, (((SqlOperation)migration).Inverse as SqlOperation)?.Sql);
        }
    }
}