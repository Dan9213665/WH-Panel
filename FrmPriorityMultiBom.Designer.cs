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
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(852, 637);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel2.Controls.Add(lblLoading, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(429, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel2, 2);
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 6.9730587F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 93.02694F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(420, 631);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            tableLayoutPanel2.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(txtbLog);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 47);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(414, 581);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log";
            // 
            // txtbLog
            // 
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Font = new Font("Segoe UI", 12F);
            txtbLog.Location = new Point(3, 19);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(408, 559);
            txtbLog.TabIndex = 1;
            txtbLog.Text = "";
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.Location = new Point(3, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(204, 44);
            lblLoading.TabIndex = 3;
            lblLoading.Text = "Loading";
            lblLoading.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(groupBox1, 0, 2);
            tableLayoutPanel3.Controls.Add(groupBox3, 0, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 0, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel1.SetRowSpan(tableLayoutPanel3, 2);
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.Size = new Size(420, 631);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dgvBomsList);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 129);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(414, 499);
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
            dgvBomsList.Location = new Point(3, 19);
            dgvBomsList.MultiSelect = false;
            dgvBomsList.Name = "dgvBomsList";
            dgvBomsList.ReadOnly = true;
            dgvBomsList.RowTemplate.Height = 25;
            dgvBomsList.Size = new Size(408, 477);
            dgvBomsList.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(cmbWarehouses);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(414, 57);
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
            cmbWarehouses.Location = new Point(3, 19);
            cmbWarehouses.Name = "cmbWarehouses";
            cmbWarehouses.Size = new Size(408, 40);
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
            tableLayoutPanel4.Location = new Point(3, 66);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(414, 57);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Dock = DockStyle.Fill;
            lblSelected.Font = new Font("Segoe UI", 12F);
            lblSelected.Location = new Point(3, 0);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(76, 28);
            lblSelected.TabIndex = 0;
            lblSelected.Text = "Selected";
            lblSelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSim1
            // 
            btnSim1.Dock = DockStyle.Fill;
            btnSim1.Location = new Point(3, 31);
            btnSim1.Name = "btnSim1";
            btnSim1.Size = new Size(76, 23);
            btnSim1.TabIndex = 1;
            btnSim1.Text = "Simulation";
            btnSim1.UseVisualStyleBackColor = true;
            btnSim1.Click += btnSim1_Click;
            // 
            // btnCheckAll
            // 
            btnCheckAll.Dock = DockStyle.Fill;
            btnCheckAll.Location = new Point(85, 3);
            btnCheckAll.Name = "btnCheckAll";
            btnCheckAll.Size = new Size(76, 22);
            btnCheckAll.TabIndex = 2;
            btnCheckAll.Text = "*";
            btnCheckAll.UseVisualStyleBackColor = true;
            btnCheckAll.Click += btnCheckAll_Click;
            // 
            // btnFull
            // 
            btnFull.Dock = DockStyle.Fill;
            btnFull.Location = new Point(167, 3);
            btnFull.Name = "btnFull";
            btnFull.Size = new Size(76, 22);
            btnFull.TabIndex = 3;
            btnFull.Text = "קיט מלא";
            btnFull.UseVisualStyleBackColor = true;
            btnFull.Click += btnFull_Click;
            // 
            // btnReleased
            // 
            btnReleased.Dock = DockStyle.Fill;
            btnReleased.Location = new Point(331, 3);
            btnReleased.Name = "btnReleased";
            btnReleased.Size = new Size(80, 22);
            btnReleased.TabIndex = 4;
            btnReleased.Text = "שוחררה";
            btnReleased.UseVisualStyleBackColor = true;
            btnReleased.Click += btnReleased_Click;
            // 
            // btnPartialAssy
            // 
            btnPartialAssy.Dock = DockStyle.Fill;
            btnPartialAssy.Location = new Point(249, 3);
            btnPartialAssy.Name = "btnPartialAssy";
            btnPartialAssy.Size = new Size(76, 22);
            btnPartialAssy.TabIndex = 5;
            btnPartialAssy.Text = "הרכבה בחוסר";
            btnPartialAssy.UseVisualStyleBackColor = true;
            btnPartialAssy.Click += btnPartialAssy_Click;
            // 
            // btnByIPN
            // 
            btnByIPN.Dock = DockStyle.Fill;
            btnByIPN.Location = new Point(85, 31);
            btnByIPN.Name = "btnByIPN";
            btnByIPN.Size = new Size(76, 23);
            btnByIPN.TabIndex = 6;
            btnByIPN.Text = "by IPN";
            btnByIPN.UseVisualStyleBackColor = true;
            btnByIPN.Click += btnByIPN_Click;
            // 
            // btnByKit
            // 
            btnByKit.Dock = DockStyle.Fill;
            btnByKit.Location = new Point(167, 31);
            btnByKit.Name = "btnByKit";
            btnByKit.Size = new Size(76, 23);
            btnByKit.TabIndex = 7;
            btnByKit.Text = "by KIT";
            btnByKit.UseVisualStyleBackColor = true;
            btnByKit.Click += btnByKit_Click;
            // 
            // btnAwaitingComp
            // 
            btnAwaitingComp.Dock = DockStyle.Fill;
            btnAwaitingComp.Location = new Point(249, 31);
            btnAwaitingComp.Name = "btnAwaitingComp";
            btnAwaitingComp.Size = new Size(76, 23);
            btnAwaitingComp.TabIndex = 8;
            btnAwaitingComp.Text = "ממתין להשלמה";
            btnAwaitingComp.UseVisualStyleBackColor = true;
            btnAwaitingComp.Click += btnAwaitingComp_Click;
            // 
            // btnNotSentYet
            // 
            btnNotSentYet.Dock = DockStyle.Fill;
            btnNotSentYet.Location = new Point(331, 31);
            btnNotSentYet.Name = "btnNotSentYet";
            btnNotSentYet.Size = new Size(80, 23);
            btnNotSentYet.TabIndex = 9;
            btnNotSentYet.Text = "טרם נשלח קיט";
            btnNotSentYet.UseVisualStyleBackColor = true;
            btnNotSentYet.Click += btnNotSentYet_Click;
            // 
            // FrmPriorityMultiBom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(852, 637);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
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
    }
}