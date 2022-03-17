using Media.DTOs;
using Media.Models;
using Media.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Media.Controllers;

[ApiController]
[Route("api/hash_tags")]
public class HashTagsController : ControllerBase
{
    private readonly ILogger<HashTagsController> _logger;
    private readonly IHashTagsRepository _hash_tags;
   private readonly IPostsRepository _posts;

    public HashTagsController(ILogger<HashTagsController> logger,
    IHashTagsRepository hash_tags,IPostsRepository posts)
    {
        _logger = logger;
        _hash_tags = hash_tags;
     
      _posts =posts;
    }

   


    [HttpGet]
    public async Task<ActionResult<List<HashTagsDTO>>> GetList()
    {
        var usersList = await _hash_tags.GetList();

      
        var dtoList = usersList.Select(x => x.asDto);

        return Ok(dtoList);
    }

    [HttpGet("{hash_id}")]
    public async Task<ActionResult<HashTagsDTO>> GetById([FromRoute] long hash_id)
    {
        var hash_tags = await _hash_tags.GetById(hash_id);

        if (hash_tags is null)
            return NotFound("No hash_tags found with given hash id");

        var dto = hash_tags.asDto;

        dto.Posts = (await _posts.GetListByHashTagsId(hash_id)).Select(x => x.asDto).ToList();

        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult<HashTagsDTO>> CreateHashTags([FromBody] HashTagsCreateDTO Data)
    {


        var toCreateHashTags = new HashTags
        {
            HashName = Data.HashName.Trim(),

        };

        var createdHashTags = await _hash_tags.Create(toCreateHashTags);

        return StatusCode(StatusCodes.Status201Created, createdHashTags.asDto);
    }

   

    [HttpDelete("{hash_id}")]
    public async Task<ActionResult> DeleteHashTags([FromRoute] long hash_id)
    {
        var existing = await _hash_tags.GetById(hash_id);
        if (existing is null)
            return NotFound("No hash found with given hash id");

        var didDelete = await _hash_tags.Delete(hash_id);

        return NoContent();
    }
}