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
    public class ProductController : ControllerBase
    {
       
        private readonly ProductService _productservice;
        public ProductController( ProductService productservice)
        {
            _productservice = productservice;
            
        }
        [Authorize]
        [HttpPost]
        [Route("Create")]
        public ActionResult CreateProduct(ProductRequset product)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Contains("Seller"))
            {
                return Unauthorized("User Not Seller");
            }
            var result  = _productservice.CreateProduct(product, GetUserIdFromClaims());
            if(result.IsSuccess)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult GetProduct()
        {
            var res = _productservice.GetProducts();
            if(res.IsSuccess)
            {
                return Ok(res.SavedObject);
            }
            else
            {
                return NotFound(res.Message);
            }
            
        }
        [Authorize]
        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteProduct (int id)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Contains("Seller"))
            {
                return Unauthorized("User Not Seller");
            }
            var result = _productservice.DeleteProduct(id,GetUserIdFromClaims());
            if (result.IsSuccess)
                return Ok(result.Message);
           
            else
                return NotFound(result.Message);
           
        }
        [Authorize]
        [HttpPut]
        [Route("Update")]
        public ActionResult UpdateProduct (ProductRequset product , int id)
        {
            var roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
            if (!roles.Contains("Seller"))
            {
                return Unauthorized("User Not Seller");
            }
            var result = _productservice.UpdateProduct(product, id,GetUserIdFromClaims());
            if (result.IsSuccess)
                return Ok(result.Message);

            else
                return NotFound(result.Message);
        }
        private int GetUserIdFromClaims()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return string.IsNullOrEmpty(userId) ? 0 : int.Parse(userId);
        }


    }
}
