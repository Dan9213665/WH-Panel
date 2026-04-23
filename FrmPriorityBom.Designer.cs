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
            progressBarContainer = new Panel();
            progressBarFill = new Panel();
            lblTpmText = new Label();
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
            progressBarContainer.SuspendLayout();
            progressBarFill.SuspendLayout();
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
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 93F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 147F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 87F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(1335, 879);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 4);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(795, 85);
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
            tableLayoutPanel2.Location = new Point(3, 24);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(789, 57);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.BackColor = Color.IndianRed;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.ForeColor = Color.White;
            lblLoading.Location = new Point(649, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(41, 57);
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
            cmbROBxList.Location = new Point(3, 4);
            cmbROBxList.Margin = new Padding(3, 4, 3, 4);
            cmbROBxList.Name = "cmbROBxList";
            cmbROBxList.Size = new Size(593, 36);
            cmbROBxList.TabIndex = 0;
            cmbROBxList.SelectedIndexChanged += cmbROBxList_SelectedIndexChanged;
            // 
            // btnKitLabel
            // 
            btnKitLabel.BackgroundImage = Properties.Resources.kitLabelPrint;
            btnKitLabel.BackgroundImageLayout = ImageLayout.Stretch;
            btnKitLabel.Dock = DockStyle.Fill;
            btnKitLabel.Location = new Point(696, 4);
            btnKitLabel.Margin = new Padding(3, 4, 3, 4);
            btnKitLabel.Name = "btnKitLabel";
            btnKitLabel.Size = new Size(41, 49);
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
            btnReport.Location = new Point(743, 4);
            btnReport.Margin = new Padding(3, 4, 3, 4);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(43, 49);
            btnReport.TabIndex = 3;
            btnReport.UseVisualStyleBackColor = true;
            btnReport.Click += btnReport_Click;
            // 
            // cnkbClosed
            // 
            cnkbClosed.AutoSize = true;
            cnkbClosed.CheckAlign = ContentAlignment.MiddleCenter;
            cnkbClosed.Dock = DockStyle.Fill;
            cnkbClosed.Location = new Point(602, 4);
            cnkbClosed.Margin = new Padding(3, 4, 3, 4);
            cnkbClosed.Name = "cnkbClosed";
            cnkbClosed.Size = new Size(41, 49);
            cnkbClosed.TabIndex = 4;
            cnkbClosed.TextAlign = ContentAlignment.MiddleCenter;
            cnkbClosed.UseVisualStyleBackColor = true;
            // 
            // gbxLoadedWo
            // 
            gbxLoadedWo.AutoSize = true;
            gbxLoadedWo.Controls.Add(tableLayoutPanel3);
            gbxLoadedWo.Dock = DockStyle.Fill;
            gbxLoadedWo.Location = new Point(3, 97);
            gbxLoadedWo.Margin = new Padding(3, 4, 3, 4);
            gbxLoadedWo.Name = "gbxLoadedWo";
            gbxLoadedWo.Padding = new Padding(3, 4, 3, 4);
            gbxLoadedWo.Size = new Size(795, 139);
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
            tableLayoutPanel3.Location = new Point(3, 24);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.Size = new Size(789, 111);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // txtbRob
            // 
            txtbRob.Dock = DockStyle.Fill;
            txtbRob.Location = new Point(3, 4);
            txtbRob.Margin = new Padding(3, 4, 3, 4);
            txtbRob.Name = "txtbRob";
            txtbRob.ReadOnly = true;
            txtbRob.Size = new Size(151, 27);
            txtbRob.TabIndex = 0;
            txtbRob.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbName
            // 
            txtbName.Dock = DockStyle.Fill;
            txtbName.Location = new Point(160, 4);
            txtbName.Margin = new Padding(3, 4, 3, 4);
            txtbName.Name = "txtbName";
            txtbName.ReadOnly = true;
            txtbName.Size = new Size(151, 27);
            txtbName.TabIndex = 1;
            txtbName.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbQty
            // 
            txtbQty.Dock = DockStyle.Fill;
            txtbQty.Location = new Point(474, 4);
            txtbQty.Margin = new Padding(3, 4, 3, 4);
            txtbQty.Name = "txtbQty";
            txtbQty.ReadOnly = true;
            txtbQty.Size = new Size(151, 27);
            txtbQty.TabIndex = 2;
            txtbQty.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbStatus
            // 
            txtbStatus.Dock = DockStyle.Fill;
            txtbStatus.Location = new Point(631, 4);
            txtbStatus.Margin = new Padding(3, 4, 3, 4);
            txtbStatus.Name = "txtbStatus";
            txtbStatus.ReadOnly = true;
            txtbStatus.Size = new Size(155, 27);
            txtbStatus.TabIndex = 3;
            txtbStatus.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbInputIPN
            // 
            txtbInputIPN.AcceptsTab = true;
            txtbInputIPN.Dock = DockStyle.Fill;
            txtbInputIPN.Location = new Point(3, 40);
            txtbInputIPN.Margin = new Padding(3, 4, 3, 4);
            txtbInputIPN.Name = "txtbInputIPN";
            txtbInputIPN.PlaceholderText = "Filter by IPN";
            txtbInputIPN.Size = new Size(151, 27);
            txtbInputIPN.TabIndex = 4;
            txtbInputIPN.TextAlign = HorizontalAlignment.Center;
            txtbInputIPN.KeyDown += txtbInputIPN_KeyDown;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(160, 40);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Filter by MFPN";
            textBox2.Size = new Size(151, 27);
            textBox2.TabIndex = 4;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(317, 40);
            textBox3.Margin = new Padding(3, 4, 3, 4);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Filter by Description";
            textBox3.Size = new Size(151, 27);
            textBox3.TabIndex = 4;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(474, 40);
            textBox4.Margin = new Padding(3, 4, 3, 4);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Filter by ALT";
            textBox4.Size = new Size(151, 27);
            textBox4.TabIndex = 4;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbRev
            // 
            txtbRev.Dock = DockStyle.Fill;
            txtbRev.Location = new Point(317, 4);
            txtbRev.Margin = new Padding(3, 4, 3, 4);
            txtbRev.Name = "txtbRev";
            txtbRev.ReadOnly = true;
            txtbRev.Size = new Size(151, 27);
            txtbRev.TabIndex = 5;
            txtbRev.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbINPUTqty
            // 
            txtbINPUTqty.Dock = DockStyle.Fill;
            txtbINPUTqty.Location = new Point(3, 77);
            txtbINPUTqty.Margin = new Padding(3, 4, 3, 4);
            txtbINPUTqty.Name = "txtbINPUTqty";
            txtbINPUTqty.PlaceholderText = "Input Qty";
            txtbINPUTqty.Size = new Size(151, 27);
            txtbINPUTqty.TabIndex = 6;
            txtbINPUTqty.TextAlign = HorizontalAlignment.Center;
            txtbINPUTqty.KeyDown += txtbINPUTqty_KeyDown;
            txtbINPUTqty.KeyUp += txtbINPUTqty_KeyUp;
            // 
            // btnGetMFNs
            // 
            btnGetMFNs.Dock = DockStyle.Fill;
            btnGetMFNs.Font = new Font("Segoe UI", 7F);
            btnGetMFNs.Location = new Point(160, 77);
            btnGetMFNs.Margin = new Padding(3, 4, 3, 4);
            btnGetMFNs.Name = "btnGetMFNs";
            btnGetMFNs.Size = new Size(151, 30);
            btnGetMFNs.TabIndex = 7;
            btnGetMFNs.Text = "GET MFPNs";
            btnGetMFNs.UseVisualStyleBackColor = true;
            btnGetMFNs.Click += btnGetMFNs_Click;
            btnGetMFNs.MouseDown += btnGetMFNs_MouseDown;
            // 
            // progressBar1
            // 
            progressBar1.Dock = DockStyle.Fill;
            progressBar1.Location = new Point(631, 40);
            progressBar1.Margin = new Padding(3, 4, 3, 4);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(155, 29);
            progressBar1.TabIndex = 8;
            // 
            // lblProgress
            // 
            lblProgress.AutoSize = true;
            lblProgress.Dock = DockStyle.Fill;
            lblProgress.Location = new Point(631, 73);
            lblProgress.Name = "lblProgress";
            lblProgress.Size = new Size(155, 38);
            lblProgress.TabIndex = 9;
            lblProgress.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSim
            // 
            lblSim.AutoSize = true;
            lblSim.Dock = DockStyle.Fill;
            lblSim.Location = new Point(474, 73);
            lblSim.Name = "lblSim";
            lblSim.Size = new Size(151, 38);
            lblSim.TabIndex = 10;
            lblSim.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGetWHstock
            // 
            btnGetWHstock.Dock = DockStyle.Fill;
            btnGetWHstock.Font = new Font("Segoe UI", 7F);
            btnGetWHstock.Location = new Point(317, 77);
            btnGetWHstock.Margin = new Padding(3, 4, 3, 4);
            btnGetWHstock.Name = "btnGetWHstock";
            btnGetWHstock.Size = new Size(151, 30);
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
            dgwBom.Location = new Point(3, 244);
            dgwBom.Margin = new Padding(3, 4, 3, 4);
            dgwBom.Name = "dgwBom";
            dgwBom.ReadOnly = true;
            dgwBom.RowHeadersWidth = 51;
            tableLayoutPanel1.SetRowSpan(dgwBom, 3);
            dgwBom.Size = new Size(795, 631);
            dgwBom.TabIndex = 3;
            dgwBom.CellClick += dgwBom_CellClick;
            dgwBom.MouseDown += dgwBom_MouseDown;
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
            tableLayoutPanel4.Controls.Add(progressBarContainer, 0, 2);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(804, 4);
            tableLayoutPanel4.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 3;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel4, 3);
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 18.3908043F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 81.60919F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 41F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 27F));
            tableLayoutPanel4.Size = new Size(528, 319);
            tableLayoutPanel4.TabIndex = 6;
            // 
            // lblPing
            // 
            lblPing.AutoSize = true;
            lblPing.Dock = DockStyle.Fill;
            lblPing.Location = new Point(267, 0);
            lblPing.Name = "lblPing";
            lblPing.Size = new Size(258, 53);
            lblPing.TabIndex = 5;
            lblPing.Text = "tProc";
            lblPing.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtbLog
            // 
            tableLayoutPanel4.SetColumnSpan(txtbLog, 2);
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Location = new Point(3, 57);
            txtbLog.Margin = new Padding(3, 4, 3, 4);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(522, 230);
            txtbLog.TabIndex = 6;
            txtbLog.Text = "";
            // 
            // btnInStock
            // 
            btnInStock.Dock = DockStyle.Fill;
            btnInStock.Location = new Point(3, 4);
            btnInStock.Margin = new Padding(3, 4, 3, 4);
            btnInStock.Name = "btnInStock";
            btnInStock.Size = new Size(258, 45);
            btnInStock.TabIndex = 7;
            btnInStock.Text = "IN STOCK";
            btnInStock.UseVisualStyleBackColor = true;
            btnInStock.Visible = false;
            btnInStock.Click += btnInStock_Click;
            // 
            // progressBarContainer
            // 
            tableLayoutPanel4.SetColumnSpan(progressBarContainer, 2);
            progressBarContainer.Controls.Add(progressBarFill);
            progressBarContainer.Dock = DockStyle.Fill;
            progressBarContainer.Location = new Point(3, 295);
            progressBarContainer.Margin = new Padding(3, 4, 3, 4);
            progressBarContainer.Name = "progressBarContainer";
            progressBarContainer.Size = new Size(522, 20);
            progressBarContainer.TabIndex = 8;
            // 
            // progressBarFill
            // 
            progressBarFill.Controls.Add(lblTpmText);
            progressBarFill.Dock = DockStyle.Left;
            progressBarFill.Location = new Point(0, 0);
            progressBarFill.Margin = new Padding(3, 4, 3, 4);
            progressBarFill.Name = "progressBarFill";
            progressBarFill.Size = new Size(521, 20);
            progressBarFill.TabIndex = 0;
            // 
            // lblTpmText
            // 
            lblTpmText.AutoSize = true;
            lblTpmText.Dock = DockStyle.Left;
            lblTpmText.ForeColor = Color.White;
            lblTpmText.Location = new Point(0, 0);
            lblTpmText.Name = "lblTpmText";
            lblTpmText.Size = new Size(65, 20);
            lblTpmText.TabIndex = 0;
            lblTpmText.Text = "Progress";
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(tableLayoutPanel5);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(804, 772);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(528, 103);
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
            tableLayoutPanel5.Location = new Point(3, 24);
            tableLayoutPanel5.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.Size = new Size(522, 75);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // rtxtbComments
            // 
            rtxtbComments.Dock = DockStyle.Fill;
            rtxtbComments.Location = new Point(3, 4);
            rtxtbComments.Margin = new Padding(3, 4, 3, 4);
            rtxtbComments.Name = "rtxtbComments";
            tableLayoutPanel5.SetRowSpan(rtxtbComments, 2);
            rtxtbComments.Size = new Size(371, 67);
            rtxtbComments.TabIndex = 0;
            rtxtbComments.Text = "";
            rtxtbComments.Enter += rtxtbComments_Enter;
            rtxtbComments.Leave += rtxtbComments_Leave;
            // 
            // btnSaveComments
            // 
            btnSaveComments.Dock = DockStyle.Fill;
            btnSaveComments.Location = new Point(380, 41);
            btnSaveComments.Margin = new Padding(3, 4, 3, 4);
            btnSaveComments.Name = "btnSaveComments";
            btnSaveComments.Size = new Size(139, 30);
            btnSaveComments.TabIndex = 1;
            btnSaveComments.Text = "SET Comments";
            btnSaveComments.UseVisualStyleBackColor = true;
            btnSaveComments.Click += btnSaveComments_Click;
            // 
            // btnGetComms
            // 
            btnGetComms.Dock = DockStyle.Fill;
            btnGetComms.Location = new Point(380, 4);
            btnGetComms.Margin = new Padding(3, 4, 3, 4);
            btnGetComms.Name = "btnGetComms";
            btnGetComms.Size = new Size(139, 29);
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
            tableLayoutPanel6.Location = new Point(804, 331);
            tableLayoutPanel6.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel6.Size = new Size(528, 433);
            tableLayoutPanel6.TabIndex = 8;
            // 
            // gbxIPNstockMovements
            // 
            gbxIPNstockMovements.AutoSize = true;
            tableLayoutPanel6.SetColumnSpan(gbxIPNstockMovements, 2);
            gbxIPNstockMovements.Controls.Add(dgwIPNmoves);
            gbxIPNstockMovements.Dock = DockStyle.Fill;
            gbxIPNstockMovements.Location = new Point(3, 177);
            gbxIPNstockMovements.Margin = new Padding(3, 4, 3, 4);
            gbxIPNstockMovements.Name = "gbxIPNstockMovements";
            gbxIPNstockMovements.Padding = new Padding(3, 4, 3, 4);
            gbxIPNstockMovements.Size = new Size(522, 252);
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
            dgwIPNmoves.Location = new Point(3, 24);
            dgwIPNmoves.Margin = new Padding(3, 4, 3, 4);
            dgwIPNmoves.Name = "dgwIPNmoves";
            dgwIPNmoves.ReadOnly = true;
            dgwIPNmoves.RowHeadersWidth = 51;
            dgwIPNmoves.Size = new Size(516, 224);
            dgwIPNmoves.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dgwINSTOCK);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 4);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(522, 165);
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
            dgwINSTOCK.Location = new Point(3, 24);
            dgwINSTOCK.Margin = new Padding(3, 4, 3, 4);
            dgwINSTOCK.Name = "dgwINSTOCK";
            dgwINSTOCK.ReadOnly = true;
            dgwINSTOCK.RowHeadersWidth = 51;
            dgwINSTOCK.Size = new Size(516, 137);
            dgwINSTOCK.TabIndex = 0;
            // 
            // FrmPriorityBom
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1335, 879);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
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
            progressBarContainer.ResumeLayout(false);
            progressBarFill.ResumeLayout(false);
            progressBarFill.PerformLayout();
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
        private Panel progressBarContainer;
        private Panel progressBarFill;
        private Label lblTpmText;
    }
}