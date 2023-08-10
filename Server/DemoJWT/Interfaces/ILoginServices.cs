using DemoJWT.Models;
using Microsoft.AspNetCore.Mvc;

namespace DemoJWT.Interfaces
{
    public interface ILoginServices
    {
        public Task<IActionResult> LoginSubmit(string Username, string Password, ControllerBase controllerBase);
        public Task<IActionResult> GetAllUser(ControllerBase controllerBase);

        public Task<IActionResult> GetUserById(int id, ControllerBase controllerBase);

        public Task<IActionResult> DeleteUser(int id, ControllerBase controllerBase);

        public Task<IActionResult> AddUser(User user, ControllerBase controllerBase);

        public Task<IActionResult> UpdateUser(int id, User updatedUser, ControllerBase controllerBase);

        public Task<User> GetUser(string username, string password);
    }
}
