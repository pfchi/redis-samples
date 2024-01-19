using System;
using BenchmarkDotNet.Attributes;
using StackExchange.Redis;
using System.Data.SqlClient;
using Microsoft.Extensions.Caching.Memory;

public class DataStoreBenchmark
{
    ConnectionMultiplexer _redisConnection;
    IDatabase _redisDatabase;
    SqlConnection _sqlConnection;
    MemoryCache _memoryCache;
    string key = "key";

    [GlobalSetup]
    public void Setup()
    {
        //redis
        _redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
        _redisDatabase = _redisConnection.GetDatabase();
        
        var data = "some data";

        _redisDatabase.StringSet(key, data);

        //sql
        _sqlConnection = new SqlConnection("Server=tcp:<your-server>.database.windows.net,1433;Initial Catalog=<database>;Persist Security Info=False;User ID=<user>;Password=<password>;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        _sqlConnection.Open();

        // using (var command = _sqlConnection.CreateCommand())
        // {
        //     command.CommandText = "CREATE TABLE IF NOT EXISTS MyTable (Id INTEGER PRIMARY KEY, Name TEXT)";
        //     command.ExecuteNonQuery();
        // }

        // using (var command = _sqlConnection.CreateCommand())
        // {
        //     command.CommandText = "INSERT INTO MyTable (Name) VALUES ('SQL')";
        //     command.ExecuteNonQuery();
        // }

        //memcache
         _memoryCache = new MemoryCache(new MemoryCacheOptions());
         _memoryCache.Set(key,data);
    }


    [Benchmark]
    public void SqlRead()
    {
        using (var command = _sqlConnection.CreateCommand())
        {
            command.CommandText = "SELECT TOP (1) [ProductID] ,[Name]  FROM [SalesLT].[Product]";
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    int id = reader.GetInt32(0);
                    string name = reader.GetString(1);
                }
            }
        }
    }

    [Benchmark]
    public void RedisRead()
    {
        var retrievedValue = _redisDatabase.StringGet(key);
    }
    
    [Benchmark]
    public void MemoryCacheRead(){
        _memoryCache.TryGetValue(key, out var data);
    }


    [GlobalCleanup]
    public void Cleanup()
    {
        _redisConnection.Close();
        _sqlConnection.Close();
    }
}