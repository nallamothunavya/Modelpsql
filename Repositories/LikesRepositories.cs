using Media.Models;
using Dapper;
using Media.Utilities;
using Media.Repositories;


namespace Media.Repositories;

public interface ILikesRepository
{
    Task<Likes> Create(Likes Item);

    Task<bool> Delete(long LikeId);
    Task<Likes> GetById(long LikeId);
    Task<List<Likes>> GetList();

    Task<List<Likes>> GetListByPostId(long PostId);

}
public class LikesRepository : BaseRepository, ILikesRepository
{
    public LikesRepository(IConfiguration config) : base(config)
    {

    }


    public async Task<Likes> Create(Likes Item)
    {
       var query = $@"INSERT INTO ""{TableNames.likes}"" 
        (like_id) 
        VALUES (@LikeId) 
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<Likes>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long LikeId)
    {
       var query = $@"DELETE FROM ""{TableNames.likes}"" 
        WHERE like_id = @LikeId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { LikeId });
            return res > 0;
        }
    }

    public async Task<Likes> GetById(long LikeId)
    {
         var query = $@"SELECT * FROM ""{TableNames.likes}"" 
        WHERE like_id = @LikeId";
        

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<Likes>(query, new { LikeId });
    }

    public async Task<List<Likes>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.likes}""";

        List<Likes> res;
        using (var con = NewConnection) 
            res = (await con.QueryAsync<Likes>(query)).AsList(); 
        

        
        return res;
    }

    public async Task<List<Likes>> GetListByPostId(long PostId)
    {
         var query = $@"SELECT * FROM ""{TableNames.likes}""
       
       WHERE like_id = @PostId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<Likes>(query, new {PostId})).AsList();
           return res;
       }
    }
}