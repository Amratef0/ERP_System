using ERP_System_Project.DTOs;
using ERP_System_Project.Repository.Interfaces;

namespace ERP_System_Project.Helpers
{
    public static class Pagination<TEntity> where TEntity : class
    {
        public static async Task<PageSourcePagination<TEntity>> GetPaginatedData(
            IRepository<TEntity> repository,
            int pageNumber = 1, int pageSize = 10
            )
        {
            if(pageNumber < 1) pageNumber = 1;
            if(pageSize < 10) pageSize = 10;

            var result = await repository
                .SkipAndTake((pageNumber - 1 ) * pageSize , pageSize);

            var totalRecords = await repository.Count();
            var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

            return new PageSourcePagination<TEntity>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages =  totalPages,
                Data = result
            };
        }
    }
}
