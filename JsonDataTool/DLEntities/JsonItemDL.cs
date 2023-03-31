using System;
using System.Xml.Serialization;

namespace JsonDataTool.DLEntities
{
    [Serializable]
    public class JsonItemDL
    {
        [XmlAttribute]
        public Guid Id { get; set; }

        [XmlAttribute]
        public string Name { get; set; }

        public string JsonValue { get; set; }
    }
}
