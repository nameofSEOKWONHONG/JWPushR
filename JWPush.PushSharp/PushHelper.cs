using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Data.SqlClient;
using JWPush.Models;

namespace JWPush
{
    public class PushHelper
    {
        private SqlConnection _con;

        public PushHelper(string connection)
        {
            _con = new SqlConnection(connection);
        }

        public List<PushApp> GetRegisteredApps()
        {
            return _con.Query<PushApp>("SELECT * FROM JW.PUSHAPPS").ToList();
        }
    }
}
