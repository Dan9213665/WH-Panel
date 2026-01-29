namespace WH_Panel
{
    partial class FrmPriorityCount
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPriorityCount));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox11 = new GroupBox();
            groupBox10 = new GroupBox();
            groupBox9 = new GroupBox();
            cmbPackage = new ComboBox();
            groupBox8 = new GroupBox();
            txtbQTY = new TextBox();
            groupBox7 = new GroupBox();
            txtbMFPN = new TextBox();
            groupBox1 = new GroupBox();
            cmbSelectedWH = new ComboBox();
            groupBox2 = new GroupBox();
            btnStockReport = new Button();
            groupBox3 = new GroupBox();
            btnDeltaReport = new Button();
            gbCreateCountDB = new GroupBox();
            btnCreateCountDB = new Button();
            groupBox5 = new GroupBox();
            txtSearchIPN = new TextBox();
            groupBox6 = new GroupBox();
            dgwAVL = new DataGridView();
            groupBox12 = new GroupBox();
            lblBalance = new Label();
            gbAllWhs = new GroupBox();
            cmbAllWhs = new ComboBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox8.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            gbCreateCountDB.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwAVL).BeginInit();
            groupBox12.SuspendLayout();
            gbAllWhs.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(groupBox11, 0, 4);
            tableLayoutPanel1.Controls.Add(groupBox10, 0, 3);
            tableLayoutPanel1.Controls.Add(groupBox9, 3, 1);
            tableLayoutPanel1.Controls.Add(groupBox8, 2, 1);
            tableLayoutPanel1.Controls.Add(groupBox7, 1, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 2, 0);
            tableLayoutPanel1.Controls.Add(gbCreateCountDB, 4, 0);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox6, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox12, 3, 0);
            tableLayoutPanel1.Controls.Add(gbAllWhs, 4, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 9.345795F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.6809082F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 18.36493F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 23.696682F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 38.27014F));
            tableLayoutPanel1.Size = new Size(2038, 844);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox11
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox11, 5);
            groupBox11.Dock = DockStyle.Fill;
            groupBox11.Location = new Point(3, 523);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(2032, 318);
            groupBox11.TabIndex = 11;
            groupBox11.TabStop = false;
            groupBox11.Text = "FULL LOG";
            // 
            // groupBox10
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox10, 5);
            groupBox10.Dock = DockStyle.Fill;
            groupBox10.Location = new Point(3, 324);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(2032, 193);
            groupBox10.TabIndex = 10;
            groupBox10.TabStop = false;
            groupBox10.Text = "IN STOCK";
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(cmbPackage);
            groupBox9.Dock = DockStyle.Fill;
            groupBox9.Location = new Point(1224, 81);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(401, 83);
            groupBox9.TabIndex = 8;
            groupBox9.TabStop = false;
            groupBox9.Text = "Package";
            // 
            // cmbPackage
            // 
            cmbPackage.Dock = DockStyle.Fill;
            cmbPackage.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbPackage.FormattingEnabled = true;
            cmbPackage.Location = new Point(3, 23);
            cmbPackage.Name = "cmbPackage";
            cmbPackage.Size = new Size(395, 49);
            cmbPackage.TabIndex = 0;
            // 
            // groupBox8
            // 
            groupBox8.Controls.Add(txtbQTY);
            groupBox8.Dock = DockStyle.Fill;
            groupBox8.Location = new Point(817, 81);
            groupBox8.Name = "groupBox8";
            groupBox8.Size = new Size(401, 83);
            groupBox8.TabIndex = 7;
            groupBox8.TabStop = false;
            groupBox8.Text = "Qty";
            // 
            // txtbQTY
            // 
            txtbQTY.Dock = DockStyle.Fill;
            txtbQTY.Font = new Font("Segoe UI", 18F);
            txtbQTY.Location = new Point(3, 23);
            txtbQTY.Name = "txtbQTY";
            txtbQTY.Size = new Size(395, 47);
            txtbQTY.TabIndex = 1;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(txtbMFPN);
            groupBox7.Dock = DockStyle.Fill;
            groupBox7.Location = new Point(410, 81);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(401, 83);
            groupBox7.TabIndex = 6;
            groupBox7.TabStop = false;
            groupBox7.Text = "MFPN";
            // 
            // txtbMFPN
            // 
            txtbMFPN.Dock = DockStyle.Fill;
            txtbMFPN.Font = new Font("Segoe UI", 18F);
            txtbMFPN.Location = new Point(3, 23);
            txtbMFPN.Name = "txtbMFPN";
            txtbMFPN.Size = new Size(395, 47);
            txtbMFPN.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(cmbSelectedWH);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(401, 72);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Selected Warehouse";
            // 
            // cmbSelectedWH
            // 
            cmbSelectedWH.Dock = DockStyle.Fill;
            cmbSelectedWH.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSelectedWH.FormattingEnabled = true;
            cmbSelectedWH.Location = new Point(3, 23);
            cmbSelectedWH.Name = "cmbSelectedWH";
            cmbSelectedWH.Size = new Size(395, 28);
            cmbSelectedWH.TabIndex = 0;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(btnStockReport);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(410, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(401, 72);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Stock Report";
            // 
            // btnStockReport
            // 
            btnStockReport.Dock = DockStyle.Fill;
            btnStockReport.Location = new Point(3, 23);
            btnStockReport.Name = "btnStockReport";
            btnStockReport.Size = new Size(395, 46);
            btnStockReport.TabIndex = 0;
            btnStockReport.Text = "Generate Stock Report";
            btnStockReport.UseVisualStyleBackColor = true;
            btnStockReport.Click += btnStockReport_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(btnDeltaReport);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(817, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(401, 72);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Delta Report";
            // 
            // btnDeltaReport
            // 
            btnDeltaReport.Dock = DockStyle.Fill;
            btnDeltaReport.Location = new Point(3, 23);
            btnDeltaReport.Name = "btnDeltaReport";
            btnDeltaReport.Size = new Size(395, 46);
            btnDeltaReport.TabIndex = 0;
            btnDeltaReport.Text = "Generate Delta Report";
            btnDeltaReport.UseVisualStyleBackColor = true;
            btnDeltaReport.Click += btnDeltaReport_Click;
            // 
            // gbCreateCountDB
            // 
            gbCreateCountDB.Controls.Add(btnCreateCountDB);
            gbCreateCountDB.Dock = DockStyle.Fill;
            gbCreateCountDB.Location = new Point(1631, 3);
            gbCreateCountDB.Name = "gbCreateCountDB";
            gbCreateCountDB.Size = new Size(404, 72);
            gbCreateCountDB.TabIndex = 3;
            gbCreateCountDB.TabStop = false;
            gbCreateCountDB.Text = "Create Count DB";
            gbCreateCountDB.Visible = false;
            // 
            // btnCreateCountDB
            // 
            btnCreateCountDB.Dock = DockStyle.Fill;
            btnCreateCountDB.Location = new Point(3, 23);
            btnCreateCountDB.Name = "btnCreateCountDB";
            btnCreateCountDB.Size = new Size(398, 46);
            btnCreateCountDB.TabIndex = 0;
            btnCreateCountDB.Text = "Create Count DB for selected WH";
            btnCreateCountDB.UseVisualStyleBackColor = true;
            btnCreateCountDB.Click += btnCreateCountDB_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(txtSearchIPN);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(3, 81);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(401, 83);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "IPN";
            // 
            // txtSearchIPN
            // 
            txtSearchIPN.Dock = DockStyle.Fill;
            txtSearchIPN.Font = new Font("Segoe UI", 18F);
            txtSearchIPN.Location = new Point(3, 23);
            txtSearchIPN.Name = "txtSearchIPN";
            txtSearchIPN.Size = new Size(395, 47);
            txtSearchIPN.TabIndex = 0;
            // 
            // groupBox6
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox6, 5);
            groupBox6.Controls.Add(dgwAVL);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(3, 170);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(2032, 148);
            groupBox6.TabIndex = 9;
            groupBox6.TabStop = false;
            groupBox6.Text = "AVL";
            // 
            // dgwAVL
            // 
            dgwAVL.AllowUserToAddRows = false;
            dgwAVL.AllowUserToDeleteRows = false;
            dgwAVL.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwAVL.Dock = DockStyle.Fill;
            dgwAVL.Location = new Point(3, 23);
            dgwAVL.Name = "dgwAVL";
            dgwAVL.ReadOnly = true;
            dgwAVL.RowHeadersWidth = 51;
            dgwAVL.Size = new Size(2026, 122);
            dgwAVL.TabIndex = 0;
            // 
            // groupBox12
            // 
            groupBox12.Controls.Add(lblBalance);
            groupBox12.Dock = DockStyle.Fill;
            groupBox12.Location = new Point(1224, 3);
            groupBox12.Name = "groupBox12";
            groupBox12.Size = new Size(401, 72);
            groupBox12.TabIndex = 12;
            groupBox12.TabStop = false;
            groupBox12.Text = "BALANCE";
            // 
            // lblBalance
            // 
            lblBalance.AutoSize = true;
            lblBalance.Dock = DockStyle.Fill;
            lblBalance.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblBalance.Location = new Point(3, 23);
            lblBalance.Name = "lblBalance";
            lblBalance.Size = new Size(200, 41);
            lblBalance.TabIndex = 0;
            lblBalance.Text = "counted/total";
            lblBalance.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // gbAllWhs
            // 
            gbAllWhs.Controls.Add(cmbAllWhs);
            gbAllWhs.Dock = DockStyle.Fill;
            gbAllWhs.Location = new Point(1631, 81);
            gbAllWhs.Name = "gbAllWhs";
            gbAllWhs.Size = new Size(404, 83);
            gbAllWhs.TabIndex = 13;
            gbAllWhs.TabStop = false;
            gbAllWhs.Text = "All WHs";
            gbAllWhs.Visible = false;
            // 
            // cmbAllWhs
            // 
            cmbAllWhs.Dock = DockStyle.Fill;
            cmbAllWhs.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbAllWhs.FormattingEnabled = true;
            cmbAllWhs.Location = new Point(3, 23);
            cmbAllWhs.Name = "cmbAllWhs";
            cmbAllWhs.Size = new Size(398, 28);
            cmbAllWhs.TabIndex = 0;
            cmbAllWhs.SelectedIndexChanged += cmbAllWhs_SelectedIndexChanged;
            // 
            // FrmPriorityCount
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2038, 844);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPriorityCount";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Priority Count";
            WindowState = FormWindowState.Maximized;
            Load += FrmPriorityCount_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            groupBox8.ResumeLayout(false);
            groupBox8.PerformLayout();
            groupBox7.ResumeLayout(false);
            groupBox7.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            gbCreateCountDB.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwAVL).EndInit();
            groupBox12.ResumeLayout(false);
            groupBox12.PerformLayout();
            gbAllWhs.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private ComboBox cmbSelectedWH;
        private Button btnStockReport;
        private Button btnDeltaReport;
        private GroupBox gbCreateCountDB;
        private Button btnCreateCountDB;
        private GroupBox groupBox9;
        private GroupBox groupBox8;
        private GroupBox groupBox7;
        private GroupBox groupBox5;
        private TextBox txtbQTY;
        private TextBox txtbMFPN;
        private TextBox txtSearchIPN;
        private ComboBox cmbPackage;
        private GroupBox groupBox6;
        private GroupBox groupBox11;
        private GroupBox groupBox10;
        private GroupBox groupBox12;
        private Label lblBalance;
        private GroupBox gbAllWhs;
        private ComboBox cmbAllWhs;
        private DataGridView dgwAVL;
    }
}