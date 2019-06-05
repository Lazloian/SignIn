using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using System.Windows.Forms;

namespace SignIn
{
    public partial class EditRoster : Form
    {
        string roster_file = @"C:\Users\Public\Documents\Roster.txt";
        string overwrite_file = @"C:\Users\Public\Documents\Roster_Overwrite.txt";

        public EditRoster()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e) //Add Student
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);
            string first_name = "",
                   last_name = "",
                   id_num = "";

            int name_place,
                id_num_place,
                id_num_end;

            bool error = false;

            foreach (string student in roster)
            {
                name_place = 0;
                id_num_place = 0;
                id_num_end = 0;
                id_num = "";
                error = false;
                for (int i = 0; i < student.Length; i++)
                {
                    if ((student.Substring(i, 1) == " ") && name_place == 0)
                    {
                        name_place = i;
                        last_name = student.Substring(0, name_place);
                    }

                    else if ((student.Substring(i, 1) == " ") && id_num_place == 0)
                    {
                        id_num_place = i;
                        first_name = student.Substring(name_place + 1, id_num_place - name_place - 1);
                    }

                    else if ((student.Substring(i, 1) == " ") && id_num_end == 0)
                    {
                        id_num_end = i;
                        id_num = student.Substring(id_num_place + 1, id_num_end - id_num_place - 1);
                    }
                }

                if (textBox6.Text.Length != 7)
                {
                    textBox9.ForeColor = Color.Red;
                    textBox9.Text = "Student ID Number Invalid";
                    error = true;
                    break;
                }

                if (textBox6.Text == id_num)
                {
                    textBox9.ForeColor = Color.Red;
                    textBox9.Text = "Student ID Number Already Added";
                    error = true;
                    break;
                }
            }

            if (error == false)
            {
                textBox9.ForeColor = Color.Green;
                textBox9.Text = "Student Added";
                System.IO.File.AppendAllText(roster_file, textBox5.Text + " " + textBox4.Text + " " + textBox6.Text + " 00:00" + " 0" + " 0" + "\r\n");
                System.Diagnostics.Debug.WriteLine(textBox5.Text + " " + textBox4.Text + " " + textBox6.Text + " Added");
                roster = System.IO.File.ReadAllLines(roster_file);
                Array.Sort(roster);
                System.IO.File.WriteAllText(roster_file, "");
                foreach (string student in roster)
                {
                    System.IO.File.AppendAllText(roster_file, student + "\r\n");
                }
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
            }
        }

        private void button3_Click(object sender, EventArgs e) // Exit
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e) // Remove Student
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string id_num = "";

            bool id_stop = false,
                 id_found = false;

            foreach (string student in roster)
            {
                id_stop = false;

                for (int i = 0; i < student.Length; i++)
                {
                    if ((student.Substring(i, 1) == "2") && id_stop == false)
                    {
                        id_num = student.Substring(i, 7);
                        id_stop = true;

                        if (id_num == textBox8.Text)
                        {
                            id_found = true;
                            System.Diagnostics.Debug.WriteLine(id_num + " Removed");
                        }

                        else
                        {
                            System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                        }
                    }
                }
            }

            if (id_found)
            {
                System.IO.File.WriteAllText(roster_file, "");

                string[] new_roster = System.IO.File.ReadAllLines(overwrite_file);

                foreach (string student in new_roster)
                {
                    System.IO.File.AppendAllText(roster_file, student + "\r\n");
                }

                textBox10.ForeColor = Color.Green;
                textBox10.Text = "Student Removed";
                System.IO.File.WriteAllText(overwrite_file, "");
            }

            else
            {
                textBox10.ForeColor = Color.Red;
                textBox10.Text = "ID Number Not Found";
            }
            textBox8.Text = "";
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
