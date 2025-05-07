namespace SuParty.Service.Redis
{
    using Microsoft.EntityFrameworkCore.Storage;
    //using StackExchange.Redis;
    using System;
    using System.Threading.Tasks;

    public class RedisService
    {
        //private readonly ConnectionMultiplexer _redis;
        //private readonly IDatabase _db;

        //public RedisService(string connectionString)
        //{
        //    _redis = ConnectionMultiplexer.Connect(connectionString);
        //    _db = _redis.GetDatabase();
        //}

        //public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
        //{
        //    await _db.StringSetAsync(key, value, expiry);
        //}

        //public async Task<string?> GetAsync(string key)
        //{
        //    return await _db.StringGetAsync(key);
        //}

        //public async Task<bool> DeleteAsync(string key)
        //{
        //    return await _db.KeyDeleteAsync(key);
        //}

        //public async Task<bool> KeyExistsAsync(string key)
        //{
        //    return await _db.KeyExistsAsync(key);
        //}

//        var redisService = new RedisService("localhost:6379");

//        // 設定
//        await redisService.SetAsync("user:1:name", "Alice");

//        // 取得
//        var name = await redisService.GetAsync("user:1:name");
//        Console.WriteLine($"Name: {name}");

//// 刪除
//await redisService.DeleteAsync("user:1:name");

    }

}
