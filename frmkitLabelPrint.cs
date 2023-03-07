using Microsoft.Office.Interop.Excel;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Menu;
namespace WH_Panel
{
    public partial class frmkitLabelPrint : Form
    {
        public frmkitLabelPrint()
        {
            InitializeComponent();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source='\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm'; Extended Properties=\"Excel 12.0 Xml;HDR=NO;\"";
        }
        private void btnBrowseToFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = "\\\\dbr1\\Data\\WareHouse\\"+ DateTime.Now.ToString("yyyy") + "\\"+ DateTime.Now.ToString("MM") + "."+ DateTime.Now.ToString("yyyy");
            openFileDialog1.Filter = "xlsm files (*.xlsm)|*.xlsm";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            //DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                //string file = openFileDialog1.FileName;
                string fileN = openFileDialog1.SafeFileName;
                string r = fileN.Substring(0, fileN.Length - 5);
                txtbPasteCPQ.Text =r;
                try
                {
                    //string kitLabelExcel = @"\\\\dbr1\\Data\\WareHouse\\KitLabel.xlsm";
                    string kitLabelExcel = @"C:\\1\\KitLabel.xlsm";
                    string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source="+ kitLabelExcel+ "; Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=0\"";
                    //using (
                    OleDbConnection conn = new OleDbConnection(constr);
                    //{
                    conn.Open();

                    OleDbCommand command = new OleDbCommand();
                    

                    //OleDbCommand command = new OleDbCommand("Insert into [Sheet1$].[1,2] values (" + r + ")");
                    //OleDbCommand command = new OleDbCommand("UPDATE [Sheet1$ + $B1:B1] SET F1=" + r + '"');
                    //string sql = "UPDATE [KIT$ + $B1:B1] SET F1=5";
                    string stSheetName = "KIT";
                   //string  sql = "UPDATE [" + stSheetName + "$B2:B2] ToPrint =@ToPrint";
                   
                    command.CommandText = "INSERT INTO [" + stSheetName + "$B2:B2] (IPN) values ("+"12313"+")";
                    //command.Parameters.AddWithValue("@ToPrint", r);
                    command.Connection = conn;
                   
                    command.ExecuteNonQuery();
                        //OleDbCommand command = new OleDbCommand("Select * from [Sheet1$]", conn);
                        //OleDbDataReader reader = command.ExecuteReader();
                        //if (reader.HasRows)
                        //{
                        //    while (reader.Read())
                        //    {
                        //    }
                        //}
                        conn.Close();
                    //}
                }
                catch (IOException)
                {
                }
            }
        }
        private void btnPrintKitLabel_Click(object sender, EventArgs e)
        {

        }
    }
}
