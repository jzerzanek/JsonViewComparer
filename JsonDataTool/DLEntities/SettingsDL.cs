using System.Collections.Generic;
using System.Xml.Serialization;

namespace JsonDataTool.DLEntities
{
    [XmlRoot("Settings")]
    public class SettingsDL
    {
        [XmlArrayItem("JsonItem")]
        public List<JsonItemDL> JsonItems { get; set; } = new();
    }
}
