namespace App.Contracts.BLL.ImportResults
{
    public class EhakImportResult
    {
        public bool Success { get; set; }
        public bool ParentUnitNotFound { get; set; }
        public bool ApiError { get; set; }
        public int CountOfImportedEntities { get; set; }
    }
}
