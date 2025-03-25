namespace WH_Panel
{
    partial class FrmPriorityPanDbSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPriorityPanDbSearch));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            txtbWH = new TextBox();
            groupBox3 = new GroupBox();
            txtbDESC = new TextBox();
            groupBox2 = new GroupBox();
            txtbMFPN = new TextBox();
            dgwALLDATA = new DataGridView();
            groupBox1 = new GroupBox();
            txtbIPN = new TextBox();
            txtLog = new RichTextBox();
            groupBox5 = new GroupBox();
            groupBox6 = new GroupBox();
            dgwTRANSACTIONS = new DataGridView();
            btnGETMFPN = new Button();
            dgwINSTOCK = new DataGridView();
            tableLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwALLDATA).BeginInit();
            groupBox1.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwTRANSACTIONS).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgwINSTOCK).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28571F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857161F));
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 2, 1);
            tableLayoutPanel1.Controls.Add(groupBox2, 2, 0);
            tableLayoutPanel1.Controls.Add(dgwALLDATA, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(txtLog, 4, 0);
            tableLayoutPanel1.Controls.Add(groupBox5, 4, 2);
            tableLayoutPanel1.Controls.Add(groupBox6, 4, 3);
            tableLayoutPanel1.Controls.Add(btnGETMFPN, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 75F));
            tableLayoutPanel1.Size = new Size(1138, 647);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtbWH);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(156, 54);
            groupBox4.TabIndex = 6;
            groupBox4.TabStop = false;
            groupBox4.Text = "Filter Warehouse";
            // 
            // txtbWH
            // 
            txtbWH.Dock = DockStyle.Fill;
            txtbWH.Location = new Point(3, 19);
            txtbWH.Name = "txtbWH";
            txtbWH.Size = new Size(150, 23);
            txtbWH.TabIndex = 0;
            txtbWH.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(txtbDESC);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(327, 63);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(156, 54);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Filter Description";
            // 
            // txtbDESC
            // 
            txtbDESC.Dock = DockStyle.Fill;
            txtbDESC.Location = new Point(3, 19);
            txtbDESC.Name = "txtbDESC";
            txtbDESC.Size = new Size(150, 23);
            txtbDESC.TabIndex = 1;
            txtbDESC.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(txtbMFPN);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(327, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(156, 54);
            groupBox2.TabIndex = 3;
            groupBox2.TabStop = false;
            groupBox2.Text = "Filter MFPN";
            // 
            // txtbMFPN
            // 
            txtbMFPN.Dock = DockStyle.Fill;
            txtbMFPN.Location = new Point(3, 19);
            txtbMFPN.Name = "txtbMFPN";
            txtbMFPN.Size = new Size(150, 23);
            txtbMFPN.TabIndex = 1;
            txtbMFPN.TextAlign = HorizontalAlignment.Center;
            // 
            // dgwALLDATA
            // 
            dgwALLDATA.AllowUserToAddRows = false;
            dgwALLDATA.AllowUserToDeleteRows = false;
            dgwALLDATA.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dgwALLDATA, 4);
            dgwALLDATA.Dock = DockStyle.Fill;
            dgwALLDATA.Location = new Point(3, 123);
            dgwALLDATA.Name = "dgwALLDATA";
            dgwALLDATA.ReadOnly = true;
            tableLayoutPanel1.SetRowSpan(dgwALLDATA, 2);
            dgwALLDATA.Size = new Size(642, 521);
            dgwALLDATA.TabIndex = 0;
            dgwALLDATA.CellClick += dataGridView1_CellClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtbIPN);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(165, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(156, 54);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Filter IPN";
            // 
            // txtbIPN
            // 
            txtbIPN.Dock = DockStyle.Fill;
            txtbIPN.Location = new Point(3, 19);
            txtbIPN.Name = "txtbIPN";
            txtbIPN.Size = new Size(150, 23);
            txtbIPN.TabIndex = 1;
            txtbIPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtLog
            // 
            tableLayoutPanel1.SetColumnSpan(txtLog, 3);
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(651, 3);
            txtLog.Name = "txtLog";
            tableLayoutPanel1.SetRowSpan(txtLog, 2);
            txtLog.Size = new Size(484, 114);
            txtLog.TabIndex = 5;
            txtLog.Text = "";
            // 
            // groupBox5
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox5, 3);
            groupBox5.Controls.Add(dgwINSTOCK);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(651, 123);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(484, 125);
            groupBox5.TabIndex = 7;
            groupBox5.TabStop = false;
            groupBox5.Text = "IN STOCK";
            // 
            // groupBox6
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox6, 3);
            groupBox6.Controls.Add(dgwTRANSACTIONS);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(651, 254);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(484, 390);
            groupBox6.TabIndex = 8;
            groupBox6.TabStop = false;
            groupBox6.Text = "TRANSACTIONS";
            // 
            // dgwTRANSACTIONS
            // 
            dgwTRANSACTIONS.AllowUserToAddRows = false;
            dgwTRANSACTIONS.AllowUserToDeleteRows = false;
            dgwTRANSACTIONS.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwTRANSACTIONS.Dock = DockStyle.Fill;
            dgwTRANSACTIONS.Location = new Point(3, 19);
            dgwTRANSACTIONS.Name = "dgwTRANSACTIONS";
            dgwTRANSACTIONS.ReadOnly = true;
            dgwTRANSACTIONS.Size = new Size(478, 368);
            dgwTRANSACTIONS.TabIndex = 1;
            // 
            // btnGETMFPN
            // 
            btnGETMFPN.BackgroundImage = (Image)resources.GetObject("btnGETMFPN.BackgroundImage");
            btnGETMFPN.BackgroundImageLayout = ImageLayout.Stretch;
            btnGETMFPN.Dock = DockStyle.Fill;
            btnGETMFPN.Location = new Point(489, 3);
            btnGETMFPN.Name = "btnGETMFPN";
            tableLayoutPanel1.SetRowSpan(btnGETMFPN, 2);
            btnGETMFPN.Size = new Size(156, 114);
            btnGETMFPN.TabIndex = 9;
            btnGETMFPN.UseVisualStyleBackColor = true;
            btnGETMFPN.Click += btnGETMFPN_Click;
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
            dgwINSTOCK.Size = new Size(478, 103);
            dgwINSTOCK.TabIndex = 0;
            // 
            // FrmPriorityPanDbSearch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1138, 647);
            Controls.Add(tableLayoutPanel1);
            Name = "FrmPriorityPanDbSearch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPriorityPanDbSearch";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgwALLDATA).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwTRANSACTIONS).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgwINSTOCK).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dgwALLDATA;
        private DataGridView dgwTRANSACTIONS;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private RichTextBox txtLog;
        private GroupBox groupBox4;
        private TextBox txtbWH;
        private TextBox txtbDESC;
        private TextBox txtbMFPN;
        private TextBox txtbIPN;
        private GroupBox groupBox5;
        private GroupBox groupBox6;
        private Button btnGETMFPN;
        private DataGridView dgwINSTOCK;
    }
}