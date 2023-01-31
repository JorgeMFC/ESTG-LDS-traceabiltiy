namespace MyTrace.Models
{
    public class ModelsRequest
    {
        public Model model { get; set; }

        public IFormFile? image { get; set; }

        public List<Color>colors { get; set; }

        public List<Size> sizes { get; set; }   

        public List<Component> components { get; set; } 

    }
}
