using MyTrace.Utils;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyTrace.Models
{
    public class RegistRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(JsonDateConverterExtension))]
        public DateTime? BirthDate { get; set; }
        public IFormFile? avatar { get; set; }
    }
}
