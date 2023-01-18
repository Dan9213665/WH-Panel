using FastMember;
using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using DataTable = System.Data.DataTable;

namespace WH_Panel
{
    public partial class FrmKITShistory : Form
    {
        public List<KitHistoryItem> KitHistoryItemsList = new List<KitHistoryItem>();
        public DataTable UDtable = new DataTable();
        public int countItems = 0;
        public int countLoadedFIles = 0;
        int i = 0;
        int loadingErrors = 0;
        public static Stopwatch stopWatch = new Stopwatch();
        
        public FrmKITShistory()
        {
            InitializeComponent();
            
            //startUpLogic();
            //SetColumsOrder();
        }

        
        
        

        private void button1_Click(object sender, EventArgs e)
        {
            stopWatch.Reset();
            ResetViews();
            startUpLogic();
            SetColumsOrder();
        }

        private void ResetViews()
        {
            listBox1.Items.Clear();
            listBox1.Update();
            label13.Text = "No Errors detected.";
            label13.BackColor = Color.LightGreen;
            label13.Update();
            label1.BackColor= Color.LightGreen;
            label11.BackColor = Color.LightGreen;
            label2.BackColor = Color.LightGreen;
            label3.BackColor = Color.LightGreen;
            KitHistoryItemsList.Clear();
            countItems = 0;
            countLoadedFIles = 0;
            i = 0;
            loadingErrors = 0;
            label12.Text = string.Empty;
            UDtable.Clear();
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            
        }

        //public class KeyValueList<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
        //{
        //    public void Add(TKey key, TValue value)
        //    {
        //        Add(new KeyValuePair<TKey, TValue>(key, value));
        //    }
        //}

        
        private void startUpLogic()

        {
            stopWatch.Start();

            
            List<string> listOfPaths = new List<string>()
            {
                "\\\\dbr1\\Data\\WareHouse\\2022\\10.2022",
                "\\\\dbr1\\Data\\WareHouse\\2022\\11.2022",
                "\\\\dbr1\\Data\\WareHouse\\2022\\12.2022",
                "\\\\dbr1\\Data\\WareHouse\\2023"
            };

            label12.BackColor = Color.IndianRed;

            foreach (string path in listOfPaths)
            {
                foreach (string file in Directory.EnumerateFiles(path, "*.xlsm", SearchOption.AllDirectories))
                {
                    countLoadedFIles++;
                    //MessageBox.Show(file);
                    string Litem = Path.GetFileName(file);
                    //MessageBox.Show(Litem);
                    DataLoader(file, Litem);
                    //listOfKitFiles.Add(@file);

                }

                

            }


      

            //MessageBox.Show(listOfKitFiles.Count.ToString());

            //for (int i = 0; i < listOfKitFiles.Count; i++)

            //{
            //    //MessageBox.Show("i:"+i+" "+listOfKitFiles[i].ToString());
            //    // DataLoader(listOfKitFiles[i].Key, listOfKitFiles[i].Value);
            //    //DataLoader(listOfKitFiles[i]);
            //}


            PopulateGridView();
            SetColumsOrder();

            stopWatch.Stop();
        }
     
        private void DataLoader(string fp,string excelFIleName)
        {
            TimeSpan ts = stopWatch.Elapsed;

            try
            {
                string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=1\string.Empty;
                
                using (OleDbConnection conn = new OleDbConnection(constr))
                {
                    try
                    {

                        conn.Open();
                        
                            DataTable dbSchema = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                            if (dbSchema == null || dbSchema.Rows.Count < 1)
                            {
                                throw new Exception("Error: Could not determine the name of the first worksheet.");
                            }
                            string firstSheetName = dbSchema.Rows[0]["TABLE_NAME"].ToString();
                            // string columnName = dbSchema.Columns.ToString();
                            //label13.Text+=columnName;
                            string cleanedUpSheetName = firstSheetName.Substring(1).Substring(0, firstSheetName.Length - 3);

                            //string cleanedUPfP = fp.Split("/",);
                            //string[] cleanedUPfP = fp.Split(string.Empty);
                            //MessageBox.Show(cleanedUPfP[0]+" "+ cleanedUPfP[1]);

                            OleDbCommand command = new OleDbCommand("Select * from [" + cleanedUpSheetName + "$]", conn);
                            OleDbDataReader reader = command.ExecuteReader();
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    KitHistoryItem abc = new KitHistoryItem
                                    {
                                        DateOfCreation = cleanedUpSheetName,
                                        ProjectName = excelFIleName,
                                        IPN = reader[1].ToString(),
                                        MFPN = reader[2].ToString(),
                                        Description = reader[3].ToString(),
                                        QtyInKit = reader[4].ToString(),
                                        Delta = reader[5].ToString(),
                                        QtyPerUnit = reader[7].ToString(),
                                        Notes = reader[8].ToString(),
                                        Alts = reader[9].ToString()
                                    };

                                    if (i > 0)
                                    {
                                        countItems = i;
                                        label12.Text = "Loaded " + (countItems).ToString() + " Rows from " + countLoadedFIles + " files. In " + string.Format("{0:00}.{1:000} Seconds", ts.Seconds, ts.Milliseconds);
                                        label12.Update();
                                        KitHistoryItemsList.Add(abc);
                                    }

                                    i++;
                                }
                            }
                       
                      

                        conn.Dispose();
                        conn.Close();

                    }
                    catch (Exception)
                    {
                        //MessageBox.Show(excelFIleName +" is open by another user !!!\n File can not be loaded ! \n"+ "Full path: " +fp , "Error loading "+ excelFIleName, MessageBoxButtons.OK,MessageBoxIcon.Stop);
                        //label13.Text += "\n"+ "File can not be loaded ! " + fp ;
                        loadingErrors++;
                        label13.Text = loadingErrors.ToString() + " Loading Errors detected: ";
                        label13.BackColor = Color.IndianRed;
                        label13.Update();
                        
                        //string er =  "\n Access denied: " + fp;
                        string er = fp;
                        listBox1.Items.Add(er);
                        listBox1.Update();
                        conn.Close();
                    }

                    
                }
            }
            catch (IOException)
            {
                throw;
            }

        }

        private void PopulateGridView()
        {
            
            //MessageBox.Show(.Count.ToString()); 
            IEnumerable<KitHistoryItem> data = KitHistoryItemsList;
            //UDtable.Clear();
            using (var reader = ObjectReader.Create(data))
            {
                UDtable.Load(reader);
            }
            dataGridView1.DataSource = UDtable;
            SetColumsOrder();
            label12.BackColor = Color.LightGreen;

            //dataGridView1.AutoResizeColumns();
            //dataGridView1.Update();

            

        }

        private void SetColumsOrder()
        {
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[3].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode  =DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[6].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[7].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[8].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[9].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            dataGridView1.Columns["DateOfCreation"].DisplayIndex = 0;
            dataGridView1.Columns["ProjectName"].DisplayIndex = 1;
            dataGridView1.Columns["IPN"].DisplayIndex = 2;
            dataGridView1.Columns["MFPN"].DisplayIndex = 3;
            dataGridView1.Columns["Description"].DisplayIndex = 4;
            dataGridView1.Columns["QtyInKit"].DisplayIndex = 5;
            dataGridView1.Columns["Delta"].DisplayIndex = 6;
            dataGridView1.Columns["QtyPerUnit"].DisplayIndex = 7;
            dataGridView1.Columns["Notes"].DisplayIndex = 8;
            dataGridView1.Columns["Alts"].DisplayIndex = 9;
        }
        private void FilterTheDataGridView()
        {
            try
            {
                DataView dv = UDtable.DefaultView;
                dv.RowFilter = "[IPN] LIKE '%" + textBox1.Text.ToString() +
                     "%' AND [ProjectName] LIKE '%" + textBox11.Text.ToString() +
                "%' AND [MFPN] LIKE '%" + textBox2.Text.ToString() +
                "%' AND [Description] LIKE '%" + textBox3.Text.ToString() + "%' ";
                dataGridView1.DataSource = dv;
                SetColumsOrder();
            }
            catch (Exception)
            {
                MessageBox.Show("Incorrect search pattern, remove invalid character and try again !", "Search pattern error", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                throw;
            }

        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            label1.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            textBox1.Text = string.Empty;
            label1.BackColor = Color.LightGreen;
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            label11.BackColor = Color.IndianRed;
            
            FilterTheDataGridView();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            textBox11.Text = string.Empty;
            label11.BackColor = Color.LightGreen;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            label2.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            textBox2.Text = string.Empty;
            label2.BackColor = Color.LightGreen;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            label3.BackColor = Color.IndianRed;
            FilterTheDataGridView();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            textBox3.Text = string.Empty;
            label3.BackColor = Color.LightGreen;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //MessageBox.Show(listBox1.SelectedItem.ToString());

            //var fpp = @listBox1.SelectedItem.ToString();
            //openWHexcelDB(fpp);
        }
        private void openWHexcelDB(string thePathToFile)
        {
            Process excel = new Process();

            excel.StartInfo.FileName = "C:\\Program Files\\Microsoft Office\\root\\Office16\\EXCEL.exe";
            excel.StartInfo.Arguments = thePathToFile;
            excel.Start();
        }
    }
}
