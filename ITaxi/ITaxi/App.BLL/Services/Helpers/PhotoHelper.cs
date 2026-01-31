using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Services.Helpers
{
    public class PhotoHelper
    {
        private readonly record struct ImageSize(int Width, int Height);

        private static async Task<ImageSize> GetImageSize(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync(stream);

            return new ImageSize(image.Width, image.Height);
        }
    }
}
