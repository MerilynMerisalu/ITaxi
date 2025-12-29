namespace WebApp.Helpers
{
    public static class DirectoryHelper
    {
        public static void DeleteDirectory(string path, bool recursiveDelete = true)
        {
            if (!Directory.Exists(path))
            {
               return;
            }
            Directory.Delete(path, recursiveDelete);
        }

       
    }
}
