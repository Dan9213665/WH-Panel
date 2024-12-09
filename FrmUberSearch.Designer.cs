namespace WH_Panel
{
    partial class FrmUberSearch
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmUberSearch));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label1 = new Label();
            button1 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox2 = new GroupBox();
            dataGridView1 = new DataGridView();
            groupBox3 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            textBox7 = new TextBox();
            textBox8 = new TextBox();
            textBox9 = new TextBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 17F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.4454689F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 72.65745F));
            tableLayoutPanel1.Size = new Size(1152, 651);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1146, 104);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Control";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(label1, 1, 0);
            tableLayoutPanel2.Controls.Add(button1, 0, 0);
            tableLayoutPanel2.Controls.Add(flowLayoutPanel1, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(1140, 82);
            tableLayoutPanel2.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(117, 0);
            label1.Name = "label1";
            label1.Size = new Size(89, 82);
            label1.TabIndex = 1;
            label1.Text = "Onusta Ordines";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(108, 76);
            button1.TabIndex = 2;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(212, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(925, 76);
            flowLayoutPanel1.TabIndex = 4;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 180);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1146, 468);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Vestri eventus";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(1140, 446);
            dataGridView1.TabIndex = 0;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(tableLayoutPanel3);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 113);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1146, 61);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Search by type";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 8;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12.5F));
            tableLayoutPanel3.Controls.Add(label2, 0, 0);
            tableLayoutPanel3.Controls.Add(label3, 1, 0);
            tableLayoutPanel3.Controls.Add(label4, 2, 0);
            tableLayoutPanel3.Controls.Add(label5, 3, 0);
            tableLayoutPanel3.Controls.Add(label6, 4, 0);
            tableLayoutPanel3.Controls.Add(label7, 5, 0);
            tableLayoutPanel3.Controls.Add(label8, 6, 0);
            tableLayoutPanel3.Controls.Add(label9, 7, 0);
            tableLayoutPanel3.Controls.Add(textBox2, 0, 1);
            tableLayoutPanel3.Controls.Add(textBox3, 1, 1);
            tableLayoutPanel3.Controls.Add(textBox4, 2, 1);
            tableLayoutPanel3.Controls.Add(textBox5, 3, 1);
            tableLayoutPanel3.Controls.Add(textBox6, 4, 1);
            tableLayoutPanel3.Controls.Add(textBox7, 5, 1);
            tableLayoutPanel3.Controls.Add(textBox8, 6, 1);
            tableLayoutPanel3.Controls.Add(textBox9, 7, 1);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 2;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(1140, 39);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 0);
            label2.Name = "label2";
            label2.Size = new Size(136, 15);
            label2.TabIndex = 0;
            label2.Text = "IPN";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Click += label2_Click;
            label2.DoubleClick += label2_DoubleClick;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(145, 0);
            label3.Name = "label3";
            label3.Size = new Size(136, 15);
            label3.TabIndex = 1;
            label3.Text = "Manufacturer";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(287, 0);
            label4.Name = "label4";
            label4.Size = new Size(136, 15);
            label4.TabIndex = 2;
            label4.Text = "MFPN";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            label4.Click += label4_Click;
            label4.DoubleClick += label4_DoubleClick;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(429, 0);
            label5.Name = "label5";
            label5.Size = new Size(136, 15);
            label5.TabIndex = 3;
            label5.Text = "Description";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            label5.Click += label5_Click;
            label5.DoubleClick += label5_DoubleClick;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(571, 0);
            label6.Name = "label6";
            label6.Size = new Size(136, 15);
            label6.TabIndex = 4;
            label6.Text = "Stock";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(713, 0);
            label7.Name = "label7";
            label7.Size = new Size(136, 15);
            label7.TabIndex = 5;
            label7.Text = "Updated On";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            label7.Click += label7_Click;
            label7.DoubleClick += label7_DoubleClick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(855, 0);
            label8.Name = "label8";
            label8.Size = new Size(136, 15);
            label8.TabIndex = 6;
            label8.Text = "Comments";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(997, 0);
            label9.Name = "label9";
            label9.Size = new Size(140, 15);
            label9.TabIndex = 7;
            label9.Text = "Source/Requester";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            label9.Click += label9_Click;
            label9.DoubleClick += label9_DoubleClick;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(3, 18);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(136, 23);
            textBox2.TabIndex = 8;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.TextChanged += textBox2_TextChanged;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Enabled = false;
            textBox3.Location = new Point(145, 18);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(136, 23);
            textBox3.TabIndex = 9;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox3.Enter += textBox3_Enter;
            textBox3.Leave += textBox3_Leave;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(287, 18);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(136, 23);
            textBox4.TabIndex = 9;
            textBox4.TextAlign = HorizontalAlignment.Center;
            textBox4.TextChanged += textBox4_TextChanged;
            textBox4.Enter += textBox4_Enter;
            textBox4.Leave += textBox4_Leave;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Fill;
            textBox5.Location = new Point(429, 18);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(136, 23);
            textBox5.TabIndex = 9;
            textBox5.TextAlign = HorizontalAlignment.Center;
            textBox5.TextChanged += textBox5_TextChanged;
            // 
            // textBox6
            // 
            textBox6.Dock = DockStyle.Fill;
            textBox6.Enabled = false;
            textBox6.Location = new Point(571, 18);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(136, 23);
            textBox6.TabIndex = 9;
            textBox6.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            textBox7.Dock = DockStyle.Fill;
            textBox7.Location = new Point(713, 18);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(136, 23);
            textBox7.TabIndex = 9;
            textBox7.TextAlign = HorizontalAlignment.Center;
            textBox7.TextChanged += textBox7_TextChanged;
            // 
            // textBox8
            // 
            textBox8.Dock = DockStyle.Fill;
            textBox8.Enabled = false;
            textBox8.Location = new Point(855, 18);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(136, 23);
            textBox8.TabIndex = 9;
            textBox8.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox9
            // 
            textBox9.Dock = DockStyle.Fill;
            textBox9.Location = new Point(997, 18);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(140, 23);
            textBox9.TabIndex = 9;
            textBox9.TextAlign = HorizontalAlignment.Center;
            textBox9.TextChanged += textBox9_TextChanged;
            // 
            // FrmUberSearch
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 651);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmUberSearch";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Summum Inquisitionis";
            WindowState = FormWindowState.Maximized;
            Load += FrmUberSearch_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label1;
        private Button button1;
        private GroupBox groupBox3;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private TextBox textBox6;
        private TextBox textBox7;
        private TextBox textBox8;
        private TextBox textBox9;
        private FlowLayoutPanel flowLayoutPanel1;
    }
}