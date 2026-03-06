using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace App.BLL.Services.Helpers
{
    public static class PhotoHelper
    {
        public readonly record struct ImageSize(int Width, int Height);

        public static Task<Image> GetImage(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            return Image.LoadAsync(stream);
        }
        public static async Task<ImageSize> GetImageSize(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            using var image = await Image.LoadAsync(stream);
            return new ImageSize(image.Width, image.Height);
        }
    }
}
