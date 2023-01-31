using Microsoft.AspNetCore.Hosting;
using System.Globalization;

namespace MyTrace.Utils
{
    public class Images
    {
        private string _pathImages;
        private const string urlImage = "public/images/";
        public Images(IWebHostEnvironment webHostEnvironment)
        {
            if (string.IsNullOrWhiteSpace(webHostEnvironment.WebRootPath))
            {
                webHostEnvironment.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            }
            _pathImages = Path.Combine(webHostEnvironment.WebRootPath ,"images");
        }

        public async Task<string?> saveImage(IFormFile image)
        {
            if (! Directory.Exists(_pathImages))
            {
                Directory.CreateDirectory(_pathImages);
            }

            string dateNow = DateTime.Now.ToString("dd-MM-yyyy", DateTimeFormatInfo.InvariantInfo);

            string fileName = $"{dateNow}_{Guid.NewGuid()}{Path.GetExtension(image.FileName).ToLower()}";

            using (var stream = System.IO.File.Create(Path.Combine(_pathImages, fileName)))
            {
                await image.CopyToAsync(stream);
            }

            return urlImage+fileName;
        }
    }
}
