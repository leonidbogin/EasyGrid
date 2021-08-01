using System.Xml;
using System.Xml.Serialization;
using EasyGrid.Core.Models.Gpx;

namespace EasyGrid.Core.Providers
{
    public class SaveGpxProvider
    {
        private readonly XmlWriterSettings xmlWriterSettings;

        public SaveGpxProvider()
        {
            xmlWriterSettings = new XmlWriterSettings { Indent = true };
        }

        public void Save(Gpx gpx, string path)
        {
            using var writer = XmlWriter.Create(path, xmlWriterSettings);
            var serializer = new XmlSerializer(typeof(Gpx));
            serializer.Serialize(writer, gpx);
        }
    }
}
