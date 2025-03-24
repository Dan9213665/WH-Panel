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
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            txtbWH = new TextBox();
            groupBox3 = new GroupBox();
            txtbDESC = new TextBox();
            groupBox2 = new GroupBox();
            txtbMFPN = new TextBox();
            dataGridView1 = new DataGridView();
            dataGridView2 = new DataGridView();
            groupBox1 = new GroupBox();
            txtbIPN = new TextBox();
            txtLog = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 7;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.2857141F));
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 3, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 2, 0);
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 1);
            tableLayoutPanel1.Controls.Add(dataGridView2, 4, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(txtLog, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 12.2102013F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 87.7898F));
            tableLayoutPanel1.Size = new Size(1138, 647);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(txtbWH);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(156, 73);
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
            groupBox3.Location = new Point(489, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(156, 73);
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
            groupBox2.Size = new Size(156, 73);
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
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dataGridView1, 4);
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 82);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.Size = new Size(642, 562);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dataGridView2, 3);
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(651, 82);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.Size = new Size(484, 562);
            dataGridView2.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtbIPN);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(165, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(156, 73);
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
            txtLog.Size = new Size(484, 73);
            txtLog.TabIndex = 5;
            txtLog.Text = "";
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
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private DataGridView dataGridView1;
        private DataGridView dataGridView2;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private RichTextBox txtLog;
        private GroupBox groupBox4;
        private TextBox txtbWH;
        private TextBox txtbDESC;
        private TextBox txtbMFPN;
        private TextBox txtbIPN;
    }
}