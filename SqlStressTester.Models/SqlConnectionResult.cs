using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SqlStressTester.Models
{
    public class SqlConnectionResult
    {
        public DbConnection Connection { get; set; }
        public bool Connected { get; set; }

        public SqlConnectionResult(DbConnection dbConnection, bool connected)
        {
            this.Connection = dbConnection;
            this.Connected = connected;
        }

    }
}
