using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestAPIService.Application.IServices;
using RestAPIService.Application.ViewModels;
using RestAPIService.Application.ViewModels.ProductModel;
using RestAPIService.Domain.Entities;
using RestAPIService.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestAPIService.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public async Task<ServiceReponse<ProductResponse>> AddNew(ProductRequest productRequest)
        {
           var response = new ServiceReponse<ProductResponse>();
            try
            {
                if (productRequest == null)
                {
                    response.Success = false;
                    response.Message = "Not provided.";
                    return response;
                }
                var newProduct = _mapper.Map<Product>(productRequest);
                await _unitOfWork.ProductRepo.AddAsync(newProduct);

                var product = await _unitOfWork.ProductRepo.GetById(newProduct.Id);

                response.Success = true;
                response.Message = "Product added successfuly";
                response.Data = _mapper.Map<ProductResponse>(product);

            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Success = false;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<int>> Delete(int id)
        {
            var response = new ServiceReponse<int>();
            try
            {
                var check = await _unitOfWork.ProductRepo.FindEntityAsync(p  => p.Id == id);
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "ProductId not found";
                    return response;
                }

                await _unitOfWork.ProductRepo.RemoveAsync(check);
                
                response.Success = true;
                response.Message = "Product deleted successfully";
                response.Data = check.Id;

            } catch (Exception ex)
            {
                response.Success = false;
                response.Message= ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<ProductResponse>> GetById(int id)
        {
            var response = new ServiceReponse<ProductResponse>();
            try
            {
                var check = await _unitOfWork.ProductRepo.FindEntityAsync(p => p.Id == id);
                if (check == null)
                {
                    response.Success= false;
                    response.Message = "Product not found";
                    return response;
                }

                var product = await _unitOfWork.ProductRepo.GetById(id);

                response.Success = true;
                response.Message = "Product retrive successfully";
                response.Data = _mapper.Map<ProductResponse>(product);

            }catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }

        public async Task<ServiceReponse<PaginationModel<ProductResponse>>> Search(int pageNumber, int pageSize, QueryProduct? query = null)
        {
            var response = new ServiceReponse<PaginationModel<ProductResponse>>();
            try
            {
                if (query != null)
                {
                    var validationContext = new ValidationContext(query);
                    var validationResults = new List<ValidationResult>();

                    if (!Validator.TryValidateObject(query, validationContext, validationResults, true))
                    {
                        var errorMessages = validationResults.Select(r => r.ErrorMessage);
                        response.Success = false;
                        response.Message = string.Join("; ", errorMessages);
                        return response;
                    }

                    if (query.MaxPrice != null &&
                        query.MinPrice != null &&
                        query.MaxPrice < query.MinPrice)
                    {
                        response.Success = false;
                        response.Message = "Invalid range for Price.";
                        return response;
                    }
                }

                var products = await Filter(query);

                if (products != null && products.Any())
                {
                    if (pageNumber <= 0)
                    {
                        pageNumber = 1;
                    }
                    if (pageSize <= 0)
                    {
                        pageSize = 5;
                    }

                    var totalRecords = await products.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize);

                    var pagedProducts = products
                        .Skip((pageNumber - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var paginationModel = new PaginationModel<ProductResponse>
                    {
                        Page = pageNumber,
                        TotalPage = totalPages,
                        TotalRecords = totalRecords,
                        ListData = _mapper.Map<IEnumerable<ProductResponse>>(pagedProducts)
                    };

                    response.Success = true;
                    response.Message = "Products retrieved successfully";
                    response.Data = paginationModel;
                }
                else
                {
                    response.Success = false;
                    response.Message = "Products not found";
                }
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Error = ex.Message;
                response.ErrorMessages = new List<string> { ex.ToString() };
            }
            return response;
        }

        private async Task<IQueryable<Product>> Filter(QueryProduct? queries = null)
        {
            var query = _unitOfWork.ProductRepo.GetAs();

            if (queries != null)
            {
                if (!string.IsNullOrWhiteSpace(queries.CategoryName))
                {
                    var name = queries.CategoryName.ToLower().Trim();
                    query = query.Where(p => p.Category.Name.ToLower().Trim().Contains(name));
                }

                if (!string.IsNullOrWhiteSpace(queries.Name))
                {
                    var desc = queries.Name.ToLower().Trim();
                    query = query.Where(p => p.Name.ToLower().Trim().Contains(desc));
                }

                if (queries.MinPrice.HasValue)
                {
                    query = query.Where(p => p.Price >= queries.MinPrice.Value);
                }

                if (queries.MaxPrice.HasValue)
                {
                    query = query.Where(p => p.Price <= queries.MaxPrice.Value);
                }
            }

            return query;
        }

        public async Task<ServiceReponse<ProductResponse>> Update(int id, ProductRequest productRequest)
        {
            var response = new ServiceReponse<ProductResponse>();
            try
            {
                var check = await _unitOfWork.ProductRepo.FindEntityAsync(predicate => predicate.Id == id);
                if (check == null)
                {
                    response.Success = false;
                    response.Message = "ProductId not found";
                    return response;
                }

                _mapper.Map(productRequest, check);
                await _unitOfWork.ProductRepo.UpdateAsync(check);

                var productNew = await _unitOfWork.ProductRepo.GetById(check.Id);

                response.Success = true;
                response.Message = "Product updated successfully";
                response.Data = _mapper.Map<ProductResponse>(productNew);
            } catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.Message;
                response.ErrorMessages = new List<string> { ex.Message };
            }
            return response;
        }
    }
}
