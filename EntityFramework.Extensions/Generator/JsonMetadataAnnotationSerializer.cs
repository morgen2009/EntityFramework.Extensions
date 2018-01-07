namespace EntityFramework.Extensions.Generator
{
    using System.Data.Entity.Infrastructure;
    using Newtonsoft.Json;

    public class JsonMetadataAnnotationSerializer : IMetadataAnnotationSerializer
    {
        /// <inheritdoc />
        public string Serialize(string name, object value)
        {
            var result = JsonConvert.SerializeObject(value, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            return result;
        }

        /// <inheritdoc />
        public object Deserialize(string name, string value)
        {
            var result = JsonConvert.DeserializeObject(value, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Objects
            });

            return result;
        }
    }
}