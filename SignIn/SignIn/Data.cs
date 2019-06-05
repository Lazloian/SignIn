using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SignIn
{
    public partial class Data : Form
    {

        string roster_file = @"C:\Users\Public\Documents\Roster.txt";
        string settings_file = @"C:\Users\Public\Documents\Settings.txt";

        public Data()
        {
            InitializeComponent();
        }

        private void Data_Load(object sender, EventArgs e)
        {
            data_refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            data_refresh();
        }

        private void data_refresh()
        {
            dataGridView1.Columns.Clear();
            dataGridView1.ColumnCount = 6;
            dataGridView1.Columns[0].Name = "Last Name";
            dataGridView1.Columns[1].Name = "First Name";
            dataGridView1.Columns[2].Name = "ID Number";
            dataGridView1.Columns[3].Name = "Hours";
            dataGridView1.Columns[4].Name = "Minutes";
            dataGridView1.Columns[5].Name = "Sign in time";

            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string first_name = "",
                   last_name = "",
                   id_num = "",
                   in_time = "",
                   hours = "",
                   minutes = "";

            int space_count = 0,
                space_track = 0,
                first_space = 0,
                second_space = 0,
                third_space = 0,
                fourth_space = 0,
                fifth_space = 0;

            foreach (string student in roster)
            {
                space_count = 0;
                space_track = 0;

                //System.Diagnostics.Debug.WriteLine(student);
                for (int i = 0; i < student.Length; i++)
                {
                    if (student.Substring(i, 1).Equals(" "))
                    {
                        space_count += 1;
                    }
                    if ((space_count == 1) && (space_track == 0))
                    {
                        last_name = student.Substring(0, i);
                        first_space = i;
                        //System.Diagnostics.Debug.WriteLine(last_name);
                        space_track += 1;
                    }
                    else if ((space_count == 2) && (space_track == 1))
                    {
                        first_name = student.Substring(first_space + 1, i - first_space - 1);
                        second_space = i;
                        //System.Diagnostics.Debug.WriteLine(first_name);
                        space_track += 1;
                    }
                    else if ((space_count == 3) && (space_track == 2))
                    {
                        id_num = student.Substring(second_space + 1, i - second_space - 1);
                        third_space = i;
                        //System.Diagnostics.Debug.WriteLine(id_num);
                        space_track += 1;
                    }
                    else if ((space_count == 4) && (space_track == 3))
                    {
                        fourth_space = i;
                        in_time = student.Substring(third_space + 1, fourth_space - third_space - 1);
                        space_track += 1;
                        //System.Diagnostics.Debug.WriteLine(in_time);
                    }
                    else if ((space_count == 5) && (space_track == 4))
                    {
                        hours = student.Substring(fourth_space + 1, i - fourth_space - 1);
                        fifth_space = i;
                        minutes = student.Substring(fifth_space + 1, student.Length - fifth_space - 1);
                        //System.Diagnostics.Debug.WriteLine(hours);
                        //System.Diagnostics.Debug.WriteLine(minutes);
                        space_track += 1;
                    }
                }

                string[] row = new string[] { last_name, first_name, id_num, hours, minutes, in_time };
                dataGridView1.Rows.Add(row);
            }
            string[] settings = System.IO.File.ReadAllLines(settings_file);

            int setting_end = 0,
                hours_needed = 0;

            foreach (string setting in settings)
            {
                for (int i = 0; i < setting.Length; i++)
                {
                    if (setting.Substring(i, 1).Equals(":"))
                    {
                        setting_end = i;
                        if (setting.Substring(0, setting_end).Equals("Hours"))
                        {
                            hours_needed = Int32.Parse(setting.Substring(setting_end + 1, setting.Length - setting_end - 1));
                            dataGridView1.Columns[3].Name += " (" + hours_needed.ToString() + " Req)";
                            //System.Diagnostics.Debug.WriteLine("Hours needed = " + hours_needed);
                        }
                    }
                }
            }

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                //System.Diagnostics.Debug.WriteLine("Hours: " + row.Cells[3].Value);
                if (row.Cells[3].Value != null && Int32.Parse(row.Cells[3].Value.ToString()) >= hours_needed)
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(100, 255, 100);
                }
                else
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 100, 100);
                }
                //System.Diagnostics.Debug.WriteLine(row.Cells[5].Value);
                if (row.Cells[5].Value != null && row.Cells[5].Value.ToString() != "00:00")
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 255, 100);
                }
                else
                {
                    row.Cells[5].Value = "";
                }
            }
        }
    }
}
