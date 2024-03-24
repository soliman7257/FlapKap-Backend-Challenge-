using FlapKap.Data;
using FlapKap.Model;
using FlapKap.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FlapKap.Services
{
    public class UserService 
    {
        private readonly ILogger<UserService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtOptions _jwtOptions;
        public UserService(ILogger<UserService> logger ,ApplicationDbContext dbContext, JwtOptions jwtOptions) 
        {
            _dbContext = dbContext;
            _logger = logger;
            _jwtOptions = jwtOptions;
        }


        private SaveResultWithSavedObject<User> ValidUser(AuthenticationRequest requset)
        {
            SaveResultWithSavedObject<User> result = new SaveResultWithSavedObject<User>(false);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Username == requset.UserNmae);
                if (user == null)
                {
                    
                    result.Message = "Error User Name";
                    return result; 
                }
               if (user.Password  != requset.PassWord) 
                {
                    result.Message = "Error User PassWord";
                    return result;
                }
                result.IsSuccess = true;
                result.SavedObject = user;
                return result;

            }
            catch (Exception ex)
            {

                _logger.LogError("User Execption ");
                result.IsSuccess = false;
                result.Message = ex.Message;
                return result;
            }
           

        }


        public SaveResultWithSavedObject<string> AuthenticateUser(AuthenticationRequest request)
        {
            SaveResultWithSavedObject<string> result = new SaveResultWithSavedObject<string>(true);
            try
            {
                var resultOfValidation = ValidUser(request);
                if (resultOfValidation.IsSuccess)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Issuer = _jwtOptions.Issur,
                        Audience = _jwtOptions.Audience,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SigningKey))
                        , SecurityAlgorithms.HmacSha256),
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                        new (ClaimTypes.Name ,resultOfValidation.SavedObject.Username),
                        new (ClaimTypes.Role ,resultOfValidation.SavedObject.Role),
                        new (ClaimTypes.NameIdentifier,resultOfValidation.SavedObject.Id.ToString())
                        }

                        )


                    };
                    var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                    var accessToken = tokenHandler.WriteToken(securityToken);
                    result.SavedObject = accessToken;
                    return result;

                }
                
            }
            catch(Exception ex) 
            {
                
                _logger.LogError(ex.Message, "Error occurred while Authenticate User name  #{Name}  PassWord  ", request.UserNmae,request.PassWord);
            }
            result.IsSuccess = false;
            return result;
        }
        public SaveResultWithSavedObject<User> GetUser (int id) 
        {
            SaveResultWithSavedObject<User> result = new SaveResultWithSavedObject<User>(true);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Id == id);
                if (user != null)
                {
                    result.SavedObject = user;
                    result.Message = "User get Successfuly";            
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "No User Found";
                    
                }

            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "Error occurred while Get User ";
                _logger.LogError(ex.Message, "Error occurred while Get User  #{id} ",id);
                
            }
            return result;

        }

        public SaveResult CreateUser (User user)
        {
            SaveResult result = new SaveResult (true);
            try
            {
                user.Id = 0;
                _dbContext.Add(user);
                _dbContext.SaveChanges();
                result.Message = "User created successfully";
            }
            catch(Exception e)
            {
                result.IsSuccess = false;
                _logger.LogError(e.Message, "Error occurred while User Added");
                result.Message = "Error occurred while User Added";
            }
            return result;
        }
        public SaveResult DeleteUser(int id)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                User user = _dbContext.Users.FirstOrDefault(s => s.Id == id);
                if (user != null)
                {
                    _dbContext.Remove(user);
                    _dbContext.SaveChanges();
                    result.Message = "User Deleted successfully";
                }
                else
                {
                    result.IsSuccess = false;
                    result.Message = "User Not Found";
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Message = "Error occurred while Delete User ";
                _logger.LogError(e.Message, "Error occurred while Delete User #{id} ", id);
            }
            return result;
        }

        public SaveResult UpdateUser(User user)
        {
            SaveResult result = new SaveResult(true);
            try
            {
                User existingUser = _dbContext.Users.FirstOrDefault(s => s.Id == user.Id);
                if (existingUser != null)
                {
                    _dbContext.Entry(existingUser).CurrentValues.SetValues(user);
                    _dbContext.SaveChanges();
                    result.Message = "User Update successfully";
                }
                else
                {
                    result.Message = "User Not Found ";
                    result.IsSuccess = false;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                _logger.LogError(e.Message, "Error occurred while Update User #{id} ", user.Id);
                result.Message = "Error occurred while Update User ";
               
                
            }
            return result;
        }
    }
}
