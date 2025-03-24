using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace ADO.NET_Music
{
	internal class Program
	{
		static void Main(string[] args)
		{
			const int PADDING = 30;
			const string CONNECTION_STRING = 
				"Data Source=(localdb)\\MSSQLLocalDB;" +
				"Initial Catalog=Music;" +
				"Integrated Security=True;" +
				"Connect Timeout=30;" +
				"Encrypt=False;" +
				"TrustServerCertificate=False;" +
				"ApplicationIntent=ReadWrite;" +
				"MultiSubnetFailover=False";
			Console.WriteLine(CONNECTION_STRING);
			SqlConnection connection = new SqlConnection(CONNECTION_STRING);
			string cmd = "SELECT title,release_date, " +
						 "FORMATMESSAGE(N'%s', artist_name), " +
						 "FORMATMESSAGE(N'%s %s', first_name, last_name), " +
						 "FORMATMESSAGE(N'%s %s', first_name_music, last_name_music) " +
						 "FROM Music,Artists,AuthorWords,Composers " +
						 "WHERE artist=artist_id, author=author_id, composer=composer_id";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
			{
				Console.WriteLine("=================================================================");
				for(int i=0; i < reader.FieldCount; i++)
					Console.Write(reader.GetName(i).PadRight(PADDING));
				Console.WriteLine();
				Console.WriteLine("=================================================================");
				while(reader.Read())
				{
					for(int i = 0; i < reader.FieldCount; i++)
					{
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}
			}
			reader.Close();
			connection.Close();
		}
	}
}
