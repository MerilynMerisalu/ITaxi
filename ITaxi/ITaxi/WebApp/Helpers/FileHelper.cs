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

        public static void DeleteFile(string fullFilePath) 
            
        {
           File.Delete(fullFilePath);
        }

        public static string ReplaceUnderscoreWithSpaceInFileName(string fileName )
        {
            return fileName.Replace('_', ' ');
        }

        public static string ReplaceSpaceWithUnderscoreInFileName(string fileName)
        {  return fileName.Replace(" ",  "_");}
    }
}
