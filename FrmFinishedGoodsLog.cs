using FastMember;
using Microsoft.Office.Interop.Excel;
using Seagull.Framework.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Range = Microsoft.Office.Interop.Excel.Range;
namespace WH_Panel
{
    public partial class FrmFinishedGoodsLog : Form
    {
        public System.Data.DataTable PackedItemsUDtable = new System.Data.DataTable();
        public List<FinishedGoodsItem> PackedItemsList = new List<FinishedGoodsItem>();
        public string initialPath = @"\\dbr1\Data\Aegis_NPI_Projects\";
        public int counter = 0;
        int limit = 0;
        public FrmFinishedGoodsLog()
        {
            InitializeComponent();
            populateComboBox(comboBox1, initialPath);
        }
        public void populateComboBox(ComboBox cb, string path)
        {
            cb.Items.Clear();
            cb.Text = string.Empty;
            //string path = @"\\dbr1\Data\Aegis_NPI_Projects\"; // replace with your folder path
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                cb.Items.Add(Path.GetFileName(folder));
            }
        }
        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkBox1.Checked = true;
            comboBox1.Enabled = false;
            populateComboBox(comboBox2, initialPath + comboBox1.SelectedItem.ToString());
            comboBox2.DroppedDown = true;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checboxLockUnlock(sender, comboBox1);
        }
        private void checboxLockUnlock(object sender, ComboBox cbm)
        {
            if (((System.Windows.Forms.CheckBox)sender).Checked)
            {
                cbm.Enabled = false;
            }
            else
            {
                cbm.Enabled = true;
            }
        }
        private void comboBox2_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkBox2.Checked = true;
            comboBox2.Enabled = false;
            populateComboBox(comboBox3, initialPath + comboBox1.SelectedItem.ToString() + "\\" + comboBox2.SelectedItem.ToString());
            if (comboBox3.Items.Count > 0)
            {
                comboBox3.DroppedDown = true;
            }
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checboxLockUnlock(sender, comboBox2);
        }
        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            comboBox3.Enabled = false;
            txtbComments.Focus();
        }
        public string AddQuotesIfRequired(string path)
        {
            return !string.IsNullOrWhiteSpace(path) ?
                path.Contains(" ") && (!path.StartsWith("\"") && !path.EndsWith("\"")) ?
                    "\"" + path + "\"" : path :
                    string.Empty;
        }
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            checboxLockUnlock(sender, comboBox3);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            addFinishedIGoodsItemToList();
        }
        private void addFinishedIGoodsItemToList()
        {
            if (PackedItemsList.Count < limit)
            {
                if (string.IsNullOrEmpty(comboBox1.Text))
                {
                    MessageBox.Show("Please select a CUSTOMER !");
                    comboBox1.Focus();
                    comboBox1.DroppedDown = true;
                    return;
                }
                if (string.IsNullOrEmpty(comboBox2.Text))
                {
                    MessageBox.Show("Please select a PROJECT !");
                    comboBox2.Focus();
                    comboBox2.DroppedDown = true;
                    return;
                }
                if (string.IsNullOrEmpty(comboBox3.Text) && comboBox3.Items.Count != 0)
                {
                    MessageBox.Show("Please select a REVISION !");
                    comboBox3.Focus();
                    comboBox3.DroppedDown = true;
                    return;
                }
                if (string.IsNullOrEmpty(txtbSN.Text))
                {
                    MessageBox.Show("Please input a serial number!");
                    txtbSN.Focus();
                    return;
                }
                var serialNumber = txtbSN.Text.Trim();
                if (PackedItemsList.Any(item => item.serialNumber == serialNumber))
                {
                    MessageBox.Show($"Serial number '{serialNumber}' already exists!");
                    txtbSN.Focus();
                    return;
                }
                var fg = new FinishedGoodsItem
                {
                    Customer = comboBox1.Text,
                    Project = comboBox2.Text,
                    Revision = comboBox3.Text,
                    serialNumber = txtbSN.Text.Trim(),
                    packedDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"),
                    Comments = txtbComments.Text.Trim()
                };
                PackedItemsList.Insert(0, fg);
                lblCounter.Text = $"QTY: {++counter} / {limit}";
                PopulatePackedItemsGridView();
            }
            else
            {
                MessageBox.Show(String.Format("Limit of {0} items reached !", limit));
            }
        }
        private void PopulatePackedItemsGridView()
        {
            PackedItemsUDtable.Clear();
            IEnumerable<FinishedGoodsItem> data = PackedItemsList;
            using (var reader = ObjectReader.Create(data))
            {
                PackedItemsUDtable.Load(reader);
            }
            dataGridView1.DataSource = PackedItemsUDtable;
            SetColumsOrder(dataGridView1);
            dataGridView1.Update();
        }
        private void SetColumsOrder(DataGridView dgw)
        {
            dgw.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns["Customer"].DisplayIndex = 0;
            dgw.Columns["Project"].DisplayIndex = 1;
            dgw.Columns["Revision"].DisplayIndex = 2;
            dgw.Columns["serialNumber"].DisplayIndex = 3;
            dgw.Columns["packedDate"].DisplayIndex = 4;
            dgw.Columns["Comments"].DisplayIndex = 5;
        }
        private void txtbSN_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                addFinishedIGoodsItemToList();
                txtbSN.Clear();
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            string saveToPath = string.Empty;
            if (comboBox3.SelectedIndex != -1)
            {
                saveToPath = initialPath + comboBox1.SelectedItem.ToString() + "\\" + comboBox2.SelectedItem.ToString() + "\\" + comboBox3.SelectedItem.ToString();
                EXCELinserter(PackedItemsList, saveToPath);
            }
            else
            {
                saveToPath = initialPath + comboBox1.SelectedItem.ToString() + "\\" + comboBox2.SelectedItem.ToString();
                EXCELinserter(PackedItemsList, saveToPath);
            }
        }
        private void EXCELinserter(List<FinishedGoodsItem> lst, string saveToPa)
        {
            try
            {
                lst.Sort((x, y) => string.Compare(x.serialNumber, y.serialNumber));
                string client = lst[0].Customer;
                string fp = "\\\\dbr1\\Data\\WareHouse\\PACKING_SLIPS\\_templateFinishedGoods.xlsm";
                _Application docExcel = new Microsoft.Office.Interop.Excel.Application();
                docExcel.Visible = false;
                docExcel.DisplayAlerts = false;
                _Workbook workbooksExcel = docExcel.Workbooks.Open(@fp, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
                _Worksheet worksheetExcel = (_Worksheet)workbooksExcel.Worksheets["FGR"];
                int startRow = 12;
                string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                ((Range)worksheetExcel.Cells[1, "A"]).Value2 = "FGR_" + _fileTimeStamp;
                ((Range)worksheetExcel.Cells[2, "D"]).Value2 = DateTime.Now.ToString("yyyy-MM-dd");
                ((Range)worksheetExcel.Cells[8, "B"]).Value2 = client;
                ((Range)worksheetExcel.Cells[8, "E"]).Value2 = counter;
                for (int i = 0; i < lst.Count; i++)
                {
                    ((Range)worksheetExcel.Cells[startRow + i, "A"]).Value2 = lst[i].Project.ToString();
                    ((Range)worksheetExcel.Cells[startRow + i, "A"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "B"]).Value2 = lst[i].Revision.ToString();
                    ((Range)worksheetExcel.Cells[startRow + i, "B"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "C"]).Value2 = lst[i].serialNumber;
                    ((Range)worksheetExcel.Cells[startRow + i, "C"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "D"]).Value2 = lst[i].packedDate;
                    ((Range)worksheetExcel.Cells[startRow + i, "D"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                    ((Range)worksheetExcel.Cells[startRow + i, "E"]).Value2 = lst[i].Comments;
                    ((Range)worksheetExcel.Cells[startRow + i, "E"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                }
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 1, "A"]).Value2 = "Comments:                                ";
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 1, 1], worksheetExcel.Cells[startRow + lst.Count + 1, 5]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 1, 1], worksheetExcel.Cells[startRow + lst.Count + 1, 5]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 2, "A"]).Value2 = "Signature_______________________ חתימה     DATE ______/______/2023  תאריך      NAME ________________________________  שם";
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 2, "A"]).WrapText = true;
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 2, 1], worksheetExcel.Cells[startRow + lst.Count + 2, 5]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 2, 1], worksheetExcel.Cells[startRow + lst.Count + 2, 5]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 2, 1], worksheetExcel.Cells[startRow + lst.Count + 2, 5]].RowHeight = 40;
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 3, "A"]).Value2 = "Thank You";
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 3, 1], worksheetExcel.Cells[startRow + lst.Count + 3, 5]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 3, 1], worksheetExcel.Cells[startRow + lst.Count + 3, 5]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 4, "A"]).Value2 = "if you have any questions or concerns, please contact  Vlad Berezin, (972) 525118807, vlad@robotron.co.il";
                ((Range)worksheetExcel.Cells[startRow + lst.Count + 4, "A"]).BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 4, 1], worksheetExcel.Cells[startRow + lst.Count + 4, 5]].Merge();
                worksheetExcel.Range[worksheetExcel.Cells[startRow + lst.Count + 4, 1], worksheetExcel.Cells[startRow + lst.Count + 4, 5]].BorderAround(XlLineStyle.xlContinuous, XlBorderWeight.xlMedium, XlColorIndex.xlColorIndexAutomatic);
                workbooksExcel.SaveAs(saveToPa + "\\" + _fileTimeStamp + "_" + lst[0].Customer + "_" + lst[0].Project + "_" + lst[0].Revision + "_" + counter.ToString() + "PCS" + ".xlsm");
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = saveToPa,
                    UseShellExecute = true,
                    Verb = "open"
                });
                workbooksExcel.Close(false, Type.Missing, Type.Missing);
                docExcel.Application.DisplayAlerts = false;
                docExcel.Application.Quit();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Remove the line ?", "DELETE the line from the list", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string cellValue = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
                string selIPN = dataGridView1.Rows[rowindex].Cells["serialNumber"].Value.ToString();
                string selMFPN = dataGridView1.Rows[rowindex].Cells["packedDate"].Value.ToString();
                PackedItemsList.Remove(PackedItemsList.Find(r => r.serialNumber == selIPN && r.packedDate == selMFPN));
                counter--;
                lblCounter.Text = string.Format("QTY: {0}", counter);
                PopulatePackedItemsGridView();
                SetColumsOrder(dataGridView1);
            }
            else if (dialogResult == DialogResult.No)
            {
                txtbSN.Focus();
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Checked)
            {
                txtbComments.ReadOnly = true;
            }
            else if (!checkBox4.Checked)
            {
                txtbComments.ReadOnly = false;
            }
        }
        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                bool limitOk = int.TryParse(txtbSetLimit.Text.ToString(), out limit);
                if (limitOk)
                {
                    lblCounter.Text = String.Format("QTY: {0} / {1}", counter, limit.ToString());
                    txtbSetLimit.Clear();
                    txtbSetLimit.ReadOnly = true;
                    txtbSN.Focus();
                }
            }
        }
        private void txtbComments_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                checkBox4.Checked = true;
                txtbSetLimit.Focus();
            }
        }
        private void lblLimit_DoubleClick(object sender, EventArgs e)
        {
            txtbSetLimit.ReadOnly = false;
            txtbSetLimit.Focus();
        }
    }
}
