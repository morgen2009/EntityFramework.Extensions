namespace EntityFramework.Extensions.Tests.Annotations
{
    using System.IO;
    using System.Text;
    using EntityFramework.Extensions.Annotations;
    using EntityFramework.Extensions.Commons;
    using NUnit.Framework;

    [TestFixture]
    public class ViewAnnotationTests
    {
        [TestCase("sql script")]
        public void HasBodyShouldChangeBody(string script)
        {
            var annotation = new ViewAnnotation();

            var actual = annotation.HasBody(script);

            Assert.AreSame(annotation, actual);
            Assert.AreEqual(script, actual.Body);
        }

        [TestCase("sql script")]
        public void HasBodyShouldChangeBodyFromStream(string script)
        {
            var annotation = new ViewAnnotation();

            var actual = annotation.HasBody(new MemoryStream(Encoding.UTF8.GetBytes(script)));

            Assert.AreSame(annotation, actual);
            Assert.AreEqual(script, actual.Body);
        }

        [TestCase("sql script")]
        public void HasBodyEncodedShouldDecodeAndChangeBody(string script)
        {
            var annotation = new ViewAnnotation();

            var actual = annotation.HasBodyEncoded(script.ToBase64());

            Assert.AreSame(annotation, actual);
            Assert.AreEqual(script, actual.Body);
        }
    }
}