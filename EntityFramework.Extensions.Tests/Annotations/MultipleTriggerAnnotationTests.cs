namespace EntityFramework.Extensions.Tests.Annotations
{
    using System.Data.Entity.Infrastructure.Annotations;
    using System.Linq;
    using EntityFramework.Extensions.Annotations;
    using NUnit.Framework;

    [TestFixture]
    public class MultipleTriggerAnnotationTests
    {
        private TriggerAnnotation singleAnnotation;
        private MultipleTriggerAnnotation annotation;

        [SetUp]
        public void SetUp()
        {
            this.singleAnnotation = new TriggerAnnotation("TR_TABLE_DEFAULT");
            this.annotation = new MultipleTriggerAnnotation(new []{ this.singleAnnotation });
        }

        [Test]
        public void ClassShouldImplementInterface()
        {
            Assert.IsInstanceOf<IMergeableAnnotation>(this.annotation);
        }

        [Test]
        public void ConstructorShouldAddTriggerAnnotationIntoTriggersCollection()
        {
            Assert.AreSame(this.singleAnnotation, this.annotation.Triggers.Single());
        }

        [Test]
        public void ImplicitOperatorMultipleTriggerAnnotationShouldReturnMultipleTriggerAnnotation()
        {
            var actual = (MultipleTriggerAnnotation)this.singleAnnotation;

            Assert.AreSame(this.singleAnnotation, actual.Triggers.Single());
        }

        [Test]
        public void IsCompatibleWithReturnPositiveCompatibilityResultWhenObjectIsTriggerAnnotation()
        {
            var actual = this.annotation.IsCompatibleWith(this.singleAnnotation);

            Assert.IsTrue(actual.IsCompatible);
        }

        [Test]
        public void IsCompatibleWithReturnPositiveCompatibilityResultWhenObjectIsMultipleTriggerAnnotation()
        {
            var actual = this.annotation.IsCompatibleWith(this.annotation);
                
            Assert.IsTrue(actual.IsCompatible);
        }

        [Test]
        public void IsCompatibleWithReturnNegativeCompatibilityResultWhenObjectCanNotBeMerged()
        {
            var actual = this.annotation.IsCompatibleWith("string annotation");

            Assert.IsFalse(actual.IsCompatible);
        }

        [Test]
        public void MergeWithReturnMultipleTriggerAnnotationWhenOtherIsTriggerAnnotation()
        {
            var other = new TriggerAnnotation("TR_OTHER");

            var actual = this.annotation.MergeWith(other) as MultipleTriggerAnnotation;

            Assert.NotNull(actual);
            Assert.AreEqual(2, actual.Triggers.Count);
            Assert.That(this.annotation.Triggers, Is.SubsetOf(actual.Triggers));
            Assert.That(new[] { other }, Is.SubsetOf(actual.Triggers));
        }

        [Test]
        public void MergeWithReturnMultipleTriggerAnnotationWhenOtherIsMultipleTriggerAnnotation()
        {
            var other = new MultipleTriggerAnnotation(new []{ new TriggerAnnotation("TR_OTHER") });

            var actual = this.annotation.MergeWith(other) as MultipleTriggerAnnotation;

            Assert.NotNull(actual);
            Assert.AreEqual(2, actual.Triggers.Count);
            Assert.That(this.annotation.Triggers, Is.SubsetOf(actual.Triggers));
            Assert.That(other.Triggers, Is.SubsetOf(actual.Triggers));
        }
    }
}