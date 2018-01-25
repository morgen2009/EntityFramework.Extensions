namespace EntityFramework.Extensions.FunctionalTests.Fixture
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Infrastructure.DependencyResolution;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Generator;

    public class DbConfigurationTest : DbConfiguration
    {
        public DbConfigurationTest()
        {
            this.SetMetadataAnnotationSerializer(ViewAnnotation.AnnotationName, () => new JsonMetadataAnnotationSerializer());
            this.SetMetadataAnnotationSerializer(TriggerAnnotation.AnnotationName, () => new JsonMetadataAnnotationSerializer());
            this.SetMetadataAnnotationSerializer(ValueConstraintAnnotation.AnnotationName, () => new JsonMetadataAnnotationSerializer());
            this.AddDependencyResolver(new SingletonDependencyResolver<IDbContextFactory<DataContextTest>>(new DataContextFactoryTest()));
        }
    }
}