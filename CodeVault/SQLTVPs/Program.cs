using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SqlServer.Server;

// See also http://lennilobel.wordpress.com/2009/07/29/sql-server-2008-table-valued-parameters-and-c-custom-iterators-a-match-made-in-heaven/

namespace SQLTVPs
{
    class Program
    {
        static void Main(string[] args)
        {
        }

        static IEnumerable<string> GetStrings()
        {
            yield return "hello";
            yield return "world";
        }

        static void CallProcWithTVPUsingDataTables()
        {
            // Make a table to hold the data.
            DataTable table = new DataTable();
            table.Columns.Add("Media", typeof(string));

            foreach (var row in GetStrings())
            {
                table.Rows.Add(row);
            }

            using (var conn = new SqlConnection("...."))
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "dbo.NameOfStoredProc";
                // TODO: You can use an IEnumerable<SqlDataRecord> here instead.
                // Not sure if an IEnumerable<IDataRecord> can be used, but can
                // probably cast from one to the other and use my ObjectDataReader.
                // See example at the link.
                var p = cmd.Parameters.AddWithValue("@MediaRows", table);
                p.SqlDbType = SqlDbType.Structured;
                // TODO: You need to create this table type in SQL Server.
                p.TypeName = "dbo.NameOfMyType";
                conn.Open();

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                    }
                }
            }
        }
    }
}
