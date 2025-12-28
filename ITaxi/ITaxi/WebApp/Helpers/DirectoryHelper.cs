namespace WebApp.Helpers
{
    public static class DirectoryHelper
    {
        public static void DeleteDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
               return;
            }
            Directory.Delete(path, true);
        }

       
    }
}
