using Microsoft.AspNetCore.Mvc;
using server.Services;
using server.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of users.</returns>
        [HttpGet]
        public async Task<ActionResult<List<User>>> Get() =>
            await _userService.GetAll();

        /// <summary>
        /// Gets a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user.</param>
        /// <returns>The user with the specified ID.</returns>
        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<User>> Get(string id)
        {
            var user = await _userService.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }

        /// <summary>
        /// Gets a specific user by username.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <returns>The user with the specified username.</returns>
        [HttpGet("{username}")]
        public async Task<ActionResult<User>> GetByUsername(string username)
        {
            var user = await _userService.GetByUsername(username);
            if (user == null)
            {
                return NotFound();
            }
            return user;
        }



        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <param name="user">The user to create.</param>
        /// <returns>A newly created user.</returns>
        [HttpPost]
        public async Task<IActionResult> Post(User user)
        {
            await _userService.Create(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }

        /// <summary>
        /// Updates an existing user.
        /// </summary>
        /// <param name="id">The ID of the user to update.</param>
        /// <param name="updatedUser">The updated user object.</param>
        /// <returns>No content.</returns>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, User updatedUser)
        {
            var user = await _userService.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.Update(id, updatedUser);
            return NoContent();
        }

        /// <summary>
        /// Deletes a specific user by ID.
        /// </summary>
        /// <param name="id">The ID of the user to delete.</param>
        /// <returns>No content.</returns>
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userService.GetByID(id);
            if (user == null)
            {
                return NotFound();
            }
            await _userService.Remove(user.Id);
            return NoContent();
        }
    }
}
