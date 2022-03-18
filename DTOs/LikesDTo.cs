using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Media.Models;

namespace Media.DTOs;

public record LikesDTO
{
    [JsonPropertyName("like_id")]
    public long LikeId { get; set; }

    [JsonPropertyName("post_at")]
    public DateTimeOffset PostAt { get; set; }

    [JsonPropertyName("like_at")]

    public DateTimeOffset LikedAt {get;set;}

    [JsonPropertyName("posts")]
    public List<PostsDTO> Posts { get; set; }

    [JsonPropertyName("hashtags")]
    public List<HashTagsDTO> HashTags { get; set; }

    [JsonPropertyName("users")]
    public List<UsersDTO> Users { get; set; }


}



