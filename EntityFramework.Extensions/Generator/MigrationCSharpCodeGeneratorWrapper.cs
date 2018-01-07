namespace EntityFramework.Extensions.Generator
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Model;
    using System.Linq;

    public class MigrationCSharpCodeGeneratorWrapper : CSharpMigrationCodeGenerator
    {
        private readonly MigrationCodeGenerator innerCodeGenerator;
        private readonly ICollection<IMigrationOperationInterceptor> migrationProcessors;

        /// <summary>
        /// Initializes a new instance of the <see cref="MigrationCSharpCodeGeneratorWrapper"/> class.
        /// </summary>
        public MigrationCSharpCodeGeneratorWrapper(MigrationCodeGenerator innerCodeGenerator, ICollection<IMigrationOperationInterceptor> migrationProcessors)
        {
            this.innerCodeGenerator = innerCodeGenerator;
            this.migrationProcessors = migrationProcessors;
        }
        
        /// <inheritdoc />
        public override ScaffoldedMigration Generate(string migrationId, IEnumerable<MigrationOperation> migrationOperations, string sourceModel, string targetModel, string @namespace, string className)
        {
            var operations = migrationOperations as MigrationOperation[] ?? migrationOperations.ToArray();

            operations = this.migrationProcessors.Aggregate(operations, (current, generator) => generator.Process(current).ToArray());

            return this.innerCodeGenerator.Generate(migrationId, operations, sourceModel, targetModel, @namespace, className);
        }
    }
}