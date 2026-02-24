using Backend_API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Backend_API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "MANAGER")]
    public class UserController : ControllerBase
    {
        private readonly SmartDeskDbContext _context;

        public UserController(SmartDeskDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Get() 
        {
            var data = _context.Users.ToList();
            return Ok(data);
        }

        [HttpPost]
        public IActionResult AddUser(User user)
        {
            if (user == null)
            {
                return BadRequest("Invalid Data");  
            }

            _context.Users.Add(user);
            _context.SaveChanges();

            return Ok("inserted Succesfully");
        }

        
    }
}
