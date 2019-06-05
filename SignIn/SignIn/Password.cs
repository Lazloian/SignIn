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

//TODO Password for signing evryone out

namespace SignIn
{
    public partial class Password : Form
    {
        EditRoster editRoster = new EditRoster();

        string settings_file = @"C:\Users\Public\Documents\Settings.txt";

        public Password()
        {
            InitializeComponent();
        }

        private void Password_Load(object sender, EventArgs e)
        {
            textBox2.PasswordChar = '*';
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e) // Enter
        {                                                                     
            if (e.KeyCode == Keys.Enter)
            {
                password_check();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            password_check();
        }

        private void password_check()
        {
            string[] settings = System.IO.File.ReadAllLines(settings_file);

            int setting_end = 0;

            string password = "";

            foreach (string setting in settings)
            {
                for (int i = 0; i < setting.Length; i++)
                {
                    if (setting.Substring(i, 1) == ":")
                    {
                        setting_end = i;

                        if (setting.Substring(0, setting_end) == "Password")
                        {
                            password = setting.Substring(setting_end + 1, setting.Length - setting_end - 1);
                            System.Diagnostics.Debug.WriteLine(password);

                            if (textBox2.Text == password)
                            {
                                editRoster.Show();
                                textBox3.Text = "";
                                this.Hide();
                            }

                            else
                            {
                                textBox3.ForeColor = Color.Red;
                                textBox3.Text = "Password Incorrect";
                            }
                        }
                    }
                }
            }
            textBox2.Text = "";
        }
    }
}