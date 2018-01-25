namespace EntityFramework.Extensions.Tests.Helper
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Sql;

    public class MigrationSqlGeneratorStub : MigrationSqlGenerator
    {
        public ICollection<MigrationStatement> GeneratedStatements { get; set; } = new List<MigrationStatement>();

        public ICollection<MigrationOperation> LastOperations { get; } = new List<MigrationOperation>();

        public string LastProviderManifestToken { get; set; }

        /// <inheritdoc />
        public override IEnumerable<MigrationStatement> Generate(IEnumerable<MigrationOperation> migrationOperations, string providerManifestToken)
        {
            this.LastProviderManifestToken = providerManifestToken;

            foreach (var operation in migrationOperations)
            {
                this.LastOperations.Add(operation);
            }

            return this.GeneratedStatements;
        }
    }
}