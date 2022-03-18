 using Media.DTOs;

namespace Media.Models;


public record Likes
{
    
    public long LikeId { get; set; }
    public DateTimeOffset PostAt { get; set; }

    public DateTimeOffset LikedAt {get;set;}


    public LikesDTO asDto => new LikesDTO
    {
        LikeId = LikeId,
        LikedAt = LikedAt,
        PostAt = PostAt,
    };
}
