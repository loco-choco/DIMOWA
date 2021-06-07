using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Xml;
using System.Xml.Linq;

namespace IMOWA
{
    public class JsonReader
    {
        private XmlDictionaryReader jsonFile;
        public XElement RootElement { get; private set; }

        public JsonReader(string filePath)
        {
            jsonFile = JsonReaderWriterFactory.CreateJsonReader(File.ReadAllBytes(filePath), new XmlDictionaryReaderQuotas());
            RootElement = XElement.Load(jsonFile);
        }

        public XElement GetJsonElementFromRoot(string elementName)
        {
            return RootElement.Element(XName.Get(elementName));
        }

        public XElement GetJsonElement(XElement element, string childElementName)
        {
            return element.Element(XName.Get(childElementName));
        }
    }
}
