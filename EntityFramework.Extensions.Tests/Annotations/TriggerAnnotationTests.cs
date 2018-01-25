namespace EntityFramework.Extensions.Tests.Annotations
{
    using System.Data.Entity.Infrastructure.Annotations;
    using System.IO;
    using System.Text;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Commons;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TriggerAnnotationTests
    {
        private TriggerAnnotation annotation;
        private TriggerAnnotation otherAnnotation;

        [SetUp]
        public void SetUp()
        {
            this.annotation = new TriggerAnnotation("TR_TABLE_DEFAULT");
            this.otherAnnotation = new TriggerAnnotation("TR_TABLE_OTHER");
        }

        [Test]
        public void ClassShouldImplementInterface()
        {
            Assert.IsInstanceOf<IMergeableAnnotation>(this.annotation);
        }

        [TestCase("some name")]
        public void ConstructorShouldAddName(string triggerName)
        {
            var customAnnotation = new TriggerAnnotation(triggerName);

            Assert.AreEqual(triggerName,customAnnotation.Name);
        }

        [Test]
        public void IsCompatibleWithReturnPositiveCompatibilityResultWhenOtherIsTriggerAnnotation()
        {
            var actual = this.annotation.IsCompatibleWith(this.otherAnnotation);

            Assert.IsTrue(actual.IsCompatible);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsCompatibleWithShouldDelegateCallToOtherWhenOtherImplementsIMergeableAnnotation(bool isCompatible)
        {
            var other = Substitute.For<IMergeableAnnotation>();
            other.IsCompatibleWith(this.annotation).Returns(new CompatibilityResult(isCompatible, "some message"));

            var actual = this.annotation.IsCompatibleWith(other);

            Assert.AreEqual(isCompatible, actual.IsCompatible);
        }

        [Test]
        public void IsCompatibleWithReturnNegativeCompatibilityResultWhenObjectCanNotBeMerged()
        {
            var actual = this.annotation.IsCompatibleWith("string annotation");

            Assert.IsFalse(actual.IsCompatible);
        }

        [Test]
        public void MergeWithReturnMultipleTriggerAnnotationWhenObjectIsTriggerAnnotation()
        {
            var actual = this.annotation.MergeWith(this.otherAnnotation) as MultipleTriggerAnnotation;

            Assert.NotNull(actual);
            Assert.AreEqual(2, actual.Triggers.Count);
            Assert.IsTrue(actual.Triggers.Contains(this.annotation));
            Assert.IsTrue(actual.Triggers.Contains(this.otherAnnotation));
        }

        [Test]
        public void MergeWithShouldDelegateWhenObjectIsMergeableAnnotation()
        {
            var other = Substitute.For<IMergeableAnnotation>();
            var resultAnnotation = "result annotation";
            other.MergeWith(this.annotation).Returns(resultAnnotation);

            var actual = this.annotation.MergeWith(other);

            Assert.AreSame(resultAnnotation, actual);
        }

        [TestCase(TriggerEventEnum.Insert)]
        public void AfterShouldChangeTriggerTypeToAfterAndTriggerEvents(TriggerEventEnum triggerEventEnum)
        {
            this.annotation.After(triggerEventEnum);

            Assert.AreEqual(TriggerTypeEnum.After, this.annotation.TriggerType);
            Assert.AreEqual(triggerEventEnum, this.annotation.TriggerEvents);
        }

        [TestCase(TriggerEventEnum.Delete)]
        public void InsteadOfShouldChangeTriggerTypeToAfterAndTriggerEvents(TriggerEventEnum triggerEventEnum)
        {
            this.annotation.InsteadOf(triggerEventEnum);

            Assert.AreEqual(TriggerTypeEnum.InsteadOf, this.annotation.TriggerType);
            Assert.AreEqual(triggerEventEnum, this.annotation.TriggerEvents);
        }

        [TestCase("some sql script")]
        public void HasBodyShouldChangeScript(string sqlScript)
        {
            this.annotation.HasBody(sqlScript);

            Assert.AreEqual(sqlScript, this.annotation.Body);
        }


        [TestCase("some sql script")]
        public void HasBodyDecodedShouldChangeScriptWithDecodedValue(string sqlScript)
        {
            this.annotation.HasBodyEncoded(sqlScript.ToBase64());

            Assert.AreEqual(sqlScript, this.annotation.Body);
        }

        [TestCase("some sql script")]
        public void HasBodyShouldChangeScriptFromStream(string sqlScript)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(sqlScript));

            this.annotation.HasBody(stream);

            Assert.AreEqual(sqlScript, this.annotation.Body);
        }
    }
}