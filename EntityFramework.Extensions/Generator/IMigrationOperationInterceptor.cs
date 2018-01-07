namespace EntityFramework.Extensions.Generator
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;

    public interface IMigrationOperationInterceptor
    {
        /// <summary>
        /// Converts a set of migration operations into other set of migration operations.
        /// </summary>
        IEnumerable<MigrationOperation> Process(IEnumerable<MigrationOperation> operations);
    }
}