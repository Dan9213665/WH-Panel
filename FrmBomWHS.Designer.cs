namespace WH_Panel
{
    partial class FrmBomWHS
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBomWHS));
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            groupBox1 = new GroupBox();
            dataGridView1 = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox4 = new GroupBox();
            tableLayoutPanel6 = new TableLayoutPanel();
            button3 = new Button();
            button4 = new Button();
            chkBlockInWHonly = new CheckBox();
            textBox10 = new TextBox();
            label15 = new Label();
            btnFound = new Button();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            comboBox1 = new ComboBox();
            button1 = new Button();
            btnFontIncrease = new Button();
            btnFontDecrease = new Button();
            button2 = new Button();
            button5 = new Button();
            groupBox2 = new GroupBox();
            dataGridView2 = new DataGridView();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 75);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1106, 426);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Missing Items";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Raised;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Control;
            dataGridViewCellStyle2.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Window;
            dataGridViewCellStyle3.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle3.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 30;
            dataGridView1.Size = new Size(1100, 404);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Size = new Size(1112, 721);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox4
            // 
            groupBox4.AutoSize = true;
            groupBox4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox4.Controls.Add(tableLayoutPanel6);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 507);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1106, 66);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Stock VIEW filtered by selected item";
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.AutoSize = true;
            tableLayoutPanel6.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel6.ColumnCount = 6;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel6.Controls.Add(button3, 0, 0);
            tableLayoutPanel6.Controls.Add(button4, 4, 0);
            tableLayoutPanel6.Controls.Add(chkBlockInWHonly, 5, 0);
            tableLayoutPanel6.Controls.Add(textBox10, 2, 0);
            tableLayoutPanel6.Controls.Add(label15, 1, 0);
            tableLayoutPanel6.Controls.Add(btnFound, 3, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(3, 19);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle());
            tableLayoutPanel6.Size = new Size(1100, 44);
            tableLayoutPanel6.TabIndex = 1;
            // 
            // button3
            // 
            button3.AutoSize = true;
            button3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button3.BackgroundImage = (Image)resources.GetObject("button3.BackgroundImage");
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(3, 3);
            button3.Name = "button3";
            button3.Size = new Size(177, 38);
            button3.TabIndex = 1;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button4
            // 
            button4.AutoSize = true;
            button4.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            button4.Dock = DockStyle.Fill;
            button4.Location = new Point(735, 3);
            button4.Name = "button4";
            button4.Size = new Size(177, 38);
            button4.TabIndex = 4;
            button4.Text = "FIlter Current WH stock ONLY";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // chkBlockInWHonly
            // 
            chkBlockInWHonly.AutoSize = true;
            chkBlockInWHonly.Checked = true;
            chkBlockInWHonly.CheckState = CheckState.Checked;
            chkBlockInWHonly.Dock = DockStyle.Fill;
            chkBlockInWHonly.Location = new Point(918, 3);
            chkBlockInWHonly.Name = "chkBlockInWHonly";
            chkBlockInWHonly.Size = new Size(179, 38);
            chkBlockInWHonly.TabIndex = 2;
            chkBlockInWHonly.Text = "LOCK IN WH ONLY";
            chkBlockInWHonly.TextAlign = ContentAlignment.MiddleCenter;
            chkBlockInWHonly.UseVisualStyleBackColor = true;
            chkBlockInWHonly.CheckedChanged += chkBlockInWHonly_CheckedChanged;
            // 
            // textBox10
            // 
            textBox10.Dock = DockStyle.Fill;
            textBox10.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            textBox10.Location = new Point(369, 3);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(177, 33);
            textBox10.TabIndex = 3;
            textBox10.TextAlign = HorizontalAlignment.Center;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Dock = DockStyle.Fill;
            label15.Font = new Font("Segoe UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label15.Location = new Point(186, 0);
            label15.Name = "label15";
            label15.Size = new Size(177, 44);
            label15.TabIndex = 5;
            label15.Text = "BALANCE : ";
            label15.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnFound
            // 
            btnFound.BackColor = Color.LightGreen;
            btnFound.Dock = DockStyle.Fill;
            btnFound.Font = new Font("Microsoft Tai Le", 15.75F, FontStyle.Bold, GraphicsUnit.Point);
            btnFound.Location = new Point(552, 3);
            btnFound.Name = "btnFound";
            btnFound.Size = new Size(177, 38);
            btnFound.TabIndex = 6;
            btnFound.Text = "Found";
            btnFound.UseVisualStyleBackColor = false;
            btnFound.Click += btnFound_Click;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 7;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(comboBox1, 1, 0);
            tableLayoutPanel2.Controls.Add(button1, 5, 0);
            tableLayoutPanel2.Controls.Add(btnFontIncrease, 3, 0);
            tableLayoutPanel2.Controls.Add(btnFontDecrease, 4, 0);
            tableLayoutPanel2.Controls.Add(button2, 6, 0);
            tableLayoutPanel2.Controls.Add(button5, 2, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 3);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(1106, 66);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(215, 66);
            label1.TabIndex = 0;
            label1.Text = "Select WAREHOUSE";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "ARAN", "DIGITRONIX", "ENERCON", "EPS", "HEPTAGON", "LEADER-TECH", "NETLINE", "ROBOTRON", "SOS", "SONOTRON", "SOLANIUM", "VALENS", "VAYYAR" });
            comboBox1.Location = new Point(224, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(325, 40);
            comboBox1.TabIndex = 1;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(885, 3);
            button1.Name = "button1";
            button1.Size = new Size(104, 60);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // btnFontIncrease
            // 
            btnFontIncrease.BackgroundImage = (Image)resources.GetObject("btnFontIncrease.BackgroundImage");
            btnFontIncrease.BackgroundImageLayout = ImageLayout.Stretch;
            btnFontIncrease.Dock = DockStyle.Fill;
            btnFontIncrease.Location = new Point(665, 3);
            btnFontIncrease.Name = "btnFontIncrease";
            btnFontIncrease.Size = new Size(104, 60);
            btnFontIncrease.TabIndex = 3;
            btnFontIncrease.UseVisualStyleBackColor = true;
            btnFontIncrease.Click += btnFontIncrease_Click;
            // 
            // btnFontDecrease
            // 
            btnFontDecrease.BackgroundImage = (Image)resources.GetObject("btnFontDecrease.BackgroundImage");
            btnFontDecrease.BackgroundImageLayout = ImageLayout.Stretch;
            btnFontDecrease.Dock = DockStyle.Fill;
            btnFontDecrease.Location = new Point(775, 3);
            btnFontDecrease.Name = "btnFontDecrease";
            btnFontDecrease.Size = new Size(104, 60);
            btnFontDecrease.TabIndex = 4;
            btnFontDecrease.UseVisualStyleBackColor = true;
            btnFontDecrease.Click += btnFontDecrease_Click;
            // 
            // button2
            // 
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(995, 3);
            button2.Name = "button2";
            button2.Size = new Size(108, 60);
            button2.TabIndex = 5;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // button5
            // 
            button5.BackgroundImage = (Image)resources.GetObject("button5.BackgroundImage");
            button5.BackgroundImageLayout = ImageLayout.Zoom;
            button5.Dock = DockStyle.Fill;
            button5.Location = new Point(555, 3);
            button5.Name = "button5";
            button5.Size = new Size(104, 60);
            button5.TabIndex = 6;
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 579);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1106, 139);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "WH STOCK status for selected item";
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridViewCellStyle4.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridView2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Control;
            dataGridViewCellStyle5.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle5.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dataGridView2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Window;
            dataGridViewCellStyle6.Font = new Font("Microsoft Tai Le", 9F, FontStyle.Regular, GraphicsUnit.Point);
            dataGridViewCellStyle6.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dataGridView2.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 19);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(1100, 117);
            dataGridView2.TabIndex = 0;
            // 
            // FrmBomWHS
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1112, 721);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmBomWHS";
            StartPosition = FormStartPosition.CenterParent;
            Text = "FrmBomWHS";
            WindowState = FormWindowState.Maximized;
            Load += FrmBomWHS_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private GroupBox groupBox1;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private ComboBox comboBox1;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel6;
        private Label label15;
        private TextBox textBox10;
        private Button button4;
        private Button button3;
        private GroupBox groupBox2;
        private DataGridView dataGridView2;
        private CheckBox chkBlockInWHonly;
        private Button btnFound;
        private Button button1;
        private Button btnFontIncrease;
        private Button btnFontDecrease;
        private Button button2;
        private Button button5;
    }
}