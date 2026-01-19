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
    public partial class FormUpdateProduct : Form
    {
        private string connectionString = "Data Source=LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
        private Farmer farmer;
        private FormFarmerHome formFarmerHome;
        public FormUpdateProduct(int farmerID)
        {
            InitializeComponent();
            farmer = new Farmer(farmerID);
        }

        public FormUpdateProduct(FormFarmerHome formFarmerHome, int farmerID)
        {
            InitializeComponent();
            this.formFarmerHome = formFarmerHome;
            this.farmer = new Farmer(farmerID);
        }

        private void FormUpdateProduct_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {
            this.Hide();
            formFarmerHome.Show();
        }

        private void lblProductID_Click(object sender, EventArgs e)
        {

        }

        private void FormUpdateProduct_Load(object sender, EventArgs e)
        {
            dataGridViewProduct.Hide();
            lblNewName.Hide();
            lblNewPrice.Hide();
            lblNewQuantity.Hide();
            txtNewName.Hide();
            txtNewPrice.Hide();
            txtNewQuantity.Hide();
            btnUpdate.Hide();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int ProductID;
            try
            {
                ProductID = int.Parse(txtSearchProductID.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid numeric Product ID.");
                this.txtSearchProductID.Clear();
                txtSearchProductID.Focus();
                return;
            }

            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();

                string query = "SELECT * FROM ProductTable WHERE ProductID = @pid";
                SqlCommand cmd = new SqlCommand(query, sqlCon);
                cmd.Parameters.AddWithValue("@pid", ProductID);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    MessageBox.Show("Product is not available, try a different one!");
                    this.txtSearchProductID.Clear();
                    txtSearchProductID.Focus();
                    return;
                }

                int ownerId = Convert.ToInt32(dt.Rows[0]["FarmerID"]);
                if (ownerId != farmer.FarmerID)
                {
                    MessageBox.Show("You cannot update this product.");
                    this.txtSearchProductID.Clear();
                    txtSearchProductID.Focus();
                    return;
                }

                dataGridViewProduct.Visible = true;
                dataGridViewProduct.DataSource = dt;

                txtSearchProductID.Visible = false;
                btnSearch.Visible = false;
                lblProductID.Visible = false;

                txtNewName.Visible = true;
                txtNewPrice.Visible = true;
                txtNewQuantity.Visible = true;
                btnUpdate.Visible = true;
                lblNewName.Visible = true;
                lblNewPrice.Visible = true;
                lblNewQuantity.Visible = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int productId = Convert.ToInt32(dataGridViewProduct.Rows[0].Cells["ProductID"].Value);

            string newName;
            if (string.IsNullOrWhiteSpace(txtNewName.Text))
            {
                newName = null;
            }
            else
            {
                newName = txtNewName.Text;
            }

            int newPrice = -1;
            int newQuantity = -1;

            if (!string.IsNullOrWhiteSpace(txtNewPrice.Text))
                int.TryParse(txtNewPrice.Text, out newPrice);

            if (!string.IsNullOrWhiteSpace(txtNewQuantity.Text))
                int.TryParse(txtNewQuantity.Text, out newQuantity);

            farmer.UpdateProduct(productId, newName, newPrice, newQuantity);

            dataGridViewProduct.Hide();
            lblNewName.Hide();
            lblNewPrice.Hide();
            lblNewQuantity.Hide();
            txtNewName.Hide();
            txtNewPrice.Hide();
            txtNewQuantity.Hide();
            btnUpdate.Hide();

            txtSearchProductID.Visible = true;
            btnSearch.Visible = true;
            lblProductID.Visible = true;

            this.txtNewName.Clear();
            this.txtNewPrice.Clear();
            this.txtNewQuantity.Clear();
            txtNewName.Focus();

            this.txtSearchProductID.Clear();
            txtSearchProductID.Focus();
        }
    }
}