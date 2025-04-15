﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Configuration;

namespace Academy
{
	public partial class MainForm : Form
	{
		Connector connector;

		public Dictionary<string, int> d_directions;
		public Dictionary<string, int> d_groups;

		DataGridView[] tables;

		Query[] queries = new Query[]
		{
			new Query
				("last_name,first_name,middle_name,birth_date,group_name,direction_name",
				"Students JOIN Groups ON ([group]=group_id) JOIN Directions ON (direction=direction_id)"
				//"[group]=group_id AND direction=direction_id"
				),
			new Query
				("group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name",
				"Groups,Directions",
				"direction=direction_id"
				),
			new Query
				("direction_name,COUNT(DISTINCT group_id) AS N'Количество групп', COUNT(stud_id) AS N'Количество студентов'",
						"Students RIGHT JOIN Groups ON([group]=group_id) RIGHT JOIN Directions ON(direction=direction_id)",
						"",
						"direction_name"
				),
			new Query("*", "Disciplines"),
			new Query("*", "Teachers")
		};
		string[] status_messages = new string[]
		{
			$"Количество студентов: ",
			$"Количество групп: ",
			$"Количество направлений: ",
			$"Количество дисциплин: ",
			$"Количество преподавателей: ",
		};
		public MainForm()
		{
			InitializeComponent();
			tables = new DataGridView[]
			{
				dgvStudents,
				dgvGroups,
				dgvDirections,
				dgvDisciplines,
				dgvTeachers
			};

			connector = new Connector
				(
					ConfigurationManager.ConnectionStrings["PV_319_Import"].ConnectionString
				);
			d_directions = connector.GetDictionary("*", "Directions"); //d_ - Dictionary
			d_groups = connector.GetDictionary("group_id,group_name", "Groups");
			
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());
			cbGroupsDirection.Items.AddRange(d_directions.Select(k => k.Key).ToArray());
			cbStudentsDirection.Items.AddRange(d_directions.Select(d => d.Key).ToArray());
			cbStudentsGroup.Items.Insert(0, "Все группы");
			cbStudentsDirection.Items.Insert(0, "Все направления");
			cbGroupsDirection.Items.Insert(0, "Все направления");
			cbStudentsDirection.SelectedIndex = 0;
			cbStudentsGroup.SelectedIndex = 0;
			//dgv - DataGridView
			dgvStudents.DataSource = connector.Select("last_name,first_name,middle_name,birth_date,group_name,direction_name", "Students,Groups,Directions", "[group]=group_id AND direction=direction_id");
			toolStripStatusLabelCount.Text = $"Количество студентов:{dgvStudents.RowCount - 1}.";
			//dgvGroups.DataSource = connector.Select("*", "Groups");
			//toolStripStatusLabelCount.Text = $"Количество групп:{dgvGroups.RowCount - 1}.";
			//dgvDirections.DataSource = connector.Select("*", "Directions");
			//toolStripStatusLabelCount.Text = $"Количество направлений:{dgvDirections.RowCount - 1}.";
			//dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
			//toolStripStatusLabelCount.Text = $"Количество дисциплин:{dgvDisciplines.RowCount - 1}.";
			//dgvTeachers.DataSource = connector.Select("*", "Teachers");
			//toolStripStatusLabelCount.Text = $"Количество преподавателей:{dgvTeachers.RowCount - 1}.";
		}

		void LoadPage(int i, Query query = null)
		{
			if(query == null) query = queries[i];
			tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.Group_by);
			toolStripStatusLabelCount.Text = status_messages[i] + CountRecordsInDGV(tables[i]);
		}
		private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			//Console.WriteLine(tabControl.SelectedIndex);
			LoadPage(tabControl.SelectedIndex);
			//int i = tabControl.SelectedIndex;
			//Query query = queries[i];
			//tables[i].DataSource = connector.Select(query.Columns, query.Tables, query.Condition, query.Group_by);
			//toolStripStatusLabelCount.Text = status_messages[i] + CountRecordsInDGV(tables[i]);
			/*switch(tabControl.SelectedIndex)
			{
				case 0:
					dgvStudents.DataSource = connector.Select("last_name,first_name,middle_name,birth_date,group_name,direction_name", "Students,Groups,Directions","[group]=group_id AND direction=direction_id");
					toolStripStatusLabelCount.Text = $"Количество студентов:{dgvStudents.RowCount - 1}.";
					break;
				case 1:
					dgvGroups.DataSource = connector.Select("group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name", "Groups,Directions","direction=direction_id");
					toolStripStatusLabelCount.Text = $"Количество групп:{dgvGroups.RowCount - 1}.";
					break;
				case 2:
					dgvDirections.DataSource = connector.Select
						(
						"direction_name,COUNT(DISTINCT group_id) AS N'Количество групп', COUNT(stud_id) AS N'Количество студентов'", 
						"Students RIGHT JOIN Groups ON([group]=group_id) RIGHT JOIN Directions ON(direction=direction_id)", 
						"", 
						"direction_name"
						);
					toolStripStatusLabelCount.Text = $"Количество направлений:{dgvDirections.RowCount - 1}.";
					break;
				case 3:
					dgvDisciplines.DataSource = connector.Select("*", "Disciplines");
					toolStripStatusLabelCount.Text = $"Количество дисциплин:{dgvDisciplines.RowCount - 1}.";
					break;
				case 4:
					dgvTeachers.DataSource = connector.Select("*", "Teachers");
					toolStripStatusLabelCount.Text = $"Количество преподавателей:{dgvTeachers.RowCount - 1}.";
					break;
			}*/
		}

		//private void cbDirection_SelectedIndexChanged(object sender, EventArgs e)
		//{
		//	dgvGroups.DataSource = connector.Select
		//		(
		//			"group_name,dbo.GetLearningDaysFor(group_name) AS weekdays,start_time,direction_name",
		//			"Groups,Directions",
		//			$"direction=direction_id AND direction=N'{d_directions[cbGroupsDirection.SelectedItem.ToString()]}'"
		//		);
		//	toolStripStatusLabelCount.Text = $"Количество групп: {CountRecordsInDGV(dgvGroups)}.";
		//}
		int CountRecordsInDGV(DataGridView dgv)
		{
			return dgv.RowCount == 0 ? 0 : dgv.RowCount - 1;
		}

		private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			string cb_name = (sender as ComboBox).Name;
			string tab_name = tabControl.SelectedTab.Name;
			int last_capital_index = Array.FindLastIndex<char>(cb_name.ToCharArray(), Char.IsUpper);
			string cb_suffix = cb_name.Substring(last_capital_index);
			Console.WriteLine(cb_name);
			Console.WriteLine(tab_name);
			Console.WriteLine(cb_suffix);	
						
			string dictionary_name = $"d_{cb_suffix.ToLower()}s";
			//По имени словаря, которое хранится в строке мы получаем сам словарь при помощи Рефлексии:
			Dictionary<string, int> dictionary = this.GetType().GetField(dictionary_name).GetValue(this) as Dictionary<string, int>;
			//Reflection - это подход, который позволяет обратиться к переменной, когда ее имя хранится в строке
			int i = (sender as ComboBox).SelectedIndex;

			#region Filtercb_StudentsGroup
			//Фильтруем выпадающий список групп на вкладке 'Students':
			Dictionary<string, int> d_groups = connector.GetDictionary
				("group_id,group_name", "Groups", i == 0 ? "" : $"[{cb_suffix.ToLower()}]={dictionary[(sender as ComboBox).SelectedItem.ToString()]}");
			cbStudentsGroup.Items.Clear();
			cbStudentsGroup.Items.AddRange(d_groups.Select(g => g.Key).ToArray());
			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ 
			#endregion

			//int t = tabControl.SelectedIndex;
			//dgvStudents.DataSource = connector.Select
			//	(queries[t].Columns, 
			//	queries[t].Tables, 
			//	i == 0 ? "" : $"direction = {d_directions[cbStudentsDirection.SelectedItem.ToString()]}"

			//	);
			Query query = new Query(queries[tabControl.SelectedIndex]);
			string condition = (i == 0 || (sender as ComboBox).SelectedItem == null ? "" : $"[{cb_suffix.ToLower()}]={dictionary[$"{(sender as ComboBox).SelectedItem}"]}");
			if (query.Condition == "") query.Condition = condition;
			else if (condition != "") query.Condition += $" AND {condition}";
			LoadPage(tabControl.SelectedIndex, query);
		}
		
		private void ckbDirectionsNull_Click(object sender, EventArgs e)
		{
			dgvDirections.DataSource = connector.Select
				(
				"direction_name, COUNT(DISTINCT group_id) AS group_count, COUNT(DISTINCT stud_id) AS student_count",
				"Students RIGHT JOIN Groups ON ([group]=group_id) RIGHT JOIN Directions ON (direction=direction_id)",
				"",
				"direction_name HAVING COUNT(group_id) = 0 AND COUNT(stud_id) = 0"
				);
			toolStripStatusLabelCount.Text = $"Количество пустых направлений: {CountRecordsInDGV(dgvDirections)}";
		}

		
	}
}
