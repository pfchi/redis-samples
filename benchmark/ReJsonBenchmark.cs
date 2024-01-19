using System;
using BenchmarkDotNet.Attributes;
using StackExchange.Redis;
using NReJSON;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using System.Text.Json.Nodes;
using Newtonsoft.Json.Linq;

public class ReJsonBenchmark{
    
    private const string BookData = "book-data.json";

    
    ConnectionMultiplexer _redisConnection;
    IDatabase _redisDatabase;
    JsonSerializerOptions _jsonSerializerOptions;


    [GlobalSetup]
    public void Setup(){
        _redisConnection = ConnectionMultiplexer.Connect("localhost:6379");
        _redisDatabase = _redisConnection.GetDatabase();
        _jsonSerializerOptions = new JsonSerializerOptions { 
                WriteIndented = true,
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
            };
        
        var json = System.IO.File.ReadAllText(Path.Combine(AppContext.BaseDirectory, BookData));
        _redisDatabase.JsonSet(BookData, json);
    }

    [Benchmark]
    public void System_Text_Json_Read_First()
    {
        var jsonString = _redisDatabase.JsonGet(BookData).ToString();
        var jsonNode = JsonNode.Parse(jsonString);
        var t = jsonNode[0].ToJsonString(_jsonSerializerOptions);
    }

    [Benchmark]
    public void Newtonsoft_Json_Read_First()
    {
        var jsonString = _redisDatabase.JsonGet(BookData).ToString();
        var jArray = JArray.Parse(jsonString);
        var t = jArray[0].ToString();
    }

    [Benchmark]
    public void NREJson_Read_First()
    {
        var jsonString = _redisDatabase.JsonGet(BookData, "$[0]").ToString();
    }


    [GlobalCleanup]
    public void Cleanup()
    {
        _redisConnection.Close();
    }


}