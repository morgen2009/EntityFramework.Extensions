﻿namespace EntityFramework.Extensions.Tests
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Migrations.Design;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator.CSharpCode;
    using EntityFramework.Extensions.Tests.Helper;
    using EntityFramework.Extensions.Tests.Model;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ConfigurationExtensionsTests
    {
        private MigrationCodeGenerator codeGenerator;

        [SetUp]
        public void SetUp()
        {
            this.codeGenerator = Substitute.For<MigrationCodeGenerator>();
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

            var model = modelBuilder.Build(new DbProviderInfo("System.Data.SqlClient", "2008"));

            var annotations = model.TableAnnotation(typeof(UserEntity));

            // assert
            var annotation = annotations[TriggerAnnotation.AnnotationName] as TriggerAnnotation;
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

            var model = modelBuilder.Build(new DbProviderInfo("System.Data.SqlClient", "2008"));

            var annotations = model.TableAnnotation(typeof(UserEntity));

            // assert
            var annotation = annotations[TriggerAnnotation.AnnotationName] as MultipleTriggerAnnotation;
            Assert.NotNull(annotation);
            Assert.AreEqual(triggerName1, annotation.Triggers.First().Name);
            Assert.AreEqual(triggerName2, annotation.Triggers.Last().Name);
        }
    }
}