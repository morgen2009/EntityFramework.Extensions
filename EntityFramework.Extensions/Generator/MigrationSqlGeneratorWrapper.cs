namespace EntityFramework.Extensions.Generator
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;
    using System.Linq;

    public class MigrationSqlGeneratorWrapper : MigrationSqlGenerator
    {
        private readonly MigrationSqlGenerator innerSqlGenerator;
        private readonly ICollection<IMigrationOperationInterceptor> migrationProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationSqlGeneratorWrapper"/> class.
        /// </summary>
        public MigrationSqlGeneratorWrapper(MigrationSqlGenerator innerSqlGenerator, ICollection<IMigrationOperationInterceptor> migrationProcessors)
        {
            this.innerSqlGenerator = innerSqlGenerator;
            this.migrationProcessors = migrationProcessors;
        }

        /// <inheritdoc />
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            var operations = migrationOperations as MigrationOperation[] ?? migrationOperations.ToArray();

            operations = this.migrationProcessors.Aggregate(operations, (current, generator) => generator.Process(current).ToArray());

            return this.innerSqlGenerator.Generate(operations, providerManifestToken);
        }
    }
}