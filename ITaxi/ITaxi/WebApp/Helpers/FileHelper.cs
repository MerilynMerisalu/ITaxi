namespace WebApp.Helpers
{
    public static class FileHelper
    {
        public static string GetImageRelativePath(string path)
        {
            return $"\\{path}";
        }
        
        public static string GetFileFullPath(string fullFilePath, string fileNameOnDisk)
        {
            string fullPath = $"{fullFilePath}\\{fileNameOnDisk}";
            return fullPath;
        }
    }
}
