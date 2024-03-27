using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;


namespace LawetaWeb
{
    public static class JsonFileUtils
    {
        private static readonly JsonSerializerOptions _options =
            new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                WriteIndented = true
            };

        public static void Write(object obj, string fileName)
        {
            using var fileStream = File.Create(fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(fileStream);
            
            JsonSerializer.Serialize(utf8JsonWriter, obj, _options);
        }
        public static async Task WriteAsync(object obj, string fileName)
        {
            await using var fileStream = File.Create(fileName);

            await JsonSerializer.SerializeAsync(fileStream, obj, _options);
        }
        public static void WriteDynamicJsonObject(JsonObject jsonObj, string fileName) 
        {
            using var fileStream = File.Create(fileName);
            using var utf8JsonWriter = new Utf8JsonWriter(fileStream);

            jsonObj.WriteTo(utf8JsonWriter);
        }

        public static T Read<T>(string filePath) 
        {            
            string fileContent = File.ReadAllText(filePath);
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(fileContent);
        }
    }
}
