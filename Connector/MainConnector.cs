using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;
using System.Configuration;
using System.Runtime.InteropServices;

namespace Connector
{
	public partial class MainConnector : Form
	{
		readonly string CONNECTION_STRING;
		SqlConnection connection;
		public MainConnector()
		{
			InitializeComponent();

		}
		public MainConnector(string connection_String)
		{
			//CONNECTION_STRING = ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString;
			CONNECTION_STRING = connection_String;
			connection = new SqlConnection(CONNECTION_STRING);
			AllocConsole();
			Console.WriteLine(CONNECTION_STRING);
		}
		~MainConnector()
		{
			FreeConsole();
		}
		public Dictionary<string, int> GetDictionary(string columns, string tables, string condition = "")
		{
			Dictionary<string, int> values = new Dictionary<string, int>();
			string cmd = $"SELECT {columns} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();
			if (reader.HasRows)
			{
				while (reader.Read())
				{
					values[reader[1].ToString()] = Convert.ToInt32(reader[0]);
				}
			}
			reader.Close();
			connection.Close();
			return values;
		}
		public DataTable Select(string columns, string tables, string condition = "", string group_by = "")
		{

			DataTable table = null;
			string cmd = $"SELECT {columns} FROM {tables}";
			if (condition != "") cmd += $" WHERE {condition}";
			if (group_by != "") cmd += $" GROUP BY {group_by}";
			cmd += ";";
			SqlCommand command = new SqlCommand(cmd, connection);
			connection.Open();
			SqlDataReader reader = command.ExecuteReader();

			if (reader.HasRows)
			{
				//1) Создаем таблицу:
				table = new DataTable();
				table.Load(reader);
			}
			reader.Close();
			connection.Close();

			return table;
		}
		[DllImport("kernel32.dll")]
		public static extern bool AllocConsole();
		[DllImport("kernel32.dll")]
		public static extern bool FreeConsole();
	}
}
