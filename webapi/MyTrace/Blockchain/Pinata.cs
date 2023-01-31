using System.Text.Json;
using System.Text.Json.Nodes;

namespace MyTrace.Blockchain
{
    public class Pinata
    {
        public static async Task<string?> CreateNFT(String filePath, PinataEnv pinataEnv)
        {
            using (var httpClient = new HttpClient())
            {
                var fileName = Path.GetFileName(filePath);
                if (fileName == null)
                {
                    return null;
                }
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.pinata.cloud/pinning/pinFileToIPFS"))
                {
                    var fileNameWithoutExtension = fileName;
                    int fileExtPos = fileNameWithoutExtension.LastIndexOf(".");
                    if (fileExtPos >= 0)
                        fileNameWithoutExtension = fileNameWithoutExtension.Substring(0, fileExtPos);

                    request.Headers.TryAddWithoutValidation("pinata_api_key", pinataEnv.ApiKey);
                    request.Headers.TryAddWithoutValidation("pinata_secret_api_key", pinataEnv.APISecret);

                    JsonObject pinataOptions = new()
                    {
                        ["cidVersion"] = 1,
                    };
                    JsonObject pinataMetadata = new()
                    {
                        ["name"] = fileNameWithoutExtension,
                        ["keyvalues"] = new JsonObject
                        {
                            ["company"] = "MyTrace",

                        },

                    };

                    var multipartContent = new MultipartFormDataContent();
                    multipartContent.Add(new ByteArrayContent(File.ReadAllBytes(filePath)), "file", fileName);
                    multipartContent.Add(new StringContent(pinataOptions.ToString()), "pinataOptions");
                    multipartContent.Add(new StringContent(pinataMetadata.ToString()), "pinataMetadata");
                    request.Content = multipartContent;

                    try
                    {
                        using HttpResponseMessage response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                        using HttpContent content = response.Content;
                        string json = content.ReadAsStringAsync().GetAwaiter().GetResult().ToString();
                        PinataResponse? pinataResponse = JsonSerializer.Deserialize<PinataResponse>(json, new JsonSerializerOptions
                        {
                            PropertyNameCaseInsensitive = true
                        });
                        return pinataResponse?.IpfsHash;
                    }
                    catch (Exception e)
                    {
                        return null;
                    }
                }
            }
        }
    }
}
