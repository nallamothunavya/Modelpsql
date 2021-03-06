using Media.DTOs;
using Media.Models;
using Media.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Media.Controllers;

[ApiController]
[Route("api/likes")]
public class LikesController : ControllerBase
{
    private readonly ILogger<LikesController> _logger;
    private readonly ILikesRepository _likes;

    private readonly IPostsRepository _posts;

    private readonly IHashTagsRepository _hashtags;

    private readonly IUsersRepository _users;
   

    public LikesController(ILogger<LikesController> logger,
    ILikesRepository likes,IPostsRepository posts,IHashTagsRepository hashtags,IUsersRepository users)
    {
        _logger = logger;
        _likes = likes;
        _posts = posts;
        _hashtags = hashtags;
        _users = users;
    }

     [HttpGet]
    public async Task<ActionResult<List<LikesDTO>>> GetList()
    {
        var usersList = await _likes.GetList();

      
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }


    [HttpGet("{like_id}")]
    public async Task<ActionResult<LikesDTO>> GetById([FromRoute] long like_id)
    {
        var likes = await _likes.GetById(like_id);

        if (likes is null)
         return NotFound("No likes found with given like id");

         var dto = likes.asDto;

         dto.Posts = (await _posts.GetListByLikeId(like_id)).Select(x => x.asDto).ToList();
         dto.HashTags = (await _hashtags.GetListByLikeId(like_id)).Select(x => x.asDto).ToList();
         dto.Users = (await _users.GetListByLikeId(like_id)).Select(x => x.asDto).ToList();

        return Ok (dto);

        
    }


   

    [HttpDelete("{like_id}")]
    public async Task<ActionResult> DeleteLikes([FromRoute] long like_id)
    {
        var existing = await _likes.GetById(like_id);
        if (existing is null)
            return NotFound("No like found with given like id");

        var didDelete = await _likes.Delete(like_id);

        return NoContent();
    }
}