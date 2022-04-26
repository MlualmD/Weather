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

            table.Columns.Add("name", typeof(string));
            table.Columns.Add("tmp", typeof(float));


            var citiesFromFile = aPI.Load();
            foreach (var city in citiesFromFile)
            {
                table.Rows.Add(city.Key, city.Value.current.temp_c);
            }

           
            dataGridView1.DataSource = table;
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
            label4.Visible = true;
            pictureBox1.Visible = true;
            label4.Text = file.current.temp_c.ToString();
            pictureBox1.Load($"http:{file.current.condition.icon }");
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            await request.UpdateList();
            MakeDataTableAndDisplay();
        }
    }
}
