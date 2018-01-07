namespace EntityFramework.Extensions.Commons
{
    using System;
    using System.IO;

    public static class StreamExtensions
    {
        public static string ReadAsString(this Stream stream)
        {
            Guard.NotNull(stream, nameof(stream));

            if (!stream.CanSeek && stream.Position > 0)
            {
                throw new ArgumentException("Stream is already opened");
            }

            if (stream.Position > 0)
            {
                stream.Position = 0;
            }

            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}