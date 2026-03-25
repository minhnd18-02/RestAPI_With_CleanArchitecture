using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.ProductModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.IServices
{
    public interface IProductService
    {
        Task<ServiceReponse<ProductResponse>> GetById(int id);
        Task<ServiceReponse<ProductResponse>> AddNew(ProductRequest productRequest);
        Task<ServiceReponse<ProductResponse>> Update(int id, ProductRequest productRequest);
        Task<ServiceReponse<int>> Delete(int id);
        Task<ServiceReponse<PaginationModel<ProductResponse>>> Search(int pageNumber, int pageSize, QueryProduct? query = null);
    }
}
