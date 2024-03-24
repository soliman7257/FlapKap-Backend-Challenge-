using FlapKap.Data;
using FlapKap.Model;
using FlapKap.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlapKap.Services
{
    public class ProductService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _dbContext;
        public ProductService(ILogger<UserService> logger ,ApplicationDbContext dbContext ) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public SaveResultWithSavedObject<IEnumerable<ProductRequset>> GetProducts() 
        {
            SaveResultWithSavedObject<IEnumerable<ProductRequset>> result = new SaveResultWithSavedObject<IEnumerable<ProductRequset>>(true);
            try
            {
                result.SavedObject = _dbContext.Products.Where(s => true).Select
                    (p=> new ProductRequset()
                    {
                        ProductName = p.ProductName,
                        AmountAvailable = p.AmountAvailable,
                        Cost = p.Cost,
                    });
                

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching products");
                result.IsSuccess = false;
                result.Message = "Error occurred while fetching products";
            }
            return result;

        }

        public SaveResult CreateProduct (ProductRequset productReq,int userId)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                Product product = new Product()
                {
                    ProductName = productReq.ProductName,
                    AmountAvailable = productReq.AmountAvailable,
                    Cost = productReq.Cost,
                    SellerId = userId,
                };
                _dbContext.Add(product);
                _dbContext.SaveChanges();
                result.Message = "Product created successfully";
            }
            catch(Exception e)
            {
                result.IsSuccess = false;
                _logger.LogError(e.Message, "Error occurred while Product Added");
                result.Message = "Error occurred while Product Added";
            }
            return result;
        }
        public SaveResult DeleteProduct(int productId,int userId)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                Product product = _dbContext.Products.FirstOrDefault(s => s.Id == productId);
                if (product == null)
                {
                    result.IsSuccess = false;
                    result.Message = "product Not Found";
                    return result;
                }
                if (product.SellerId != userId)
                {
                    result.IsSuccess = false;
                    result.Message = "product Not Belong To You ";
                    return result;
                }
                    _dbContext.Remove(product);
                    _dbContext.SaveChanges();
                    result.Message = "product Deleted successfully";

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "Error occurred while Delete Product ";
                _logger.LogError(e.Message, "Error occurred while Delete Product #{id} ", productId);

            }
            return result;
        }

        public SaveResultWithSavedObject<int> UpdateProduct(ProductRequset productReq ,int productId,int userId)
        {
            SaveResultWithSavedObject<int> result = new SaveResultWithSavedObject<int>(true);
            try
            {
                Product existingproduct = _dbContext.Products.FirstOrDefault(s => s.Id == productId);
                if (existingproduct == null)
                {
                    result.Message = "product Not Found ";
                    result.IsSuccess = false;
                    return result;
                }
                if (existingproduct.SellerId != userId )
                {
                    result.Message = "product Not Belong To You ";
                    result.IsSuccess = false;
                    return result;
                }

                _dbContext.Entry(existingproduct).CurrentValues.SetValues(productReq); // Update the properties of existingUser with user's values
                _dbContext.SaveChanges();
                result.Message = "product Update Successfuly";

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "Error occurred while Update Product ";
                _logger.LogError(e.Message, "Error occurred while Delete Product #{id} ", productId);
            }
            return result;
        }
    }
}
