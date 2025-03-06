namespace WH_Panel
{
    partial class FrmPackingSlipShip
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPackingSlipShip));
            groupBox3 = new GroupBox();
            dataGridView1 = new DataGridView();
            textBox11 = new TextBox();
            groupBox4 = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            btnPrintSticker = new Button();
            txtbIPN = new TextBox();
            txtbMFPN = new TextBox();
            txtbDescription = new TextBox();
            label14 = new Label();
            label16 = new Label();
            label17 = new Label();
            label21 = new Label();
            tableLayoutPanel6 = new TableLayoutPanel();
            checkBox1 = new CheckBox();
            txtbQty = new TextBox();
            groupBox6 = new GroupBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            checkBox2 = new CheckBox();
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
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            button1 = new Button();
            label12 = new Label();
            tableLayoutPanel4 = new TableLayoutPanel();
            label13 = new Label();
            listBox1 = new ListBox();
            button3 = new Button();
            button2 = new Button();
            button4 = new Button();
            groupBox2 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            label8 = new Label();
            label7 = new Label();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            textBox1 = new TextBox();
            groupBox5 = new GroupBox();
            dataGridView2 = new DataGridView();
            openFileDialog1 = new OpenFileDialog();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox4.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBox6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            SuspendLayout();
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(dataGridView1);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 289);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(1262, 137);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "FIltered Items Found";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = Color.DarkGray;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.BackgroundColor = SystemColors.ControlDarkDark;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.DarkGray;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
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
            dataGridView1.Size = new Size(1256, 115);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.SelectionChanged += dataGridView1_SelectionChanged;
            // 
            // textBox11
            // 
            textBox11.Dock = DockStyle.Fill;
            textBox11.Location = new Point(117, 60);
            textBox11.Name = "textBox11";
            textBox11.ReadOnly = true;
            textBox11.Size = new Size(108, 23);
            textBox11.TabIndex = 21;
            textBox11.TextAlign = HorizontalAlignment.Center;
            textBox11.TextChanged += textBox11_TextChanged;
            textBox11.Enter += textBox11_Enter;
            textBox11.Leave += textBox11_Leave;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(tableLayoutPanel5);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(3, 432);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(1262, 137);
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
            tableLayoutPanel5.Controls.Add(btnPrintSticker, 4, 0);
            tableLayoutPanel5.Controls.Add(txtbIPN, 0, 1);
            tableLayoutPanel5.Controls.Add(txtbMFPN, 0, 1);
            tableLayoutPanel5.Controls.Add(txtbDescription, 2, 1);
            tableLayoutPanel5.Controls.Add(label14, 0, 0);
            tableLayoutPanel5.Controls.Add(label16, 2, 0);
            tableLayoutPanel5.Controls.Add(label17, 3, 0);
            tableLayoutPanel5.Controls.Add(label21, 1, 0);
            tableLayoutPanel5.Controls.Add(tableLayoutPanel6, 3, 1);
            tableLayoutPanel5.Controls.Add(groupBox6, 5, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 2;
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.RowStyles.Add(new RowStyle());
            tableLayoutPanel5.Size = new Size(1256, 115);
            tableLayoutPanel5.TabIndex = 8;
            // 
            // btnPrintSticker
            // 
            btnPrintSticker.AutoSize = true;
            btnPrintSticker.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btnPrintSticker.BackgroundImageLayout = ImageLayout.Stretch;
            btnPrintSticker.Dock = DockStyle.Fill;
            btnPrintSticker.Location = new Point(839, 3);
            btnPrintSticker.Name = "btnPrintSticker";
            tableLayoutPanel5.SetRowSpan(btnPrintSticker, 2);
            btnPrintSticker.Size = new Size(203, 115);
            btnPrintSticker.TabIndex = 15;
            btnPrintSticker.TextAlign = ContentAlignment.MiddleLeft;
            btnPrintSticker.UseVisualStyleBackColor = true;
            btnPrintSticker.Click += btnPrintSticker_Click_1;
            // 
            // txtbIPN
            // 
            txtbIPN.Dock = DockStyle.Fill;
            txtbIPN.Location = new Point(3, 18);
            txtbIPN.Name = "txtbIPN";
            txtbIPN.ReadOnly = true;
            txtbIPN.Size = new Size(203, 23);
            txtbIPN.TabIndex = 1;
            txtbIPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbMFPN
            // 
            txtbMFPN.Dock = DockStyle.Fill;
            txtbMFPN.Location = new Point(212, 18);
            txtbMFPN.Name = "txtbMFPN";
            txtbMFPN.ReadOnly = true;
            txtbMFPN.Size = new Size(203, 23);
            txtbMFPN.TabIndex = 0;
            txtbMFPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbDescription
            // 
            txtbDescription.Dock = DockStyle.Fill;
            txtbDescription.Location = new Point(421, 18);
            txtbDescription.Multiline = true;
            txtbDescription.Name = "txtbDescription";
            txtbDescription.ReadOnly = true;
            txtbDescription.Size = new Size(203, 100);
            txtbDescription.TabIndex = 3;
            txtbDescription.TextAlign = HorizontalAlignment.Center;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Dock = DockStyle.Fill;
            label14.Location = new Point(3, 0);
            label14.Name = "label14";
            label14.Size = new Size(203, 15);
            label14.TabIndex = 8;
            label14.Text = "IPN";
            label14.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Dock = DockStyle.Fill;
            label16.Location = new Point(421, 0);
            label16.Name = "label16";
            label16.Size = new Size(203, 15);
            label16.TabIndex = 8;
            label16.Text = "Description";
            label16.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Dock = DockStyle.Fill;
            label17.Location = new Point(630, 0);
            label17.Name = "label17";
            label17.Size = new Size(203, 15);
            label17.TabIndex = 8;
            label17.Text = "Quantity";
            label17.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Dock = DockStyle.Fill;
            label21.Location = new Point(212, 0);
            label21.Name = "label21";
            label21.Size = new Size(203, 15);
            label21.TabIndex = 9;
            label21.Text = "MFPN";
            label21.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 1;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Controls.Add(checkBox1, 0, 1);
            tableLayoutPanel6.Controls.Add(txtbQty, 0, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(630, 18);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 2;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel6.Size = new Size(203, 100);
            tableLayoutPanel6.TabIndex = 17;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Dock = DockStyle.Fill;
            checkBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            checkBox1.Location = new Point(3, 53);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(197, 44);
            checkBox1.TabIndex = 17;
            checkBox1.Text = "Print Sticker";
            checkBox1.TextAlign = ContentAlignment.MiddleRight;
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // txtbQty
            // 
            txtbQty.Dock = DockStyle.Fill;
            txtbQty.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            txtbQty.Location = new Point(3, 3);
            txtbQty.Name = "txtbQty";
            txtbQty.Size = new Size(197, 39);
            txtbQty.TabIndex = 5;
            txtbQty.TextAlign = HorizontalAlignment.Center;
            txtbQty.Enter += txtbQty_Enter;
            txtbQty.KeyDown += txtbQty_KeyDown;
            txtbQty.Leave += txtbQty_Leave;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(tableLayoutPanel7);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(1048, 3);
            groupBox6.Name = "groupBox6";
            tableLayoutPanel5.SetRowSpan(groupBox6, 2);
            groupBox6.Size = new Size(205, 115);
            groupBox6.TabIndex = 18;
            groupBox6.TabStop = false;
            groupBox6.Text = "Edit  / Delete";
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 1;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.Controls.Add(checkBox2, 0, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(3, 19);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 2;
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel7.Size = new Size(199, 93);
            tableLayoutPanel7.TabIndex = 0;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Dock = DockStyle.Fill;
            checkBox2.Location = new Point(3, 3);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(193, 40);
            checkBox2.TabIndex = 0;
            checkBox2.Text = "EDIT MODE";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // textBox10
            // 
            textBox10.Dock = DockStyle.Fill;
            textBox10.Enabled = false;
            textBox10.Location = new Point(3, 60);
            textBox10.Name = "textBox10";
            textBox10.ReadOnly = true;
            textBox10.Size = new Size(108, 23);
            textBox10.TabIndex = 20;
            textBox10.TextAlign = HorizontalAlignment.Center;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Fill;
            label11.Location = new Point(117, 0);
            label11.Name = "label11";
            label11.Size = new Size(108, 57);
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
            label10.Size = new Size(108, 57);
            label10.TabIndex = 18;
            label10.Text = "DATE";
            label10.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox9
            // 
            textBox9.Dock = DockStyle.Fill;
            textBox9.Enabled = false;
            textBox9.Location = new Point(1143, 60);
            textBox9.Name = "textBox9";
            textBox9.ReadOnly = true;
            textBox9.Size = new Size(110, 23);
            textBox9.TabIndex = 17;
            textBox9.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox8
            // 
            textBox8.Dock = DockStyle.Fill;
            textBox8.Enabled = false;
            textBox8.Location = new Point(1029, 60);
            textBox8.Name = "textBox8";
            textBox8.ReadOnly = true;
            textBox8.Size = new Size(108, 23);
            textBox8.TabIndex = 16;
            textBox8.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox7
            // 
            textBox7.Dock = DockStyle.Fill;
            textBox7.Enabled = false;
            textBox7.Location = new Point(915, 60);
            textBox7.Name = "textBox7";
            textBox7.ReadOnly = true;
            textBox7.Size = new Size(108, 23);
            textBox7.TabIndex = 15;
            textBox7.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox6
            // 
            textBox6.Dock = DockStyle.Fill;
            textBox6.Enabled = false;
            textBox6.Location = new Point(801, 60);
            textBox6.Name = "textBox6";
            textBox6.ReadOnly = true;
            textBox6.Size = new Size(108, 23);
            textBox6.TabIndex = 14;
            textBox6.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Fill;
            textBox5.Enabled = false;
            textBox5.Location = new Point(687, 60);
            textBox5.Name = "textBox5";
            textBox5.ReadOnly = true;
            textBox5.Size = new Size(108, 23);
            textBox5.TabIndex = 13;
            textBox5.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Enabled = false;
            textBox4.Location = new Point(573, 60);
            textBox4.Name = "textBox4";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(108, 23);
            textBox4.TabIndex = 12;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(459, 60);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(108, 23);
            textBox3.TabIndex = 11;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox3.TextChanged += textBox3_TextChanged;
            textBox3.Enter += textBox3_Enter;
            textBox3.Leave += textBox3_Leave;
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Location = new Point(345, 60);
            textBox2.Name = "textBox2";
            textBox2.ReadOnly = true;
            textBox2.Size = new Size(108, 23);
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
            label9.Location = new Point(1143, 0);
            label9.Name = "label9";
            label9.Size = new Size(110, 57);
            label9.TabIndex = 8;
            label9.Text = "ALTs";
            label9.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.AutoSize = true;
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox4, 0, 3);
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 4);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 5;
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.RowStyles.Add(new RowStyle());
            tableLayoutPanel1.Size = new Size(1268, 716);
            tableLayoutPanel1.TabIndex = 1;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1262, 137);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Controls";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.AutoSize = true;
            tableLayoutPanel3.ColumnCount = 6;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.Controls.Add(button1, 0, 0);
            tableLayoutPanel3.Controls.Add(label12, 2, 0);
            tableLayoutPanel3.Controls.Add(tableLayoutPanel4, 3, 0);
            tableLayoutPanel3.Controls.Add(button3, 5, 0);
            tableLayoutPanel3.Controls.Add(button2, 4, 0);
            tableLayoutPanel3.Controls.Add(button4, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 1;
            tableLayoutPanel3.RowStyles.Add(new RowStyle());
            tableLayoutPanel3.Size = new Size(1256, 115);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // button1
            // 
            button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            button1.AutoSize = true;
            button1.BackColor = Color.Transparent;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Location = new Point(3, 3);
            button1.Name = "button1";
            button1.Size = new Size(119, 110);
            button1.TabIndex = 0;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Fill;
            label12.Location = new Point(253, 0);
            label12.Name = "label12";
            label12.Size = new Size(119, 116);
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
            tableLayoutPanel4.Location = new Point(378, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Size = new Size(622, 110);
            tableLayoutPanel4.TabIndex = 2;
            // 
            // label13
            // 
            label13.Dock = DockStyle.Fill;
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(616, 55);
            label13.TabIndex = 2;
            label13.Text = "Loading Errors :";
            label13.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // listBox1
            // 
            listBox1.Dock = DockStyle.Fill;
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 15;
            listBox1.Location = new Point(3, 58);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(616, 49);
            listBox1.TabIndex = 3;
            // 
            // button3
            // 
            button3.BackgroundImage = Properties.Resources.box_214671;
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(1131, 3);
            button3.Name = "button3";
            button3.Size = new Size(122, 110);
            button3.TabIndex = 3;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.Control;
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Dock = DockStyle.Fill;
            button2.Enabled = false;
            button2.Location = new Point(1006, 3);
            button2.Name = "button2";
            button2.Size = new Size(119, 110);
            button2.TabIndex = 4;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // button4
            // 
            button4.BackgroundImage = (Image)resources.GetObject("button4.BackgroundImage");
            button4.BackgroundImageLayout = ImageLayout.Zoom;
            button4.Dock = DockStyle.Fill;
            button4.Location = new Point(128, 3);
            button4.Name = "button4";
            button4.Size = new Size(119, 110);
            button4.TabIndex = 5;
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(tableLayoutPanel2);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 146);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1262, 137);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Filters";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
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
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1256, 115);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Fill;
            label8.Location = new Point(1029, 0);
            label8.Name = "label8";
            label8.Size = new Size(108, 57);
            label8.TabIndex = 7;
            label8.Text = "NOTEs";
            label8.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Fill;
            label7.Location = new Point(915, 0);
            label7.Name = "label7";
            label7.Size = new Size(108, 57);
            label7.TabIndex = 6;
            label7.Text = "QTY per UNIT";
            label7.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(801, 0);
            label6.Name = "label6";
            label6.Size = new Size(108, 57);
            label6.TabIndex = 5;
            label6.Text = "ORDER QTY";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(687, 0);
            label5.Name = "label5";
            label5.Size = new Size(108, 57);
            label5.TabIndex = 4;
            label5.Text = "DELTA";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(573, 0);
            label4.Name = "label4";
            label4.Size = new Size(108, 57);
            label4.TabIndex = 3;
            label4.Text = "Qty in KIT";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(459, 0);
            label3.Name = "label3";
            label3.Size = new Size(108, 57);
            label3.TabIndex = 2;
            label3.Text = "Description";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            label3.Click += label3_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(345, 0);
            label2.Name = "label2";
            label2.Size = new Size(108, 57);
            label2.TabIndex = 1;
            label2.Text = "MFPN";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            label2.Click += label2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(231, 0);
            label1.Name = "label1";
            label1.Size = new Size(108, 57);
            label1.TabIndex = 0;
            label1.Text = "IPN";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            label1.Click += label1_Click;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(231, 60);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(108, 23);
            textBox1.TabIndex = 9;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.Enter += textBox1_Enter;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox1.Leave += textBox1_Leave;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(dataGridView2);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(3, 575);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(1262, 138);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Packed Items";
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 19);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.Size = new Size(1256, 116);
            dataGridView2.TabIndex = 0;
            dataGridView2.CellClick += dataGridView2_CellClick;
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // FrmPackingSlipShip
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1268, 716);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPackingSlipShip";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Shipping Items";
            WindowState = FormWindowState.Maximized;
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            groupBox6.ResumeLayout(false);
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
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
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion
        private GroupBox groupBox3;
        private DataGridView dataGridView1;
        private TextBox textBox11;
        private GroupBox groupBox4;
        private TableLayoutPanel tableLayoutPanel5;
        private Button btnPrintSticker;
        private TextBox txtbIPN;
        private TextBox txtbMFPN;
        private TextBox txtbDescription;
        private Label label14;
        private Label label16;
        private Label label17;
        private Label label21;
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
        private Label label9;
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private Button button1;
        private Label label12;
        private TableLayoutPanel tableLayoutPanel4;
        private Label label13;
        private ListBox listBox1;
        private GroupBox groupBox2;
        private TableLayoutPanel tableLayoutPanel2;
        private Label label8;
        private Label label7;
        private Label label6;
        private Label label5;
        private Label label4;
        private Label label3;
        private Label label2;
        private Label label1;
        private TextBox textBox1;
        private GroupBox groupBox5;
        private DataGridView dataGridView2;
        private Button button3;
        private TableLayoutPanel tableLayoutPanel6;
        private CheckBox checkBox1;
        private TextBox txtbQty;
        private Button button2;
        private OpenFileDialog openFileDialog1;
        private GroupBox groupBox6;
        private TableLayoutPanel tableLayoutPanel7;
        private CheckBox checkBox2;
        private Button button4;
    }
}