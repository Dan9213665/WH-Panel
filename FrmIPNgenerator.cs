using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using Button = System.Windows.Forms.Button;
using ComboBox = System.Windows.Forms.ComboBox;
using GroupBox = System.Windows.Forms.GroupBox;
using Label = System.Windows.Forms.Label;
using TextBox = System.Windows.Forms.TextBox;
namespace WH_Panel
{
    public partial class FrmIPNgenerator : Form
    {
        public List<WHitem> avlItemsFromTheMainForm = new List<WHitem>();
        public string sqlAvlConnectionStringFromMainForm = string.Empty;
        List<string> typesNamesList = new List<string> { "CAP", "RES", "IND", "OSC", "TRN", "DID", "PWR", "CON", "ICT", "PCB" };
        List<string> manufacturersList = new List<string> { "SAMTEC","Texas Instruments",
"FINISAR",
"JDSU",
"Meanwell",
"Nedis",
"SINFORCON ELECTRONICS",
"Advice",
"PHIHONG",
"Ktec",
"G1PRO",
"DONGYANG UNITECH CO.,LTD",
"Connect One",
"Dagan Multimedia",
"FiberNet",
"Aten",
"Cannon",
"Dvir Shmuel",
"Radian Heatsinks",
"Valens",
"Wurth",
"Palboreg",
"UDE",
"Netbit",
"ALFATEC GMBH & CO KG",
"3M",
"Lascar",
"TTE",
"CanaKit",
"Lenno",
"IDI",
"MIRACASE",
"ROLINE",
"JORJIN",
"G.Beres Marketing",
"Stontronics",
"IGOS MN",
"DCPACK",
"LEONI Kabel GmbH",
"IXCC",
"VANDESAIL",
"TE Conectivity",
"Plantek",
"LEENO",
"Shaked Solomon",
"SANYO DENKI",
"JST",
"ATS",
"DELTA",
"NMB",
"ALPHA NOVATECH",
"StarTech",
"MOLEX",
"FABORY",
"Proto-Advantage",
"B&H",
"SparkFun Electronics",
"U-TEK",
"MICRON1",
"Xilinx",
"MAC8",
"Bergquist",
"I-PEX",
"Bivar",
"Sunon",
"ACCU SCREW",
"t-Global Technology",
"Li Tone",
"Whayueb Tech",
"Rosenberger",
"IC+",
"TE-Connectivity",
"Qualtek",
"Auvidea",
"Loctite",
"Essentra Components",
"SCS",
"mcmaster.com",
"FAR Electronic Hardware",
"Adafruit",
"RAF Electronic Hardware",
"Wakefield Thermal Solutions",
"Wave Technologies",
"ToTeam",
"Teldor",
"4CHAIN",
"Nienyi",
"Multicomp",
"Barsys",
"Neewer",
"Dialight",
"RaspberryPi",
"GEAO TECHNOLOGY",
"STS Master",
"HARWIN",
"Aptiv",
"Shirtronics",
"M.Solution",
"Micron",
"Protektive Pak",
"100DEC",
"TR FASTENINGS",
"Laird Performance Materials",
"Bumper Specialties Inc.",
"Hammond Electronics Ltd",
"AVX",
"Yageo",
"Samsung",
"Vishay",
"Walsin Tech Corp.",
"Prosperity Dielectric Co.",
"Kemet",
"TDK",
"Murata",
"WALSIN",
"Johanson DielectricsInc",
"Murata2",
"MURATA1",
"Taiyo Yuden",
"Ningxia Xingri Electronics",
"United ChemiCon",
"SAMXON",
"Jackcon",
"CAL-CHIP",
"nichicon",
"TDK1",
"Panasonic",
"TDK-Lambda",
"VENKEL",
"Knowles Novacap",
"Rubycon",
"Fonitech Industrial",
"TAI-TECH",
"CSAK",
"Pulse",
"MOLEX1",
"CUI Inc.",
"Connfly",
"Omron ElectrpnicsInc.",
"DragonCity",
"Kycon",
"JAE Electronics",
"KAIFENG ELECTRONICS",
"Year Round Technology Corporatio",
"HIROSE ELECTRIC CO",
"NORCOMP Inc.",
"SWICHTECH",
"BEL",
"Amphenhol",
"FOXCONN",
"Emulation Technology",
"Amphenol",
"AMA",
"Delphi",
"KYOCERA CH",
"SWITCHCRAFT",
"TRP Connector",
"Coxoc",
"IMS",
"Keystone",
"ATTEND",
"LINK-PP",
"SAMTEC1",
"ASSMANN",
"SULLINS",
"CINCH",
"PHOENIX",
"POLYWELL",
"Abracon",
"PRECI-DIP",
"ADAMICU/Dongguan Elc",
"ADAM-TECH",
"Tensility International Corp",
"Global Connector Technology(GCT)",
"Sumitomo",
"APPS",
"COAX CONNECTORS",
"KJCOMTECH",
"LJV",
"Champway Electronics",
"On-Semi",
"ST",
"LittleFuse",
"Micro Commercial Components(MCC)",
"Semtech",
"INPAQTECHNALOGY",
"Bourns",
"Littelfuse",
"HOLLY",
"Diodes Inc.",
"TI",
"Infineon Technologies",
"On Semi",
"NXP",
"PROTEK DEVICES",
"Diodes",
"NEXPERIA",
"DCComponentsCo.",
"Pan-Jit",
"Central Semiconductor",
"Rectron",
"WEJ",
"Fairchild",
"International Rectifier",
"Comchip Technology",
"SMC",
"TOSHIBA",
"TSMC1",
"TSMC FAB 15",
"Audio Precision",
"PriSmSound",
"Fluke",
"Frankonia",
"Stanford Research systems",
"Agilent",
"B.K Precision",
"Electronics Harizon",
"Lion",
"EMTEST",
"HAEFELY",
"Quantum Data",
"TELEDYNE LECROY",
"Keysight",
"APPA",
"UNI-T",
"XGXC",
"JDS Uniphase",
"AFL",
"Spirent",
"Smartech",
"Mechanical Devices",
"Eldrotec",
"Tektronics",
"Sonoma instruments",
"RDT",
"WERLATONE",
"Amplifier Research",
"AMETEK",
"Vectawave",
"Ixia",
"Cypress",
"Saelig company",
"Com-Power",
"Detectus",
"Transforming Technologies",
"ASTRO",
"BONDLINE",
"MRC",
"MEMMERT",
"Chauvin-Arnoux",
"Introspect",
"TSN Systems",
"Unigraf",
"Paralight",
"EVERLIGHT",
"EDISON",
"Elmec",
"Holland Shielding",
"Leader Tech",
"Avago Technologies",
"Broadcom",
"MASACH TECHNOLOGIES",
"Maxim",
"Analog Devices",
"Micrel",
"Microchip",
"LinearTech.",
"Renesas Electronics",
"Realtek",
"SMSC",
"GSI Technology",
"Numonyx/Micron",
"Explore",
"ATMEL",
"Semiconn",
"Nanya",
"Lattice",
"Aspeed",
"Marvell",
"Vitesse",
"Macronix",
"IDT",
"AMIC",
"Winbond",
"IDT1",
"NXP1",
"Eyenix",
"Nextchip",
"EON-SSI",
"Silicon Image",
"Davicom",
"AKROSSILICON",
"PERICOM",
"ITE Tech",
"Microchip1",
"ALTERA",
"AKM",
"Maxim1",
"TI_",
"Silicon Labs",
"FTDI",
"TIBBO",
"MICROSEMI",
"ASMEDIA",
"Intel",
"ALGOLTEK",
"Terminus",
"ASIX",
"Chrontel",
"Kinetic",
"Rohm Semiconductor",
"Prolific Technology",
"Sony",
"WCH",
"QT-Brightek",
"MPS",
"Cirrus Logic",
"Coilcraft",
"Gotrend",
"Toko",
"Wurth1",
"Fastron",
"Panasonic ",
"Laird-Signal Integrity",
"Pulse old",
"Bothhand USA",
"North Hills",
"XFMRS",
"Mini-Circuits",
"Sumida",
"Eaton Bussmann",
"CHILISIN",
"Bright Led Electronics Corp.",
"OptekTech",
"LUMEX",
"Kingbright",
"OSRAM",
"Lite-On Semiconductor Corp.",
"SunLED",
"FOX Electronics",
"MERCURY ElectInd",
"CTS",
"TXC CORPORATION",
"Raltron",
"SCTF",
"ECS",
"Crec",
"Discera",
"SiTIME",
"EPSON",
"TAITIEN",
"YOKETAN",
"Interquip Electronics",
"Ecliptek",
"Richtek",
"Akros Silicon",
"Kiwi Sem",
"GE Energy",
"Nemic_Lambda",
"Emerson",
"Traco Power",
"Silver Telcom",
"Torex",
"GE Critical Power",
"CINCON",
"Anpec",
"M3TEK",
"Techcode",
"Aerosemi Tech",
"Recom",
"ALPHA & OMEGA",
"Vishay1",
"KOA",
"Stack Pole Electronics",
"Royal OHM",
"Susumu",
"HONG-XING",
"tamars test plant",
"Guangdong Fenghua Advanced Tech",
"Salecom Electronics",
"C&K Components",
"Wealth Metal Factory",
"TONE PART",
"E-SWITCH",
"Diptronics Manufacturing Inc.",
"APEM Components",
"Dailywell Electronics",
"Grayhill",
"NKK SWITCHES",
"NIDEC COPAL",
"TENMA",
"GoMax Electronics",
"CIE-Group Limited",
"Fini",
"Socomec",
"Hochiki",
"Total Phase",
"Murideo",
"Vision Engineering",
"Central-Semiconductor",
"CEL",
"NEC Electronics America",
"AMPENOL",
};
        public string _clientPrefix = string.Empty;
        //public List<WHitem> avlItems = new List<WHitem>();
        //public string avlROBOTRON = "\\\\dbr1\\Data\\WareHouse\\STOCK_CUSTOMERS\\ROBOTRON\\ROBOTRON_AVL.xlsm";
        public FrmIPNgenerator(List<WHitem> avlItems, string clientPrefix, string sqlAvl)
        {
            this._clientPrefix = clientPrefix;
            this.avlItemsFromTheMainForm = avlItems;
            this.sqlAvlConnectionStringFromMainForm = sqlAvl;


            InitializeComponent();
            StartUpLogic();
            UpdateControlColors(this);
            //DataLoaderAVL(avlROBOTRON, "AVL");
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Update control colors based on your criteria
                control.BackColor = Color.LightGray;
                control.ForeColor = Color.White;
                // Handle Button controls separately
                if (control is Button button)
                {
                    button.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    button.FlatAppearance.BorderColor = Color.DarkGray; // Change border color
                    button.ForeColor = Color.Black;
                }
                // Handle Button controls separately
                if (control is GroupBox groupbox)
                {
                    groupbox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    groupbox.ForeColor = Color.Black;
                }
                // Handle TextBox controls separately
                if (control is TextBox textBox)
                {
                    textBox.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    textBox.BackColor = Color.LightGray; // Change background color
                    textBox.ForeColor = Color.Black; // Change text color
                }
                // Handle Label controls separately
                if (control is Label label)
                {
                    label.BorderStyle = BorderStyle.FixedSingle; // Set border style to FixedSingle
                    label.BackColor = Color.Gray; // Change background color
                    label.ForeColor = Color.Black; // Change text color
                }
                // Handle TabControl controls separately
                if (control is TabControl tabControl)
                {
                    //tabControl.BackColor = Color.Black; // Change TabControl background color
                    tabControl.ForeColor = Color.Black;
                    // Handle each TabPage within the TabControl
                    foreach (TabPage tabPage in tabControl.TabPages)
                    {
                        tabPage.BackColor = Color.Gray; // Change TabPage background color
                        tabPage.ForeColor = Color.Black; // Change TabPage text color
                    }
                }
                // Handle DataGridView controls separately
                if (control is DataGridView dataGridView)
                {
                    // Update DataGridView styles
                    dataGridView.EnableHeadersVisualStyles = false;
                    dataGridView.BackgroundColor = Color.DarkGray;
                    dataGridView.ColumnHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                    dataGridView.RowHeadersDefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.BackColor = Color.Gray;
                    dataGridView.DefaultCellStyle.ForeColor = Color.White;
                    dataGridView.DefaultCellStyle.SelectionBackColor = Color.Green;
                    dataGridView.DefaultCellStyle.SelectionForeColor = Color.White;
                    // Change the header cell styles for each column
                    foreach (DataGridViewColumn column in dataGridView.Columns)
                    {
                        column.HeaderCell.Style.BackColor = Color.DarkGray;
                        column.HeaderCell.Style.ForeColor = Color.Black;
                    }
                }
                // Handle ComboBox controls separately
                if (control is ComboBox comboBox)
                {
                    comboBox.FlatStyle = FlatStyle.Flat; // Set FlatStyle to Flat
                    comboBox.BackColor = Color.DarkGray; // Change ComboBox background color
                    comboBox.ForeColor = Color.Black; // Change ComboBox text color
                }
                // Handle DateTimePicker controls separately
                if (control is DateTimePicker dateTimePicker)
                {
                    // Change DateTimePicker's custom properties here
                    dateTimePicker.BackColor = Color.DarkGray; // Change DateTimePicker background color
                    dateTimePicker.ForeColor = Color.White; // Change DateTimePicker text color
                                                            // Customize other DateTimePicker properties as needed
                }
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    UpdateControlColors(control);
                }
            }
        }
        public void StartUpLogic()
        {
            List<string> uniqueManufacturers = avlItemsFromTheMainForm
            .Select(item => item.Manufacturer)
            .Distinct()
            .OrderBy(manufacturer => manufacturer)
            .ToList();
            typesNamesList = typesNamesList.OrderBy(typeName => typeName).ToList();
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(typesNamesList.ToArray());
            List<string> orderedmanList = manufacturersList.OrderBy(manufacturer => manufacturer).ToList();
            comboBox2.Items.Clear();
            comboBox2.Items.AddRange(orderedmanList.ToArray());
        }
        private void IPNstringConstructor()
        {
            textBox3.Text = string.Empty;
            string typeOfTheIem = comboBox1.SelectedItem.ToString();
            try
            {
                //Sample selected type
                string selectedType = comboBox1.SelectedItem.ToString(); // Replace with your actual ComboBox logic
                // Filter WHitems by the selected type
                var filteredItems = avlItemsFromTheMainForm
                    .Where(item => item.IPN.StartsWith($"ROB_{selectedType}-"))
                    .ToList();
                // Extract numbers from the filtered WHitems
                var numbers = filteredItems
                    .Select(item => int.Parse(item.IPN.Split('-')[1]))
                    .ToList();
                // Find the lowest available number
                int lowestAvailableNumber = Enumerable.Range(1, 9999)
                    .Except(numbers)
                    .Min();
                // Display the result in textbox2
                textBox1.Text = $"{lowestAvailableNumber:D4}";
            }
            catch (InvalidOperationException ex)
            {
                // Handle the case where the selected type does not exist
                // You can display a message or take appropriate action here
                textBox1.Text = "0001";
            }
            string numericVal = textBox1.Text.ToString();
            textBox3.Text = _clientPrefix + "_" + typeOfTheIem + "-" + numericVal;
            textBox3.Update();
        }
        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            IPNstringConstructor();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //WHitem itemToAddToAvl = new WHitem();
            //itemToAddToAvl.IPN = textBox3.Text.ToString();
            //itemToAddToAvl.Manufacturer = comboBox2.SelectedItem.ToString();
            //itemToAddToAvl.MFPN = textBox2.Text.ToString();
            //itemToAddToAvl.Description = richTextBox1.Text.ToString();
            //DataInserter(avlROBOTRON, "AVL", itemToAddToAvl);
            //        if (!string.IsNullOrEmpty(textBox3.Text) &&
            //comboBox2.SelectedItem != null &&
            //!string.IsNullOrEmpty(textBox2.Text) &&
            //!string.IsNullOrEmpty(richTextBox1.Text))
            //        {
            //            WHitem itemToAddToAvl = new WHitem();
            //            itemToAddToAvl.IPN = textBox3.Text.ToString();
            //            itemToAddToAvl.Manufacturer = comboBox2.SelectedItem.ToString();
            //            itemToAddToAvl.MFPN = textBox2.Text.ToString();
            //            itemToAddToAvl.Description = richTextBox1.Text.ToString();
            //            DataInserter(avlROBOTRON, "AVL", itemToAddToAvl);
            //            this.Dispose();
            //            this.Close();
            //        }
            //        else
            //        {
            //            MessageBox.Show("Please populate all the required fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        }
            if (!string.IsNullOrEmpty(textBox3.Text) &&
    comboBox2.SelectedItem != null &&
    !string.IsNullOrEmpty(textBox2.Text) &&
    !string.IsNullOrEmpty(richTextBox1.Text))
            {
                WHitem itemToAddToAvl = new WHitem();
                itemToAddToAvl.IPN = textBox3.Text.ToString();
                itemToAddToAvl.Manufacturer = comboBox2.SelectedItem.ToString();
                itemToAddToAvl.MFPN = textBox2.Text.ToString();
                itemToAddToAvl.Description = richTextBox1.Text.ToString();
                DataInserter(itemToAddToAvl);

                //this.Dispose();
                this.Close();
            }
            else
            {
                if (string.IsNullOrEmpty(textBox3.Text))
                {
                    textBox3.Focus();
                }
                else if (comboBox2.SelectedItem == null)
                {
                    comboBox2.Focus();
                }
                else if (string.IsNullOrEmpty(textBox2.Text))
                {
                    textBox2.Focus();
                }
                else if (string.IsNullOrEmpty(richTextBox1.Text))
                {
                    richTextBox1.Focus();
                }
                MessageBox.Show("Please populate all the required fields before proceeding.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void DataInserter(WHitem wHitem)
        {
            //try
            //{
            //    string constr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fp + "; Extended Properties=\"Excel 12.0 Macro;HDR=YES;IMEX=0\"";
            //    using (OleDbConnection conn = new OleDbConnection(constr))
            //    {
            //        conn.Open();
            //        OleDbCommand command = new OleDbCommand("INSERT INTO [" + thesheetName + "$] (IPN,Manufacturer,MFPN,Description) values('" + wHitem.IPN + "','" + wHitem.Manufacturer + "','" + wHitem.MFPN + "','" + wHitem.Description + "')", conn);
            //        command.ExecuteNonQuery();
            //        conn.Close();
            //    }
            //    MessageBox.Show(wHitem.IPN.ToString() + " added to AVL");
            //}
            //catch (IOException)
            //{
            //    MessageBox.Show("Error");
            //}


            try
            {
                string constr = sqlAvlConnectionStringFromMainForm;
                using (SqlConnection conn = new SqlConnection(constr))
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand("INSERT INTO AVL (IPN, Manufacturer, MFPN, Description) VALUES (@IPN, @Manufacturer, @MFPN, @Description)", conn);
                    command.Parameters.AddWithValue("@IPN", wHitem.IPN);
                    command.Parameters.AddWithValue("@Manufacturer", wHitem.Manufacturer);
                    command.Parameters.AddWithValue("@MFPN", wHitem.MFPN);
                    command.Parameters.AddWithValue("@Description", wHitem.Description);
                    command.ExecuteNonQuery();
                    conn.Close();
                }

                string successMessage = "Item successfully added to AVL!" + Environment.NewLine +
        "IPN: " + wHitem.IPN + Environment.NewLine +
        "Manufacturer: " + wHitem.Manufacturer + Environment.NewLine +
        "MFPN: " + wHitem.MFPN + Environment.NewLine +
        "Description: " + wHitem.Description;

                MessageBox.Show(successMessage, "Success");
            }
            catch (IOException)
            {
                MessageBox.Show("Error");
            }

        }
        private void textBox1_KeyUp(object sender, KeyEventArgs e)
        {
            IPNstringConstructor();
        }
    }
}
