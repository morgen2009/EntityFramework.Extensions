namespace EntityFramework.Extensions.Tests.Commons
{
    using System;
    using System.IO;
    using System.Text;
    using EntityFramework.Extensions.Commons;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class StreamExtensionsTests
    {
        [TestCase("some text", "some text")]
        [TestCase("other text", "other text")]
        public void ReadAsStringReturnStreamContent(string text, string expected)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));

            var actual = stream.ReadAsString();

            Assert.AreEqual(expected, actual);
        }

        [TestCase("some text", 0)]
        [TestCase("other text", 1)]
        public void ReadAsStringShouldRewindStream(string text, int position)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            stream.Position = position;

            var actual = stream.ReadAsString();

            Assert.AreEqual(text, actual);
        }

        [Test]
        public void ReadAsStringShouldThrowExceptionWhenRewindNotAllowed()
        {
            var stream = Substitute.For<Stream>();
            stream.CanSeek.Returns(false);
            stream.Position = 1;

            TestDelegate action = () => stream.ReadAsString();

            Assert.Throws<ArgumentException>(action);
        }
    }
}