namespace WebApp.Helpers
{
    public static class FileHelper
    {
        public static string GetImageRelativePath(string path)
        {
            return $"\\{path}";
        }
    }
}
