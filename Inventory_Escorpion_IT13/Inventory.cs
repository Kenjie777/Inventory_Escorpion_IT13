using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_Escorpion_IT13
{
    public partial class Inventory : Form
    {
        public Inventory()
        {
            InitializeComponent();
        }

        private void Inventory_Load(object sender, EventArgs e)
        {
            DisplayProducts();
        }


        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO Products (ProductName, Category, Price, Quantity) VALUES (@name, @cat, @price, @qty)", con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@cat", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@qty", txtQuantity.Text);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product Added Successfully!");
                DisplayProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

 
        private void DisplayProducts()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Products", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    dt.Columns.Add("No", typeof(int));
                    for (int i = 0; i < dt.Rows.Count; i++)
                        dt.Rows[i]["No"] = i + 1;

                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns["No"].DisplayIndex = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a product.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    con.Open();
                    SqlCommand cmd = new SqlCommand(
                        "UPDATE Products SET ProductName=@name, Category=@cat, Price=@price, Quantity=@qty WHERE ProductID=@id", con);

                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@cat", txtCategory.Text);
                    cmd.Parameters.AddWithValue("@price", txtPrice.Text);
                    cmd.Parameters.AddWithValue("@qty", txtQuantity.Text);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product Updated Successfully!");
                DisplayProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a product to delete.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
            string name = txtName.Text;
            string category = txtCategory.Text;
            decimal price = Convert.ToDecimal(txtPrice.Text);
            int qty = Convert.ToInt32(txtQuantity.Text);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    con.Open();

                    SqlCommand archiveCmd = new SqlCommand(
                        "INSERT INTO Archive (ProductName, Category, Price, Quantity) VALUES (@name, @cat, @price, @qty)", con);
                    archiveCmd.Parameters.AddWithValue("@name", name);
                    archiveCmd.Parameters.AddWithValue("@cat", category);
                    archiveCmd.Parameters.AddWithValue("@price", price);
                    archiveCmd.Parameters.AddWithValue("@qty", qty);
                    archiveCmd.ExecuteNonQuery();

                    SqlCommand deleteCmd = new SqlCommand(
                        "DELETE FROM Products WHERE ProductID=@id", con);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product moved to Archive!");
                DisplayProducts();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtCategory.Clear();
            txtPrice.Clear();
            txtQuantity.Clear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["ProductName"].Value.ToString();
                txtCategory.Text = dataGridView1.Rows[e.RowIndex].Cells["Category"].Value.ToString();
                txtPrice.Text = dataGridView1.Rows[e.RowIndex].Cells["Price"].Value.ToString();
                txtQuantity.Text = dataGridView1.Rows[e.RowIndex].Cells["Quantity"].Value.ToString();
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            DisplayProducts();
        }


        private void btnProducts_Click(object sender, EventArgs e)
        {

        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
            Archive archiveForm = new Archive();
            archiveForm.Show();
            this.Hide();
        }
    }
}
