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
    public partial class FormCustomerHome : Form
    {
        private Customer customer;
        private FormLogin formLogin;

        private string connectionString = "Data Source=ASPIRE-5;Initial Catalog=AgroBridgeDB;Integrated Security=True;Encrypt=False;TrustServerCertificate=True";

        public FormCustomerHome(Customer customer, FormLogin formLogin)
        {
            InitializeComponent();
            this.customer = customer;
            this.formLogin = formLogin;
            LoadProductsUI();
        }

        private void LoadProductsUI(List<Product> products = null)
        {
            flowLayoutPanelProducts.Controls.Clear();

            if (products == null)
                products = customer.LoadProducts();

            else if (products.Count == 0)
            {
                MessageBox.Show("No products available at the moment.");
                return;
            }

            foreach (var product in products)
            {
                Panel card = new Panel
                {
                    Width = 250,
                    Height = 150,
                    BorderStyle = BorderStyle.FixedSingle,
                    Margin = new Padding(10),
                    BackColor = Color.WhiteSmoke
                };

                Label lblName = new Label
                {
                    Text = $"Product: {product.ProductName}",
                    Font = new Font("Arial", 10, FontStyle.Bold),
                    Location = new Point(10, 10),
                    AutoSize = true
                };

                Label lblPrice = new Label
                {
                    Text = $"Price: {product.Price} BDT",
                    Location = new Point(10, 40),
                    AutoSize = true
                };

                Label lblProductID = new Label
                {
                    Text = $"ProductID: {product.ProductID}",
                    Location = new Point(120, 40),
                    AutoSize = true
                };

                Label lblQty = new Label
                {
                    Text = $"Quantity: {product.Quantity}",
                    Location = new Point(10, 70),
                    AutoSize = true
                };

                Label lblFarmerID = new Label
                {
                    Text = $"FarmerID: {product.FarmerID}",
                    Location = new Point(120, 70),
                    AutoSize = true
                };

                Button btnAddToCart = new Button
                {
                    Text = "Add to Cart",
                    Location = new Point(10, 100),
                    Width = 80,
                    Tag = product.ProductID
                };
                btnAddToCart.Click += BtnAddToCart_Click;

                card.Controls.Add(lblName);
                card.Controls.Add(lblPrice);
                card.Controls.Add(lblProductID);
                card.Controls.Add(lblQty);
                card.Controls.Add(lblFarmerID);
                card.Controls.Add(btnAddToCart);

                flowLayoutPanelProducts.Controls.Add(card);
            }
        }

        private void BtnAddToCart_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn != null)
            {
                int productId = (int)btn.Tag;
                Product selectedProduct = customer.LoadProducts().FirstOrDefault(p => p.ProductID == productId);

                if (selectedProduct != null)
                {
                    try
                    {
                        customer.AddToCart(selectedProduct);
                        MessageBox.Show($"Product '{selectedProduct.ProductName}' added to cart!");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }

        private void FormCustomerHome_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnCustomerGoBack_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            FormLogin formLogin = this.formLogin;
            formLogin.Show();
        }

        private void txtSearchProduct_TextChanged(object sender, EventArgs e)
        {
            if (customer == null) return;

            string searchTerm = txtSearchProduct.Text;
            List<Product> filteredProducts = customer.SearchProducts(searchTerm);

            LoadProductsUI(filteredProducts);
        }

        private void btnProfile_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCustomerProfile formCustomerProfile = new FormCustomerProfile(this.customer, this);
            formCustomerProfile.Show();
        }

        private void btnGoToYourCart_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormCustomerCart formCustomerCart = new FormCustomerCart(this.customer, this);
            formCustomerCart.Show();
        }

        public void RefreshProducts()
        {
            LoadProductsUI();
        }

    }
}