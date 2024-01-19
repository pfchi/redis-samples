# 利用 Redis 達成 .NET 應用程式的高可用：範例程式

部落格文章〈[利用 Redis 達成 .NET 應用程式的高可用（簡報）](https://dailybrew.dev/posts/2024/01/redis-dotnet-high-availability-slides/)〉的範例程式。

所有專案都以 .NET 8 撰寫。

## 專案

| 目錄 | 內容 | 對應的簡報段落 |
|---|---|---|
| `RedisPoc/` | 用 StackExchange.Redis 寫入、讀取一筆字串 | 寫一點點 Code |
| `memCache/` | 用 `MemoryCache` 做同樣的事，對照 Redis 的寫法 | 和使用 MemoryCache 有 87% 像 |
| `sql/` | 用 SQLite 建表、寫入、讀取 | 有連線字串！！也和讀寫 SQL 87% 像 |
| `benchmark/` | BenchmarkDotNet 基準測試：SQL、Redis、MemoryCache 讀取比較；JSONPath 讀取與整份反序列化比較 | 效能比較、JSONPath 讀取 vs 整個取出反序列化 |
| `Web/` | ASP.NET Core 網站，以 nginx 負載平衡到兩台 AP，示範多台 AP 的 Session、Cookie 驗證、CSRF、SignalR 問題，以及改用 Redis 之後的結果 | 多台 AP 會遇到什麼問題、.NET with Redis、NReJSON 套件 |

## 執行前準備

`RedisPoc`、`benchmark` 需要本機有 Redis，可以用 Docker 啟動 Redis Stack（含 RedisJSON 與 RedisInsight）：

```shell
docker run -d -p 6379:6379 -p 8001:8001 --name redis redis/redis-stack
```

## 主控台範例

```shell
cd RedisPoc
dotnet run
```

`memCache`、`sql` 的執行方式相同。`sql` 第一次執行時會在專案目錄建立 `sqlite.db`。

## 效能比較

```shell
cd benchmark
dotnet run -c Release
```

`Program.cs` 預設執行 `ReJsonBenchmark`（JSONPath 讀取比較）。要跑 SQL、Redis、MemoryCache 的讀取比較，改成執行 `DataStoreBenchmark`，並把 `DataStoreBenchmark.cs` 裡的 SQL 連線字串換成可以連線的資料庫；查詢的是 Azure SQL 範例資料庫 AdventureWorksLT 的 `SalesLT.Product` 資料表。

## 多台 AP 示範（Web）

`docker-compose.yml` 啟動兩台 AP（`ap1`、`ap2`）、一台 nginx 和一台 Redis，網站入口是 <http://localhost:8800>。回應標頭 `Running-On` 會顯示這次是哪一台 AP 回應的。

不使用 Redis（Session、快取、SignalR 連線與 Data Protection 金鑰都只存在各台 AP 自己身上，換一台 AP 回應就接不上）：

```shell
cd Web
docker compose up --build
```

改用 Redis（以 PowerShell 為例）：

```powershell
cd Web
$Env:REDIS_ON="true"; docker compose up --build
```

網站頁面：

- `Login`：Cookie 驗證登入
- `SESSION`：寫入 Session
- `CSRF`：送出帶防偽權杖的表單
- `ChatRoom`：SignalR 聊天室
- `ReJSON`：用 NReJSON 以 JSONPath 查詢 Redis 中的 JSON 資料

`wwwroot/lib/` 內的 Bootstrap 與 SignalR 用戶端是以 LibMan 還原的第三方套件（MIT 授權），因為 Docker 建置時不會執行 LibMan 還原，所以一併放在 repo 中。
