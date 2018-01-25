namespace EntityFramework.Extensions.FunctionalTests.Fixture
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    public class DataContextTest : DbContext
    {
        public DataContextTest(DbCompiledModel model) : base(model)
        {
        }
    }
}