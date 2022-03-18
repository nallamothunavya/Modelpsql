using Media.Models;
using Dapper;
using Media.Utilities;
using Media.Repositories;


namespace Media.Repositories;

public interface IHashTagsRepository
{
    Task<HashTags> Create(HashTags Item);
     Task<bool> Delete(long HashId);
    Task<HashTags> GetById(long HashId);
    Task<List<HashTags>> GetList();

    Task<List<HashTags>> GetListByPostId(long PostId);

    Task<List<HashTags>> GetListByLikeId(long LikeId);

}
public class HashTagsRepository : BaseRepository, IHashTagsRepository
{

    public HashTagsRepository(IConfiguration config) : base(config)
    {

    }

    public async Task<HashTags> Create(HashTags Item)
    {
       var query = $@"INSERT INTO ""{TableNames.hash_tags}"" 
        ( hash_name) 
        VALUES ( @HashName) 
        RETURNING *";

        using (var con = NewConnection)
        {
            var res = await con.QuerySingleOrDefaultAsync<HashTags>(query, Item);
            return res;
        }
    }

    public async Task<bool> Delete(long HashId)
    {
       var query = $@"DELETE FROM ""{TableNames.hash_tags}"" 
        WHERE hash_id = @HashId";

        using (var con = NewConnection)
        {
            var res = await con.ExecuteAsync(query, new { HashId });
            return res > 0;
        }
    }


    public async Task<HashTags> GetById(long HashId)
    {
        var query = $@"SELECT * FROM ""{TableNames.hash_tags}"" 
        WHERE hash_id = @HashId";

        using (var con = NewConnection)
            return await con.QuerySingleOrDefaultAsync<HashTags>(query, new { HashId });
    }

    public async Task<List<HashTags>> GetList()
    {
        var query = $@"SELECT * FROM ""{TableNames.hash_tags}""";

        List<HashTags> res;
        using (var con = NewConnection) 
            res = (await con.QueryAsync<HashTags>(query)).AsList(); 
        

        
        return res;
    }

    public async Task<List<HashTags>> GetListByLikeId(long LikeId)
    {
        var query = $@"SELECT * FROM ""{TableNames.hash_tags}""
       
       WHERE hash_id = @LikeId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<HashTags>(query, new {LikeId})).AsList();
           return res;
       }
    }

    public async Task<List<HashTags>> GetListByPostId(long PostId)
    {
       var query = $@"SELECT * FROM  ""{TableNames.post_hash}"" ph LEFT JOIN ""{TableNames.hash_tags}"" h 
	   ON ph.hash_id = h.hash_id
	   WHERE post_id = @PostId";

       using(var con = NewConnection){
           var res = (await con.QueryAsync<HashTags>(query, new {PostId})).AsList();
           return res;
    }
    }
}