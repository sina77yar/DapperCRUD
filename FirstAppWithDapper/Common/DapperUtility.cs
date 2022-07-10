using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace FirstAppWithDapper.Common
{
    public class DapperUtility
    {
        private IConfiguration configuration;
        public DapperUtility(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public SqlConnection GetConnection()
        {
            return new SqlConnection(configuration.GetConnectionString("NorthwindConnection"));
        }
    }
}
