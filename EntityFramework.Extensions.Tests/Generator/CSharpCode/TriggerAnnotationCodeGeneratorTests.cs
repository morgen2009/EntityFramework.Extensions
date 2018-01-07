namespace EntityFramework.Extensions.Tests.Generator.CSharpCode
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Utilities;
    using System.IO;
    using System.Text;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator.CSharpCode;
    using EntityFramework.Extensions.Tests.Helper;
    using NUnit.Framework;
    using NUnit.Framework.Constraints;

    [TestFixture]
    public class TriggerAnnotationCodeGeneratorTests
    {
        private TriggerAnnotationCodeGenerator generator;
        private CSharpMigrationCodeGenerator codeGenerator;

        [SetUp]
        public void SetUp()
        {
            this.generator = new TriggerAnnotationCodeGenerator();
            this.codeGenerator = new CSharpMigrationCodeGenerator();
        }

        [Test]
        public void GenerateShouldWriteValidCSharpCode()
        {
            this.codeGenerator.AnnotationGenerators[TriggerAnnotation.AnnotationName] = () => this.generator;
            var annotation = new TriggerAnnotation("TR_USER_UPDATE").HasBody("BEGIN END");
            var operation = new CreateTableOperation("UserTable", new Dictionary<string, object> { { TriggerAnnotation.AnnotationName, annotation } });

            var migration = this.codeGenerator.Generate("init", new[] { operation }, "sourcemodel", "targetMode", "EntityFramework.Extensions.Tests.Fixture.Migrations", "Init");

            AssertHelper.That(migration.UserCode).IsValidCSharp();
        }

        [Test]
        public void GenerateShouldWriteValidCSharpCodeWhenMultipleTriggers()
        {
            this.codeGenerator.AnnotationGenerators[TriggerAnnotation.AnnotationName] = () => this.generator;
            var annotation1 = new TriggerAnnotation("TR_USER_UPDATE1").HasBody("BEGIN END");
            var annotation2 = new TriggerAnnotation("TR_USER_UPDATE2").HasBody("BEGIN END");
            var operation = new CreateTableOperation("UserTable", new Dictionary<string, object> { { TriggerAnnotation.AnnotationName, annotation1.MergeWith(annotation2) } });

            var migration = this.codeGenerator.Generate("init", new[] { operation }, "sourcemodel", "targetMode", "EntityFramework.Extensions.Tests.Fixture.Migrations", "Init");

            AssertHelper.That(migration.UserCode).IsValidCSharp();
        }

        [TestCase(TriggerAnnotation.AnnotationName, false)]
        [TestCase("other annotation", true)]
        public void GenerateShouldIgnoreWhenAnnotationNameIsNotAnnotationName(string annotation, bool expectedEmpty)
        {
            var builder = new StringBuilder();
            var writer = new IndentedTextWriter(new StringWriter(builder));

            this.generator.Generate(annotation, new TriggerAnnotation("name"), writer);

            Assert.That(builder.ToString().Length, expectedEmpty ? (IResolveConstraint) Is.EqualTo(0) : Is.GreaterThan(0));
        }
    }
}