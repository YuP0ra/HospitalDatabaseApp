﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HospitalDatabaseApp
{
    public partial class PatientFrom : Form
    {
        List<string> unames;

        public PatientFrom()
        {
            InitializeComponent();
        }

        private void PatientFrom_Load(object sender, EventArgs e)
        {
            Program.UpdateExcuterCommand("SELECT firstName, lastName  FROM  Users WHERE username = @uname");
            Program.commandExcuter.Parameters.AddWithValue("@uname", Program.username);
            Program.StartReader();

            if (Program.reader.HasRows)
            {
                Program.reader.Read();
                this.Text = string.Format("Hello Mr/s {0} {1}.", Program.reader["firstName"], Program.reader["lastName"]);
            }


            Program.UpdateExcuterCommand("SELECT DISTINCT specialization  FROM  Users");
            Program.StartReader();

            if (Program.reader.HasRows)
            {
                while (Program.reader.Read())
                {
                    if (Program.reader["specialization"].ToString() != "")
                        listBox1.Items.Add(Program.reader["specialization"].ToString());
                }
            }

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            Program.UpdateExcuterCommand("SELECT username, firstName, lastName  FROM  Users WHERE Users.specialization = @spz");
            Program.commandExcuter.Parameters.AddWithValue("@spz", listBox1.SelectedItem.ToString());
            Program.StartReader();

            if (Program.reader.HasRows)
            {
                unames = new List<string>();

                while (Program.reader.Read())
                {
                    unames.Add(Program.reader["username"].ToString());
                    listBox2.Items.Add(string.Format("Dr {0} {1}", Program.reader["firstName"], Program.reader["lastName"]));
                }
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex == -1)
                return;

            Program.UpdateExcuterCommand("INSERT INTO MedicalAppointments (doctorUsername, patientUsername, complainment) VALUES (@pra1, @pra2, @pra3)");
            Program.commandExcuter.Parameters.AddWithValue("@pra1", unames[listBox2.SelectedIndex]);
            Program.commandExcuter.Parameters.AddWithValue("@pra2", Program.username);
            Program.commandExcuter.Parameters.AddWithValue("@pra3", richTextBox1.Text);

            try
            {
                Program.Excute();
                MessageBox.Show("Done Succeccfully!");
            }
            catch (SqlException)
            {
                MessageBox.Show("Somthing went wrong! Please try again later.");
            }

        }
    }
}
