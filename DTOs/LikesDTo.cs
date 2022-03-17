using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Media.Models;

namespace Media.DTOs;

public record LikesDTO
{
    [JsonPropertyName("like_id")]
    public long LikeId { get; set; }

    [JsonPropertyName("post_id")]
    public string PostId { get; set; }


}



