using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;

namespace ExternalBase
{
	internal class Connector
	{
		static readonly int PADDING = 16;
		static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString;
		static SqlConnection connection;
		static Connector()
		{
			Console.WriteLine(CONNECTION_STRING);
			connection = new SqlConnection(CONNECTION_STRING);
		}
		public static void Select(string fields, string tables, string condition = "")
		{
			string cmd = $"SELECT {fields} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader.GetName(i).PadRight(PADDING));
				}
			}
			Console.WriteLine();
			while (reader.Read())
			{
				for (int i = 0; i < reader.FieldCount; i++)
				{
					Console.Write(reader[i].ToString().PadRight(PADDING));
				}
				Console.WriteLine();
			}
			reader.Close();
			string sqlExpression = "SELECT discipline_id('Объектно') FROM Disciplines";
			SqlCommand com = new SqlCommand(sqlExpression, connection);
			object id = command.ExecuteScalar();

			connection.Close();
		}
		static void RetrieveIdentity(string connectionString)
		{
			using (SqlConnection connection = new SqlConnection (connectionString))
			{
				SqlDataAdapter adapter = new SqlDataAdapter("SELECT discipline_id FROM Discipline", connection)
				{
					InsertCommand = new SqlCommand("")
				};
			}
		}
	}
}
