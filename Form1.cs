using Npgsql;
using NpgsqlTypes;
using System;
using System.Data;
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
                    MessageBox.Show("Employee data inserted successfully...");
                    BindGrid();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            BindGrid();
        }

        private void BindGrid()
        {
            DataTable dtEmployees = GetEmployeesByNamdAndDept(txtempsearch.Text, txtdeptsearch.Text);
            dgvEmployees.DataSource = dtEmployees;
        }

        private DataTable GetEmployeesByNamdAndDept(string Name, string Department)
        {
            DataTable dtEmployees = new DataTable();
            string pgsqlConnection = System.Configuration.ConfigurationManager.ConnectionStrings["Postgresql"].ToString();
            using (NpgsqlConnection npgsqlConnection = new NpgsqlConnection(pgsqlConnection))
            {
                using (NpgsqlCommand cmd = new NpgsqlCommand(@"SELECT * FROM fn_get_employees
                                                                    ( 
                                                                        :p_empname,
                                                                        :p_empdept
                                                                    )", npgsqlConnection))
                {
                    cmd.CommandType = CommandType.Text; //
                    if (!String.IsNullOrEmpty(Name))
                        cmd.Parameters.AddWithValue("p_empname", DbType.String).Value = Name;
                    else
                        cmd.Parameters.AddWithValue("p_empname", DbType.String).Value = DBNull.Value;

                    if(!String.IsNullOrEmpty(Department))
                        cmd.Parameters.AddWithValue("p_empdept", DbType.String).Value = Department;
                    else
                        cmd.Parameters.AddWithValue("p_empdept", DbType.String).Value = DBNull.Value;
                    npgsqlConnection.Open();

                    NpgsqlDataAdapter dataAdapter = new NpgsqlDataAdapter(cmd);                    
                    dataAdapter.Fill(dtEmployees);
                }
            }
            return dtEmployees;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
        }
    }
}
