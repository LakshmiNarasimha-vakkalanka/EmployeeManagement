using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EmployeeManagement
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string pgsqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["Postgresql"].ToString();
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(pgsqlConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(@"call sp_insert_employee
                                                                    ( 
                                                                        :p_empname,
                                                                        :p_empdept,
                                                                        :p_empsalary,
                                                                        :p_empdob
                                                                    )", npgsqlConnection))
                {
                    cmd.CommandType = CommandType.Text; //
                    cmd.Parameters.AddWithValue("p_empname", DbType.String).Value = txtName.Text;
                    cmd.Parameters.AddWithValue("p_empdept", DbType.String).Value = cmbDept.SelectedItem.ToString();
                    cmd.Parameters.AddWithValue("p_empsalary", DbType.Decimal).Value =Convert.ToDecimal(txtSalary.Text);
                    cmd.Parameters.AddWithValue("p_empdob", DbType.DateTime).Value = datepicketDOB.Value;
                    npgsqlConnection.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
