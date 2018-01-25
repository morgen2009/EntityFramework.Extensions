namespace EntityFramework.Extensions.Tests.Generator.Sql.Trigger
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator.Sql.Trigger;
    using EntityFramework.Extensions.Tests.Helper;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TriggerMigrationInterceptorTests
    {
        private TriggerMigrationInterceptor interceptor;
        private ITriggerSqlGenerator sqlGenerator;
        private MigrationOperation triggerOperationCreate;
        private MigrationOperation triggerOperationDrop;
        private TriggerAnnotation triggerAnnotation;
        private TriggerAnnotation triggerAnnotationNew;

        [SetUp]
        public void SetUp()
        {
            this.triggerOperationCreate = new MigrationOperationHelper();
            this.triggerOperationDrop = new MigrationOperationHelper();
            this.triggerAnnotation = new TriggerAnnotation("TriggerName");
            this.triggerAnnotationNew = new TriggerAnnotation("TriggerNameNew");

            this.sqlGenerator = Substitute.For<ITriggerSqlGenerator>();
            this.interceptor = new TriggerMigrationInterceptor(this.sqlGenerator);
        }

        [TestCase("TableName")]
        public void ProcessShouldAddCreateTriggerOperationAfterCreateTableOperation(string tableName)
        {
            this.sqlGenerator.CreateTrigger(tableName, this.triggerAnnotation).Returns(this.triggerOperationCreate);
            var operation = new CreateTableOperation(tableName)
            {
                Annotations = { { TriggerAnnotation.AnnotationName, this.triggerAnnotation } }
            };

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(operation, actual.ElementAt(0));
            Assert.AreEqual(this.triggerOperationCreate, actual.ElementAt(1));
        }

        [TestCase("TableName")]
        public void ProcessShouldAddDropTriggerOperationBeforeDropTableOperation(string tableName)
        {
            this.sqlGenerator.DropTrigger(tableName, this.triggerAnnotation).Returns(this.triggerOperationDrop);
            var operation = new DropTableOperation(tableName)
            {
                RemovedAnnotations = { { TriggerAnnotation.AnnotationName, this.triggerAnnotation } }
            };

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(2, actual.Length);
            Assert.AreEqual(this.triggerOperationDrop, actual.ElementAt(0));
            Assert.AreEqual(operation, actual.ElementAt(1));
        }

        [TestCase("TableName")]
        public void ProcessShouldAddDropAndCreateTriggerOperationsAroundAlterTableOperation(string tableName)
        {
            this.sqlGenerator.DropTrigger(tableName, this.triggerAnnotation).Returns(this.triggerOperationDrop);
            this.sqlGenerator.CreateTrigger(tableName, this.triggerAnnotationNew).Returns(this.triggerOperationCreate);
            var operation = new AlterTableOperation(tableName, new Dictionary<string, AnnotationValues>
            {
                {TriggerAnnotation.AnnotationName, new AnnotationValues(this.triggerAnnotation, this.triggerAnnotationNew)}
            });

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual(this.triggerOperationDrop, actual.ElementAt(0));
            Assert.AreEqual(operation, actual.ElementAt(1));
            Assert.AreEqual(this.triggerOperationCreate, actual.ElementAt(2));
        }
    }
}