namespace MyTrace.Models
{
    public class ModelResponse
    {

        public Model Model { get; set; } = null!;

        public List<Color> colors { get; set; } = new List<Color>();

        public List<Size> sizes { get; set; } = new List<Size>();   
    }
}
