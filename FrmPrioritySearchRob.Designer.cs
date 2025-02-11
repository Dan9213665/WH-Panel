namespace WH_Panel
{
    partial class FrmPrioritySearchRob
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPrioritySearchRob));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            txtbSearchIPN = new TextBox();
            groupBox2 = new GroupBox();
            txtbLog = new RichTextBox();
            dgwSerials = new DataGridView();
            groupBox3 = new GroupBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwSerials).BeginInit();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(816, 303);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(txtbSearchIPN);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.ForeColor = Color.White;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(320, 84);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Search IPN";
            // 
            // txtbSearchIPN
            // 
            txtbSearchIPN.Dock = DockStyle.Fill;
            txtbSearchIPN.Font = new Font("Segoe UI", 18F);
            txtbSearchIPN.Location = new Point(3, 19);
            txtbSearchIPN.Name = "txtbSearchIPN";
            txtbSearchIPN.PlaceholderText = "Input IPN to search...";
            txtbSearchIPN.Size = new Size(314, 39);
            txtbSearchIPN.TabIndex = 0;
            txtbSearchIPN.TextAlign = HorizontalAlignment.Center;
            txtbSearchIPN.DoubleClick += txtbSearchIPN_DoubleClick;
            txtbSearchIPN.Enter += txtbSearchIPN_Enter;
            txtbSearchIPN.KeyDown += txtbSearchIPN_KeyDown;
            txtbSearchIPN.Leave += txtbSearchIPN_Leave;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(txtbLog);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(329, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(484, 84);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log";
            // 
            // txtbLog
            // 
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Location = new Point(3, 19);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(478, 62);
            txtbLog.TabIndex = 0;
            txtbLog.Text = "";
            // 
            // dgwSerials
            // 
            dgwSerials.AllowUserToAddRows = false;
            dgwSerials.AllowUserToDeleteRows = false;
            dgwSerials.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgwSerials.Dock = DockStyle.Fill;
            dgwSerials.Location = new Point(3, 19);
            dgwSerials.Name = "dgwSerials";
            dgwSerials.ReadOnly = true;
            dgwSerials.RowTemplate.Height = 25;
            dgwSerials.Size = new Size(804, 185);
            dgwSerials.TabIndex = 2;
            dgwSerials.CellClick += dgwSerials_CellClick;
            // 
            // groupBox3
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox3, 3);
            groupBox3.Controls.Add(dgwSerials);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(3, 93);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(810, 207);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "ROB work orders found";
            // 
            // FrmPrioritySearchRob
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(816, 303);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPrioritySearchRob";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPrioritySearchRob";
            Load += FrmPrioritySearchRob_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwSerials).EndInit();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TextBox txtbSearchIPN;
        private GroupBox groupBox2;
        private RichTextBox txtbLog;
        private DataGridView dgwSerials;
        private GroupBox groupBox3;
    }
}