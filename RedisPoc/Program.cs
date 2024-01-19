using StackExchange.Redis;

using var redisConnection 
    = ConnectionMultiplexer.Connect("localhost:6379");
var database = redisConnection.GetDatabase();

var key = "redis_key";
var data = "Hi Redis!";

database.StringSet(key, data);
var data_from_redis = database.StringGet(key);

Console.WriteLine($"read/write redis >> '{key}':{data_from_redis}");