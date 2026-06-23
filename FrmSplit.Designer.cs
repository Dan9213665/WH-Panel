namespace WH_Panel
{
    partial class FrmSplit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSplit));
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox2 = new GroupBox();
            dataGridView1 = new DataGridView();
            groupBox3 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox1 = new TextBox();
            comboBox1 = new ComboBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            textBox2 = new TextBox();
            comboBox2 = new ComboBox();
            btnSplit = new Button();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(1160, 448);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Split the Item package";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox4, 1, 1);
            tableLayoutPanel1.Controls.Add(btnSplit, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 24);
            tableLayoutPanel1.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1154, 420);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 4);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(1148, 135);
            groupBox2.TabIndex = 0;
            groupBox2.TabStop = false;
            groupBox2.Text = "Original Item";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 24);
            dataGridView1.Margin = new Padding(3, 4, 3, 4);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1142, 107);
            dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel2);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 147);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(571, 135);
            groupBox3.TabIndex = 1;
            groupBox3.TabStop = false;
            groupBox3.Text = "Split A";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(textBox1, 0, 0);
            tableLayoutPanel2.Controls.Add(comboBox1, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 24);
            tableLayoutPanel2.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(565, 107);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            textBox1.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold);
            textBox1.Location = new Point(3, 4);
            textBox1.Margin = new Padding(3, 4, 3, 4);
            textBox1.Name = "textBox1";
            textBox1.PlaceholderText = "Input qty";
            textBox1.Size = new Size(276, 69);
            textBox1.TabIndex = 0;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyPress += textBox1_KeyPress;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 27.75F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "5\"", "7\"", "10\"", "13\"", "15\"", "Bag", "Box", "Stick", "Stick_in_a_bag", "Tray" });
            comboBox1.Location = new Point(285, 4);
            comboBox1.Margin = new Padding(3, 4, 3, 4);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(277, 70);
            comboBox1.TabIndex = 1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel3);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(580, 147);
            groupBox4.Margin = new Padding(3, 4, 3, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 4, 3, 4);
            groupBox4.Size = new Size(571, 135);
            groupBox4.TabIndex = 2;
            groupBox4.TabStop = false;
            groupBox4.Text = "Split B";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 2;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(textBox2, 0, 0);
            tableLayoutPanel3.Controls.Add(comboBox2, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 24);
            tableLayoutPanel3.Margin = new Padding(3, 4, 3, 4);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Size = new Size(565, 107);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Font = new Font("Segoe UI", 27.75F, FontStyle.Bold);
            textBox2.Location = new Point(3, 4);
            textBox2.Margin = new Padding(3, 4, 3, 4);
            textBox2.Name = "textBox2";
            textBox2.PlaceholderText = "Auto Calc";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(276, 69);
            textBox2.TabIndex = 0;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // comboBox2
            // 
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox2.Font = new Font("Segoe UI", 27.75F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Items.AddRange(new object[] { "5\"", "7\"", "10\"", "13\"", "15\"", "Bag", "Box", "Stick", "Stick_in_a_bag", "Tray" });
            comboBox2.Location = new Point(285, 4);
            comboBox2.Margin = new Padding(3, 4, 3, 4);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(277, 70);
            comboBox2.TabIndex = 1;
            // 
            // btnSplit
            // 
            btnSplit.BackgroundImage = (Image)resources.GetObject("btnSplit.BackgroundImage");
            btnSplit.BackgroundImageLayout = ImageLayout.Zoom;
            tableLayoutPanel1.SetColumnSpan(btnSplit, 2);
            btnSplit.Dock = DockStyle.Fill;
            btnSplit.Location = new Point(3, 290);
            btnSplit.Margin = new Padding(3, 4, 3, 4);
            btnSplit.Name = "btnSplit";
            btnSplit.Size = new Size(1148, 135);
            btnSplit.TabIndex = 3;
            btnSplit.UseVisualStyleBackColor = true;
            btnSplit.Click += btnSplit_Click;
            // 
            // FrmSplit
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1160, 448);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 4, 3, 4);
            Name = "FrmSplit";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Split Warehouse Item";
            Load += FrmSplit_Load;
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox4.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private GroupBox groupBox4;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox textBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private TextBox textBox2;
        private Button btnSplit;
        private ComboBox comboBox1;
        private ComboBox comboBox2;
    }
}