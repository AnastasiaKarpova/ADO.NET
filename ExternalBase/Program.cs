using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternalBase
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Connector.Select("*", "Disciplines");
			Console.WriteLine("\n-----------------------------------------\n");
			Console.WriteLine(Connector.DisciplineID("AutoCAD"));
			Console.WriteLine(Connector.DisciplineID("HTML/CSS"));

			Console.WriteLine("\n-----------------------------------------\n");
			Connector.Select("*", "Teachers");
			Console.WriteLine("\n-----------------------------------------\n");
			Console.WriteLine(Connector.TeacherID("Свищев"));
			Console.WriteLine(Connector.TeacherID("Глазунов"));

			Console.WriteLine("\n-----------------------------------------\n");
			Connector.Select("*", "Students");
			Console.WriteLine("\n-----------------------------------------\n");
			Console.WriteLine(Connector.CountStudents("Students"));
		}
	}
}
