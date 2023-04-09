﻿using FastMember;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace WH_Panel
{
    public partial class FrmBomWHS : Form
    {
        public List<KitHistoryItem> fromTheMainBom { get; set; }
        public DataTable misItemsDT = new DataTable();
        public List<KitHistoryItem> misItemsLST = new List<KitHistoryItem>();
        public List<BOMitem>misBOMItemsLST = new List<BOMitem> ();
        public string avlNETLINE = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_AVL.xlsx";
        public string stockNETLINE = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\NETLINE\\NETLINE_STOCK.xlsm";
        public string stockLeader_Tech = @"\\dbr1\Data\WareHouse\STOCK_CUSTOMERS\G.I.Leader_Tech\G.I.Leader_Tech_STOCK.xlsm";
        public string avlLeader_Tech = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\G.I.Leader_Tech\\G.I.Leader_Tech_AVL.xlsm";
        public string avlVAYAR = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_AVL.xlsx";
        public string stockVAYAR = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VAYAR\\VAYAR_stock.xlsm";
        public string avlVALENS = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_AVL.xlsx";
        public string stockVALENS = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\VALENS\\VALENS_STOCK.xlsm";
        public string avlROBOTRON = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsm";
        public string stockROBOTRON = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_STOCK.xlsm";
        public string avlFile;
        public string stockFile;
        public string projectName = string.Empty;
        public List<WHitem> avlItems = new List<WHitem>();
        public List<WHitem> stockItems = new List<WHitem>();
        public DataTable avlDTable = new DataTable();
        public DataTable stockDTable = new DataTable();
        public int countAVLItems = 0;
        public int countStockItems = 0;
        int iAVL = 0;
        int iStock = 0;
        public FrmBomWHS()
        {
            InitializeComponent();

           

        }
        
        private void FrmBomWHS_Load(object sender, EventArgs e)
        {
            foreach(KitHistoryItem it in fromTheMainBom)
            {
                BOMitem n = new BOMitem {
                    IPN= it.IPN ,
                    MFPN= it.MFPN ,
                    Description= it.Description ,
                    WHbalance = 0,
                    QtyInKit= it.QtyInKit,
                    Delta=it.Delta,
                    QtyPerUnit=it.QtyPerUnit,
                    Calc=it.Calc,
                    Alts=it.Alts
                };
                misBOMItemsLST.Add(n);
                //misItemsLST.Add(it);
            }
            //MessageBox.Show(misItemsLST.Count.ToString());
            projectName = fromTheMainBom[0].ProjectName;

            comboBox1.SelectedItem = warehouseSelectorOnLoad();
            //MasterReload(avlFile, stockFile);
            foreach(BOMitem b in misBOMItemsLST)
            {
                b.WHbalance = calculateWBbalance(b.IPN);
            }
            PopulateMissingGridView();
        }
        public int calculateWBbalance(string IPN)
        {
            int balance = 0;
                try
                {
                    DataView dv = stockDTable.DefaultView;
                    dv.RowFilter = "[IPN] LIKE '%" + IPN +
                        "%'";
                    dataGridView2.DataSource = dv;
                    //dataGridView2.Update();
                    //SetSTOCKiewColumsOrder();
                    List<int> qtys = new List<int>();
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        int qty = 0;
                        int result;
                        bool prs = int.TryParse(dataGridView2.Rows[i].Cells[dataGridView2.Columns["Stock"].Index].Value.ToString(), out result);
                        if (prs)
                        {
                            qty = result;
                        }
                        qtys.Add(qty);
                    }
                    foreach (int i in qtys)
                    {
                        balance += i;
                    }
                }
                catch
                {

                }
            return balance;
       }
        private string warehouseSelectorOnLoad()
        {
            string selection = "ROBOTRON";
          
            if (misBOMItemsLST[0].IPN.StartsWith("C100")|| misBOMItemsLST[0].IPN.StartsWith("A00"))
            {
                selection = "LEADER-TECH";
                MasterReload(avlLeader_Tech, stockLeader_Tech);
            }
            else if (misBOMItemsLST[0].IPN.StartsWith("NET"))
            {
                selection = "NETLINE";
                MasterReload(avlNETLINE, stockNETLINE);
            }
            else if (misBOMItemsLST[0].IPN.StartsWith("VAY"))
            {
                selection = "VAYYAR";
                MasterReload(avlVAYAR, stockVAYAR);
            }
            else if (misBOMItemsLST[0].IPN.StartsWith("VAL"))
            {
                selection = "VALENS";
                MasterReload(avlVALENS, stockVALENS);
            }
            else
            {
                selection = "ROBOTRON";
                MasterReload(avlROBOTRON, stockROBOTRON);
            }



            return selection;
        }
        private void PopulateMissingGridView()
        {
            misItemsDT.Clear();
            IEnumerable<BOMitem> data = misBOMItemsLST;
            using (var reader = ObjectReader.Create(data))
            {
                misItemsDT.Load(reader);
            }
            dataGridView1.DataSource = misItemsDT;
            SetColumsOrder(dataGridView1);
            groupBox1.Text = String.Format("Missing items : {0}", misBOMItemsLST.Count);
        }
        private void SetColumsOrder(DataGridView dgw)
        {
            dgw.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgw.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //dgw.Columns["DateOfCreation"].DisplayIndex = 0;
            dgw.Columns["DateOfCreation"].Visible = false;
            //dgw.Columns["ProjectName"].DisplayIndex = 1;
            dgw.Columns["ProjectName"].Visible = false;
            dgw.Columns["IPN"].DisplayIndex = 0;
            dgw.Columns["MFPN"].DisplayIndex = 1;
            dgw.Columns["Description"].DisplayIndex = 2;
            dgw.Columns["WHbalance"].DisplayIndex = 3;
            dgw.Columns["QtyInKit"].DisplayIndex = 4;
            dgw.Columns["Delta"].DisplayIndex = 5;
            //dgw.Columns["QtyPerUnit"].DisplayIndex = 7;
            dgw.Columns["QtyPerUnit"].Visible = false;
            dgw.Columns["Calc"].DisplayIndex = 6;
            dgw.Columns["Alts"].DisplayIndex = 7;

            dgw.Sort(dgw.Columns["IPN"], ListSortDirection.Ascending);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "ROBOTRON")
            {
                MasterReload(avlROBOTRON, stockROBOTRON);
            }
            else if (comboBox1.Text == "LEADER-TECH")
            {
                MasterReload(avlLeader_Tech, stockLeader_Tech);
            }
            else if (comboBox1.Text == "NETLINE")
            {
                MasterReload(avlNETLINE, stockNETLINE);
            }
            else if (comboBox1.Text == "VAYYAR")
            {
                MasterReload(avlVAYAR, stockVAYAR);
            }
            else if (comboBox1.Text == "VALENS")
            {
                MasterReload(avlVALENS, stockVALENS);
            }
        }
        private void MasterReload(string avlParam, string stockParam)
        {

            avlFile = avlParam;
            stockFile = stockParam;
            label1.BackColor = Color.LightGreen;
            StockViewDataLoader(stockParam, "STOCK");
            button3_Click(this, new EventArgs());

        }

        private void DataLoaderAVL(string fp, string thesheetName)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=NO;IMEX=1\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            WHitem abc = new WHitem
                            {
                                IPN = reader[1].ToString(),
                                Manufacturer = reader[2].ToString(),
                                MFPN = reader[3].ToString(),
                                Description = reader[4].ToString(),
                                Stock = 0,
                                UpdatedOn = string.Empty,
                                CommentsWHitem = string.Empty,
                                SourceRequester = string.Empty
                            };
                            if (iAVL > 0)
                            {
                                countAVLItems = iAVL;
                                label1.Text = "Rows in AVL: " + (countAVLItems).ToString();
                                if (countAVLItems % 1000 == 0)
                                {
                                    label1.Update();
                                }
                                avlItems.Add(abc);
                            }
                            iAVL++;
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
        private void StockViewDataLoader(string fp, string thesheetName)
        {
            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    conn.Open();
                    OleDbCommand command = new OleDbCommand("Select * from [" + thesheetName + "$]", conn);
                    OleDbDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            try
                            {
                                int res = 0;
                                int toStk;
                                bool stk = int.TryParse(reader[4].ToString(), out res);
                                if (stk)
                                {
                                    toStk = res;
                                }
                                else
                                {
                                    toStk = 0;
                                }
                                WHitem abc = new WHitem
                                {
                                    IPN = reader[0].ToString().Trim().Replace(" ", ""),
                                    Manufacturer = reader[1].ToString(),
                                    MFPN = reader[2].ToString(),
                                    Description = reader[3].ToString(),
                                    Stock = toStk,
                                    UpdatedOn = reader[5].ToString(),
                                    CommentsWHitem = reader[6].ToString(),
                                    SourceRequester = reader[7].ToString()
                                };
                                countStockItems = iStock;
                                label1.Text = "Rows in STOCK: " + (countStockItems).ToString();
                                if (countStockItems % 1000 == 0)
                                {
                                    label1.Update();
                                }
                                stockItems.Add(abc);
                                iStock++;
                            }
                            catch (Exception E)
                            {
                                MessageBox.Show(E.Message);
                                throw;
                            }
                        }
                    }
                    label1.BackColor = Color.LightGreen;
                    conn.Close();
                }
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }
        }
        private void PopulateStockView()
        {
            dataGridView2.DataSource = null;
            IEnumerable<WHitem> data = stockItems;
            stockDTable.Clear();
            using (var reader = ObjectReader.Create(data))
            {
                stockDTable.Load(reader);
            }
            dataGridView2.DataSource = stockDTable;
            dataGridView2.Update();
            SetSTOCKiewColumsOrder();

        }
        private void SetSTOCKiewColumsOrder()
        {
            dataGridView2.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView2.Columns["IPN"].DisplayIndex = 0;
            dataGridView2.Columns["Manufacturer"].DisplayIndex = 1;
            dataGridView2.Columns["MFPN"].DisplayIndex = 2;
            dataGridView2.Columns["Description"].DisplayIndex = 3;
            dataGridView2.Columns["Stock"].DisplayIndex = 4;
            dataGridView2.Columns["UpdatedOn"].DisplayIndex = 5;
            dataGridView2.Columns["CommentsWHitem"].DisplayIndex = 6;
            dataGridView2.Columns["SourceRequester"].DisplayIndex = 7;

            dataGridView2.Sort(dataGridView2.Columns["UpdatedOn"], ListSortDirection.Descending);
        }
        private void FilterStockDataGridView(string IPN)
        {
            
                try
                {
                    int balance = 0;
                    DataView dv = stockDTable.DefaultView;
                    dv.RowFilter = "[IPN] LIKE '%" + IPN +
                        "%'";
                    dataGridView2.DataSource = dv;
                    dataGridView2.Update();
                    SetSTOCKiewColumsOrder();
                    List<int> qtys = new List<int>();
                    for (int i = 0; i < dataGridView2.RowCount; i++)
                    {
                        int qty = 0;
                        int result;
                        bool prs = int.TryParse(dataGridView2.Rows[i].Cells[dataGridView2.Columns["Stock"].Index].Value.ToString(), out result);
                        if (prs)
                        {
                            qty = result;
                        }
                        qtys.Add(qty);
                    }
                    foreach (int i in qtys)
                    {
                        balance += i;
                    }
                    foreach(BOMitem bi in misBOMItemsLST)
                {
                    if(bi.IPN==IPN)
                    {
                        bi.WHbalance = balance;
                    }

                }

                label15.Text = "BALANCE: " + balance;
                if (balance > 0)
                    label15.BackColor = Color.LightGreen;
                else if (balance == 0)
                    label15.BackColor = Color.IndianRed;
                label15.Update();
                }
                catch (Exception)
                {
                    MessageBox.Show("Incorrect search pattern, remove invalid character and try again !");
                    throw;
                }
            
          
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string cellValue = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString();
                textBox10.Text = cellValue;
                FilterStockDataGridView(cellValue);
                if(chkBlockInWHonly.Checked)
                {
                    FilterInStockItemsOnly();
                }
            }
        }

        private bool dataGridViewIsBound = false;

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (dataGridViewIsBound && dataGridView1.SelectedCells.Count > 0)
            {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                //int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string cellValue = dataGridView1.Rows[rowindex].Cells[dataGridView1.Columns["IPN"].Index].Value.ToString();
                FilterStockDataGridView(cellValue);
            }
        }

        private void dataGridView1_BindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {

            if (e.ListChangedType == ListChangedType.Reset && dataGridView1.Rows.Count > 0)
            {
                //dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells[0];
                dataGridViewIsBound = true;
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            stockItems.Clear();
            stockDTable.Clear();
            countStockItems = 0;
            iStock = 0;
            label1.Text = "RELOAD STOCK";
            StockViewDataLoader(stockFile, "STOCK");
            PopulateStockView();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FilterInStockItemsOnly();
        }

        private void FilterInStockItemsOnly()
        {

            //dataGridView2.DataSource = dv;
            dataGridView2.DataSource=createFilteredInStockDataview();
            dataGridView2.Update();
            SetSTOCKiewColumsOrder();
        }

        private DataView createFilteredInStockDataview()
        {
            List<WHitem> inWHstock = new List<WHitem>();
            for (int i = 0; i < dataGridView2.RowCount; i++)
            {
                int res = 0;
                int toStk;
                bool stk = int.TryParse(dataGridView2.Rows[i].Cells[dataGridView2.Columns["Stock"].Index].Value.ToString(), out res);
                if (stk)
                {
                    toStk = res;
                }
                else
                {
                    toStk = 0;
                }
                WHitem wHitemABC = new WHitem()
                {
                    IPN = dataGridView2.Rows[i].Cells[dataGridView2.Columns["IPN"].Index].Value.ToString(),
                    Manufacturer = dataGridView2.Rows[i].Cells[dataGridView2.Columns["Manufacturer"].Index].Value.ToString(),
                    MFPN = dataGridView2.Rows[i].Cells[dataGridView2.Columns["MFPN"].Index].Value.ToString(),
                    Description = dataGridView2.Rows[i].Cells[dataGridView2.Columns["Description"].Index].Value.ToString(),
                    Stock = toStk,
                    UpdatedOn = dataGridView2.Rows[i].Cells[dataGridView2.Columns["UpdatedOn"].Index].Value.ToString(),
                    CommentsWHitem = dataGridView2.Rows[i].Cells[dataGridView2.Columns["CommentsWHitem"].Index].Value.ToString(),
                    SourceRequester = dataGridView2.Rows[i].Cells[dataGridView2.Columns["SourceRequester"].Index].Value.ToString()
                };
                inWHstock.Add(wHitemABC);
            }
            List<WHitem> negatiVEQTYs = new List<WHitem>();
            for (int i = 0; i < inWHstock.Count; i++)
            {
                if (inWHstock[i].Stock < 0)
                {
                    negatiVEQTYs.Add(inWHstock[i]);
                }
            }
            List<WHitem> positiveInWH = new List<WHitem>();
            for (int k = 0; k < inWHstock.Count; k++)
            {
                if (inWHstock[k].Stock > 0)
                {
                    positiveInWH.Add(inWHstock[k]);
                }
            }
            for (int i = 0; i < negatiVEQTYs.Count; i++)
            {
                for (int j = 0; j < positiveInWH.Count; j++)
                {
                    if (Math.Abs(negatiVEQTYs[i].Stock) == positiveInWH[j].Stock)
                    {
                        positiveInWH.Remove((WHitem)positiveInWH[j]);
                        break;
                    }
                }
            }
            IEnumerable<WHitem> WHdata = positiveInWH;
            DataTable INWH = new DataTable();
            using (var reader = ObjectReader.Create(WHdata))
            {
                INWH.Load(reader);
            }
            DataView dv = INWH.DefaultView;

            return dv;
        }

        private void chkBlockInWHonly_CheckedChanged(object sender, EventArgs e)
        {
            System.Windows.Forms.CheckBox chk = (System.Windows.Forms.CheckBox)sender;
            if(chk.Checked)
            {
                StockViewDataLoader(stockFile, "STOCK");
                FilterInStockItemsOnly();
            }
            else
            {
                StockViewDataLoader(stockFile, "STOCK");
            }
        }

        private void btnFound_Click(object sender, EventArgs e)
        {
                int rowindex = dataGridView1.CurrentCell.RowIndex;
                int columnindex = dataGridView1.CurrentCell.ColumnIndex;
                string cellValue = dataGridView1.Rows[rowindex].Cells[columnindex].Value.ToString();
                string selIPN =  dataGridView1.Rows[rowindex].Cells["IPN"].Value.ToString();
                string selMFPN = dataGridView1.Rows[rowindex].Cells["MFPN"].Value.ToString();
                misBOMItemsLST.Remove(misBOMItemsLST.Find(r => r.IPN == selIPN && r.MFPN == selMFPN));
                PopulateMissingGridView();
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _fileTimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
            ExportToHTML(dataGridView1, "\\\\dbr1\\Data\\WareHouse\\2023\\WHsearcher\\"+ _fileTimeStamp+"_"+projectName.Substring(0, projectName.Length - 5) + ".html");
        }
        private void ExportToHTML(DataGridView dataGridView, string fileName)
        {
            dataGridView.Columns["Calc"].Visible = false;
            List<WHitem> searchInstockList = new List<WHitem>();
            searchInstockList.Clear();
            foreach (WHitem w in stockItems)
            {
                int stk = 0;
                WHitem sil = new WHitem();
                sil.IPN = w.IPN;
                sil.Manufacturer= w.Manufacturer;
                sil.MFPN= w.MFPN;
                sil.Description= w.Description;
                sil.SourceRequester= w.SourceRequester;
                sil.CommentsWHitem= w.CommentsWHitem;

                bool pOk = int.TryParse(w.Stock.ToString(), out stk);
                    if(pOk)
                {
                    sil.Stock = stk;
                }
                else
                {
                    sil.Stock = 0;
                }
                sil.UpdatedOn = w.UpdatedOn;

                searchInstockList.Add(sil);
            }


            List<WHitem> negatiVEQTYsWH = new List<WHitem>();
            for (int i = 0; i < searchInstockList.Count; i++)
            {
                if (searchInstockList[i].Stock < 0)
                {
                    negatiVEQTYsWH.Add(searchInstockList[i]);
                }
            }
            List<WHitem> positiveInWH2 = new List<WHitem>();
            for (int k = 0; k < searchInstockList.Count; k++)
            {
                if (searchInstockList[k].Stock > 0)
                {
                    positiveInWH2.Add(searchInstockList[k]);
                }
            }
            for (int i = 0; i < negatiVEQTYsWH.Count; i++)
            {
                for (int j = 0; j < positiveInWH2.Count; j++)
                {
                    if (Math.Abs(negatiVEQTYsWH[i].Stock) == positiveInWH2[j].Stock)
                    {
                        positiveInWH2.Remove((WHitem)positiveInWH2[j]);
                        break;
                    }
                }
            }

            IEnumerable<WHitem> WHdata = positiveInWH2;
            DataTable INWH = new DataTable();
            using (var reader = ObjectReader.Create(WHdata))
            {
                INWH.Load(reader);
            }
            //DataView dv = INWH.DefaultView;


            StringBuilder sb = new StringBuilder();
           
            // Create HTML header
            sb.Append("<html>");
            sb.Append("<head>");
            sb.Append("<title>" +projectName + "</title>");
            sb.Append("</head>");

            // Create HTML body
            sb.Append("<body>");
            sb.Append("<table border='1px' cellpadding='1' cellspacing='0' style='text-align:center'>");
            sb.Append("<h2 style='text-align:center'>" + projectName + "</h2>");
            // Add header row
            sb.Append("<tr>");
            //dataGridView.Columns["IPN"].DisplayIndex = 2;
            //dataGridView.Columns["Delta"].DisplayIndex = 7;
            
            foreach (DataGridViewColumn column in dataGridView.Columns)
            {
                
                if (column.Visible)
                {
                    sb.Append("<th>" + column.HeaderText + "</th>");
                }
                
            }
            sb.Append("</tr>");

            // Add data rows
            foreach (DataGridViewRow row in dataGridView.Rows)
            {
                
                  
                sb.Append("<tr>");
                foreach (DataGridViewCell cell in row.Cells)
                {
                    if(cell.Visible)
                    {
                        sb.Append("<td><h4>" + cell.Value + "</h4></td>");
                      
                    }
                    
                }
                sb.Append("</tr>");

                DataView dv = INWH.DefaultView;

                dv.RowFilter = "[IPN] LIKE '%" + row.Cells["IPN"].Value +"%'";
                dataGridView2.DataSource = dv;

                sb.Append("<tr>");
                foreach (DataGridViewColumn column in dataGridView2.Columns)
                {
                    dataGridView2.Columns["SourceRequester"].Visible= false;
                    dataGridView2.Columns["Manufacturer"].Visible = false;
                    dataGridView2.Columns["Description"].Visible = false;
                    if (column.Visible)
                    {
                        //sb.Append("<th>" + column.HeaderText + "</th>");
                    }

                }
                sb.Append("</tr>");

                foreach (DataGridViewRow r2 in dataGridView2.Rows)
                {

                    sb.Append("<tr>");
                        foreach (DataGridViewCell c2 in r2.Cells)
                        {
                        if (c2.Visible)
                            sb.Append("<td>" + c2.Value + "</td>");
                        }
                    sb.Append("</tr>");
                }



            }

            // Close HTML tags
            sb.Append("</table>");
            sb.Append("</body>");
            sb.Append("</html>");

            // Write HTML to file
            File.WriteAllText(fileName, sb.ToString());

            // Open HTML file in default browser
            //System.Diagnostics.Process.Start(fileName);

            var p = new Process();
            p.StartInfo = new ProcessStartInfo(@fileName)
            {
                UseShellExecute = true
            };
            p.Start();

            
            //dataGridView.Columns["Calc"].Visible = true;
            dataGridView2.Columns["SourceRequester"].Visible = true;
            dataGridView2.Columns["Manufacturer"].Visible = true;
            dataGridView2.Columns["Description"].Visible = true;
            dataGridView.Columns["Calc"].Visible=true;
        }

    }
}