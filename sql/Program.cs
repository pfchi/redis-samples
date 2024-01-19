using Microsoft.Data.Sqlite;
using var connection = new SqliteConnection("Data Source=sqlite.db");

connection.Open();

// 建立資料表 (如果不存在的話)
using var crt_table = connection.CreateCommand();

crt_table.CommandText = "CREATE TABLE IF NOT EXISTS MyTable (Id INTEGER PRIMARY KEY, Name TEXT)";
crt_table.ExecuteNonQuery();


// 插入資料
using var insert = connection.CreateCommand();

insert.CommandText = "INSERT INTO MyTable (Name) VALUES ('SQL')";
insert.ExecuteNonQuery();


// 讀取資料
using var select = connection.CreateCommand();

select.CommandText = "SELECT * FROM MyTable";
using var reader = select.ExecuteReader();

while (reader.Read())
{
    var id = reader.GetInt32(0);
    var name = reader.GetString(1);
    Console.WriteLine($"Id: {id}, Name: {name}");
}



