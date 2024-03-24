using FlapKap.Data;
using FlapKap.Model;
using FlapKap.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace FlapKap.Services
{
    public class VendingMachineService
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _dbContext;
        public VendingMachineService(ILogger<UserService> logger ,ApplicationDbContext dbContext ) 
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public SaveResult DepositCoins(int userId , int amount)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Id == userId);
                if (user == null)
                {
                    return new SaveResult(false, "User not found.");
                }
                user.Deposit += amount;
                _dbContext.SaveChanges();
                result.Message = "Deposit updated successfully";


            }
            catch(Exception ex)
            {
                result.IsSuccess = false;
                _logger.LogError(ex.Message, "Error occurred while updated Deposit");
                result.Message = "Error occurred while updated Deposit";
            }
            return result;
        }



        public SaveResult ResetDeposit(int userId)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Id == userId);
                if (user == null)
                {
                    return new SaveResult(false, "User not found.");
                }
                user.Deposit = 0;
                _dbContext.SaveChanges();
                result.Message = "Deposit reset successfully.";


            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _logger.LogError(ex.Message, "Error occurred while rest Deposit");
                result.Message = "Error occurred while rest Deposit";
            }
            return result;
        }

        public SaveResultWithSavedObject<ResponseBuyProduct> BuyProduct(int productId, int quantity ,int userId)
        {
            SaveResultWithSavedObject<ResponseBuyProduct> result = new SaveResultWithSavedObject<ResponseBuyProduct>(false);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Id == userId);
                if (user == null)
                {
                    result.Message = "User not found.";
                    return result;
                }
                Product product = _dbContext.Products.FirstOrDefault(s => s.Id == productId);
                if (product == null)
                {
                    result.Message = "Product not found.";
                    return result;
                    
                }
                if (quantity <= 0 )
                {
                    result.Message = "Quantity must be greater than zero.";
                    return result;
                }
                decimal totalCost = product.Cost * quantity;
                if (user.Deposit < totalCost)
                {
                    result.Message = "Insufficient funds.";
                    return result;
                   
                }
                user.Deposit -= totalCost;
                product.AmountAvailable -= quantity;
                _dbContext.SaveChanges();
                result.SavedObject = new ResponseBuyProduct() {
                    PurchasedProducts = product.ProductName,
                    ChangeDeposit = user.Deposit,
                    TotalSpent = totalCost
                };
                result.IsSuccess = true;

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                _logger.LogError(ex.Message, "Error occurred while Buy  Product");
                result.Message = "Error occurred while Buy Product";
            }
            return result;
        }
         
    }
}
