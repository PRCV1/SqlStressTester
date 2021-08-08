using MySqlConnector;
using SqlStressTester.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SqlStressTester.Utils.DataAccess
{
    public class MySqlDatabase : Database
    {
        private readonly string connectionString;

        internal MySqlDatabase(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public override async Task<SqlConnectionResult> ConnectAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                MySqlConnection sqlConnection = new(this.connectionString);
                await sqlConnection.OpenAsync(cancellationToken);
                return new(sqlConnection, sqlConnection.State == System.Data.ConnectionState.Open);
            }
            catch (Exception)
            {
                return new(null, false);
            }
        }

        public override Task ExecuteAsync(string query, IProgress<SqlResult> progress = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public override async Task<IReadOnlyList<string>> GetDatabasesAsync(CancellationToken cancellationToken = default)
        {
            SqlConnectionResult connectionResult = await ConnectAsync();
            if (!connectionResult.Connected)
            {
                return Enumerable.Empty<string>().ToList();
            }

            string sql = "SHOW DATABASES;";
            using DbConnection con = connectionResult.Connection;
            using DbCommand dbCommand = con.CreateCommand();
            dbCommand.CommandText = sql;
            using DbDataReader dbDataReader = await dbCommand.ExecuteReaderAsync();

            List<string> result = new();
            while (await dbDataReader.ReadAsync())
            {
                string database = await dbDataReader.GetFieldValueAsync<string>(0);
                result.Add(database);
            }

            return result;
        }
    }
}
