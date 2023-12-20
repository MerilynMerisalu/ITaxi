using Microsoft.AspNetCore.Components.Forms;
using static MudBlazor.CategoryTypes;

namespace Webapp.Helpers
{
    public class FileUploadViewModel
    {
        public string Name { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public string GetDataUri() =>
            $"data:{ContentType};base64,{Convert.ToBase64String(Data)}";


        public static async Task<FileUploadViewModel> FromIBrowserFile(IBrowserFile file,
            int maxFileSize)
        {
            using var memoryStream = new MemoryStream((int)file.Size);
            await file.OpenReadStream(maxFileSize).CopyToAsync(memoryStream);

           var model = new FileUploadViewModel
            {
                ContentType = file.ContentType,
                Name = file.Name,
                Data = memoryStream.ToArray(),
            };

            return model;
        }
    }
}
