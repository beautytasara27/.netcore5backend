using authentication.Context;
using authentication.Helpers;
using authentication.Models;
using authentication.PostModels.cs;
using authentication.ViewModels;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace authentication.Controllers
{
    [ApiController, Route("api/v1/[controller]"), EnableCors("MyCorsPolicy")]
    public class UsersController : ControllerBase
    {
        private readonly DatabaseContext _context;

        public UsersController(DatabaseContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserViewModel>>> GetUsers() => _context.Users.Select(EntityModelConverter.ConvertUserEntityToUserViewModel).ToList();
        
        [HttpGet("{id}")]
        public async Task<ActionResult<UserViewModel>> GetUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound($"Category with id {id} not found.");
            }

            return EntityModelConverter.ConvertUserEntityToUserViewModel(user);
        }

       
        [HttpPut("{id}")]
        public async Task<ActionResult<UserViewModel>> PutUser(Guid id, UserViewModel user)
        {
            if (id != user.Id)
            {
                return BadRequest($"Ids {id} and {user.Id} doesn't match.");
            }
            var userModel = _context.Users.FindAsync(id);
            if (userModel.Result == null) { return NotFound($"User with id {id} was not found"); }
            _context.Entry(userModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(user);
        }

       
        [HttpPost]
        public async Task<ActionResult<UserViewModel>> PostUser(UserPostModel user)
        {
            var userModel = EntityModelConverter.ConvertUserPostModelToUserEntity(user);
            _context.Users.Add(userModel);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Post operation failed. {ex}");
            }
            return CreatedAtAction("GetUser", new { id = userModel.Id }, userModel);
        }
     
        [HttpPost("Auth")]
        public async Task<ActionResult<AuthResponse>> SignIn(Authenticate user)
        {
            var authenticatedUser = await _context.Users.FirstOrDefaultAsync(a=>a.Username == user.Username);
            ;
            if (authenticatedUser == null)
            {
                return NotFound($"There is no record with username {user.Username}.");
            }
            else if (authenticatedUser.PasswordHash == PasswordHasher.HashText(user.Password, authenticatedUser.Username))
            {
                return Ok(EntityModelConverter.ConvertUserEntityToAuthResponse(authenticatedUser));
            }
            
            return BadRequest("username or password incorrect");
        }

        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound($"There is no record with id {id}.");
            }

            _context.Users.Remove(user);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return BadRequest($"Delete Operation failed. {ex}");
            }

            return Ok("Deletion Succcessful");
        }

        private bool UserExists(Guid id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
