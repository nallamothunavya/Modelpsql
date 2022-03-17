using Media.DTOs;

namespace Media.Models;


public record Users
{
    
    public long UserId { get; set; }
    public string UserName { get; set; }

    public DateTimeOffset DateOfBirth { get; set; }
    public long Mobile { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    

    public UsersDTO asDto => new UsersDTO
    {
        UserId = UserId,
        UserName = UserName,
        Mobile = Mobile,
          
    };
}