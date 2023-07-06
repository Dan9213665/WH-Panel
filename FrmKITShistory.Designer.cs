namespace WH_Panel
{
    partial class FrmKITShistory
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
            DateTimePicker dateTimePicker1;
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmKITShistory));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Button();
            label12 = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            label13 = new Label();
            listBox1 = new ListBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            textBox11 = new TextBox();
            textBox10 = new TextBox();
            label11 = new Label();
            label10 = new Label();
            textBox9 = new TextBox();
            textBox8 = new TextBox();
            textBox7 = new TextBox();
            textBox6 = new TextBox();
            textBox5 = new TextBox();
            textBox4 = new TextBox();
            textBox3 = new TextBox();
            textBox2 = new TextBox();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            groupBox3 = new GroupBox();
            dataGridView1 = new DataGridView();
            groupBox4 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnPrintSticker = new Button();
            txtbIPN = new TextBox();
            txtbMFPN = new TextBox();
            txtbDescription = new TextBox();
            txtbQty = new TextBox();
            label14 = new Label();
            label16 = new Label();
            label17 = new Label();
            label18 = new Label();
            label21 = new Label();
            dateTimePicker1 = new DateTimePicker();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            SuspendLayout();
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Dock = DockStyle.Fill;
            dateTimePicker1.Enabled = false;
            dateTimePicker1.Location = new Point(767, 18);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(185, 23);
            dateTimePicker1.TabIndex = 5;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 3);
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1161, 572);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1155, 137);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Controls";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60F));
            tableLayoutPanel3.Controls.Add(button1, 0, 0);
            tableLayoutPanel3.Controls.Add(label12, 1, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 2, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Size = new Size(1149, 115);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.AutoSize = true;
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = Properties.Resources.reloadDB;
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(166, 109);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(175, 0);
            label12.Name = "label12";
            label12.Size = new Size(281, 115);
            label12.TabIndex = 1;
            label12.Text = "loaded rows";
            label12.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.AutoSize = true;
            tableLayoutPanel4.ColumnCount = 1;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Controls.Add(label13, 0, 0);
            tableLayoutPanel4.Controls.Add(listBox1, 0, 1);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(462, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(684, 109);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // label13
            // 
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(678, 54);
            label13.TabIndex = 2;
            label13.Text = "Loading Errors :";
            label13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(3, 57);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(678, 49);
            listBox1.TabIndex = 3;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 146);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1155, 137);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Filters";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 11;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 9.090908F));
            tableLayoutPanel2.Controls.Add(textBox11, 1, 1);
            tableLayoutPanel2.Controls.Add(textBox10, 0, 1);
            tableLayoutPanel2.Controls.Add(label11, 1, 0);
            tableLayoutPanel2.Controls.Add(label10, 0, 0);
            tableLayoutPanel2.Controls.Add(textBox9, 10, 1);
            tableLayoutPanel2.Controls.Add(textBox8, 9, 1);
            tableLayoutPanel2.Controls.Add(textBox7, 8, 1);
            tableLayoutPanel2.Controls.Add(textBox6, 7, 1);
            tableLayoutPanel2.Controls.Add(textBox5, 6, 1);
            tableLayoutPanel2.Controls.Add(textBox4, 5, 1);
            tableLayoutPanel2.Controls.Add(textBox3, 4, 1);
            tableLayoutPanel2.Controls.Add(textBox2, 3, 1);
            tableLayoutPanel2.Controls.Add(label9, 10, 0);
            tableLayoutPanel2.Controls.Add(label8, 9, 0);
            tableLayoutPanel2.Controls.Add(label7, 8, 0);
            tableLayoutPanel2.Controls.Add(label6, 7, 0);
            tableLayoutPanel2.Controls.Add(label5, 6, 0);
            tableLayoutPanel2.Controls.Add(label4, 5, 0);
            tableLayoutPanel2.Controls.Add(label3, 4, 0);
            tableLayoutPanel2.Controls.Add(label2, 3, 0);
            tableLayoutPanel2.Controls.Add(label1, 2, 0);
            tableLayoutPanel2.Controls.Add(textBox1, 2, 1);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.Size = new Size(1149, 115);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // textBox11
            // 
            textBox11.Dock = DockStyle.Fill;
            textBox11.Location = new Point(107, 18);
            textBox11.Name = "textBox11";
            textBox11.Size = new Size(98, 23);
            textBox11.TabIndex = 21;
            textBox11.TextAlign = HorizontalAlignment.Center;
            textBox11.TextChanged += textBox11_TextChanged;
            // 
            // textBox10
            // 
            textBox10.Dock = DockStyle.Fill;
            textBox10.Enabled = false;
            textBox10.Location = new Point(3, 18);
            textBox10.Name = "textBox10";
            textBox10.Size = new Size(98, 23);
            textBox10.TabIndex = 20;
            textBox10.TextAlign = HorizontalAlignment.Center;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(107, 0);
            label11.Name = "label11";
            label11.Size = new Size(98, 15);
            label11.TabIndex = 19;
            label11.Text = "Project Name";
            label11.TextAlign = ContentAlignment.MiddleCenter;
            label11.Click += label11_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Fill;
            label10.Location = new Point(3, 0);
            label10.Name = "label10";
            label10.Size = new Size(98, 15);
            label10.TabIndex = 18;
            label10.Text = "DATE";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox9
            // 
            textBox9.Dock = DockStyle.Fill;
            textBox9.Location = new Point(1043, 18);
            textBox9.Name = "textBox9";
            textBox9.Size = new Size(103, 23);
            textBox9.TabIndex = 17;
            textBox9.TextAlign = HorizontalAlignment.Center;
            textBox9.TextChanged += textBox9_TextChanged;
            textBox9.Enter += textBox9_Enter;
            textBox9.Leave += textBox9_Leave;
            // 
            // textBox8
            // 
            textBox8.Dock = DockStyle.Fill;
            textBox8.Enabled = false;
            textBox8.Location = new Point(939, 18);
            textBox8.Name = "textBox8";
            textBox8.Size = new Size(98, 23);
            textBox8.TabIndex = 16;
            textBox8.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            textBox7.Dock = DockStyle.Fill;
            textBox7.Enabled = false;
            textBox7.Location = new Point(835, 18);
            textBox7.Name = "textBox7";
            textBox7.Size = new Size(98, 23);
            textBox7.TabIndex = 15;
            textBox7.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            textBox6.Dock = DockStyle.Fill;
            textBox6.Enabled = false;
            textBox6.Location = new Point(731, 18);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(98, 23);
            textBox6.TabIndex = 14;
            textBox6.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Fill;
            textBox5.Enabled = false;
            textBox5.Location = new Point(627, 18);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(98, 23);
            textBox5.TabIndex = 13;
            textBox5.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Enabled = false;
            textBox4.Location = new Point(523, 18);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(98, 23);
            textBox4.TabIndex = 12;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(419, 18);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(98, 23);
            textBox3.TabIndex = 11;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox3.TextChanged += textBox3_TextChanged;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(315, 18);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(98, 23);
            textBox2.TabIndex = 10;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.TextChanged += textBox2_TextChanged;
            textBox2.Enter += textBox2_Enter;
            textBox2.KeyDown += textBox2_KeyDown;
            textBox2.Leave += textBox2_Leave;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Dock = DockStyle.Fill;
            label9.Location = new Point(1043, 0);
            label9.Name = "label9";
            label9.Size = new Size(103, 15);
            label9.TabIndex = 8;
            label9.Text = "ALTs";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            label9.Click += label9_Click;
            label9.DoubleClick += label9_DoubleClick;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(939, 0);
            label8.Name = "label8";
            label8.Size = new Size(98, 15);
            label8.TabIndex = 7;
            label8.Text = "NOTEs";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(835, 0);
            label7.Name = "label7";
            label7.Size = new Size(98, 15);
            label7.TabIndex = 6;
            label7.Text = "QTY per UNIT";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(731, 0);
            label6.Name = "label6";
            label6.Size = new Size(98, 15);
            label6.TabIndex = 5;
            label6.Text = "ORDER QTY";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(627, 0);
            label5.Name = "label5";
            label5.Size = new Size(98, 15);
            label5.TabIndex = 4;
            label5.Text = "DELTA";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(523, 0);
            label4.Name = "label4";
            label4.Size = new Size(98, 15);
            label4.TabIndex = 3;
            label4.Text = "Qty in KIT";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(419, 0);
            label3.Name = "label3";
            label3.Size = new Size(98, 15);
            label3.TabIndex = 2;
            label3.Text = "Description";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(315, 0);
            label2.Name = "label2";
            label2.Size = new Size(98, 15);
            label2.TabIndex = 1;
            label2.Text = "MFPN";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Click += label2_Click;
            label2.DoubleClick += label2_DoubleClick;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(211, 0);
            label1.Name = "label1";
            label1.Size = new Size(98, 15);
            label1.TabIndex = 0;
            label1.Text = "IPN";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            label1.DoubleClick += label1_DoubleClick;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(211, 18);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(98, 23);
            textBox1.TabIndex = 9;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Enter += textBox1_Enter;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox1.Leave += textBox1_Leave;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dataGridView1);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 432);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1155, 137);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Results";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.DarkGray;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ControlDarkDark;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.DarkGray;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = Color.White;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(1149, 115);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel5);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 289);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1155, 137);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "Print STIKER";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.AutoSize = true;
            tableLayoutPanel5.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            tableLayoutPanel5.ColumnCount = 6;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.66667F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            tableLayoutPanel5.Controls.Add(btnPrintSticker, 5, 0);
            tableLayoutPanel5.Controls.Add(txtbIPN, 0, 1);
            tableLayoutPanel5.Controls.Add(txtbMFPN, 0, 1);
            tableLayoutPanel5.Controls.Add(dateTimePicker1, 4, 1);
            tableLayoutPanel5.Controls.Add(txtbDescription, 2, 1);
            tableLayoutPanel5.Controls.Add(txtbQty, 3, 1);
            tableLayoutPanel5.Controls.Add(label14, 0, 0);
            tableLayoutPanel5.Controls.Add(label16, 2, 0);
            tableLayoutPanel5.Controls.Add(label17, 3, 0);
            tableLayoutPanel5.Controls.Add(label18, 4, 0);
            tableLayoutPanel5.Controls.Add(label21, 1, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(1149, 115);
            tableLayoutPanel5.TabIndex = 8;
            // 
            // btnPrintSticker
            // 
            btnPrintSticker.AutoSize = true;
            btnPrintSticker.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnPrintSticker.BackgroundImage = (Image)resources.GetObject("btnPrintSticker.BackgroundImage");
            btnPrintSticker.BackgroundImageLayout = ImageLayout.Stretch;
            btnPrintSticker.Dock = DockStyle.Fill;
            btnPrintSticker.Location = new Point(958, 3);
            btnPrintSticker.Name = "btnPrintSticker";
            tableLayoutPanel5.SetRowSpan(btnPrintSticker, 2);
            btnPrintSticker.Size = new Size(188, 109);
            btnPrintSticker.TabIndex = 15;
            btnPrintSticker.TextAlign = ContentAlignment.MiddleLeft;
            btnPrintSticker.UseVisualStyleBackColor = true;
            btnPrintSticker.Click += btnPrintSticker_Click;
            // 
            // txtbIPN
            // 
            txtbIPN.Dock = DockStyle.Fill;
            txtbIPN.Location = new Point(3, 18);
            txtbIPN.Name = "txtbIPN";
            txtbIPN.ReadOnly = true;
            txtbIPN.Size = new Size(185, 23);
            txtbIPN.TabIndex = 1;
            txtbIPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbMFPN
            // 
            txtbMFPN.Dock = DockStyle.Fill;
            txtbMFPN.Location = new Point(194, 18);
            txtbMFPN.Name = "txtbMFPN";
            txtbMFPN.ReadOnly = true;
            txtbMFPN.Size = new Size(185, 23);
            txtbMFPN.TabIndex = 0;
            txtbMFPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbDescription
            // 
            txtbDescription.Dock = DockStyle.Fill;
            txtbDescription.Location = new Point(385, 18);
            txtbDescription.Multiline = true;
            txtbDescription.Name = "txtbDescription";
            txtbDescription.ReadOnly = true;
            txtbDescription.Size = new Size(185, 94);
            txtbDescription.TabIndex = 3;
            txtbDescription.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbQty
            // 
            txtbQty.Dock = DockStyle.Fill;
            txtbQty.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            txtbQty.Location = new Point(576, 18);
            txtbQty.Name = "txtbQty";
            txtbQty.Size = new Size(185, 39);
            txtbQty.TabIndex = 4;
            txtbQty.TextAlign = HorizontalAlignment.Center;
            txtbQty.Enter += txtbQty_Enter;
            txtbQty.KeyDown += txtbQty_KeyDown;
            txtbQty.Leave += txtbQty_Leave;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Dock = DockStyle.Fill;
            label14.Location = new Point(3, 0);
            label14.Name = "label14";
            label14.Size = new Size(185, 15);
            label14.TabIndex = 8;
            label14.Text = "IPN";
            label14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Dock = DockStyle.Fill;
            label16.Location = new Point(385, 0);
            label16.Name = "label16";
            label16.Size = new Size(185, 15);
            label16.TabIndex = 8;
            label16.Text = "Description";
            label16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Dock = DockStyle.Fill;
            label17.Location = new Point(576, 0);
            label17.Name = "label17";
            label17.Size = new Size(185, 15);
            label17.TabIndex = 8;
            label17.Text = "Quantity";
            label17.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Dock = DockStyle.Fill;
            label18.Location = new Point(767, 0);
            label18.Name = "label18";
            label18.Size = new Size(185, 15);
            label18.TabIndex = 8;
            label18.Text = "Date";
            label18.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Dock = DockStyle.Fill;
            label21.Location = new Point(194, 0);
            label21.Name = "label21";
            label21.Size = new Size(185, 15);
            label21.TabIndex = 9;
            label21.Text = "MFPN";
            label21.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // FrmKITShistory
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1161, 572);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmKITShistory";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "EXCEL Ripper";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private Button button1;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label9;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private GroupBox groupBox3;
        private TextBox textBox11;
        private TextBox textBox10;
        private Label label11;
        private Label label10;
        private TextBox textBox9;
        private TextBox textBox8;
        private TextBox textBox7;
        private TextBox textBox6;
        private TextBox textBox5;
        private TextBox textBox4;
        private TextBox textBox3;
        private TextBox textBox2;
        private TextBox textBox1;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel3;
        private Label label12;
        private Label label13;
        private TableLayoutPanel tableLayoutPanel4;
        private ListBox listBox1;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel5;
        private Button btnPrintSticker;
        private TextBox txtbMFPN;
        private TextBox txtbDescription;
        private TextBox txtbQty;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label label18;
        private Label label21;
        private TextBox txtbIPN;
    }
}