namespace WH_Panel
{
    partial class FrmPriorityMultiBom
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPriorityMultiBom));
            tableLayoutPanel1 = new TableLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            txtbLog = new RichTextBox();
            lblLoading = new Label();
            btnGetBOMs = new Button();
            cmbBom = new ComboBox();
            cmbRev = new ComboBox();
            btnAddBom = new Button();
            tableLayoutPanel3 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            dgvBomsList = new DataGridView();
            groupBox3 = new GroupBox();
            cmbWarehouses = new ComboBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            lblSelected = new Label();
            btnSim1 = new Button();
            btnCheckAll = new Button();
            btnFull = new Button();
            btnReleased = new Button();
            btnPartialAssy = new Button();
            btnByIPN = new Button();
            btnByKit = new Button();
            btnAwaitingComp = new Button();
            btnNotSentYet = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBomsList).BeginInit();
            groupBox3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel3, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(974, 849);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333359F));
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 2);
            tableLayoutPanel2.Controls.Add(lblLoading, 0, 0);
            tableLayoutPanel2.Controls.Add(btnGetBOMs, 1, 0);
            tableLayoutPanel2.Controls.Add(cmbBom, 2, 0);
            tableLayoutPanel2.Controls.Add(cmbRev, 2, 1);
            tableLayoutPanel2.Controls.Add(btnAddBom, 1, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(490, 4);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 3;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 5F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel2.Size = new Size(481, 841);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            tableLayoutPanel2.SetColumnSpan(groupBox2, 3);
            groupBox2.Controls.Add(txtbLog);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 88);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(475, 749);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log";
            // 
            // txtbLog
            // 
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Font = new Font("Segoe UI", 12F);
            txtbLog.Location = new Point(3, 24);
            txtbLog.Margin = new Padding(3, 4, 3, 4);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(469, 721);
            txtbLog.TabIndex = 1;
            txtbLog.Text = "";
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.Location = new Point(3, 0);
            lblLoading.Name = "lblLoading";
            tableLayoutPanel2.SetRowSpan(lblLoading, 2);
            lblLoading.Size = new Size(154, 84);
            lblLoading.TabIndex = 3;
            lblLoading.Text = "Loading";
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnGetBOMs
            // 
            btnGetBOMs.Dock = DockStyle.Fill;
            btnGetBOMs.Location = new Point(163, 4);
            btnGetBOMs.Margin = new Padding(3, 4, 3, 4);
            btnGetBOMs.Name = "btnGetBOMs";
            btnGetBOMs.Size = new Size(154, 34);
            btnGetBOMs.TabIndex = 4;
            btnGetBOMs.Text = "GET BOMS";
            btnGetBOMs.UseVisualStyleBackColor = true;
            btnGetBOMs.Click += btnGetBOMs_Click;
            // 
            // cmbBom
            // 
            cmbBom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbBom.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbBom.Dock = DockStyle.Fill;
            cmbBom.FormattingEnabled = true;
            cmbBom.Location = new Point(323, 4);
            cmbBom.Margin = new Padding(3, 4, 3, 4);
            cmbBom.Name = "cmbBom";
            cmbBom.Size = new Size(155, 28);
            cmbBom.TabIndex = 5;
            cmbBom.SelectedIndexChanged += cmbBom_SelectedIndexChanged;
            // 
            // cmbRev
            // 
            cmbRev.Dock = DockStyle.Fill;
            cmbRev.FormattingEnabled = true;
            cmbRev.Location = new Point(323, 46);
            cmbRev.Margin = new Padding(3, 4, 3, 4);
            cmbRev.Name = "cmbRev";
            cmbRev.Size = new Size(155, 28);
            cmbRev.TabIndex = 6;
            // 
            // btnAddBom
            // 
            btnAddBom.Dock = DockStyle.Fill;
            btnAddBom.Location = new Point(163, 46);
            btnAddBom.Margin = new Padding(3, 4, 3, 4);
            btnAddBom.Name = "btnAddBom";
            btnAddBom.Size = new Size(154, 34);
            btnAddBom.TabIndex = 7;
            btnAddBom.Text = "Add Bom to Sim";
            btnAddBom.UseVisualStyleBackColor = true;
            btnAddBom.Click += btnAddBom_Click;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(groupBox1, 0, 2);
            tableLayoutPanel3.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 4);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel3, 2);
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.Size = new Size(481, 841);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvBomsList);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 172);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(475, 665);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Work Orders List";
            // 
            // dgvBomsList
            // 
            dgvBomsList.AllowUserToAddRows = false;
            dgvBomsList.AllowUserToDeleteRows = false;
            dgvBomsList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBomsList.Dock = DockStyle.Fill;
            dgvBomsList.Location = new Point(3, 24);
            dgvBomsList.Margin = new Padding(3, 4, 3, 4);
            dgvBomsList.MultiSelect = false;
            dgvBomsList.Name = "dgvBomsList";
            dgvBomsList.ReadOnly = true;
            dgvBomsList.RowHeadersWidth = 51;
            dgvBomsList.Size = new Size(469, 637);
            dgvBomsList.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cmbWarehouses);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 4);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(475, 76);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Select Client Warehouse";
            // 
            // cmbWarehouses
            // 
            cmbWarehouses.Dock = DockStyle.Fill;
            cmbWarehouses.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbWarehouses.Font = new Font("Segoe UI", 18F);
            cmbWarehouses.FormattingEnabled = true;
            cmbWarehouses.Location = new Point(3, 24);
            cmbWarehouses.Margin = new Padding(3, 4, 3, 4);
            cmbWarehouses.Name = "cmbWarehouses";
            cmbWarehouses.Size = new Size(469, 49);
            cmbWarehouses.TabIndex = 0;
            cmbWarehouses.SelectedIndexChanged += cmbWarehouses_SelectedIndexChanged;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 5;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel4.Controls.Add(lblSelected, 0, 0);
            tableLayoutPanel4.Controls.Add(btnSim1, 0, 1);
            tableLayoutPanel4.Controls.Add(btnCheckAll, 1, 0);
            tableLayoutPanel4.Controls.Add(btnFull, 2, 0);
            tableLayoutPanel4.Controls.Add(btnReleased, 4, 0);
            tableLayoutPanel4.Controls.Add(btnPartialAssy, 3, 0);
            tableLayoutPanel4.Controls.Add(btnByIPN, 1, 1);
            tableLayoutPanel4.Controls.Add(btnByKit, 2, 1);
            tableLayoutPanel4.Controls.Add(btnAwaitingComp, 3, 1);
            tableLayoutPanel4.Controls.Add(btnNotSentYet, 4, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 88);
            tableLayoutPanel4.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(475, 76);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Dock = DockStyle.Fill;
            lblSelected.Font = new Font("Segoe UI", 12F);
            lblSelected.Location = new Point(3, 0);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(89, 38);
            lblSelected.TabIndex = 0;
            lblSelected.Text = "Selected";
            lblSelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSim1
            // 
            btnSim1.Dock = DockStyle.Fill;
            btnSim1.Location = new Point(3, 42);
            btnSim1.Margin = new Padding(3, 4, 3, 4);
            btnSim1.Name = "btnSim1";
            btnSim1.Size = new Size(89, 30);
            btnSim1.TabIndex = 1;
            btnSim1.Text = "סימולציה מצרפית";
            btnSim1.UseVisualStyleBackColor = true;
            btnSim1.Click += btnSim1_Click;
            btnSim1.MouseDown += btnSim1_MouseDown;
            // 
            // btnCheckAll
            // 
            btnCheckAll.Dock = DockStyle.Fill;
            btnCheckAll.Location = new Point(98, 4);
            btnCheckAll.Margin = new Padding(3, 4, 3, 4);
            btnCheckAll.Name = "btnCheckAll";
            btnCheckAll.Size = new Size(89, 30);
            btnCheckAll.TabIndex = 2;
            btnCheckAll.Text = "*";
            btnCheckAll.UseVisualStyleBackColor = true;
            btnCheckAll.Click += btnCheckAll_Click;
            // 
            // btnFull
            // 
            btnFull.Dock = DockStyle.Fill;
            btnFull.Location = new Point(193, 4);
            btnFull.Margin = new Padding(3, 4, 3, 4);
            btnFull.Name = "btnFull";
            btnFull.Size = new Size(89, 30);
            btnFull.TabIndex = 3;
            btnFull.Text = "קיט מלא";
            btnFull.UseVisualStyleBackColor = true;
            btnFull.Click += btnFull_Click;
            // 
            // btnReleased
            // 
            btnReleased.Dock = DockStyle.Fill;
            btnReleased.Location = new Point(383, 4);
            btnReleased.Margin = new Padding(3, 4, 3, 4);
            btnReleased.Name = "btnReleased";
            btnReleased.Size = new Size(89, 30);
            btnReleased.TabIndex = 4;
            btnReleased.Text = "שוחררה";
            btnReleased.UseVisualStyleBackColor = true;
            btnReleased.Click += btnReleased_Click;
            // 
            // btnPartialAssy
            // 
            btnPartialAssy.Dock = DockStyle.Fill;
            btnPartialAssy.Location = new Point(288, 4);
            btnPartialAssy.Margin = new Padding(3, 4, 3, 4);
            btnPartialAssy.Name = "btnPartialAssy";
            btnPartialAssy.Size = new Size(89, 30);
            btnPartialAssy.TabIndex = 5;
            btnPartialAssy.Text = "הרכבה בחוסר";
            btnPartialAssy.UseVisualStyleBackColor = true;
            btnPartialAssy.Click += btnPartialAssy_Click;
            // 
            // btnByIPN
            // 
            btnByIPN.Dock = DockStyle.Fill;
            btnByIPN.Location = new Point(98, 42);
            btnByIPN.Margin = new Padding(3, 4, 3, 4);
            btnByIPN.Name = "btnByIPN";
            btnByIPN.Size = new Size(89, 30);
            btnByIPN.TabIndex = 6;
            btnByIPN.Text = "הצג ע\"פ מק\"ט";
            btnByIPN.UseVisualStyleBackColor = true;
            btnByIPN.Click += btnByIPN_Click;
            // 
            // btnByKit
            // 
            btnByKit.Dock = DockStyle.Fill;
            btnByKit.Location = new Point(193, 42);
            btnByKit.Margin = new Padding(3, 4, 3, 4);
            btnByKit.Name = "btnByKit";
            btnByKit.Size = new Size(89, 30);
            btnByKit.TabIndex = 7;
            btnByKit.Text = "הצג ע\"פ קיט";
            btnByKit.UseVisualStyleBackColor = true;
            btnByKit.Click += btnByKit_Click;
            // 
            // btnAwaitingComp
            // 
            btnAwaitingComp.Dock = DockStyle.Fill;
            btnAwaitingComp.Location = new Point(288, 42);
            btnAwaitingComp.Margin = new Padding(3, 4, 3, 4);
            btnAwaitingComp.Name = "btnAwaitingComp";
            btnAwaitingComp.Size = new Size(89, 30);
            btnAwaitingComp.TabIndex = 8;
            btnAwaitingComp.Text = "ממתין להשלמה";
            btnAwaitingComp.UseVisualStyleBackColor = true;
            btnAwaitingComp.Click += btnAwaitingComp_Click;
            // 
            // btnNotSentYet
            // 
            btnNotSentYet.Dock = DockStyle.Fill;
            btnNotSentYet.Location = new Point(383, 42);
            btnNotSentYet.Margin = new Padding(3, 4, 3, 4);
            btnNotSentYet.Name = "btnNotSentYet";
            btnNotSentYet.Size = new Size(89, 30);
            btnNotSentYet.TabIndex = 9;
            btnNotSentYet.Text = "טרם נשלח קיט";
            btnNotSentYet.UseVisualStyleBackColor = true;
            btnNotSentYet.Click += btnNotSentYet_Click;
            // 
            // FrmPriorityMultiBom
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(974, 849);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmPriorityMultiBom";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPriorityMultiBom";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvBomsList).EndInit();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private RichTextBox txtbLog;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblLoading;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private ComboBox cmbWarehouses;
        private TableLayoutPanel tableLayoutPanel4;
        private Label lblSelected;
        private Button btnSim1;
        private DataGridView dgvBomsList;
        private Button btnCheckAll;
        private Button btnFull;
        private Button btnReleased;
        private Button btnPartialAssy;
        private Button btnByIPN;
        private Button btnByKit;
        private Button btnAwaitingComp;
        private Button btnNotSentYet;
        private Button btnGetBOMs;
        private ComboBox cmbBom;
        private ComboBox cmbRev;
        private Button btnAddBom;
    }
}