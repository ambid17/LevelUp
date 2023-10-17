using System.IO;
using Newtonsoft.Json;

namespace Minigames.Fight
{
    public static class FileUtils
    {
        public static T LoadFile<T>(string fileLocation)
        {
            if (File.Exists(fileLocation))
            {
                string fileData = File.ReadAllText(fileLocation);
                var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

                T toReturn = JsonConvert.DeserializeObject<T>(fileData, settings);
                return toReturn;
            }

            return default(T);
        }

        public static void SaveFile<T>(string filePath, T objectToSerialize)
        {
            var settings = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };

            string fileContent = JsonConvert.SerializeObject(objectToSerialize, settings);

            File.WriteAllText(filePath, fileContent);
        }
    }
}
