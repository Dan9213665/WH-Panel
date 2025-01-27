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
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblLoading = new Label();
            cmbROBxList = new ComboBox();
            btnKitLabel = new Button();
            btnReport = new Button();
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
            dgwBom = new DataGridView();
            gbxIPNstockMovements = new GroupBox();
            dgwIPNmoves = new DataGridView();
            lblPing = new Label();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            gbxLoadedWo.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwBom).BeginInit();
            gbxIPNstockMovements.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwIPNmoves).BeginInit();
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
            tableLayoutPanel1.Controls.Add(gbxIPNstockMovements, 1, 2);
            tableLayoutPanel1.Controls.Add(lblPing, 1, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1158, 613);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(688, 64);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Select ROB******  WO";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 4;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 51.02041F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.32653F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.32653F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.32653F));
            tableLayoutPanel2.Controls.Add(lblLoading, 1, 0);
            tableLayoutPanel2.Controls.Add(cmbROBxList, 0, 0);
            tableLayoutPanel2.Controls.Add(btnKitLabel, 2, 0);
            tableLayoutPanel2.Controls.Add(btnReport, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(682, 42);
            tableLayoutPanel2.TabIndex = 2;
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.BackColor = Color.IndianRed;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.ForeColor = Color.White;
            lblLoading.Location = new Point(350, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(105, 42);
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
            cmbROBxList.Size = new Size(341, 29);
            cmbROBxList.TabIndex = 0;
            cmbROBxList.SelectedIndexChanged += cmbROBxList_SelectedIndexChanged;
            // 
            // btnKitLabel
            // 
            btnKitLabel.BackgroundImage = Properties.Resources.kitLabelPrint;
            btnKitLabel.BackgroundImageLayout = ImageLayout.Stretch;
            btnKitLabel.Dock = DockStyle.Fill;
            btnKitLabel.Location = new Point(461, 3);
            btnKitLabel.Name = "btnKitLabel";
            btnKitLabel.Size = new Size(105, 36);
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
            btnReport.Location = new Point(572, 3);
            btnReport.Name = "btnReport";
            btnReport.Size = new Size(107, 36);
            btnReport.TabIndex = 3;
            btnReport.UseVisualStyleBackColor = true;
            btnReport.Click += btnReport_Click;
            // 
            // gbxLoadedWo
            // 
            gbxLoadedWo.AutoSize = true;
            gbxLoadedWo.Controls.Add(tableLayoutPanel3);
            gbxLoadedWo.Dock = DockStyle.Fill;
            gbxLoadedWo.Location = new Point(3, 73);
            gbxLoadedWo.Name = "gbxLoadedWo";
            gbxLoadedWo.Size = new Size(688, 104);
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
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel3.Size = new Size(682, 82);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // txtbRob
            // 
            txtbRob.Dock = DockStyle.Fill;
            txtbRob.Location = new Point(3, 3);
            txtbRob.Name = "txtbRob";
            txtbRob.ReadOnly = true;
            txtbRob.Size = new Size(130, 23);
            txtbRob.TabIndex = 0;
            txtbRob.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbName
            // 
            txtbName.Dock = DockStyle.Fill;
            txtbName.Location = new Point(139, 3);
            txtbName.Name = "txtbName";
            txtbName.ReadOnly = true;
            txtbName.Size = new Size(130, 23);
            txtbName.TabIndex = 1;
            txtbName.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbQty
            // 
            txtbQty.Dock = DockStyle.Fill;
            txtbQty.Location = new Point(411, 3);
            txtbQty.Name = "txtbQty";
            txtbQty.ReadOnly = true;
            txtbQty.Size = new Size(130, 23);
            txtbQty.TabIndex = 2;
            txtbQty.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbStatus
            // 
            txtbStatus.Dock = DockStyle.Fill;
            txtbStatus.Location = new Point(547, 3);
            txtbStatus.Name = "txtbStatus";
            txtbStatus.ReadOnly = true;
            txtbStatus.Size = new Size(132, 23);
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
            txtbInputIPN.Size = new Size(130, 23);
            txtbInputIPN.TabIndex = 4;
            txtbInputIPN.TextAlign = HorizontalAlignment.Center;
            txtbInputIPN.KeyDown += textBox1_KeyDown;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(139, 30);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Filter by MFPN";
            textBox2.Size = new Size(130, 23);
            textBox2.TabIndex = 4;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(275, 30);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Filter by Description";
            textBox3.Size = new Size(130, 23);
            textBox3.TabIndex = 4;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(411, 30);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Filter by ALT";
            textBox4.Size = new Size(130, 23);
            textBox4.TabIndex = 4;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbRev
            // 
            txtbRev.Dock = DockStyle.Fill;
            txtbRev.Location = new Point(275, 3);
            txtbRev.Name = "txtbRev";
            txtbRev.ReadOnly = true;
            txtbRev.Size = new Size(130, 23);
            txtbRev.TabIndex = 5;
            txtbRev.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbINPUTqty
            // 
            txtbINPUTqty.Dock = DockStyle.Fill;
            txtbINPUTqty.Location = new Point(3, 57);
            txtbINPUTqty.Name = "txtbINPUTqty";
            txtbINPUTqty.PlaceholderText = "Input Qty";
            txtbINPUTqty.Size = new Size(130, 23);
            txtbINPUTqty.TabIndex = 6;
            txtbINPUTqty.TextAlign = HorizontalAlignment.Center;
            txtbINPUTqty.KeyDown += txtbINPUTqty_KeyDown;
            txtbINPUTqty.KeyUp += txtbINPUTqty_KeyUp;
            // 
            // btnGetMFNs
            // 
            btnGetMFNs.Dock = DockStyle.Fill;
            btnGetMFNs.Font = new Font("Segoe UI", 7F);
            btnGetMFNs.Location = new Point(139, 57);
            btnGetMFNs.Name = "btnGetMFNs";
            btnGetMFNs.Size = new Size(130, 22);
            btnGetMFNs.TabIndex = 7;
            btnGetMFNs.Text = "GET MFPNs";
            btnGetMFNs.UseVisualStyleBackColor = true;
            btnGetMFNs.Click += btnGetMFNs_Click;
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
            dgwBom.RowTemplate.Height = 25;
            dgwBom.Size = new Size(688, 519);
            dgwBom.TabIndex = 3;
            dgwBom.CellClick += dgwBom_CellClick;
            // 
            // gbxIPNstockMovements
            // 
            tableLayoutPanel1.SetColumnSpan(gbxIPNstockMovements, 2);
            gbxIPNstockMovements.Controls.Add(dgwIPNmoves);
            gbxIPNstockMovements.Dock = DockStyle.Fill;
            gbxIPNstockMovements.Location = new Point(697, 183);
            gbxIPNstockMovements.Name = "gbxIPNstockMovements";
            gbxIPNstockMovements.Size = new Size(458, 519);
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
            dgwIPNmoves.RowTemplate.Height = 25;
            dgwIPNmoves.Size = new Size(452, 497);
            dgwIPNmoves.TabIndex = 0;
            // 
            // lblPing
            // 
            lblPing.AutoSize = true;
            lblPing.Location = new Point(697, 0);
            lblPing.Name = "lblPing";
            lblPing.Size = new Size(35, 15);
            lblPing.TabIndex = 5;
            lblPing.Text = "tProc";
            lblPing.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmPriorityBom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1158, 613);
            Controls.Add(tableLayoutPanel1);
            Name = "FrmPriorityBom";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPriorityBom";
            WindowState = FormWindowState.Maximized;
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
            gbxIPNstockMovements.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwIPNmoves).EndInit();
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
    }
}