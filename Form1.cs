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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace AgroBridge
{
    public partial class SellsReport : Form
    {
        private string connectionString = "Data Source=LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public SellsReport()
        {
            InitializeComponent();
        }

        private void SellsReport_Load(object sender, EventArgs e)
        {
            // Fetch order details (OrderID and TotalAmount) from the database
            List<OrderSummary> orderSummaries = GetOrderSummaries();

            // Use StringBuilder to create the formatted string
            StringBuilder sb = new StringBuilder();

            // Loop through each order summary and append the details to the StringBuilder
            foreach (var orderSummary in orderSummaries)
            {
                sb.AppendLine($"OrderID: {orderSummary.OrderID} | Total: {orderSummary.TotalAmount:C}");
            }

            // Set the text of the TextBox with the formatted string
            textBox1.Text = sb.ToString();
        }

        // Method to fetch order summaries from the database
        private List<OrderSummary> GetOrderSummaries()
        {
            List<OrderSummary> orderSummaries = new List<OrderSummary>();

            // Adjust the table name to match your database schema
            string query = "SELECT OrderID, TotalAmount FROM OrderTable";  // Corrected to use 'OrderTable'

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataReader reader = command.ExecuteReader();

                    // Read the data from the database
                    while (reader.Read())
                    {
                        orderSummaries.Add(new OrderSummary
                        {
                            OrderID = reader.GetInt32(0),  // OrderID is assumed to be the first column
                            TotalAmount = reader.GetDouble(1)  // TotalAmount is assumed to be the second column and it's a float
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return orderSummaries;
        }

        // Class to hold order data
        public class OrderSummary
        {
            public int OrderID { get; set; }
            public double TotalAmount { get; set; }  // Use double for the float value in your database
        }



        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormAdminHome formAdminHome = new FormAdminHome();
            formAdminHome.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
