namespace EntityFramework.Extensions.Tests.Helper
{
    using System.CodeDom.Compiler;
    using System.Data.Entity.Migrations;
    using Microsoft.CSharp;
    using NUnit.Framework;

    public static class AssertHelper
    {
        public static AssertContainer<T> That<T>(T value)
        {
            return new AssertContainer<T>(value);
        }

        public static AssertContainer<string> IsValidCSharp(this AssertContainer<string> container)
        {
            var result = CompileCode(container.Value);

            Assert.IsEmpty(result.Errors, container.Value);

            return container;
        }

        public static CompilerResults CompileCode(string code)
        {
            CodeDomProvider cpd = new CSharpCodeProvider();
            var cp = new CompilerParameters();

            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add(typeof(ConfigurationExtensions).Assembly.Location);
            cp.ReferencedAssemblies.Add(typeof(DbMigration).Assembly.Location);
            cp.GenerateInMemory = true;
            cp.GenerateExecutable = false;

            var cr = cpd.CompileAssemblyFromSource(cp, code);

            return cr;
        }
    }
}