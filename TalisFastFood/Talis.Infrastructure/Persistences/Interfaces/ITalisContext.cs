using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Talis.Infrastructure.Persistences.Interfaces
{
    public interface ITalisContext
    {
        SqlConnection GetSqlConnection();
    }
}
