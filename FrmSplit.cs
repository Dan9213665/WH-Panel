using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WH_Panel
{
    public partial class FrmSplit : Form
    {
        public event EventHandler<AdjustmentEventArgs> AdjustmentCompleted;
        public FrmSplit()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 5;
        }
        private WHitem getTheItem()
        {
            WHitem wHitem = new WHitem();
            wHitem = wHitemToSplitFromTheMainForm;
            return wHitem;
        }
        public WHitem wHitemToSplitFromTheMainForm { get; set; }
        private void splitStartLogic(WHitem w)
        {
            if (w != null)
            {
                // Clear any previous columns and data from the DataGridView
                dataGridView1.Columns.Clear();
                dataGridView1.Rows.Clear();
                // Set the DataSource to the object properties
                dataGridView1.DataSource = GetPropertiesAsDataTable(w);
                // Set AutoSizeColumnsMode to make columns fill the available width
                dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                // Set AutoSizeMode for individual columns to adjust to cell content
                foreach (DataGridViewColumn column in dataGridView1.Columns)
                {
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    column.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    // Style the "Stock" column
                    if (column.Name == "Stock")
                    {
                        column.DefaultCellStyle.BackColor = Color.IndianRed;
                        column.DefaultCellStyle.Font = new Font("Arial", 18);
                        column.DefaultCellStyle.ForeColor = Color.White;
                    }
                }
                //dataGridView1.DefaultCellStyle.Font = new Font("Arial", 10); // Adjust "Arial" to the desired font family
                if (w.ReelBagTrayStick != null)
                {
                    string rbg = w.ReelBagTrayStick.ToString();
                    int index = comboBox2.FindStringExact(rbg);
                    if (index != -1)  // Check if the item was found
                    {
                        comboBox2.SelectedIndex = index;
                    }
                    else
                    {
                        // The item was not found in the ComboBox's items
                        // Handle this case accordingly
                    }
                }
            }
            else
            {
                MessageBox.Show("Nope");
                // Handle the case when the object is null
            }
        }
        private DataTable GetPropertiesAsDataTable(object obj)
        {
            DataTable dataTable = new DataTable();
            // Add columns to the DataTable
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                dataTable.Columns.Add(property.Name, typeof(string)); // Assuming all properties are strings
            }
            // Add a single row with property values
            DataRow row = dataTable.NewRow();
            foreach (PropertyInfo property in obj.GetType().GetProperties())
            {
                row[property.Name] = property.GetValue(obj)?.ToString() ?? "";
            }
            dataTable.Rows.Add(row);
            return dataTable;
        }
        private void FrmSplit_Load(object sender, EventArgs e)
        {
            if (getTheItem() != null)
            {
                splitStartLogic(getTheItem());
            }
            else
            {
                MessageBox.Show("Nope");
                // Handle the case when wHitemToSplitFromTheMainForm is null
            }
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                int calc = 0;
                calc = wHitemToSplitFromTheMainForm.Stock - int.Parse(textBox1.Text.ToString());
                if (calc > 0)
                {
                    textBox2.Text = calc.ToString();
                }
                else
                {
                    MessageBox.Show("Qty should be less than " + wHitemToSplitFromTheMainForm.Stock.ToString() + " PCS");
                    textBox1.Clear();
                    textBox2.Clear();
                    textBox1.Focus();
                }
            }
            else
            {
                MessageBox.Show("Input valid qty in A");
                textBox1.Clear();
                textBox2.Clear();
                textBox1.Focus();
            }
        }
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the entered key is a digit or control key (e.g., backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Suppress the non-digit character
            }
        }
        private void btnSplit_Click(object sender, EventArgs e)
        {
            // not yet implemented logic
            WHitem originalItem = wHitemToSplitFromTheMainForm; // Initialize with original values
                                                                //WHitem adjustedItemA = // Initialize with adjusted values
                                                                // WHitem adjustedItemB = // Initialize with adjusted values
            WHitem adjustedItemA = new WHitem
            {
                IPN = wHitemToSplitFromTheMainForm.IPN,
                Manufacturer = wHitemToSplitFromTheMainForm.Manufacturer,
                MFPN = wHitemToSplitFromTheMainForm.MFPN,
                Description = wHitemToSplitFromTheMainForm.Description,
                Stock = int.Parse(textBox1.Text), // Use the value from textBox1
                UpdatedOn = wHitemToSplitFromTheMainForm.UpdatedOn, // Keep original value
                ReelBagTrayStick = comboBox1.Text, // Use the selected value from comboBox1
                SourceRequester = wHitemToSplitFromTheMainForm.SourceRequester
            };
            WHitem adjustedItemB = new WHitem
            {
                IPN = wHitemToSplitFromTheMainForm.IPN,
                Manufacturer = wHitemToSplitFromTheMainForm.Manufacturer,
                MFPN = wHitemToSplitFromTheMainForm.MFPN,
                Description = wHitemToSplitFromTheMainForm.Description,
                Stock = int.Parse(textBox2.Text), // Use the value from textBox1
                UpdatedOn = wHitemToSplitFromTheMainForm.UpdatedOn, // Keep original value
                ReelBagTrayStick = comboBox2.Text, // Use the selected value from comboBox1
                SourceRequester = wHitemToSplitFromTheMainForm.SourceRequester
            };
            AdjustmentCompleted?.Invoke(this, new AdjustmentEventArgs(originalItem, adjustedItemA, adjustedItemB));
            // Close the subform
            this.Close();
        }
    }
}
