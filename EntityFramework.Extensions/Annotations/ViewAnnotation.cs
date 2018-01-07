namespace EntityFramework.Extensions.Annotations
{
    using System.IO;
    using EntityFramework.Extensions.Commons;

    public class ViewAnnotation
    {
        public const string AnnotationName = "view";

        public string Body { get; set; }

        /// <summary>
        /// Change sql script of view.
        /// </summary>
        public ViewAnnotation HasBody(string body)
        {
            this.Body = body;

            return this;
        }

        /// <summary>
        /// Change sql script of view.
        /// </summary>
        /// <param name="body">Base64encoded sql script.</param>
        public ViewAnnotation HasBodyEncoded(string body)
        {
            this.Body = body.FromBase64();

            return this;
        }

        /// <summary>
        /// Change sql script of view.
        /// </summary>
        /// <param name="stream">Stream containing sql script.</param>
        public ViewAnnotation HasBody(Stream stream)
        {
            this.Body = stream.ReadAsString();

            return this;
        }
    }
}