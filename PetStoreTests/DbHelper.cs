using Microsoft.Data.Sqlite; // <-- Новый using
using System.IO;

namespace PetStoreTests
{
    public class DbHelper
    {
        private const string DbFile = "TestDb.sqlite";
        private const string ConnectionString = "Data Source=" + DbFile; // <-- Чуть проще строка

        public static void InitializeDb()
        {
            if (File.Exists(DbFile))
            {
                // Закрываем соединения и удаляем файл (для чистоты)
                // Garbage Collector вызовем, чтобы освободить файл
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
                File.Delete(DbFile);
            }

            // Файл создастся сам при открытии
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                string sql = "CREATE TABLE Users (id INT, username VARCHAR(20), status VARCHAR(20))";

                var command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public static void AddUser(int id, string name, string status)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string sql = $"INSERT INTO Users (id, username, status) VALUES ({id}, '{name}', '{status}')";

                var command = new SqliteCommand(sql, connection);
                command.ExecuteNonQuery();
            }
        }

        public static string GetUserStatus(string name)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string sql = $"SELECT status FROM Users WHERE username = '{name}'";

                var command = new SqliteCommand(sql, connection);
                var result = command.ExecuteScalar();

                return result?.ToString(); // Добавил ? на случай null
            }
        }
    }
}