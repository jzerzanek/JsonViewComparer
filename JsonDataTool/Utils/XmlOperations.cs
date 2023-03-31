using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace JsonDataTool.Utils
{
    internal static class XmlOperations
    {
        public static T LoadData<T>(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name can not be empty.", nameof(fileName));
            }

            if (!File.Exists(fileName))
            {
                return Activator.CreateInstance<T>();
            }

            try
            {
                var xs = new XmlSerializer(typeof(T));

                using (var fs = new FileStream(fileName, FileMode.OpenOrCreate))
                {
                    return (fs.Length == 0 
                        ? Activator.CreateInstance<T>() 
                        : (T)xs.Deserialize(fs));
                }
            }
            catch (Exception exception)
            {
                return Activator.CreateInstance<T>();
            }
        }

        public static void SaveData<T>(string fileName, [NotNull] T data)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name can not be empty.", nameof(fileName));
            }

            var xs = new XmlSerializer(data!.GetType());

            using (var writeFileStream = XmlWriter.Create(fileName))
            {
                xs.Serialize(writeFileStream, data);
            }
        }
    }
}
