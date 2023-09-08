using Npgsql;
using System;
using System.Threading.Tasks;

namespace DiscordBotTemplate.Database
{
    public class DBEngine
    {
        private string connectionString = "Host=b5siykqdvfriib0pkesn-postgresql.services.clever-cloud.com;Username=uleybizuyfu2ivb2cfiw;Password=Nbz4m4AFb40j8xsBSSU2uMLigq64WU;Database=b5siykqdvfriib0pkesn";

        public async Task<bool> StoreUserAsync(DUser user)
        {
            var userNo = await GetTotalUsersAsync() + 1;
            if (userNo == -1)
            {
                throw new Exception();
            }

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "INSERT INTO data.userinfo (userno, username, servername, serverid) " +
                                   $"VALUES ('{userNo}', '{user.userName}', '{user.serverName}', '{user.serverID}');";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        public async Task<(bool, DUser)> GetUserAsync(string username, ulong serverID)
        {
            DUser result;

            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT u.userno, u.username, u.servername, u.serverid " +
                                   "FROM data.userinfo u " +
                                   $"WHERE username = '{username}' AND serverid = {serverID}";

                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var reader = await cmd.ExecuteReaderAsync();
                        await reader.ReadAsync();

                        result = new DUser
                        {
                            userName = reader.GetString(1),
                            serverName = reader.GetString(2),
                            serverID = (ulong)reader.GetInt64(3)
                        };
                    }
                }

                return (true, result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return (false, null);
            }
        }

        private async Task<long> GetTotalUsersAsync()
        {
            try
            {
                using (var conn = new NpgsqlConnection(connectionString))
                {
                    await conn.OpenAsync();

                    string query = "SELECT COUNT (*) FROM data.userinfo";
                    using (var cmd = new NpgsqlCommand(query, conn))
                    {
                        var userCount = await cmd.ExecuteScalarAsync();
                        return Convert.ToInt64(userCount);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return -1;
            }
        }
    }
}
