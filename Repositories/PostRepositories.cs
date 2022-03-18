using Media.Models;
using Dapper;
using Media.Utilities;
using Media.Repositories;


namespace Media.Repositories;

public interface IPostsRepository
{
    Task<Posts> Create(Posts Item);
 
    Task<bool> Delete(long PostId);
    Task<Posts> GetById(long PostId);
    Task<List<Posts>> GetList();

    Task<List<Posts>> GetListByUserId(long UserId);
    Task<List<Posts>> GetListByHashTagsId(long HashId);

    Task<List<Posts>> GetListByLikeId(long LikeId);
    

}
public class PostsRepository : BaseRepository, IPostsRepository
{
    public PostsRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<Posts> Create(Posts Item)
    {
        var query = $@"INSERT INTO ""{TableNames.post}"" 
        (post_type) 
        VALUES ( @PostType) 
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Posts>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long PostId)
    {
       var query = $@"DELETE FROM ""{TableNames.post}"" 
        WHERE post_id = @PostId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { PostId });
            return res > 0;
        }
    }


    public async Task<Posts> GetById(long PostId)
    {
       var query = $@"SELECT * FROM ""{TableNames.post}"" 
        WHERE post_id = @PostId";
        
        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Posts>(query, new { PostId });
    }

    public async Task<List<Posts>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.post}""";

        List<Posts> res;
        using (var con = NewConnection) 
            res = (await con.QueryAsync<Posts>(query)).AsList(); 
        

        
        return res;
    }

    public async Task<List<Posts>> GetListByHashTagsId(long HashId)
      {
        var query = $@"SELECT * FROM  ""{TableNames.post_hash}"" ph LEFT JOIN ""{TableNames.post}"" p 
	   ON ph.post_id = p.post_id
	   WHERE hash_id = @HashId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Posts>(query, new {HashId})).AsList();
           return res;
      }
    }

    public async Task<List<Posts>> GetListByLikeId(long LikeId)
    {
        
       var query = $@"SELECT * FROM ""{TableNames.post}""
       
       WHERE post_id = @LikeId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Posts>(query, new {LikeId})).AsList();
           return res;
       }
    }

    public async Task<List<Posts>> GetListByUserId(long UserId)
    {
       
       var query = $@"SELECT * FROM ""{TableNames.post}""
       
       WHERE post_id = @UserId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Posts>(query, new {UserId})).AsList();
           return res;
       }
    }

}