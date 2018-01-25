namespace EntityFramework.Extensions.Tests.Generator
{
    using System.Collections.Generic;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Data.Entity.SqlServer;
    using System.Diagnostics.CodeAnalysis;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator;
    using EntityFramework.Extensions.Generator.Sql.Trigger;
    using NUnit.Framework;

    [TestFixture]
    [ExcludeFromCodeCoverage]
    // ReSharper disable once TestFileNameWarning
    public class MigrationSqlGeneratorFunctionalTests
    {
        private MigrationSqlGeneratorWrapper sqlGenerator;
        private MigrationSqlGenerator innerSqlGenerator;
        private ICollection<IMigrationOperationInterceptor> interceptors;

        [SetUp]
        public void SetUp()
        {
            this.innerSqlGenerator = new SqlServerMigrationSqlGenerator();
            this.interceptors = new List<IMigrationOperationInterceptor>();
            this.sqlGenerator = new MigrationSqlGeneratorWrapper(this.innerSqlGenerator, this.interceptors);
        }

        [Test]
        public void TestGenerate()
        {
            this.interceptors.Add(new TriggerMigrationInterceptor(new TriggerSqlGenerator()));
            var annotation = new TriggerAnnotation("TR_UserName_CHANGE").After(TriggerEventEnum.Update).HasBody("BEGIN\nPRINT N'Trigger'\nEND");
            var createTableOperation = new CreateTableOperation("UserName", new Dictionary<string, object>
            {
                {TriggerAnnotation.AnnotationName, annotation}
            }) {Columns = {new ColumnModel(PrimitiveTypeKind.Int16) {Name = "Id", StoreType = "INT"}}};

            var actual = this.sqlGenerator.Generate(new List<MigrationOperation> { createTableOperation }, "2008");
        }
    }
}