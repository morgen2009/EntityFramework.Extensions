namespace EntityFramework.Extensions.Tests.Helper
{
    using System.Data.Entity.Migrations.Model;

    public class MigrationOperationHelper : MigrationOperation
    {
        /// <inheritdoc />
        public MigrationOperationHelper() : base(null)
        {
            this.IsDestructiveChange = false;
        }

        /// <inheritdoc />
        public override bool IsDestructiveChange { get; }
    }
}