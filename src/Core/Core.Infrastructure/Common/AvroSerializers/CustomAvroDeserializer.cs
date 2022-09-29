using Avro.IO;
using Avro.Specific;
using Confluent.Kafka;

namespace Core.Infrastructure.Common.AvroSerializers;

public class CustomAvroDeserializer<T> : IDeserializer<T> where T : class, ISpecificRecord
{
    public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
    {
        using (var ms = new MemoryStream(data.ToArray()))
        {
            var dec = new BinaryDecoder(ms);
            var regenObj = (T)Activator.CreateInstance(typeof(T));

            var reader = new SpecificDefaultReader(regenObj.Schema, regenObj.Schema);
            reader.Read(regenObj, dec);
            return regenObj;
        }
    }
}