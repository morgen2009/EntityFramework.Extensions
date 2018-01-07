namespace EntityFramework.Extensions.Generator.CSharpCode
{
    using System.Data.Entity.Migrations.Design;
    using System.Data.Entity.Migrations.Model;
    using System.Data.Entity.Migrations.Utilities;

    public class CSharpMigrationCodeGeneratorWrapper : CSharpMigrationCodeGenerator
    {
        /// <inheritdoc />
        protected override void Generate(CreateTableOperation createTableOperation, IndentedTextWriter writer)
        {
            base.Generate(createTableOperation, writer);
        }
    }
}