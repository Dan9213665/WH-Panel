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
            chkbBomsList = new CheckedListBox();
            groupBox3 = new GroupBox();
            comboBox1 = new ComboBox();
            tableLayoutPanel4 = new TableLayoutPanel();
            lblSelected = new Label();
            btnSim1 = new Button();
            tableLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox1.SuspendLayout();
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
            tableLayoutPanel1.Size = new Size(800, 450);
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
            tableLayoutPanel2.Location = new Point(403, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 19.1780815F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 80.8219147F));
            tableLayoutPanel2.Size = new Size(394, 219);
            tableLayoutPanel2.TabIndex = 3;
            // 
            // groupBox2
            // 
            tableLayoutPanel2.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(txtbLog);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 45);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(388, 171);
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
            txtbLog.Size = new Size(382, 149);
            txtbLog.TabIndex = 1;
            txtbLog.Text = "";
            // 
            // lblLoading
            // 
            lblLoading.AutoSize = true;
            lblLoading.Dock = DockStyle.Fill;
            lblLoading.Location = new Point(3, 0);
            lblLoading.Name = "lblLoading";
            lblLoading.Size = new Size(191, 42);
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
            tableLayoutPanel3.Size = new Size(394, 444);
            tableLayoutPanel3.TabIndex = 4;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(chkbBomsList);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 91);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(388, 350);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Work Orders List";
            // 
            // chkbBomsList
            // 
            chkbBomsList.Dock = DockStyle.Fill;
            chkbBomsList.Font = new Font("Segoe UI", 12F);
            chkbBomsList.FormattingEnabled = true;
            chkbBomsList.Location = new Point(3, 19);
            chkbBomsList.Name = "chkbBomsList";
            chkbBomsList.Size = new Size(382, 328);
            chkbBomsList.TabIndex = 0;
            chkbBomsList.ItemCheck += chkbBomsList_ItemCheck;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBox1);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(388, 38);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Select Client Warehouse";
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 18F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 19);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(382, 40);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel4.Controls.Add(lblSelected, 0, 0);
            tableLayoutPanel4.Controls.Add(btnSim1, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(3, 47);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(388, 38);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Dock = DockStyle.Fill;
            lblSelected.Font = new Font("Segoe UI", 18F);
            lblSelected.Location = new Point(3, 0);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(123, 38);
            lblSelected.TabIndex = 0;
            lblSelected.Text = "Selected";
            lblSelected.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnSim1
            // 
            btnSim1.Dock = DockStyle.Fill;
            btnSim1.Location = new Point(132, 3);
            btnSim1.Name = "btnSim1";
            btnSim1.Size = new Size(123, 32);
            btnSim1.TabIndex = 1;
            btnSim1.Text = "Simulation";
            btnSim1.UseVisualStyleBackColor = true;
            btnSim1.Click += btnSim1_Click;
            // 
            // FrmPriorityMultiBom
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
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
            groupBox3.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            tableLayoutPanel4.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private CheckedListBox chkbBomsList;
        private RichTextBox txtbLog;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblLoading;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private ComboBox comboBox1;
        private TableLayoutPanel tableLayoutPanel4;
        private Label lblSelected;
        private Button btnSim1;
    }
}