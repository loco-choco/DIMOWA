using System.IO;
using Newtonsoft.Json;

namespace IMOWA
{
    public class JsonReader
    {
        static public T ReadFromJson<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }

        static public void WriteToJson (object objectToSerialize, string filePath)
        {
            string fileData = JsonConvert.SerializeObject(objectToSerialize);
            StreamWriter file = File.CreateText(filePath);
            file.Write(fileData);
            file.Flush();
            file.Close();
        }
    }
    [JsonObject(MemberSerialization.OptIn)]
    public struct ModManifestJson
    {
        [JsonProperty(PropertyName = "name")] public string Name { get; set; }
        [JsonProperty(PropertyName =  "filename")] public string FileName { get; set; }
        [JsonProperty(PropertyName = "uniqueName")] public string UniqueName { get; set; }
        [JsonProperty(PropertyName = "version")] public string Version { get; set; }
        [JsonProperty(PropertyName = "author")] public string Author { get; set; }
        [JsonProperty(PropertyName = "description")] public string Description { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public struct ManagerConfig
    {
        [JsonProperty(PropertyName = "gameFolder")] public string GameFolder { get; set; }
        [JsonProperty(PropertyName = "modsFolder")] public string ModsFolder { get; set; }
    }
}
