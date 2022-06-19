using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instrument_Infrastructure.Data
{
    public abstract class DbConfig
    {
        private readonly IConfiguration _configuration;

        public DbConfig(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected IDbConnection Connection
        {
            get
            {
                return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));
            }
        }
    }
}
