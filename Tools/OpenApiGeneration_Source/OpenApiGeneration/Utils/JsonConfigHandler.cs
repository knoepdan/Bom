using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OpenApiGeneration.Utils
{
    public static class JsonConfigHandler
    {
        public static T GetObjectFromJsonFile<T>(string file)
        {
            if (!System.IO.File.Exists(file))
            {
                throw new ArgumentNullException("file does not exist" + file, nameof(file));
            }
            string json = System.IO.File.ReadAllText(file);

            var deserialized = GetObjectFromJsonString<T>(json);
            return deserialized;
        }


        public static T GetObjectFromJsonString<T>(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<T>(json);
            return deserialized;
        }

        public static string GetJson(object ob)
        {
            var output = JsonConvert.SerializeObject(ob, Formatting.Indented);
            return output;
        }
        public static void SaveToJsonFile(object ob, string filePath)
        {
            var json = GetJson(ob);
            System.IO.File.WriteAllText(filePath, json);
        }

        private static string FixJsonFilePath(string filepath)
        {
            if (!string.IsNullOrEmpty(filepath))
            {
                filepath = filepath.Replace("\\", "/");
            }
            return filepath;
        }
    }
}
