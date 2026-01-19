using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgroBridge
{
    public partial class FormCustomerOrders : Form
    {
        private string connectionString = "Data Source=LAPTOP-40VLFED3\\SQLEXPRESS;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";
        public FormCustomerOrders(Customer customer)
        {

            InitializeComponent();
            flowLayoutPanelOrders.AutoScroll = true;


            var orders = Order.GetOrdersByCustomer(customer.CustomerID, connectionString);

            foreach (var order in orders)
            {
                Panel card = new Panel();
                card.Width = 350;
                card.Height = 150;
                card.BorderStyle = BorderStyle.FixedSingle;

                Label lblInfo = new Label();
                lblInfo.AutoSize = true;
                lblInfo.Text = $"OrderID: {order.OrderID}\nDate: {order.OrderDate}\nTotal: {order.TotalAmount}";
                string productsText = "\nProducts:\n";
                
                card.Controls.Add(lblInfo);

                flowLayoutPanelOrders.Controls.Add(card);
            }
        }

        private void FormCustomerOrders_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void flowLayoutPanelOrders_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            Application.Exit(); 

        }
    }
}
