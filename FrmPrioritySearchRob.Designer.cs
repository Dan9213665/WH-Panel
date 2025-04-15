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
            gbxIPN = new GroupBox();
            txtbSearchIPN = new TextBox();
            groupBox2 = new GroupBox();
            txtbLog = new RichTextBox();
            groupBox3 = new GroupBox();
            dgwSerials = new DataGridView();
            gbxMFPN = new GroupBox();
            txtbSearchMFPN = new TextBox();
            btnClear = new Button();
            tableLayoutPanel1.SuspendLayout();
            gbxIPN.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgwSerials).BeginInit();
            gbxMFPN.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel1.Controls.Add(gbxIPN, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 3);
            tableLayoutPanel1.Controls.Add(gbxMFPN, 0, 1);
            tableLayoutPanel1.Controls.Add(btnClear, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 22.2222214F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 22.2222214F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 22.2222214F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(816, 546);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // gbxIPN
            // 
            gbxIPN.Controls.Add(txtbSearchIPN);
            gbxIPN.Dock = DockStyle.Fill;
            gbxIPN.ForeColor = Color.White;
            gbxIPN.Location = new Point(3, 3);
            gbxIPN.Name = "gbxIPN";
            gbxIPN.Size = new Size(320, 115);
            gbxIPN.TabIndex = 0;
            gbxIPN.TabStop = false;
            gbxIPN.Text = "Search IPN";
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
            tableLayoutPanel1.SetRowSpan(groupBox2, 3);
            groupBox2.Size = new Size(484, 357);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Log";
            // 
            // txtbLog
            // 
            txtbLog.Dock = DockStyle.Fill;
            txtbLog.Location = new Point(3, 19);
            txtbLog.Name = "txtbLog";
            txtbLog.Size = new Size(478, 335);
            txtbLog.TabIndex = 0;
            txtbLog.Text = "";
            // 
            // groupBox3
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox3, 3);
            groupBox3.Controls.Add(dgwSerials);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(3, 366);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(810, 177);
            groupBox3.TabIndex = 3;
            groupBox3.TabStop = false;
            groupBox3.Text = "ROB work orders found";
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
            dgwSerials.Size = new Size(804, 155);
            dgwSerials.TabIndex = 2;
            dgwSerials.CellClick += dgwSerials_CellClick;
            // 
            // gbxMFPN
            // 
            gbxMFPN.Controls.Add(txtbSearchMFPN);
            gbxMFPN.Dock = DockStyle.Fill;
            gbxMFPN.ForeColor = Color.White;
            gbxMFPN.Location = new Point(3, 124);
            gbxMFPN.Name = "gbxMFPN";
            gbxMFPN.Size = new Size(320, 115);
            gbxMFPN.TabIndex = 4;
            gbxMFPN.TabStop = false;
            gbxMFPN.Text = "Search MFPN";
            // 
            // txtbSearchIMFPN
            // 
            txtbSearchMFPN.Dock = DockStyle.Fill;
            txtbSearchMFPN.Font = new Font("Segoe UI", 18F);
            txtbSearchMFPN.Location = new Point(3, 19);
            txtbSearchMFPN.Name = "txtbSearchIMFPN";
            txtbSearchMFPN.PlaceholderText = "Input MFPN to search...";
            txtbSearchMFPN.Size = new Size(314, 39);
            txtbSearchMFPN.TabIndex = 1;
            txtbSearchMFPN.TextAlign = HorizontalAlignment.Center;
            txtbSearchMFPN.DoubleClick += txtbSearchIMFPN_DoubleClick;
            txtbSearchMFPN.Enter += txtbSearchIMFPN_Enter;
            txtbSearchMFPN.KeyDown += txtbSearchMFPN_KeyDown;
            txtbSearchMFPN.Leave += txtbSearchIMFPN_Leave;
            // 
            // btnClear
            // 
            btnClear.BackgroundImage = (Image)resources.GetObject("btnClear.BackgroundImage");
            btnClear.BackgroundImageLayout = ImageLayout.Stretch;
            btnClear.Dock = DockStyle.Fill;
            btnClear.Location = new Point(3, 245);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(320, 115);
            btnClear.TabIndex = 5;
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // FrmPrioritySearchRob
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaptionText;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(816, 546);
            Controls.Add(tableLayoutPanel1);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPrioritySearchRob";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmPrioritySearchRob";
            Load += FrmPrioritySearchRob_Load;
            tableLayoutPanel1.ResumeLayout(false);
            gbxIPN.ResumeLayout(false);
            gbxIPN.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgwSerials).EndInit();
            gbxMFPN.ResumeLayout(false);
            gbxMFPN.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox gbxIPN;
        private TextBox txtbSearchIPN;
        private GroupBox groupBox2;
        private RichTextBox txtbLog;
        private DataGridView dgwSerials;
        private GroupBox groupBox3;
        private GroupBox gbxMFPN;
        private TextBox txtbSearchMFPN;
        private Button btnClear;
    }
}