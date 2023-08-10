using Dapper;
using DemoJWT.Context;
using DemoJWT.Interfaces;
using DemoJWT.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DemoJWT.Services
{
    public class LoginServices : ILoginServices
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;
        public IConfiguration _configuration;
        public LoginServices(IDbConnection dbConnection, IConfiguration configuration, ApplicationDbContext context)
        {
            _context = context;
            _dbConnection = dbConnection;
            _configuration = configuration;
        }
        //Get All Users
        public async Task<IActionResult> GetAllUser(ControllerBase controllerBase)
        {
            //List<User> users = await _context.User.ToListAsync();
            var user = _dbConnection.Query<User>("GetAllUser", commandType: System.Data.CommandType.StoredProcedure);
            if (user == null)
            {
                return controllerBase.NotFound(new { Status = "Error", Message = "User Not Found" });
            }
            return controllerBase.Ok(user);
        }

        // User get by Id
        public async Task<IActionResult> GetUserById(int id, ControllerBase controllerBase)
        {
            try
            {


                var parameters = new { UserId = id };
                var user = _dbConnection.QueryFirstOrDefault<User>("GetUserById", parameters, commandType: System.Data.CommandType.StoredProcedure);
                if (user == null)
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
                }
                return controllerBase.Ok(user);
            }
            catch (Exception ex)
            {
                return controllerBase.BadRequest(new { Message = ex.Message });
            }
        }

        public async Task<IActionResult> DeleteUser(int id, ControllerBase controllerBase)
        {
            try
            {
                var parameters = new { UserId = id };
                var user = _dbConnection.QueryFirstOrDefault<User>("GetUserById", parameters, commandType: System.Data.CommandType.StoredProcedure);
                //var user = await _context.User.FindAsync(id);
                if (user == null)
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
                }

                _context.User.Remove(user);
                await _context.SaveChangesAsync();

                return controllerBase.Ok(new
                {
                    Message = "User deleted Successfully."
                });
            }
            catch(Exception ex)
            {
                return controllerBase.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> AddUser(User? user, ControllerBase controllerBase)
        {
            try
            {
                if (user.Username == null || user.Username == "" || user.Password == null || user.Password == "" || user.Email == null || user.Email == "")
                {
                    return controllerBase.BadRequest(new
                    {
                        Message = "User data should not be blank"
                    });
                }
                _context.User.Add(user);
                await _context.SaveChangesAsync();
                return controllerBase.Ok(new
                {
                    Message = "Add User Successfully."
                });
            }
            catch(Exception ex)
            {
                return controllerBase.BadRequest(ex.Message);
            }
        }

        public async Task<IActionResult> UpdateUser(int id, User updatedUser, ControllerBase controllerBase)
        {

            var parameters = new { UserId = id };
            var user = _dbConnection.QueryFirstOrDefault<User>("GetUserById", parameters, commandType: System.Data.CommandType.StoredProcedure);
            if (user == null)
            {
                return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
            }

            user.Username = updatedUser.Username;
            user.Email = updatedUser.Email;
            user.Contact = updatedUser.Contact;
            user.Password = updatedUser.Password;

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return controllerBase.NotFound(new { Status = "Error", Message = "user Not found" });
                }
                else
                {
                    throw;
                }
            }
            return controllerBase.Ok(new
            {
                Message = "Update User Successfully."
            });
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.UserId == id);
        }


        public async Task<IActionResult> LoginSubmit(string Username, string Password, ControllerBase controllerBase)
        {
            if (Username != null && Password != null)
            {
                var userData = await GetUser(Username, Password);
                var jwt = _configuration.GetSection("jwt").Get<Jwt>();
                if (userData != null)
                {
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        //new Claim("Id", user.UserId.ToString()),
                        new Claim("UserName", Username)
                    };
                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.key));
                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                    var token = new JwtSecurityToken(
                                      jwt.Issuer,
                                      jwt.Audience,
                                      claims,
                                      expires: DateTime.Now.AddMinutes(20),
                                      signingCredentials: signIn
                                      );
                    var Token = new JwtSecurityTokenHandler().WriteToken(token);
                    return controllerBase.Ok(new
                    {
                        Message = "Login successful",
                        token = Token
                    });
                }
                else
                {
                    return controllerBase.BadRequest(new
                    {
                        Message = "Invalid Credentials"
                    });
                }
            }
            else
            {
                return controllerBase.BadRequest(new
                {
                    Message = "Invalid Credentials"
                });

            }
        }

        public async Task<User> GetUser(string username, string password)
        {
            return await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        }
    }
}
