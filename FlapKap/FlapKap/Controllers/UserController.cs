using FlapKap.Data;
using FlapKap.Model;
using FlapKap.Services;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace FlapKap.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {

        private readonly UserService _userservice;
       
        public UserController( UserService userservice )
        {
            _userservice = userservice;

        }
        [HttpPost]
        [Route("authenticate")]
        public ActionResult<string> AuthenticateUser(AuthenticationRequest request)
        {
           
            var resultOfAuthentication = _userservice.AuthenticateUser(request);
            if(resultOfAuthentication.IsSuccess == true) 
                return Ok(resultOfAuthentication.SavedObject);

            //if (resultOfValidation.IsSuccess)
            //{
            //    var tokenHandler = new JwtSecurityTokenHandler();
            //    var tokenDescriptor = new SecurityTokenDescriptor
            //    {
            //        Issuer = _jwtOptions.Issur,
            //        Audience = _jwtOptions.Audience,
            //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey))
            //        , SecurityAlgorithms.HmacSha256),
            //        Subject = new ClaimsIdentity(new Claim[]
            //        {
            //            new (ClaimTypes.Name ,resultOfValidation.SavedObject.Username),
            //            new (ClaimTypes.Role ,resultOfValidation.SavedObject.Role),
            //            new (ClaimTypes.NameIdentifier,resultOfValidation.SavedObject.Id.ToString())
            //        }

            //        )


            //    };
            //    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            //    var accessToken = tokenHandler.WriteToken(securityToken);
            //    return Ok(accessToken);
            //}
            return Unauthorized();

        }


        [HttpPost]
        [Route("Create")]
        public ActionResult CreateUser(User user)
        {

            var result  = _userservice.CreateUser(user);
            if(result.IsSuccess)
                return Ok(result.Message);
            else
                return BadRequest(result.Message);
           
        }
        [HttpGet]
        [Route("Get")]
        public ActionResult GetUser(int id)
        {
            var res = _userservice.GetUser(id);
            if(res.IsSuccess)
            {
                return Ok(res.SavedObject);
            }
            else
            {
                return NotFound(res.Message);
            }
           
        }

        [HttpDelete]
        [Route("Delete")]
        public ActionResult DeleteUser (int id)
        {

            var result = _userservice.DeleteUser(id);
            if (result.IsSuccess)
                return Ok(result.Message);
           
            else
                return NotFound(result.Message);
           
        }
        [HttpPut]
        [Route("Update")]
        public ActionResult UpdateUser (User user)
        {
            var result = _userservice.UpdateUser(user);
            if (result.IsSuccess)
                return Ok(result.Message);

            else
                return NotFound(result.Message);
            
        }


    }
}
