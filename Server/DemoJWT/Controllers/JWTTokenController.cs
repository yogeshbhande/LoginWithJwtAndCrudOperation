using DemoJWT.Context;
using DemoJWT.Interfaces;
using DemoJWT.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using Dapper;
using System.Data;

namespace DemoJWT.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JWTTokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public readonly ApplicationDbContext _context;
        private readonly ILoginServices _loginServices;
        private readonly IDbConnection _dbConnection;
        public JWTTokenController(IDbConnection dbConnection,IConfiguration configuration, ApplicationDbContext context, ILoginServices loginServices)
        {
            _configuration = configuration;
            _context = context;
            _loginServices = loginServices;
            _dbConnection = dbConnection;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginSubmit(string Username, string Password)
        {
            return await _loginServices.LoginSubmit(Username, Password, this);
        }

        [Authorize]
        [HttpPost]
        [Route("AddUser")]        
        public async Task<IActionResult> AddUser(User user)
        {
            return await _loginServices.AddUser(user, this);
        }

        [Authorize]
        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, User updatedUser)
        {
            return await _loginServices.UpdateUser(id, updatedUser, this);
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            return await _loginServices.DeleteUser(id, this);
        }

        

        [Authorize]
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUser()
        {
            var users = await _loginServices.GetAllUser(this);
            if (users == null)
            {
                return Ok("user Not found");
            }
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _loginServices.GetUserById(id, this);
            if(user  == null)
            {
                return Ok("user Not found");
            }
            return Ok(user);
        }

    }
}



