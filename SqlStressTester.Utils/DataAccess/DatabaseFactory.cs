using SqlStressTester.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStressTester.Utils.DataAccess
{
    public class DatabaseFactory
    {
        private DatabaseFactory() { }

        public static Database Create(DataSource dataSource)
        {
            string connectionString = dataSource.ConnectionString;

            Database database = dataSource.IsMySql
                ? new MySqlDatabase(connectionString)
                : new SqlDatabase(connectionString);

            return database;
        }

    }
}
