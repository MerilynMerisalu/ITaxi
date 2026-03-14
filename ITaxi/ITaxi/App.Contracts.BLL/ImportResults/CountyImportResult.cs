namespace App.Contracts.BLL.ImportResults
{
    public class CountyImportResult
    {
        public bool Success { get; set; }
        public bool CountryNotFound { get; set; }
        public bool ApiError { get; set; }
        public int CountOfImportedCounties { get; set; }
    }
}
