namespace EntityFramework.Extensions.Tests.Generator
{
    using EntityFramework.Extensions.Generator;
    using EntityFramework.Extensions.Tests.Fixture;
    using NUnit.Framework;

    [TestFixture]
    public class JsonMetadataAnnotationSerializerTests
    {
        private JsonMetadataAnnotationSerializer serializer;

        [SetUp]
        public void SetUp()
        {
            this.serializer = new JsonMetadataAnnotationSerializer();
        }

        private static TestCaseData[] serializeCaseSource = {
            new TestCaseData(new UserEntity
                {
                    Id = 1,
                    Name = "Some name",
                    Type = UserTypeEnum.SuperAdmin
                },
                "{\"$type\":\"EntityFramework.Extensions.Tests.Fixture.UserEntity, EntityFramework.Extensions.Tests\",\"Id\":1,\"Name\":\"Some name\",\"Type\":2}"
            ),
            new TestCaseData(new UserEntity
                {
                    Id = 2,
                    Name = "Other",
                    Type = UserTypeEnum.Admin
                },
                "{\"$type\":\"EntityFramework.Extensions.Tests.Fixture.UserEntity, EntityFramework.Extensions.Tests\",\"Id\":2,\"Name\":\"Other\",\"Type\":1}"
            ),
        };

        [TestCaseSource(nameof(serializeCaseSource))]
        public void SerializeReturnStringWithSerializedObject(UserEntity entity, string code)
        {
            var actual = this.serializer.Serialize("annotation name", entity);

            Assert.AreEqual(code, actual);
        }

        [TestCaseSource(nameof(serializeCaseSource))]
        public void DeserializeReturnObject(UserEntity entity, string code)
        {
            var actual = this.serializer.Deserialize("annotation name", code);

            Assert.NotNull(actual as UserEntity);
            Assert.AreEqual(entity.Name, (actual as UserEntity).Name);
            Assert.AreEqual(entity.Id, (actual as UserEntity).Id);
            Assert.AreEqual(entity.Type, (actual as UserEntity).Type);
        }
    }
}