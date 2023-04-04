using FastMember;
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

namespace WH_Panel
{
    public partial class FrmFinishedGoodsLog : Form
    {
        public DataTable PackedItemsUDtable = new DataTable();
        public List<FinishedGoodsItem> PackedItemsList = new List<FinishedGoodsItem>();
        public string initialPath= @"\\dbr1\Data\Aegis_NPI_Projects\";
        public FrmFinishedGoodsLog()
        {
            InitializeComponent();
            populateComboBox(comboBox1, initialPath);
        }
        public void populateComboBox(ComboBox cb, string path)
        {
            cb.Items.Clear();
            //string path = @"\\dbr1\Data\Aegis_NPI_Projects\"; // replace with your folder path
            string[] folders = Directory.GetDirectories(path);

            foreach (string folder in folders)
            {
                cb.Items.Add(Path.GetFileName(folder));
            }
        }

        private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkBox1.Checked= true;
            comboBox1.Enabled= false;
            populateComboBox(comboBox2, initialPath + comboBox1.SelectedItem.ToString());
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            checboxLockUnlock(sender,comboBox1);
        }

        private void checboxLockUnlock(object sender,ComboBox cbm)
        {
            if (((CheckBox)sender).Checked)
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
            populateComboBox(comboBox3,  initialPath+ comboBox1.SelectedItem.ToString()+"\\"+ comboBox2.SelectedItem.ToString());
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            checboxLockUnlock(sender,comboBox2);
        }

        private void comboBox3_SelectionChangeCommitted(object sender, EventArgs e)
        {
            checkBox3.Checked = true;
            comboBox3.Enabled = false;
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
            FinishedGoodsItem fg = new FinishedGoodsItem()
            {
                Customer = comboBox1.Text,
                Project = comboBox2.Text,
                Revision = comboBox3.Text,
                serialNumber = txtbSN.Text.ToString(),
                packedDate = DateTime.Now.ToString("yyyy-MM-dd") + " " + DateTime.Now.ToString("HH:mm:ss"),
                Comments = txtbComments.Text.ToString()

            };
            PackedItemsList.Add(fg);
            PopulatePackedItemsGridView();
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
    }
}
