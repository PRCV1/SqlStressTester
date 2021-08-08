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
    public abstract class Database
    {
        public abstract Task<SqlConnectionResult> ConnectAsync(CancellationToken cancellationToken = default);
        public abstract Task ExecuteAsync(string query, IProgress<SqlResult> progress = default, CancellationToken cancellationToken = default);
        public abstract Task<IReadOnlyList<string>> GetDatabasesAsync(CancellationToken cancellationToken = default);
    }
}
