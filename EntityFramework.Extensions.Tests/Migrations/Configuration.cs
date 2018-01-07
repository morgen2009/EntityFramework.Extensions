namespace EntityFramework.Extensions.Tests.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    internal sealed class Configuration : DbMigrationsConfiguration<Fixture.DataContextTest>
    {
        public Configuration()
        {
            AppDomain.CurrentDomain.SetData("DataDirectory", AppDomain.CurrentDomain.BaseDirectory);

            this.AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Fixture.DataContextTest contextHelper)
        {

        }
    }
}