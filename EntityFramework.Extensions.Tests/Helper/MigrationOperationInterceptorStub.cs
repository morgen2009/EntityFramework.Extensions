namespace EntityFramework.Extensions.Tests.Helper
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using EntityFramework.Extensions.Generator;

    public class MigrationOperationInterceptorStub : IMigrationOperationInterceptor
    {
        public ICollection<MigrationOperation> ModifiedOperations { get; set; } = new List<MigrationOperation>();

        public ICollection<MigrationOperation> LastOperations { get; } = new List<MigrationOperation>();

        /// <inheritdoc />
        public IEnumerable<MigrationOperation> Process(IEnumerable<MigrationOperation> operations)
        {
            foreach (var operation in operations)
            {
                this.LastOperations.Add(operation);
            }

            return this.ModifiedOperations;
        }
    }
}