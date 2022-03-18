using Media.DTOs;
using Media.Models;
using Media.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Media.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly IUsersRepository _users;
    private readonly IPostsRepository _posts;

    private readonly ILikesRepository _likes;

    public UsersController(ILogger<UsersController> logger,
    IUsersRepository users, IPostsRepository posts,ILikesRepository likes)
    {
        _logger = logger;
        _users = users;
       _posts = posts;
       _likes = likes;
    }

    [HttpGet]
    public async Task<ActionResult<List<UsersDTO>>> GetList()
    {
        var usersList = await _users.GetList();

      
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }

    [HttpGet("{user_id}")]
    public async Task<ActionResult<UsersDTO>> GetById([FromRoute] long user_id)
    {
        var users = await _users.GetById(user_id);

        if (users is null)
            return NotFound("No user found with given user id");

        var dto = users.asDto;

        dto.Posts = (await _posts.GetListByUserId(user_id)).Select(x => x.asDto).ToList();

        dto.Likes = (await _likes.GetListByUserId(user_id)).Select(x => x.asDto).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<UsersDTO>> CreateUsers([FromBody] UsersCreateDTO Data)
    {

        var toCreateUsers = new Users
        {
            UserName = Data.UserName.Trim(),
            Mobile = Data.Mobile,

        };

        var createdUsers = await _users.Create(toCreateUsers);

        return StatusCode(StatusCodes.Status201Created, createdUsers.asDto);
    }

    [HttpPut("{user_id}")]
    public async Task<ActionResult> UpdateUsers([FromRoute] long user_id,
    [FromBody] UsersUpdateDTO Data)
    {
        var existing = await _users.GetById(user_id);
        if (existing is null)
            return NotFound("No user found with given user id");

        var toUpdateUsers = existing with
        {
            UserName = Data.UserName?.Trim()?.ToLower() ?? existing.UserName,
            Mobile = existing.Mobile,
           
        };

        var didUpdate = await _users.Update(toUpdateUsers);

        if (!didUpdate)
            return StatusCode(StatusCodes.Status500InternalServerError, "Could not update user");

        return NoContent();
    }

    
}