using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Talis.Infrastructure.Persistences.Interfaces;

namespace Talis.Infrastructure.Persistences.Data
{
    public class TalisContext : ITalisContext
    {
        private readonly string _connectionString;

        public TalisContext(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public SqlConnection GetSqlConnection()
        {
            return new SqlConnection(this._connectionString);
        }
    }
}
