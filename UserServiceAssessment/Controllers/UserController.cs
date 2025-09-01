using Microsoft.AspNetCore.Mvc;
using UserServiceAssessment.Helpers.Exceptions;
using UserServiceAssessment.Models.User;
using UserServiceAssessment.Services;

namespace UserServiceAssessment.Controllers;

[ApiController]
[Route("users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public ActionResult<UserModel> GetUserById(int id)
    {
        try
        {
            var user = userService.GetUserById(id);
            return Ok(user);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpDelete("{id}")]
    public ActionResult DeleteUserById(int id)
    {
        try
        {
            userService.RemoveUserById(id);
            return NoContent();
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    [HttpPost]
    public ActionResult<UserModel> AddUser([FromBody] CreateUserModel user)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var newUserId = userService.AddUser(user);

        var createdUser = userService.GetUserById(newUserId);
        return CreatedAtAction(nameof(GetUserById), new { id = newUserId }, createdUser);
    }
    [HttpPut("{id}/{email}")]
    public ActionResult<UserModel> UpdateUserEmailById(int id, string email)
    {
        try
        {
            var updatedUser = userService.UpdateUserEmailById(id, email);
            return Ok(updatedUser);
        }
        catch (UserNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet]
    public ActionResult<List<UserModel>> GetAllUsers()
    {
        return Ok(userService.GetAllUsers());
    }
}