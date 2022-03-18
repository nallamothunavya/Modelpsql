using Media.Models;
using Dapper;
using Media.Utilities;
using Media.Repositories;


namespace Media.Repositories;

public interface IUsersRepository
{
    Task<Users> Create(Users Item);
    Task<bool> Update(Users Item);
    
    Task<Users> GetById(long UserId);
    Task<List<Users>> GetList();

    Task<List<Users>> GetListByPostId(long PostId);

     Task<List<Users>> GetListByHashTagsId(long HashId);

      Task<List<Users>> GetListByLikeId(long LikeId);

}
public class UsersRepository : BaseRepository, IUsersRepository
{
    public UsersRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Users> Create(Users Item)
    {
        var query = $@"INSERT INTO ""{TableNames.users}"" 
        (user_name, mobile,created_at,date_of_birth) 
        VALUES ( @UserName, @Mobile,@CreatedAt,@DateOfBirth) 
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Users>(query, Item);
            return res;
        }
    }
    
    public async Task<bool> Update(Users Item)
    {
       var query = $@"UPDATE ""{TableNames.users}"" SET  
        user_name = @UserName, mobile = @Mobile WHERE user_id = @UserId";

        using (var con = NewConnection)
        {
            var rowCount = await con.ExecuteAsync(query, Item);
            return rowCount == 1;
        }
    }

    public async Task<Users> GetById(long UserId)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}"" 
        WHERE user_id = @userId";
        
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Users>(query, new { UserId });
    }

    public async Task<List<Users>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""";

        List<Users> res;
        using (var con = NewConnection) // Open connection
            res = (await con.QueryAsync<Users>(query)).AsList(); // Execute the query
        
        return res;
    }

    public async Task<List<Users>> GetListByPostId(long PostId)
    {
        var query = $@"SELECT * FROM ""{TableNames.users}""
       
       WHERE user_id = @PostId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Users>(query, new {PostId})).AsList();
           return res;
       }
    }

    public async Task<List<Users>> GetListByHashTagsId(long HashId)
    {
       var query = $@"SELECT * FROM ""{TableNames.users}""
       
       WHERE user_id = @HashId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Users>(query, new {HashId})).AsList();
           return res;
       }
    }

    public async Task<List<Users>> GetListByLikeId(long LikeId)
    {
      var query = $@"SELECT * FROM ""{TableNames.users}""
       
       WHERE user_id = @LikeId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Users>(query, new {LikeId})).AsList();
           return res;
       }
    }
}