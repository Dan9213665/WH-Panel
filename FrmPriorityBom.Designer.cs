namespace WH_Panel
{
    partial class FrmPriorityBom
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPriorityBom));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblLoading = new Label();
            cmbROBxList = new ComboBox();
            btnKitLabel = new Button();
            btnReport = new Button();
            cnkbClosed = new CheckBox();
            gbxLoadedWo = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            txtbRob = new TextBox();
            txtbName = new TextBox();
            txtbQty = new TextBox();
            txtbStatus = new TextBox();
            txtbInputIPN = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            txtbRev = new TextBox();
            txtbINPUTqty = new TextBox();
            btnGetMFNs = new Button();
            progressBar1 = new ProgressBar();
            lblProgress = new Label();
            lblSim = new Label();
            btnGetWHstock = new Button();
            dgwBom = new DataGridView();
            tableLayoutPanel4 = new TableLayoutPanel();
            lblPing = new Label();
            txtbLog = new RichTextBox();
            btnInStock = new Button();
            tpmProgressBar = new ProgressBar();
            groupBox2 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            rtxtbComments = new RichTextBox();
            btnSaveComments = new Button();
            btnGetComms = new Button();
            tableLayoutPanel6 = new TableLayoutPanel();
            gbxIPNstockMovements = new GroupBox();
            dgwIPNmoves = new DataGridView();
            groupBox3 = new GroupBox();
            dgwINSTOCK = new DataGridView();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            gbxLoadedWo.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwBom).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            gbxIPNstockMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwIPNmoves).BeginInit();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwINSTOCK).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(gbxLoadedWo, 0, 1);
            tableLayoutPanel1.Controls.Add(dgwBom, 0, 2);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 4);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel6, 1, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(1168, 659);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(694, 64);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Select ROB******  WO";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 76F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 6F));
            tableLayoutPanel2.Controls.Add(lblLoading, 2, 0);
            tableLayoutPanel2.Controls.Add(cmbROBxList, 0, 0);
            tableLayoutPanel2.Controls.Add(btnKitLabel, 3, 0);
            tableLayoutPanel2.Controls.Add(btnReport, 4, 0);
            tableLayoutPanel2.Controls.Add(cnkbClosed, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(688, 42);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.BackColor = Color.IndianRed;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.ForeColor = Color.White;
            lblLoading.Location = new Point(566, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(35, 42);
            lblLoading.TabIndex = 1;
            lblLoading.Text = "Loading";
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbROBxList
            // 
            cmbROBxList.Dock = DockStyle.Fill;
            cmbROBxList.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbROBxList.Font = new Font("Segoe UI", 12F);
            cmbROBxList.FormattingEnabled = true;
            cmbROBxList.Location = new Point(3, 3);
            cmbROBxList.Name = "cmbROBxList";
            cmbROBxList.Size = new Size(516, 29);
            cmbROBxList.TabIndex = 0;
            cmbROBxList.SelectedIndexChanged += cmbROBxList_SelectedIndexChanged;
            // 
            // btnKitLabel
            // 
            btnKitLabel.BackgroundImage = Properties.Resources.kitLabelPrint;
            btnKitLabel.BackgroundImageLayout = ImageLayout.Stretch;
            btnKitLabel.Dock = DockStyle.Fill;
            btnKitLabel.Location = new Point(607, 3);
            btnKitLabel.Name = "btnKitLabel";
            btnKitLabel.Size = new Size(35, 36);
            btnKitLabel.TabIndex = 2;
            btnKitLabel.UseVisualStyleBackColor = true;
            btnKitLabel.Click += btnKitLabel_Click;
            btnKitLabel.MouseDown += btnKitLabel_MouseDown;
            // 
            // btnReport
            // 
            btnReport.BackgroundImage = Properties.Resources.sendtoprinter;
            btnReport.BackgroundImageLayout = ImageLayout.Center;
            btnReport.Dock = DockStyle.Fill;
            btnReport.Location = new Point(648, 3);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(37, 36);
            btnReport.TabIndex = 3;
            btnReport.UseVisualStyleBackColor = true;
            btnReport.Click += btnReport_Click;
            // 
            // cnkbClosed
            // 
            cnkbClosed.AutoSize = true;
            cnkbClosed.CheckAlign = ContentAlignment.MiddleCenter;
            cnkbClosed.Dock = DockStyle.Fill;
            cnkbClosed.Location = new Point(525, 3);
            cnkbClosed.Name = "cnkbClosed";
            cnkbClosed.Size = new Size(35, 36);
            cnkbClosed.TabIndex = 4;
            cnkbClosed.TextAlign = ContentAlignment.MiddleCenter;
            cnkbClosed.UseVisualStyleBackColor = true;
            // 
            // gbxLoadedWo
            // 
            gbxLoadedWo.AutoSize = true;
            gbxLoadedWo.Controls.Add(tableLayoutPanel3);
            gbxLoadedWo.Dock = DockStyle.Fill;
            gbxLoadedWo.Location = new Point(3, 73);
            gbxLoadedWo.Name = "gbxLoadedWo";
            gbxLoadedWo.Size = new Size(694, 104);
            gbxLoadedWo.TabIndex = 2;
            gbxLoadedWo.TabStop = false;
            gbxLoadedWo.Text = "Loaded WO";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 5;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel3.Controls.Add(txtbRob, 0, 0);
            tableLayoutPanel3.Controls.Add(txtbName, 1, 0);
            tableLayoutPanel3.Controls.Add(txtbQty, 3, 0);
            tableLayoutPanel3.Controls.Add(txtbStatus, 4, 0);
            tableLayoutPanel3.Controls.Add(txtbInputIPN, 0, 1);
            tableLayoutPanel3.Controls.Add(textBox2, 1, 1);
            tableLayoutPanel3.Controls.Add(textBox3, 2, 1);
            tableLayoutPanel3.Controls.Add(textBox4, 3, 1);
            tableLayoutPanel3.Controls.Add(txtbRev, 2, 0);
            tableLayoutPanel3.Controls.Add(txtbINPUTqty, 0, 2);
            tableLayoutPanel3.Controls.Add(btnGetMFNs, 1, 2);
            tableLayoutPanel3.Controls.Add(progressBar1, 4, 1);
            tableLayoutPanel3.Controls.Add(lblProgress, 4, 2);
            tableLayoutPanel3.Controls.Add(lblSim, 3, 2);
            tableLayoutPanel3.Controls.Add(btnGetWHstock, 2, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.Size = new Size(688, 82);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // txtbRob
            // 
            txtbRob.Dock = DockStyle.Fill;
            txtbRob.Location = new Point(3, 3);
            txtbRob.Name = "txtbRob";
            txtbRob.ReadOnly = true;
            txtbRob.Size = new Size(131, 23);
            txtbRob.TabIndex = 0;
            txtbRob.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbName
            // 
            txtbName.Dock = DockStyle.Fill;
            txtbName.Location = new Point(140, 3);
            txtbName.Name = "txtbName";
            txtbName.ReadOnly = true;
            txtbName.Size = new Size(131, 23);
            txtbName.TabIndex = 1;
            txtbName.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbQty
            // 
            txtbQty.Dock = DockStyle.Fill;
            txtbQty.Location = new Point(414, 3);
            txtbQty.Name = "txtbQty";
            txtbQty.ReadOnly = true;
            txtbQty.Size = new Size(131, 23);
            txtbQty.TabIndex = 2;
            txtbQty.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbStatus
            // 
            txtbStatus.Dock = DockStyle.Fill;
            txtbStatus.Location = new Point(551, 3);
            txtbStatus.Name = "txtbStatus";
            txtbStatus.ReadOnly = true;
            txtbStatus.Size = new Size(134, 23);
            txtbStatus.TabIndex = 3;
            txtbStatus.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbInputIPN
            // 
            txtbInputIPN.AcceptsTab = true;
            txtbInputIPN.Dock = DockStyle.Fill;
            txtbInputIPN.Location = new Point(3, 30);
            txtbInputIPN.Name = "txtbInputIPN";
            txtbInputIPN.PlaceholderText = "Filter by IPN";
            txtbInputIPN.Size = new Size(131, 23);
            txtbInputIPN.TabIndex = 4;
            txtbInputIPN.TextAlign = HorizontalAlignment.Center;
            txtbInputIPN.KeyDown += txtbInputIPN_KeyDown;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(140, 30);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Filter by MFPN";
            textBox2.Size = new Size(131, 23);
            textBox2.TabIndex = 4;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(277, 30);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Filter by Description";
            textBox3.Size = new Size(131, 23);
            textBox3.TabIndex = 4;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(414, 30);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Filter by ALT";
            textBox4.Size = new Size(131, 23);
            textBox4.TabIndex = 4;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbRev
            // 
            txtbRev.Dock = DockStyle.Fill;
            txtbRev.Location = new Point(277, 3);
            txtbRev.Name = "txtbRev";
            txtbRev.ReadOnly = true;
            txtbRev.Size = new Size(131, 23);
            txtbRev.TabIndex = 5;
            txtbRev.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbINPUTqty
            // 
            txtbINPUTqty.Dock = DockStyle.Fill;
            txtbINPUTqty.Location = new Point(3, 57);
            txtbINPUTqty.Name = "txtbINPUTqty";
            txtbINPUTqty.PlaceholderText = "Input Qty";
            txtbINPUTqty.Size = new Size(131, 23);
            txtbINPUTqty.TabIndex = 6;
            txtbINPUTqty.TextAlign = HorizontalAlignment.Center;
            txtbINPUTqty.KeyDown += txtbINPUTqty_KeyDown;
            txtbINPUTqty.KeyUp += txtbINPUTqty_KeyUp;
            // 
            // btnGetMFNs
            // 
            btnGetMFNs.Dock = DockStyle.Fill;
            btnGetMFNs.Font = new Font("Segoe UI", 7F);
            btnGetMFNs.Location = new Point(140, 57);
            btnGetMFNs.Name = "btnGetMFNs";
            btnGetMFNs.Size = new Size(131, 22);
            btnGetMFNs.TabIndex = 7;
            btnGetMFNs.Text = "GET MFPNs";
            btnGetMFNs.UseVisualStyleBackColor = true;
            btnGetMFNs.Click += btnGetMFNs_Click;
            btnGetMFNs.MouseDown += btnGetMFNs_MouseDown;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new Point(551, 30);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(134, 21);
            progressBar1.TabIndex = 8;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Dock = DockStyle.Fill;
            lblProgress.Location = new Point(551, 54);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(134, 28);
            lblProgress.TabIndex = 9;
            lblProgress.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSim
            // 
            lblSim.AutoSize = true;
            lblSim.Dock = DockStyle.Fill;
            lblSim.Location = new Point(414, 54);
            lblSim.Name = "lblSim";
            lblSim.Size = new Size(131, 28);
            lblSim.TabIndex = 10;
            lblSim.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGetWHstock
            // 
            btnGetWHstock.Dock = DockStyle.Fill;
            btnGetWHstock.Font = new Font("Segoe UI", 7F);
            btnGetWHstock.Location = new Point(277, 57);
            btnGetWHstock.Name = "btnGetWHstock";
            btnGetWHstock.Size = new Size(131, 22);
            btnGetWHstock.TabIndex = 11;
            btnGetWHstock.Text = "GET WH stock";
            btnGetWHstock.UseVisualStyleBackColor = true;
            btnGetWHstock.Click += btnGetWHstock_Click;
            // 
            // dgwBom
            // 
            dgwBom.AllowUserToAddRows = false;
            dgwBom.AllowUserToDeleteRows = false;
            dgwBom.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwBom.Dock = DockStyle.Fill;
            dgwBom.Location = new Point(3, 183);
            dgwBom.Name = "dgwBom";
            dgwBom.ReadOnly = true;
            tableLayoutPanel1.SetRowSpan(dgwBom, 3);
            dgwBom.Size = new Size(694, 473);
            dgwBom.TabIndex = 3;
            dgwBom.CellClick += dgwBom_CellClick;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel4, 2);
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(lblPing, 1, 0);
            tableLayoutPanel4.Controls.Add(txtbLog, 0, 1);
            tableLayoutPanel4.Controls.Add(btnInStock, 0, 0);
            tableLayoutPanel4.Controls.Add(tpmProgressBar, 0, 2);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(703, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel4, 3);
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 18.3908043F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 81.60919F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 31F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel4.Size = new Size(462, 239);
            tableLayoutPanel4.TabIndex = 6;
            // 
            // lblPing
            // 
            lblPing.AutoSize = true;
            lblPing.Dock = DockStyle.Fill;
            lblPing.Location = new Point(234, 0);
            lblPing.Name = "lblPing";
            lblPing.Size = new Size(225, 40);
            lblPing.TabIndex = 5;
            lblPing.Text = "tProc";
            lblPing.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtbLog
            // 
            tableLayoutPanel4.SetColumnSpan(txtbLog, 2);
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Location = new Point(3, 43);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(456, 172);
            txtbLog.TabIndex = 6;
            txtbLog.Text = "";
            // 
            // btnInStock
            // 
            btnInStock.Dock = DockStyle.Fill;
            btnInStock.Location = new Point(3, 3);
            btnInStock.Name = "btnInStock";
            btnInStock.Size = new Size(225, 34);
            btnInStock.TabIndex = 7;
            btnInStock.Text = "IN STOCK";
            btnInStock.UseVisualStyleBackColor = true;
            btnInStock.Visible = false;
            btnInStock.Click += btnInStock_Click;
            // 
            // tpmProgressBar
            // 
            tableLayoutPanel4.SetColumnSpan(tpmProgressBar, 2);
            tpmProgressBar.Dock = DockStyle.Fill;
            tpmProgressBar.Location = new Point(3, 221);
            tpmProgressBar.Name = "tpmProgressBar";
            tpmProgressBar.Size = new Size(456, 15);
            tpmProgressBar.TabIndex = 8;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(tableLayoutPanel5);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(703, 579);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(462, 77);
            groupBox2.TabIndex = 7;
            groupBox2.TabStop = false;
            groupBox2.Text = "Comments";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 2;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 72.2342758F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 27.7657223F));
            tableLayoutPanel5.Controls.Add(rtxtbComments, 0, 0);
            tableLayoutPanel5.Controls.Add(btnSaveComments, 1, 1);
            tableLayoutPanel5.Controls.Add(btnGetComms, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(456, 55);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // rtxtbComments
            // 
            rtxtbComments.Dock = DockStyle.Fill;
            rtxtbComments.Location = new Point(3, 3);
            rtxtbComments.Name = "rtxtbComments";
            tableLayoutPanel5.SetRowSpan(rtxtbComments, 2);
            rtxtbComments.Size = new Size(323, 49);
            rtxtbComments.TabIndex = 0;
            rtxtbComments.Text = "";
            rtxtbComments.Enter += rtxtbComments_Enter;
            rtxtbComments.Leave += rtxtbComments_Leave;
            // 
            // btnSaveComments
            // 
            btnSaveComments.Dock = DockStyle.Fill;
            btnSaveComments.Location = new Point(332, 30);
            btnSaveComments.Name = "btnSaveComments";
            btnSaveComments.Size = new Size(121, 22);
            btnSaveComments.TabIndex = 1;
            btnSaveComments.Text = "SET Comments";
            btnSaveComments.UseVisualStyleBackColor = true;
            btnSaveComments.Click += btnSaveComments_Click;
            // 
            // btnGetComms
            // 
            btnGetComms.Dock = DockStyle.Fill;
            btnGetComms.Location = new Point(332, 3);
            btnGetComms.Name = "btnGetComms";
            btnGetComms.Size = new Size(121, 21);
            btnGetComms.TabIndex = 2;
            btnGetComms.Text = "GET Comments";
            btnGetComms.UseVisualStyleBackColor = true;
            btnGetComms.Click += btnGetComms_Click;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel1.SetColumnSpan(tableLayoutPanel6, 2);
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Controls.Add(gbxIPNstockMovements, 0, 1);
            tableLayoutPanel6.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(703, 248);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel6.Size = new Size(462, 325);
            tableLayoutPanel6.TabIndex = 8;
            // 
            // gbxIPNstockMovements
            // 
            gbxIPNstockMovements.AutoSize = true;
            tableLayoutPanel6.SetColumnSpan(gbxIPNstockMovements, 2);
            gbxIPNstockMovements.Controls.Add(dgwIPNmoves);
            gbxIPNstockMovements.Dock = DockStyle.Fill;
            gbxIPNstockMovements.Location = new Point(3, 133);
            gbxIPNstockMovements.Name = "gbxIPNstockMovements";
            gbxIPNstockMovements.Size = new Size(456, 189);
            gbxIPNstockMovements.TabIndex = 4;
            gbxIPNstockMovements.TabStop = false;
            gbxIPNstockMovements.Text = "Stock Movements";
            // 
            // dgwIPNmoves
            // 
            dgwIPNmoves.AllowUserToAddRows = false;
            dgwIPNmoves.AllowUserToDeleteRows = false;
            dgwIPNmoves.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwIPNmoves.Dock = DockStyle.Fill;
            dgwIPNmoves.Location = new Point(3, 19);
            dgwIPNmoves.Name = "dgwIPNmoves";
            dgwIPNmoves.ReadOnly = true;
            dgwIPNmoves.Size = new Size(450, 167);
            dgwIPNmoves.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dgwINSTOCK);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(456, 124);
            groupBox3.TabIndex = 5;
            groupBox3.TabStop = false;
            groupBox3.Text = "IN STOCK";
            // 
            // dgwINSTOCK
            // 
            dgwINSTOCK.AllowUserToAddRows = false;
            dgwINSTOCK.AllowUserToDeleteRows = false;
            dgwINSTOCK.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwINSTOCK.Dock = DockStyle.Fill;
            dgwINSTOCK.Location = new Point(3, 19);
            dgwINSTOCK.Name = "dgwINSTOCK";
            dgwINSTOCK.ReadOnly = true;
            dgwINSTOCK.Size = new Size(450, 102);
            dgwINSTOCK.TabIndex = 0;
            // 
            // FrmPriorityBom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1168, 659);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPriorityBom";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPriorityBom";
            WindowState = FormWindowState.Maximized;
            FormClosing += FrmPriorityBom_FormClosing;
            Load += FrmPriorityBom_Load;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            gbxLoadedWo.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwBom).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            gbxIPNstockMovements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwIPNmoves).EndInit();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwINSTOCK).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private ComboBox cmbROBxList;
        private Label lblLoading;
        private TableLayoutPanel tableLayoutPanel2;
        private GroupBox gbxLoadedWo;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox txtbRob;
        private TextBox txtbName;
        private TextBox txtbQty;
        private TextBox txtbStatus;
        private DataGridView dgwBom;
        private TextBox txtbInputIPN;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private GroupBox gbxIPNstockMovements;
        private DataGridView dgwIPNmoves;
        private Label lblPing;
        private TextBox txtbRev;
        private TextBox txtbINPUTqty;
        private Button btnKitLabel;
        private Button btnReport;
        private Button btnGetMFNs;
        private ProgressBar progressBar1;
        private Label lblProgress;
        private Label lblSim;
        private Button btnGetWHstock;
        private TableLayoutPanel tableLayoutPanel4;
        private RichTextBox txtbLog;
        private CheckBox cnkbClosed;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel5;
        private RichTextBox rtxtbComments;
        private Button btnSaveComments;
        private Button btnGetComms;
        private Button btnInStock;
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox groupBox3;
        private DataGridView dgwINSTOCK;
        private ProgressBar tpmProgressBar;
    }
}