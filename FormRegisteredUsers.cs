using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgroBridge
{
    public partial class FormRegisteredUsers : Form
    {
        private FormAdminHome formAdminHome;
        private DataTable dt = new DataTable();
        public FormRegisteredUsers()
        {
            InitializeComponent();
        }

        public FormRegisteredUsers(FormAdminHome formAdminHome)
        {
            InitializeComponent();
            this.formAdminHome = formAdminHome;
        }

        private void FormRegisteredUsers_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void FormRegisteredUsers_Load(object sender, EventArgs e)
        {
            string sql = "SELECT ID, Username, Name, Role, Status FROM RegistrationTable ORDER BY ID";
            SqlConnection sqlCon = new SqlConnection("Data Source= LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
            sqlCon.Open();
            SqlCommand sqlcmd = new SqlCommand(sql, sqlCon);
            SqlDataAdapter sda = new SqlDataAdapter(sqlcmd);
            sda.Fill(dt);

            dataGridViewRegUsers.DataSource = dt;
            dataGridViewRegUsers.ReadOnly = true;

            foreach(DataGridViewColumn col in dataGridViewRegUsers.Columns)
            {
                col.ReadOnly = true;
            }
            dataGridViewRegUsers.Columns["Status"].ReadOnly = true;

            dataGridViewRegUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridViewRegUsers.MultiSelect = false;
            dataGridViewRegUsers.ReadOnly = false;

            sqlCon.Close();
        }

        private void txtSearchUsername_TextChanged(object sender, EventArgs e)
        {
            DataView dv = dt.DefaultView;
            dv.RowFilter = $"Username LIKE '%{txtSearchUsername.Text}%'";
            dataGridViewRegUsers.DataSource = dv;
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (dataGridViewRegUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridViewRegUsers.SelectedRows[0];
                string username = selectedRow.Cells["Username"].Value.ToString();

                string currentStatus = selectedRow.Cells["Status"].Value.ToString();
                string newStatus = currentStatus == "0" ? "1" : "0";

                SqlConnection sqlCon = new SqlConnection("Data Source=LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
                sqlCon.Open();

                string updateSQL = "UPDATE RegistrationTable SET Status = @Status WHERE Username = @Username";
                SqlCommand sqlCmd = new SqlCommand(updateSQL, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Status", newStatus);
                sqlCmd.Parameters.AddWithValue("@Username", username);

                int rowsAffected = sqlCmd.ExecuteNonQuery();
                sqlCon.Close();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Status updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    selectedRow.Cells["Status"].Value = newStatus;
                }
                else
                {
                    MessageBox.Show("Update failed!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a row first.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            formAdminHome.Show();
        }

        private void lblSearchByUsername_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string idToDelete = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(idToDelete))
            {
                MessageBox.Show("Please enter an ID to delete.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            SqlConnection sqlCon = new SqlConnection("Data Source=LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True");
            sqlCon.Open();

            string deleteSQL = "DELETE FROM RegistrationTable WHERE ID = @ID";
            SqlCommand sqlCmd = new SqlCommand(deleteSQL, sqlCon);
            sqlCmd.Parameters.AddWithValue("@ID", idToDelete);

            int rowsAffected = sqlCmd.ExecuteNonQuery();
            sqlCon.Close();

            if (rowsAffected > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (row["ID"].ToString() == idToDelete)
                    {
                        dt.Rows.Remove(row);
                        break;
                    }
                }

                MessageBox.Show("Record deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridViewRegUsers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
