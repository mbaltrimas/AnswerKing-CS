using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Answer.King.Api.Services
{
    public interface IProductService
    {
        Task<Domain.Repositories.Models.Product> CreateProduct(RequestModels.Product createProduct);

        Task<Domain.Repositories.Models.Product> GetProduct(Guid productId);

        Task<IEnumerable<Domain.Repositories.Models.Product>> GetProducts();

        Task<IEnumerable<Domain.Repositories.Models.Product>> GetProducts(IEnumerable<Guid> productIds);

        Task<Domain.Repositories.Models.Product> RetireProduct(Guid productId);

        Task<Domain.Repositories.Models.Product> UpdateProduct(Guid productId, RequestModels.Product updateProduct);
    }
}