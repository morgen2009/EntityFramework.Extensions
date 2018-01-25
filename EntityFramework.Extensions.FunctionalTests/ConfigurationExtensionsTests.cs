namespace EntityFramework.Extensions.FunctionalTests
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Model;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.FunctionalTests.Fixture;
    using EntityFramework.Extensions.FunctionalTests.Migrations;
    using EntityFramework.Extensions.Generator.CSharpCode;
    using EntityFramework.Extensions.Tests.Model;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationExtensionsTests
    {
        private MigrationCodeGenerator codeGenerator;
        private Configuration migrationsConfiguration;

        [SetUp]
        public void SetUp()
        {
            this.codeGenerator = Substitute.For<MigrationCodeGenerator>();
            this.migrationsConfiguration = new Configuration
            {
                CodeGenerator = this.codeGenerator,
                AutomaticMigrationsEnabled = false,
                ContextType = typeof(DataContextTest)
            };
            Database.SetInitializer(new DropCreateDatabaseAlways<DataContextTest>());
        }

        [TestCase("Group", "other SQL code")]
        [TestCase("User", "some SQL script")]
        public void HasTriggerShouldAddTableAnnotation(string triggerName, string triggerCode)
        {
            // arrange
            this.codeGenerator.AnnotationGenerators[TriggerAnnotation.AnnotationName] = () => new TriggerAnnotationCodeGenerator();

            // act
            var modelBuilder = new DbModelBuilder();

            modelBuilder.Entity<UserEntity>().ToTable("USER");
            modelBuilder.Entity<UserEntity>().HasTrigger(triggerName).After(TriggerEventEnum.Update).HasBody(triggerCode);

            var operations = this.BuildMigrationOperations(modelBuilder);

            // assert
            var annotation =
                ((CreateTableOperation) operations.FirstOrDefault(x => x is CreateTableOperation))?.Annotations[
                    TriggerAnnotation.AnnotationName] as TriggerAnnotation;

            Assert.NotNull(annotation);
            Assert.AreEqual(triggerName, annotation.Name);
            Assert.AreEqual(triggerCode, annotation.Body);
        }

        [TestCase("USER", "TR_TABLE_1", "TR_TABLE_2")]
        public void HasTriggersShouldAddTableAnnotation(string tableName, string triggerName1, string triggerName2)
        {
            // arrange
            this.codeGenerator.AnnotationGenerators[TriggerAnnotation.AnnotationName] = () => new TriggerAnnotationCodeGenerator();

            // act
            var modelBuilder = new DbModelBuilder();

            modelBuilder.Entity<UserEntity>().ToTable(tableName);
            modelBuilder.Entity<UserEntity>().HasTriggers(new TriggerAnnotation(triggerName1), new TriggerAnnotation(triggerName2));

            var operations = this.BuildMigrationOperations(modelBuilder);

            // assert
            var annotation =
                ((CreateTableOperation)operations.FirstOrDefault(x => x is CreateTableOperation))?.Annotations[
                    TriggerAnnotation.AnnotationName] as MultipleTriggerAnnotation;
            Assert.NotNull(annotation);
            Assert.AreEqual(triggerName1, annotation.Triggers.First().Name);
            Assert.AreEqual(triggerName2, annotation.Triggers.Last().Name);
        }

        private IEnumerable<MigrationOperation> BuildMigrationOperations(DbModelBuilder modelBuilder)
        {
            var providerInfo = new DbProviderInfo("System.Data.SqlClient", "2008");
            var dbModel = modelBuilder.Build(providerInfo);
            var model = dbModel.Compile();
            DataContextFactoryTest.Model = model;

            var migrationOperations = new List<MigrationOperation>();
            this.codeGenerator.Generate(null, null, null, null, null, null).ReturnsForAnyArgs(x =>
            {
                migrationOperations.AddRange(x.Arg<IEnumerable<MigrationOperation>>());
                return new ScaffoldedMigration();
            });

            var scaffolder = new MigrationScaffolder(this.migrationsConfiguration);
            scaffolder.Scaffold("initial migration");

            return migrationOperations;
        }
    }
}