using Microsoft.Extensions.Caching.Memory;

var memoryCache = new MemoryCache(new MemoryCacheOptions());

var key = "memcache";
var data ="Hi MemoryCache!";

memoryCache.Set(key,data);
memoryCache.TryGetValue(key, out var data_from_mem_cache);
Console.WriteLine($"read/write redis >> '{key}':{data_from_mem_cache}");
