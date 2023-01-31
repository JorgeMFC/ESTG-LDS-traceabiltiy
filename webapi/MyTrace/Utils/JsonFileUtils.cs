using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;
using System.Text.Json;
using System.Text.Json;

namespace MyTrace.Utils
{
    public class JsonFileUtils
    {
        public static FileResult OpenFile(string path)
        {
            var stream = File.OpenRead(path);
            var file = new FileStreamResult(stream, "application/octet-stream");
            return file;

        }

        public static async Task WriteObjectInFileAsync(string fileName, JsonObject jsonObj)
        {
            await File.WriteAllTextAsync(fileName, JsonPrettify(jsonObj));
        }

        public static void DeleteFile(string fileName)
        {
            File.Delete(fileName);
        }

    public static string JsonPrettify(JsonObject jsonObj)
    {
        return JsonSerializer.Serialize(jsonObj, new JsonSerializerOptions { WriteIndented = true });
    }
}

}
