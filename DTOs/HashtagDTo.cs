using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Media.Models;

namespace Media.DTOs;

public record HashTagsDTO
{
    [JsonPropertyName("hash_id")]
    public long HashId { get; set; }

    [JsonPropertyName("hash_name")]
    public string HashName { get; set; }


    [JsonPropertyName("posts")]
    public List<PostsDTO> Posts { get; set; }

    [JsonPropertyName("users")]
    public List<UsersDTO> Users { get; set; }

    [JsonPropertyName("likes")]
    public List<LikesDTO> Likes { get; set; }


}

public record HashTagsCreateDTO
{
    [JsonPropertyName("hash_id")]
    public long HashId { get; set; }

    [JsonPropertyName("hash_name")]
    public string HashName { get; set; }


}

