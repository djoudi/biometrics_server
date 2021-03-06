﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Windows.Forms;
using System.Globalization;

namespace biometrics_server
{
    class AttendanceModel
    {
        /// <summary>
        /// Load employee record
        /// </summary>
        /// <param name="lstEmp">Listview</param>
        public static void loadEmployee(ref ListView lstEmp)
        {
            try{
                using (MySqlConnection con = new MySqlConnection(biometrics_server.Config.getConnectionString()))
                {
                    con.Open();
                    string sql = "SELECT * FROM user WHERE user_active = 1";

                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    MySqlDataReader reader = cmd.ExecuteReader();

                    lstEmp.Items.Clear();
                    while (reader.Read())
                    {
                        lstEmp.Items.Add(reader["user_id"].ToString());
                        lstEmp.Items[lstEmp.Items.Count - 1].SubItems.Add(reader["user_name"].ToString());
                    }
                }
            }catch(MySqlException ex){
                //loading of employee failed
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
           
        }
        

   
       /// <summary>
       /// Search for a user
       /// </summary>
       /// <param name="lst">Listview</param>
       /// <param name="id">String</param>
        public static void searchUser(ref ListView lst, String id)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(biometrics_server.Config.getConnectionString()))
                {
                    con.Open();
                    string sql = "SELECT * FROM attendance INNER JOIN user ON user_id = attendance_employee WHERE user_active = 1 AND attendance_employee = @id ORDER BY attendance_date ASC";

                    MySqlCommand cmd = new MySqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("id", id);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    lst.Items.Clear();
                    while (reader.Read())
                    {
                        lst.Items.Add(reader["attendance_employee"].ToString());
                        DateTime aDate;
                        DateTime.TryParse(reader["attendance_date"].ToString(), out aDate);
                        lst.Items[lst.Items.Count - 1].SubItems.Add(reader["user_name"].ToString());
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("hh:mm:ss tt"));
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("error" + ex.ToString(), "error");
            }
        }

        /// <summary>
        /// Load Record with filters
        /// </summary>
        /// <param name="lst">ListView</param>
        /// <param name="lstEmp">ListView</param>
        public static void loadRecord(ref ListView lst, ref ListView lstEmp)
        {
            try
            {
                using(MySqlConnection con = new MySqlConnection(biometrics_server.Config.getConnectionString()))
                {
                    con.Open();

                    string sql = "SELECT * FROM attendance INNER JOIN user ON user_id = attendance_employee WHERE user_active = 1 ORDER BY attendance_date ASC";
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    lst.Items.Clear();
                    while(reader.Read())
                    {
                        lst.Items.Add(reader["attendance_employee"].ToString());
                        DateTime aDate;
                        DateTime.TryParse(reader["attendance_date"].ToString(), out aDate);
                        lst.Items[lst.Items.Count - 1].SubItems.Add(reader["user_name"].ToString());
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("hh:mm:ss tt"));
                    }

                }
            }
            catch(MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Loading record with date and ID filter
        /// </summary>
        /// <param name="lst">ListView</param>
        /// <param name="start">String</param>
        /// <param name="end">String</param>
        /// <param name="empID">String</param>
        public static void loadRecordWithFilter(ref ListView lst, string start, string end, string empID)
        {
            try
            {
                using (MySqlConnection con = new MySqlConnection(biometrics_server.Config.getConnectionString()))
                {
                    con.Open();

                    string sql = "";

                    if(empID == "")
                    {
                        sql = "SELECT * FROM attendance INNER JOIN user ON user_id = attendance_employee WHERE user_active = 1 AND DATE_FORMAT(attendance_date,'%Y-%m-%d') BETWEEN @start AND @end ORDER BY attendance_date ASC";
                    }
                    else
                    {
                        sql = "SELECT * FROM attendance INNER JOIN user ON user_id = attendance_employee WHERE user_active = 1 AND attendance_employee = @empID AND attendance_date BETWEEN @start AND @end ORDER BY attendance_date ASC";
                    }
                    
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    //parameters
                    if (empID != "")
                    {
                        cmd.Parameters.AddWithValue("empID", empID);
                    }
                    cmd.Parameters.AddWithValue("start", start);
                    cmd.Parameters.AddWithValue("end", end);

                    MySqlDataReader reader = cmd.ExecuteReader();

                    lst.Items.Clear();
                    while (reader.Read())
                    {
                        lst.Items.Add(reader["attendance_employee"].ToString());
                        DateTime aDate;
                        DateTime.TryParse(reader["attendance_date"].ToString(), out aDate);
                        lst.Items[lst.Items.Count - 1].SubItems.Add(reader["user_name"].ToString());
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("MMMM dd, yyyy"));
                        lst.Items[lst.Items.Count - 1].SubItems.Add(aDate.ToString("hh:mm:ss tt"));
                    }

                   // MessageBox.Show(start);

                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
        }

        /// <summary>
        /// Insert new record in the attendance
        /// </summary>
        /// <param name="attendance_employee">int</param>
        /// <param name="attendance_date">String</param>
        /// <param name="attendance_active">int</param>
        /// <param name="attendance_type">String</param>
        /// <param name="attedance_flag">int</param>
        /// <param name="attendance_created">String</param>
        /// <param name="attendace_updated">String</param>
        public static void insertRecord(int attendance_employee, string attendance_date, int attendance_active, string attendance_type, int attedance_flag, string attendance_created, string attendace_updated)
        {
            try
            {
                using(MySqlConnection con = new MySqlConnection(biometrics_server.Config.getConnectionString()))
                {
                    con.Open();

                    string sql = "INSERT INTO attendance (attendance_employee, attendance_date, attendance_active, attendance_type, attedance_flag, attendance_created, attendace_updated) VALUES (@attendance_employee, @attendance_date, @attendance_active, @attendance_type, @attedance_flag, @attendance_created, @attendace_updated)";
                    MySqlCommand cmd = new MySqlCommand(sql, con);

                    //parameters
                    cmd.Parameters.AddWithValue("attendance_employee", attendance_employee);
                    cmd.Parameters.AddWithValue("attendance_date", attendance_date);
                    cmd.Parameters.AddWithValue("attendance_active", attendance_active);
                    cmd.Parameters.AddWithValue("attendance_type", attendance_type);
                    cmd.Parameters.AddWithValue("attedance_flag", attedance_flag);
                    cmd.Parameters.AddWithValue("attendance_created", attendance_created);
                    cmd.Parameters.AddWithValue("attendace_updated", attendace_updated);

                    cmd.ExecuteNonQuery();
                }
            }
            catch(MySqlException ex)
            {
                //inserting new record failed
                MessageBox.Show("Error: " + ex.Message.ToString());
            }
            
        }

        
    }
}
