using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Opener.Helpers
{
    public class XMLSerializerNDeSerializer
    {
        public string Serialize<T>(T obj, XmlSerializerNamespaces namespaces = null, XmlWriterSettings settings = null) where T : class
        {
            if (namespaces == null)
                namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
            var serializer = new XmlSerializer(typeof(T));
            if (settings == null)
            {
                settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.OmitXmlDeclaration = false;
            }
            using (var stream = new StringWriter())
            using (var writer = XmlWriter.Create(stream, settings))
            {
                serializer.Serialize(writer, obj, namespaces);
                return stream.ToString();
            }
        }
        public T DeSerialize<T>(string XmlData)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T newClass;
            using (MemoryStream stream = new MemoryStream(UTF8Encoding.Unicode.GetBytes(XmlData)))
            {
                newClass = (T)serializer.Deserialize(stream);
            }
            return newClass;
        }
    }
}
