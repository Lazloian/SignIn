using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.VisualBasic;
using System.Windows.Forms;

//TODO Finish Sign All Out
//TODO Minutes can count over 60
//TODO Change tab index after entering new student
//TODO Stop the highlighting of the first text box

namespace SignIn
{
    public partial class Form1 : Form
    {
        Password password = new Password();
        Data data = new Data();

        string roster_file = @"C:\Users\Public\Documents\Roster.txt";
        string overwrite_file = @"C:\Users\Public\Documents\Roster_Overwrite.txt";


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("HH:mm"));
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e) // Edit Roster
        {
            password.Show();
        }

        private void button2_Click(object sender, EventArgs e) // Sign In
        {
            Sign_In();
        }

        private void button4_Click(object sender, EventArgs e) //Sign All Out
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string sign_in = "";

            int space_count = 0,
                space_track = 0,
                in_start = 0,
                in_end = 0;

            int space_count2 = 0,
                space_track2 = 0,
                in_hour = 0,
                in_minute = 0,
                out_hour = 0,
                out_minute = 0,
                total_hours = 0,
                total_minutes = 0,
                current_hours = 0, //! Use these and read the current times from the file
                current_minutes = 0;

            string before_sign_in = "";

            foreach (string student in roster)
            {
                for (int i = 0; i < student.Length; i++)
                {
                    if (student.Substring(i, 1) == " ")
                    {
                        space_count += 1;
                    }

                    if (space_count == 1 && space_track == 0)
                    {
                        in_start = i;
                        space_track += 1;
                    }

                    else if (space_count == 2 && space_track == 1)
                    {
                        in_end = i;
                        space_track += 1;
                    }

                    sign_in = student.Substring(in_start, in_end - in_start - 1);

                    if (sign_in != "00:00")
                    {
                        for (int x = 0; x < student.Length; x++)
                        {
                            if (student.Substring(x, 1) == " ")
                            {
                                space_count2 += 1;
                            }

                            if (space_count2 == 1 && space_track2 == 0)
                            {
                                before_sign_in = student.Substring(0, x + 1);
                                space_track2 += 1;
                            }

                            else if (space_count2 == 2 && space_track2 == 1)
                            {
                                in_hour = Int32.Parse(student.Substring(x + 1, 2));
                                in_minute = Int32.Parse(student.Substring(x + 3, 2));
                                space_track2 += 1;
                            }
                        }
                        out_hour = Int32.Parse(DateTime.Now.ToString("HH"));
                        out_minute = Int32.Parse(DateTime.Now.ToString("mm"));

                        total_hours = (out_hour - in_hour);

                        if (out_minute > in_minute)
                        {
                            total_minutes += (out_minute - in_minute);
                        }

                        else if (out_minute < in_minute)
                        {
                            total_hours -= 1;
                            total_minutes += (in_minute - out_minute);
                        }

                        if (total_minutes >= 60)
                        {
                            total_hours += 1;
                            total_minutes -= 60;
                        }

                        System.IO.File.AppendAllText(overwrite_file, before_sign_in + "00:00 " + total_hours.ToString() + " " + total_minutes.ToString());
                    }

                    else
                    {
                        System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                    }

                    textBox3.ForeColor = Color.Green;
                    textBox3.Text = "All Students Signed Out";
                    
                }
            }
        }

        private void Sign_In()
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string before_sign_in = "",
                   after_sign_in = "",
                   in_time = "";

            int space_count = 0,
                space_track = 0,
                sign_in_start = 0,
                sign_in_end = 0,
                sign_in_hour = 0,
                sign_in_minute = 0;

            bool id_num_stop = false,
                 id_error = true,
                 already_signed_in = false;

            foreach (string student in roster)
            {
                id_num_stop = false;
                for (int i = 0; i < student.Length; i++)
                {
                    if ((student.Substring(i, 1) == "2") && (id_num_stop == false))
                    {
                        id_num_stop = true;
                        if (student.Substring(i, 7) == textBox2.Text)
                        {
                            id_error = false;

                            for (int x = 0; x < student.Length; x++)
                            {
                                if ((student.Substring(x, 1) == " ") && space_count < 4)
                                {
                                    space_count += 1;
                                }

                                if (space_count == 3 && space_track == 0)
                                {
                                    sign_in_start = x;
                                    space_track += 1;
                                }

                                if (space_count == 4 && space_track == 1)
                                {
                                    sign_in_end = x;
                                    before_sign_in = student.Substring(0, sign_in_start + 1);
                                    after_sign_in = student.Substring(sign_in_end, student.Length - sign_in_end);
                                    space_track += 1;
                                }
                            }
                            in_time = student.Substring(sign_in_start + 1, sign_in_end - sign_in_start - 1);
                            //System.Diagnostics.Debug.WriteLine("in_time = " + in_time);
                            //System.Diagnostics.Debug.WriteLine("before: " + before_sign_in + "\r\n" + "after: " + after_sign_in);

                            if (in_time == "00:00")
                            {
                                sign_in_hour = Int32.Parse(DateTime.Now.ToString("HH"));
                                sign_in_minute = Int32.Parse(DateTime.Now.ToString("mm"));

                                System.IO.File.AppendAllText(overwrite_file, before_sign_in + sign_in_hour.ToString() + ":" + sign_in_minute.ToString() + after_sign_in + "\r\n");
                                //System.Diagnostics.Debug.WriteLine(before_sign_in + "\r\n" + after_sign_in + "\r\n" + space_count);
                                //System.Diagnostics.Debug.WriteLine(sign_in_start + "\r\n" + sign_in_end);
                            }

                            else
                            {
                                already_signed_in = true;
                                System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                            }
                        }

                        else
                        {
                            System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                        }
                    }
                }
            }

            string[] new_roster = System.IO.File.ReadAllLines(overwrite_file);
            System.IO.File.WriteAllText(roster_file, "");

            foreach (string student in new_roster)
            {
                System.IO.File.AppendAllText(roster_file, student + "\r\n");
            }

            System.IO.File.WriteAllText(overwrite_file, "");

            if (id_error)
            {
                textBox3.ForeColor = Color.Red;
                textBox3.Text = "ID Number Not Found";
                Task.Delay(1000).Wait();
                Sign_Out_Check();
            }

            else if (already_signed_in)
            {
                Sign_Out();
            }

            else
            {
                textBox3.ForeColor = Color.Green;
                textBox3.Text = "Student Signed In";
                textBox2.Text = "";
                Task.Delay(1000).Wait();
                Sign_Out_Check();
            }
        }

        private void Sign_Out()
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string before_sign_in = "",
                   hour = "",
                   minute = "";

            int space_count = 0,
                space_track = 0,
                in_hour = 0,
                in_minute = 0,
                out_hour = 0,
                out_minute = 0,
                total_hours = 0,
                total_minutes = 0,
                hours = 0,
                minutes = 0,
                fourth_space = 0,
                fifth_space = 0;

            bool id_found = false,
                 error = true,
                 sign_in_error = false;

            foreach (string student in roster)
            {
                id_found = false;

                for (int i = 0; i < student.Length; i++)
                {
                    if ((student.Substring(i, 1) == "2") && (id_found == false))
                    {
                        id_found = true;

                        if (student.Substring(i, 7) == textBox2.Text)
                        {
                            error = false;
                            for (int x = 0; x < student.Length; x++)
                            {
                                //System.Diagnostics.Debug.WriteLine(student.Substring(x, 1));
                                if (student.Substring(x, 1) == " ")
                                {
                                    space_count += 1;
                                }

                                if (space_count == 3 && space_track == 0)
                                {
                                    before_sign_in = student.Substring(0, x + 1);
                                    hour = (student.Substring(x + 1, 2));
                                    minute = (student.Substring(x + 4, 2));
                                    space_track += 1;
                                    //System.Diagnostics.Debug.WriteLine("hour, minute: " + hour + "," + minute);
                                }
                                if (space_count == 4 && space_track == 1)
                                {
                                    fourth_space = x;
                                    space_track += 1;
                                }
                                if (space_count == 5 && space_track == 2)
                                {
                                    fifth_space = x;
                                    //System.Diagnostics.Debug.WriteLine("Shit that broke value: " + student.Substring(fifth_space + 1, student.Length - fifth_space - 1));
                                    hours = Int32.Parse(student.Substring(fourth_space + 1, fifth_space - fourth_space - 1));
                                    minutes = Int32.Parse(student.Substring(fifth_space + 1, student.Length - fifth_space - 1));
                                    //System.Diagnostics.Debug.WriteLine("hours before: " + hours + "\r\n" + "minutes before: " + minutes);
                                    space_track += 1;
                                }
                            }
                            in_hour = Int32.Parse(hour);
                            in_minute = Int32.Parse(minute);
                            out_hour = Int32.Parse(DateTime.Now.ToString("HH"));
                            out_minute = Int32.Parse(DateTime.Now.ToString("mm"));

                            if ((in_hour == 00) && (in_minute == 00))
                            {
                                sign_in_error = true;
                                textBox3.ForeColor = Color.Red;
                                textBox3.Text = "Student Not Signed In";
                                //System.Diagnostics.Debug.WriteLine("Student not signed in");
                                System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                            }

                            else
                            {
                                total_hours = (out_hour - in_hour);

                                if (out_minute > in_minute)
                                {
                                    total_minutes += (out_minute - in_minute);
                                }

                                else if (out_minute < in_minute)
                                {
                                    total_hours -= 1;
                                    total_minutes += 60 - (in_minute - out_minute);
                                }

                                if (total_minutes >= 60)
                                {
                                    total_hours += 1;
                                    total_minutes -= 60;
                                }

                                total_hours += hours;
                                total_minutes += minutes;
                                if (minutes >= 60)
                                {
                                    minutes -= 60;
                                    total_hours += 1;
                                }
                                System.IO.File.AppendAllText(overwrite_file, before_sign_in + "00:00 " + total_hours.ToString() + " " + total_minutes.ToString() + "\r\n");
                            }
                        }

                        else
                        {
                            System.IO.File.AppendAllText(overwrite_file, student + "\r\n");
                        }
                    }
                }
            }

            string[] new_roster = System.IO.File.ReadAllLines(overwrite_file);
            System.IO.File.WriteAllText(roster_file, "");

            foreach (string new_student in new_roster)
            {
                System.IO.File.AppendAllText(roster_file, new_student + "\r\n");
            }

            System.IO.File.WriteAllText(overwrite_file, "");

            if (error)
            {
                textBox3.ForeColor = Color.Red;
                textBox3.Text = "Student ID Not Found";
            }

            else if (sign_in_error == false)
            {
                textBox3.ForeColor = Color.Green;
                textBox3.Text = "Student Signed Out";
                textBox2.Text = "";
                Task.Delay(1000).Wait();
                Sign_Out_Check();
            }
        }

        private void Sign_Out_Check() //Checks if everyone has signed out
        {
            string[] roster = System.IO.File.ReadAllLines(roster_file);

            string in_time = "";

            int space_count = 0,
                space_track = 0,
                in_time_start = 0,
                number = 0;

            foreach (string student in roster)
            {
                System.Diagnostics.Debug.WriteLine(student);
                space_count = 0;
                space_track = 0;
                in_time_start = 0;
                in_time = "";

                for (int i = 0; i < student.Length; i++)
                {
                    if (student.Substring(i, 1) == " ")
                    {
                        space_count += 1;
                    }
                    if (space_count == 3 && space_track == 0)
                    {
                        in_time_start = i + 1;
                        space_track += 1;
                        System.Diagnostics.Debug.WriteLine(in_time_start);
                    }
                    else if (space_count == 4 && space_track == 1)
                    {
                        in_time = student.Substring(in_time_start, i - in_time_start);
                        System.Diagnostics.Debug.WriteLine("in_time:" + in_time);
                        space_track += 1;
                    }
                }
                if (in_time != "00:00")
                {
                    number += 1;
                }
            }
            if (number == 0)
            {
                textBox3.ForeColor = Color.Blue;
                textBox3.Text = "All Students Signed Out";
            }
            else if (number == 1)
            {
                textBox3.ForeColor = Color.Orange;
                textBox3.Text = number + " Student Signed In";
            }
            else
            {
                textBox3.ForeColor = Color.Orange;
                textBox3.Text = number + " Students Signed In";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            data.Show();
        }
    }
}