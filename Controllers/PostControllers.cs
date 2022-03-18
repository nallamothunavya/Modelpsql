using Media.DTOs;
using Media.Models;
using Media.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Media.Controllers;

[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    private readonly ILogger<PostsController> _logger;
    private readonly IPostsRepository _posts;

    private readonly IHashTagsRepository _hashtags;

    private readonly IUsersRepository _user;

    private readonly ILikesRepository _likes;


    public PostsController(ILogger<PostsController> logger,
    IPostsRepository posts,IHashTagsRepository hashtags,IUsersRepository user,ILikesRepository likes)
    {
        _logger = logger;
        _posts = posts;
        _hashtags = hashtags;
        _user = user;
        _likes = likes;
    }

    [HttpGet]
    public async Task<ActionResult<List<PostsDTO>>> GetList()
    {
        var postsList = await _posts.GetList();

        var dtoList = postsList.Select(x => x.asDto);

        return Ok(dtoList);
    }

    [HttpGet("{post_id}")]
    public async Task<ActionResult<PostsDTO>> GetById([FromRoute] long post_id)
    {
        var posts = await _posts.GetById(post_id);

        if (posts is null)
            return NotFound("No post found with given post id");

        var dto = posts.asDto;

        dto.HashTags = (await _hashtags.GetListByPostId(post_id)).Select(x => x.asDto).ToList();
        dto.Users = (await _user.GetListByPostId(post_id)).Select(x=>x.asDto).ToList();
        dto.Likes = (await _likes.GetListByPostId(post_id)).Select(x=>x.asDto).ToList();

        return Ok(dto);


    }

    [HttpPost]
    public async Task<ActionResult<PostsDTO>> CreatePosts([FromBody] PostsCreateDTO Data)
    {

        var toCreatePosts = new Posts
        {
            PostType = Data.PostType.Trim(),

        };

        var createdPosts = await _posts.Create(toCreatePosts);

        return StatusCode(StatusCodes.Status201Created, createdPosts.asDto);
    }

    


    [HttpDelete("{post_id}")]
    public async Task<ActionResult> DeletePosts([FromRoute] long post_id)
    {
        var existing = await _posts.GetById(post_id);
        if (existing is null)
            return NotFound("No post found with given post id");

        var didDelete = await _posts.Delete(post_id);

        return NoContent();
    }
}