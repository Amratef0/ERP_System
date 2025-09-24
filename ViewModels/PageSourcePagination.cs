namespace ERP_System_Project.ViewModels
{
    public class PageSourcePagination<TEntity> where TEntity : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public int TotalPages { get; set; }
        public List<TEntity>? Data { get; set; }
    }
}
