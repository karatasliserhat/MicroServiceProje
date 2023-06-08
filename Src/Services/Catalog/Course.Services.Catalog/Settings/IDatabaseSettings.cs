namespace Course.Services.Catalog.Settings
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
        string CategoryCollectionName { get; set; }
        string CourseCollectionName { get; set; }
    }
}
