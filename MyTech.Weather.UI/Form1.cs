using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyTech.Weather.DAL;
using MyTech.Weather.Entities;

namespace MyTech.Weather.UI
{
    public partial class Form1 : Form
    {
        APICityRequset aPI = new APICityRequset();
        Request request = new Request();
        public Form1()
        {
            string path = @"C:\Users\מולועלם דפרשה\Desktop\MyProjectAtZiont\ProjectZiont\Tech.MyProject.Weather\MyTech.Weather.UI\bin\Debug\History.txt";
            InitializeComponent();
            if (File.Exists(path))
            {
                MakeDataTableAndDisplay();
                request.Load();
            }

        }
        private void MakeDataTableAndDisplay()
        {
            DataTable table = new DataTable();
            DataColumn column;
            DataRow row;
            DataView view;

            column = new DataColumn();
            column.DataType = Type.GetType("System.String");
            column.ColumnName = "city";
            table.Columns.Add(column);

            column = new DataColumn();
            column.DataType = Type.GetType("System.Decimal");
            column.ColumnName = "tmp";
            table.Columns.Add(column);

            var lines = aPI.Load();
            foreach (var item in lines.Values)
            {
                row = table.NewRow();
                row["city"] = item.location.name;
                row["tmp"] = item.current.temp_c;
                table.Rows.Add(row);
            }

            view = new DataView(table);
            dataGridView1.DataSource = view;
        }

        private async void button1_Click_1(object sender, EventArgs e)
        {
            string cityName = textBox1.Text;
            int secondRefersh = int.Parse(textBox2.Text);

            if (cityName != "")
            {
                await request.Add(cityName);
                request.Start_Auto_Request(cityName, secondRefersh);
            }
            else
            {
                MessageBox.Show("Please enter city!");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            request.Stop_Auto_Request();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            request.Save();

            MakeDataTableAndDisplay();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var file = aPI.LoadOneCity();
            label4.Text = file.current.temp_c.ToString();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await request.UpdateList();
            MakeDataTableAndDisplay();
        }
    }
}
