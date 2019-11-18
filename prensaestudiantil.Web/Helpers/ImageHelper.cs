using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace prensaestudiantil.Web.Helpers
{
    public class ImageHelper : IImageHelper
    {
        public async Task<string> UploadImageAsync(IFormFile imageFile)
        {
            string guid = Guid.NewGuid().ToString();
            string file = $"{guid}{imageFile.FileName}";
            string path = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot\\images\\Publications",
                $"{file}");

            using (FileStream stream = new FileStream(path, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"~/images/Properties/{file}";
        }
    }
}
