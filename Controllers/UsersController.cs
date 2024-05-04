using ApiDevBP.DATA;
using ApiDevBP.Entities;
using ApiDevBP.Models;
using ApiDevBP.Models.DTO;
using ApiDevBP.Repository.IRopository;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SQLite;
using System.Reflection;

namespace ApiDevBP.Controllers
{
    [ApiController]
    [Route("api/User")]
    public class UsersController : ControllerBase
    {

        private readonly IUserRepository _userRepository;

        private readonly ILogger<UsersController> _logger;

        private readonly IMapper _mapper;

        public UsersController(ILogger<UsersController> logger, IUserRepository userRepository, IMapper mapper)
        {
            _logger = logger;

            _userRepository = userRepository;

            _mapper = mapper;

        }


        #region SaveUser
        /// <summary>
        /// Save user to DB
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SaveUser([FromBody] UserDto users)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _mapper.Map<UserEntity>(users);
            if (!_userRepository.CreateUser(user))
            {
                ModelState.AddModelError("", $"Something went wrong while saving {user.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok(user);

        }

        #endregion

        #region GetUsers
        /// <summary>
        /// Displays all DB users
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult GetUsers()
        {

            var user = _userRepository.GetUsers();

            var listUserDto = new List<UserDto>();

            foreach (var item in user)
            {
                listUserDto.Add(_mapper.Map<UserDto>(item));
            }

            return Ok(listUserDto);

        }
        #endregion

        #region UpdateUser
        /// <summary>
        /// Update a specific user BD
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult UpdateUser([FromBody]UserDto userDto)
        {

            var user = _mapper.Map<UserEntity>(userDto);

            if (!_userRepository.UpdateUser(user))
            {
                ModelState.AddModelError("",$"Something went wrong when editing: {user}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        #endregion

        #region DeleteUser
        /// <summary>
        /// Delete a specific user BD
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("{userId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteUser(int userId)
        {
            var user = _userRepository.GetUserById(userId);
            try
            {
                var deleteUser = _userRepository.DeleteUser(user);

                //if (!_userRepository.DeleteUser(user))
                //{
                //    ModelState.AddModelError("", $"Algo salio mal al borrar {user.Name}");
                //    return StatusCode(500, ModelState);
                //}
 
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error delete user: ", ex);
            }
            return NoContent();
        }
        #endregion

        #region SearchUser
        /// <summary>
        /// Search for users by first or last name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("Search")]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        public IActionResult Search(string name)
        {
            try
            {
                var result = _userRepository.SearchUser(name.Trim());

                if (result.Any())
                {
                    return Ok(result);
                }

                return NotFound("No user found");
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error retrieving data: {@StatusCode}", ex);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion

    }
}
