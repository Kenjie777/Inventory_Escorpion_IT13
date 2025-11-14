using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Inventory_Escorpion_IT13
{
    public partial class Archive : Form
    {
        public Archive()
        {
            InitializeComponent();
        }

        private void Archive_Load(object sender, EventArgs e)
        {
            DisplayArchive();
        }

        private void DisplayArchive()
        {
            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    SqlDataAdapter da = new SqlDataAdapter("SELECT * FROM Archive", con);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    dataGridView1.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnRestore_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select a product to restore.");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["ProductID"].Value);
            string name = dataGridView1.SelectedRows[0].Cells["ProductName"].Value.ToString();
            string category = dataGridView1.SelectedRows[0].Cells["Category"].Value.ToString();
            decimal price = Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["Price"].Value);
            int qty = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Quantity"].Value);

            try
            {
                using (SqlConnection con = new SqlConnection(@"Data Source=DESKTOP-QOQT9RV;Initial Catalog=DB_Inventory_Escorpion_IT13;Integrated Security=True"))
                {
                    con.Open();


                    SqlCommand restoreCmd = new SqlCommand(
                        "INSERT INTO Products (ProductName, Category, Price, Quantity) VALUES (@name, @cat, @price, @qty)", con);
                    restoreCmd.Parameters.AddWithValue("@name", name);
                    restoreCmd.Parameters.AddWithValue("@cat", category);
                    restoreCmd.Parameters.AddWithValue("@price", price);
                    restoreCmd.Parameters.AddWithValue("@qty", qty);
                    restoreCmd.ExecuteNonQuery();


                    SqlCommand deleteCmd = new SqlCommand(
                        "DELETE FROM Archive WHERE ProductID=@id", con);
                    deleteCmd.Parameters.AddWithValue("@id", id);
                    deleteCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Product Restored Successfully!");
                DisplayArchive();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnProducts_Click(object sender, EventArgs e)
        {
            Inventory inventoryForm = new Inventory();
            inventoryForm.Show();
            this.Hide();
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {

        }
    }
}
