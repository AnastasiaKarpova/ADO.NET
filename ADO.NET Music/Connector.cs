using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;
using System.Configuration;

namespace ADO.NET_Music
{
	internal class Connector
	{
		static readonly int PADDING = 30;
		static readonly string CONNECTION_STRING = ConfigurationManager.ConnectionStrings["Music"].ConnectionString;
		static SqlConnection connection;
		static Connector()
		{
			connection = new SqlConnection(CONNECTION_STRING);
		}
		public static void SelectArtists()
		{
			Select("*", "Artists");
		}
		public static void SelectAuthorWords()
		{
			Select("*","AuthorWords");
		}
		public static void SelectComposer()
		{
			Select("*","Composer");
		}
		public static void SelectMusic()
		{
			Connector.Select("title,release_date, FORMATMESSAGE(N'%s',artist_name), " +
							"FORMATMESSAGE(N'%s %s',first_name,lsat_name), " +
							"FORMATMESSAGE(N'%s %s', first_name_music,last_name_music)",
							"Music,Artists,AuthorWords,Composer", 
							"artist=artist_id, author=author_id, composer=composer_id");
		}
		public static void Select(string columns, string tables, string condition=null)
		{
			string cmd = $"SELECT {columns} FROM {tables}";
			if (condition != null) cmd += $"WHERE {condition}";
			cmd += ";";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if(reader.HasRows)
			{
				Console.WriteLine("======================================================================");
				for(int i = 0; i < reader.FieldCount; i++)
					Console.Write(reader.GetName(i).PadRight(PADDING));
				Console.WriteLine();
				Console.WriteLine("======================================================================");
				while(reader.Read())
				{
					for(int i = 0; i < reader.FieldCount;i++)
					{
						Console.Write(reader[i].ToString().PadRight(PADDING));
					}
					Console.WriteLine();
				}
			}
			reader.Close();
			connection.Close();
		}
		public static void InsertArtist(string artist_name)
		{
			Insert("Artist","artist_name",$"N'{artist_name}'");
		}
		public static void InsertAuthorWords(string first_name, string last_name)
		{
			Insert("AuthorWords","first_name,last_name",$"N'{first_name}',N'{last_name}'");
		}
		public static void InsertComposer(string first_name_music, string last_name_music)
		{
			Insert("Composer","first_name_music,last_name_music",$"N'{first_name_music}',N'{last_name_music}'");
		}
		public static void InsertMusic(string title, string release_date, string artist, string author, string composer)
		{
			Insert("Music","title,release_date,artist,author,composer",$"N'{title}','{release_date}',{artist},{author},{composer}");
		}
		static void Insert(string table, string columns, string values, string key="")
		{
			if (key == "")
			{
				key = table.ToLower();
				key = key.Remove(key.Length - 1, 1) + "_id";
			}
			Console.WriteLine(key);
			string[] all_columns = columns.Split(',');
			string[] all_values = values.Split(',');
			string condition = " ";
			for(int i = 0; i < all_columns.Length; i++)
			{
				condition += $"{all_columns[i]} = {all_values[i]}";
				if (i != all_columns.Length - 1) condition += " AND ";
			}
			string check_string = $"IF NOT EXISTS (SELECT {key} FROM {table} WHERE {condition})";
			string query = $"INSERT {table}({columns} VALUES ({values}))";
			string cmd = $"{check_string} BEGIN {query} END";
			Console.WriteLine(cmd);
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			command.ExecuteNonQuery();
			connection.Close();
		}
	}
}
