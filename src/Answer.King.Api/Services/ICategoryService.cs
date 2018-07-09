using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Answer.King.Api.RequestModels;
using Answer.King.Domain.Inventory;

namespace Answer.King.Api.Services
{
    public interface ICategoryService
    {
        Task<Domain.Inventory.Category> CreateCategory(RequestModels.Category createCategory);
        Task<IEnumerable<Domain.Inventory.Category>> GetCategories();
        Task<Domain.Inventory.Category> GetCategory(Guid categoryId);
        Task<Domain.Inventory.Category> RetireCategory(Guid categoryId);
        Task<Domain.Inventory.Category> UpdateCategory(Guid categoryId, RequestModels.Category updateCategory);
    }
}