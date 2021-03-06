﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace biometrics_server
{
    public partial class frmAttendance : Form
    {
        public frmAttendance()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void frmAttendance_Load(object sender, EventArgs e)
        {
            AttendanceModel.loadRecord(ref lstAttendance, ref lstEmployee);
            AttendanceModel.loadEmployee(ref lstEmployee);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void lstAttendance_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstEmployee_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void lstEmployee_DoubleClick(object sender, EventArgs e)
        {
            AttendanceModel.searchUser(ref lstAttendance, lstEmployee.SelectedItems[0].Text);
            textBox1.Text = lstEmployee.SelectedItems[0].Text;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            AttendanceModel.loadRecord(ref lstAttendance, ref lstEmployee);
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            AttendanceModel.loadRecordWithFilter(ref lstAttendance, dtpStart.Value.ToString("yyyy-MM-dd"), dtpEnd.Value.ToString("yyyy-MM-dd"), textBox1.Text);
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSearch.PerformClick();
            }
        }

        private void dtpStart_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dtpStart_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSearch.PerformClick();
            }
        }

        private void dtpEnd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnSearch.PerformClick();
            }
        }
        

        
    }
}
