using JsonDataTool.Entities;
using JsonDataTool.Utils;
using System.Collections.Generic;
using System.Linq;
using JsonDataTool.DLEntities;

namespace JsonDataTool.Providers
{
    public static class DataProvider
    {
        private const string JsonDataFile = "jsonData.xml";

        public static void SaveJsonData(List<JsonItem> jsonItems)
        {
            var settings = LoadSettings();
            settings.JsonItems = jsonItems.Select(MapperProvider.Mapper.Map<JsonItemDL>).ToList();

            XmlOperations.SaveData(JsonDataFile, settings);
        }

        public static List<JsonItem> LoadJsonData()
        {
            var settings = LoadSettings();

            return settings.JsonItems.Select(MapperProvider.Mapper.Map<JsonItem>).ToList();
        }

        private static SettingsDL LoadSettings() => XmlOperations.LoadData<SettingsDL>(JsonDataFile);
    }
}
