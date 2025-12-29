using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace WH_Panel
{
    public partial class FrmRollbackITP : Form
    {
        private string _databaseName;
        private string _kitName;
        private string kitQty;
        private string sourseReqkitName;

        public FrmRollbackITP(string title)
        {
            InitializeComponent();

            ParseTitle(title, out _databaseName, out sourseReqkitName);

            MessageBox.Show("DB: " + _databaseName);
            MessageBox.Show("Kit: " + sourseReqkitName);

            LoadGrid();
        }

        private void ParseTitle(string title, out string dbName, out string sourseReqkitName)
        {
            dbName = string.Empty;
            _kitName = string.Empty;
            kitQty = string.Empty;
            sourseReqkitName = string.Empty;

            if (string.IsNullOrWhiteSpace(title))
                return;

            int lastSlash = title.LastIndexOf('\\');
            int xlsmIndex = title.LastIndexOf(".xlsm", StringComparison.OrdinalIgnoreCase);

            if (lastSlash >= 0 && xlsmIndex > lastSlash)
            {
                string filePart = title.Substring(lastSlash + 1, xlsmIndex - lastSlash - 1);

                var parts = filePart.Split('_');
                dbName = parts.Length > 0 ? parts[0] : string.Empty;
                _kitName = parts.Length > 1 ? parts[1] : string.Empty;
                kitQty = parts.Length > 2 ? parts[2] : string.Empty;
                sourseReqkitName = dbName + "_" + _kitName + "_" + kitQty;
            }
        }

        private void LoadGrid()
        {
            try
            {
                string connectionString = $"Data Source=DBR3\\SQLEXPRESS;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Use parameterized query with sourseReqkitName
                    string query = $"SELECT * FROM {_databaseName}.dbo.STOCK WHERE Source_Requester = @Source_Requester";

                    using (SqlDataAdapter adapter = new SqlDataAdapter(query, conn))
                    {
                        adapter.SelectCommand.Parameters.AddWithValue("@Source_Requester", sourseReqkitName);

                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        dataGridView1.DataSource = dt;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading grid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select at least one row to delete.", "No selection", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirm = MessageBox.Show("Are you sure you want to delete the selected row(s)?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm != DialogResult.Yes)
                return;

            try
            {
                string connectionString = $"Data Source=DBR3\\SQLEXPRESS;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        if (row.Cells["Id"].Value == null)
                            continue;

                        int id = Convert.ToInt32(row.Cells["Id"].Value);

                        using (SqlCommand cmd = new SqlCommand($"DELETE FROM {_databaseName}.dbo.STOCK WHERE Id = @Id", conn))
                        {
                            cmd.Parameters.AddWithValue("@Id", id);
                            cmd.ExecuteNonQuery();
                        }
                    }
                }

                MessageBox.Show("Selected row(s) deleted successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Reload grid after deletion
                LoadGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error deleting row(s): {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
