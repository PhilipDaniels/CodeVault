using System;
using System.Data.SqlClient;

namespace SQLConnectionTester
{
	class Program
	{
		private static string connStr = Properties.Settings.Default.ConnStr;

		static void Main(string[] args)
		{
			Log("Starting");

			Query(@"select getdate()'");

			Log("Done");
			Console.ReadKey();
		}

		private static void Query(string query)
		{
			using (var conn = new SqlConnection(connStr))
			{
				conn.Open();
				using (var cmd = conn.CreateCommand())
				{
					cmd.CommandType = System.Data.CommandType.Text;
					cmd.CommandText = query;
					cmd.ExecuteNonQuery();
				}
			}

			Log("Done query: " + query);
		}

		static void Log(string msg)
		{
			if (prev == DateTime.MinValue)
				prev = DateTime.Now;

			string m = String.Format
				(
				"Time = {0:yyyy/MM/dd hh:mm:ss}, msec = {1}: {2}",
				DateTime.Now,
				(DateTime.Now - prev).Milliseconds, 
				msg
				);

			Console.WriteLine(m);
			prev = DateTime.Now;
		}
		static DateTime prev;
	}
}
