namespace EntityFramework.Extensions.Tests.Generator.Sql.View
{
    using System.Collections.Generic;
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Data.Entity.Migrations.Model;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Commons;
    using EntityFramework.Extensions.Generator.Sql.View;
    using EntityFramework.Extensions.Tests.Helper;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ViewMigrationInterceptorTests
    {
        private ViewMigrationInterceptor interceptor;
        private IViewSqlGenerator sqlGenerator;

        [SetUp]
        public void SetUp()
        {
            this.sqlGenerator = Substitute.For<IViewSqlGenerator>();
            this.interceptor = new ViewMigrationInterceptor(this.sqlGenerator);
        }

        [Test]
        public void ProcessShouldReplaceCreateTableOperationWithCreateViewOperation()
        {
            var migrationOperation = new MigrationOperationHelper();
            var viewAnnotation = new ViewAnnotation().HasBody("sql");
            var viewName = "ViewName";
            this.sqlGenerator.CreateView(viewName, viewAnnotation).Returns(migrationOperation);
            var operation = new CreateTableOperation(viewName)
            {
                Annotations = { { ViewAnnotation.AnnotationName, viewAnnotation } }
            };

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(migrationOperation, actual.Single());
        }

        [Test]
        public void ProcessShouldReplaceDropTableOperationWithDropViewOperation()
        {
            var migrationOperation = new MigrationOperationHelper();
            var viewName = "ViewName";
            var viewAnnotation = new ViewAnnotation().HasBody(EncodingUtils.ToBase64("sql"));
            this.sqlGenerator.DropView(viewName, viewAnnotation).Returns(migrationOperation);
            var operation = new DropTableOperation(viewName)
            {
                RemovedAnnotations = { { ViewAnnotation.AnnotationName, viewAnnotation } }
            };

            var actual = this.interceptor.Process(new[] { operation });

            Assert.AreEqual(migrationOperation, actual.Single());
        }

        [Test]
        public void ProcessShouldReplaceAlterTableOperationWithAlterViewOperation()
        {
            var migrationOperation = new MigrationOperationHelper();
            var viewName = "ViewName";
            var oldAnnotation = new ViewAnnotation().HasBody("sql1");
            var newAnnotation = new ViewAnnotation().HasBody("sql2");
            this.sqlGenerator.AlterView(viewName, oldAnnotation, newAnnotation).Returns(migrationOperation);
            var operation = new AlterTableOperation(viewName,
                new Dictionary<string, AnnotationValues>
                {
                    {
                        ViewAnnotation.AnnotationName,
                        new AnnotationValues(oldAnnotation, newAnnotation)
                    }
                });

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(migrationOperation, actual.Single());
        }

        [Test]
        public void ProcessShouldReplaceRenameTableOperationWithRenameViewOperation()
        {
            var migrationOperation = new MigrationOperationHelper();
            var viewName = "ViewName1";
            var viewNameNew = "ViewName2";
            this.sqlGenerator.RenameView(viewName, viewNameNew).Returns(migrationOperation);
            var operation = new RenameTableOperation(viewName, viewNameNew);

            var actual = this.interceptor.Process(new[] { operation }).ToArray();

            Assert.AreEqual(migrationOperation, actual.Single());
        }

        [Test]
        public void ProcessShouldPassMigrationOperationWithoutAnnotation()
        {
            var operation = new CreateTableOperation("UserView");

            var actual = this.interceptor.Process(new[] { operation });

            Assert.AreSame(operation, actual.Single());
        }
    }
}