using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
using FastMember;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Diagnostics;
using Label = System.Windows.Forms.Label;
namespace WH_Panel
{
    public partial class FrmUberSearch : Form
    {
        public FrmUberSearch()
        {
            InitializeComponent();
            List<Label> _seachableFieldsLabels = new List<Label>();
            _seachableFieldsLabels.Add(label2);
            _seachableFieldsLabels.Add(label4);
            _seachableFieldsLabels.Add(label5);
            _seachableFieldsLabels.Add(label9);
            foreach (Label l in _seachableFieldsLabels)
            {
                l.BackColor = Color.LightGreen;
            }
        }
        public List<WHitem> wHitems = new List<WHitem>();
        public DataTable UDtable = new DataTable();
        public int countItems = 0;
        int i = 0;
        public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        {
            public void Add(TKey key, TValue value)
            {
                Add(new KeyValuePair<TKey, TValue>(key, value));
            }
        }
        private void FrmUberSearch_Load(object sender, EventArgs e)
        {
            startUpLogic();
        }
        private void startUpLogic()
        {
            label1.BackColor = Color.IndianRed;
            var listOfWareHouses = new KeyValueList<string, string>
                   {
                        {"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm", "STOCK_VALENS" },
                        {"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm","STOCK" },
                        {"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm","STOCK" },
                        {"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\FIELDIN\\FIELDIN_STOCK.xlsm","STOCK" },
                {"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm","STOCK"}
                   };
            for (int i = 0; i < listOfWareHouses.Count; i++)
            {
                DataLoader(listOfWareHouses[i].Key, listOfWareHouses[i].Value);
            }
            PopulateGridView();
        }
        private void DataLoader(string fp, string thesheetName)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=1\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            int number;
                            bool success = int.TryParse(reader[4].ToString(), out number);
                            if (success)
                            {
                                WHitem abc = new WHitem
                                {
                                    IPN = reader[0].ToString(),
                                    Manufacturer = reader[1].ToString(),
                                    MFPN = reader[2].ToString(),
                                    Description = reader[3].ToString(),
                                    Stock = number,
                                    UpdatedOn = reader[5].ToString(),
                                    Comments = reader[6].ToString(),
                                    SourceRequester = reader[7].ToString()
                                };
                                if (i > 0)
                                {
                                    countItems = i;
                                    label1.Text = "Rows:" + (countItems).ToString();
                                    label1.Update();
                                    wHitems.Add(abc);
                                }
                                i++;
                            }
                        }
                    }
                    conn.Close();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        private void PopulateGridView()
        {
            //MessageBox.Show(wHitems.Count.ToString()); 
            IEnumerable<WHitem> data = wHitems;
            //UDtable.Clear();
            using (var reader = ObjectReader.Create(data))
            {
                UDtable.Load(reader);
            }
            dataGridView1.DataSource = UDtable;
            SetColumsOrder();
            label1.BackColor = Color.LightGreen;
            //dataGridView1.AutoResizeColumns();
            //dataGridView1.Update();
        }
        private void SetColumsOrder()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns["IPN"].DisplayIndex = 0;
            dataGridView1.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView1.Columns["MFPN"].DisplayIndex = 2;
            dataGridView1.Columns["Description"].DisplayIndex = 3;
            dataGridView1.Columns["Stock"].DisplayIndex = 4;
            dataGridView1.Columns["UpdatedOn"].DisplayIndex = 5;
            dataGridView1.Columns["Comments"].DisplayIndex = 6;
            dataGridView1.Columns["SourceRequester"].DisplayIndex = 7;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            ResetViews();
            startUpLogic();
            //SetColumsOrder();
            //ResetViews();
            //LoadDataFromFile();
            //PopulateGW();
        }
        private void ResetViews()
        {
            wHitems.Clear();
            countItems = 0;
            i = 0;
            label1.Text = "";
            UDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void FilterTheDataGridView()
        {
            try
            {
                DataView dv = UDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox2.Text.ToString() +
                    "%' AND [MFPN] LIKE '%" + textBox4.Text.ToString() +
                    "%' AND [Description] LIKE '%" + textBox5.Text.ToString() +
                    "%' AND [SourceRequester] LIKE '%" + textBox9.Text.ToString() +
                    "%'";
                dataGridView1.DataSource = dv;
                SetColumsOrder();
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !");
                throw;
            }
        }
        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            label4.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            label5.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();
            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(Environment.MachineName.ToString());   
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\FIELDIN\\FIELDIN_STOCK.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        private void button5_Click(object sender, EventArgs e)
        {
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_STOCK.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            label9.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }
        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            label2.BackColor = Color.LightGreen;
        }
        private void label4_Click(object sender, EventArgs e)
        {
            textBox4.Text = "";
            label4.BackColor = Color.LightGreen;
        }
        private void label5_Click(object sender, EventArgs e)
        {
            textBox5.Text = "";
            label5.BackColor = Color.LightGreen;
        }
        private void label9_Click(object sender, EventArgs e)
        {
            textBox9.Text = "";
            label9.BackColor = Color.LightGreen;
        }
        private void label2_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private  void clearAllsearchTextboxes()
        {
            textBox2.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox9.Text = "";
            label2.BackColor = Color.LightGreen;
            label4.BackColor = Color.LightGreen;
            label5.BackColor = Color.LightGreen;
            label9.BackColor = Color.LightGreen;
        }
        private void label4_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void label5_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void label9_DoubleClick(object sender, EventArgs e)
        {
            clearAllsearchTextboxes();
        }
        private void button6_Click(object sender, EventArgs e)
        {
            if (Environment.MachineName.ToString() == "RT12")
            {
                var fp = @"\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\CIS\\CIS_STOCK.xlsm";
                openWHexcelDB(fp);
            }
            else
            {
                MessageBox.Show("ACCESS DENIED");
            }
        }
        //private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        //{
        //    MessageBox.Show(e.ColumnIndex.ToString());
        //}
    }
}
