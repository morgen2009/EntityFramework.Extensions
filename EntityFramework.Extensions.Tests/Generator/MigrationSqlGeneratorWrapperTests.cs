namespace EntityFramework.Extensions.Tests.Generator
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
    using EntityFramework.Extensions.Generator;
    using EntityFramework.Extensions.Tests.Helper;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MigrationSqlGeneratorWrapperTests
    {
        private MigrationSqlGeneratorWrapper sqlGenerator;
        private MigrationSqlGeneratorStub innerSqlGenerator;
        private ICollection<IMigrationOperationInterceptor> interceptors;

        [SetUp]
        public void SetUp()
        {
            this.innerSqlGenerator = new MigrationSqlGeneratorStub();
            this.interceptors = new List<IMigrationOperationInterceptor>();
            this.sqlGenerator = new MigrationSqlGeneratorWrapper(this.innerSqlGenerator, this.interceptors);
        }

        [Test]
        public void GenerateShouldDelegateCallToInnerSqlGenerator()
        {
            const string someProviderManifestToken = "2008";
            var operations = new List<MigrationOperation> { new MigrationOperationHelper() };
            var statements = new List<MigrationStatement>();
            this.innerSqlGenerator.GeneratedStatements = statements;

            var actualStatements = this.sqlGenerator.Generate(operations, someProviderManifestToken);

            Assert.AreEqual(someProviderManifestToken, this.innerSqlGenerator.LastProviderManifestToken);
            Assert.AreEqual(operations, this.innerSqlGenerator.LastOperations);
            Assert.AreSame(statements, actualStatements);
        }

        [Test]
        public void GenerateShouldCallInterceptorThenInnerSqlGenerator()
        {
            const string someProviderManifestToken = "2008";
            var operations = new List<MigrationOperation> { new MigrationOperationHelper() };
            this.innerSqlGenerator.GeneratedStatements = new List<MigrationStatement>();
            var interceptor = new MigrationOperationInterceptorStub
            {
                ModifiedOperations = new List<MigrationOperation> {new MigrationOperationHelper()}
            };
            this.interceptors.Add(interceptor);

            var actualStatements = this.sqlGenerator.Generate(operations, someProviderManifestToken);

            Assert.AreEqual(operations, interceptor.LastOperations);
            Assert.AreEqual(interceptor.ModifiedOperations, this.innerSqlGenerator.LastOperations);
            Assert.AreSame(this.innerSqlGenerator.GeneratedStatements, actualStatements);
            Assert.AreEqual(someProviderManifestToken, this.innerSqlGenerator.LastProviderManifestToken);
        }
    }
}