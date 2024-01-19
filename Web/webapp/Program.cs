using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using StackExchange.Redis;
using webapp;
using webapp.Pages;

//讀取環境變數確認是否在啟用 Redis 的 Demo
_ = bool.TryParse(Environment.GetEnvironmentVariable("REDIS_ON"), out bool redisOn);

var builder = WebApplication.CreateBuilder(args);

// cookie auth 
builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
}).AddCookie(options => { options.Cookie = new CookieBuilder() { HttpOnly = false };});



if (!redisOn)
{
    //MemoryCache
    Console.WriteLine("Use In-Memory Cache");
    //單機版分散式快取, 使用 MemoryCache 實作的 IDistributedCache), 同時後續的 AddSession 等同寫入 Memory 中
    builder.Services.AddDistributedMemoryCache();
    //Default https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/configuration/default-settings?view=aspnetcore-8.0#key-management
    builder.Services.AddDataProtection();
    //單機 Signalr
    builder.Services.AddSignalR();
}
else
{
    //Redis Cache
    Console.WriteLine("Use Redis Cache");
    var redisConnectionString = builder.Configuration.GetConnectionString("redis");

    // 啟用 redis 實作的 IDistributedCache
    // 如此在使用 IDistributedCache 時是將資料寫回 Redis 進行分散式快取
    // https://learn.microsoft.com/en-us/aspnet/core/performance/caching/distributed?view=aspnetcore-8.0#distributed-redis-cache
    // 同時在後續 AddSession 時也會將後端的儲存寫入 Redis 中 
    // https://learn.microsoft.com/en-us/aspnet/core/fundamentals/app-state?view=aspnetcore-8.0#configure-session-state
    builder.Services.AddStackExchangeRedisCache(setup =>
    {
        setup.Configuration = redisConnectionString;
        setup.InstanceName = "Redis-Demo-";
    });

    // Key 儲存位置放至 Redis 中
    // ASP.NET Core Data Protection 同時也取代了傳統 ASP.NET 1.x~4.x 的 <machineKey>
    // https://learn.microsoft.com/en-us/aspnet/core/security/data-protection/introduction?view=aspnetcore-8.0
    // TODO: 需要處理 Redis 持久化
    builder.Services.AddDataProtection()
        .PersistKeysToStackExchangeRedis(ConnectionMultiplexer.Connect(redisConnectionString));


    // redis backplane for signalr
    // https://learn.microsoft.com/en-us/aspnet/core/signalr/redis-backplane?view=aspnetcore-8.0
    builder.Services.AddSignalR()
        .AddStackExchangeRedis(redisConnectionString, config =>
        {
            config.Configuration.ChannelPrefix = "Redis-Demo-SignalR-";
        });
}

//add session
builder.Services.AddSession();

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

//append env to response header for demo
app.Use(async (context, next) =>
{
    context.Response.Headers.TryAdd("Running-On", Environment.GetEnvironmentVariable("COMPUTERNAME") ?? Environment.GetEnvironmentVariable("APNAME"));
    await next();
});

app.UseDeveloperExceptionPage();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.UseSession();

app.MapRazorPages();

app.UseWebSockets();
app.MapHub<ChatRoomSignalrHub>("/chatHub");

app.Run();
