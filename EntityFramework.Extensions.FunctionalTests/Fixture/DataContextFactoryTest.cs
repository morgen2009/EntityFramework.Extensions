namespace EntityFramework.Extensions.FunctionalTests.Fixture
{
    using System.Data.Entity.Infrastructure;

    public class DataContextFactoryTest : IDbContextFactory<DataContextTest>
    {
        public static DbCompiledModel Model { get; set; }

        /// <inheritdoc />
        public DataContextTest Create()
        {
            return new DataContextTest(Model);
        }
    }
}