using FlapKap.Data;
using FlapKap.Model;
using FlapKap.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace FlapKap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VendingMachineController : ControllerBase
    {
        private readonly VendingMachineService _vendingMachineService;
        public VendingMachineController(VendingMachineService vendingMachineService)
        {
            _vendingMachineService = vendingMachineService;
        }
        [Authorize]
        [HttpPost]
        [Route("deposit")]
        public ActionResult DepositCoins(int amount)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value.ToString();
            if (role != "Buyer")
            {
                return Unauthorized("User is not authorized to deposit coins.");
            }
            if (amount % 5 != 0)
            {
                return BadRequest("Coins must be in denominations of 5, 10, 20, 50, or 100.");
            }

            var id = GetUserIdFromClaims();
            var result = _vendingMachineService.DepositCoins(id, amount);
            if(result.IsSuccess)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }

        [HttpPost]
        [Route("buy")]
        [Authorize]
        public ActionResult BuyProduct(int productId, int quantity)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value.ToString();
            if (role != "Buyer")
            {
                return Unauthorized("User is not authorized to Buy Product.");
            }
            var userId = GetUserIdFromClaims();
            var result = _vendingMachineService.BuyProduct(productId, quantity, userId);
            if(result.IsSuccess) 
                return Ok(result.SavedObject);
            else
                return BadRequest(result.Message);
        }


        [HttpPost]
        [Route("reset")]
        [Authorize]
        public ActionResult ResetDeposit()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value.ToString();
            if (role != "Buyer")
            {
                return Unauthorized("User is not authorized to Buy Product.");
            }
            var userId = GetUserIdFromClaims();
            var result = _vendingMachineService.ResetDeposit(userId);
            if (result.IsSuccess)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }

        private int GetUserIdFromClaims()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }

    }

}
