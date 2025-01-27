namespace WH_Panel
{
    partial class FrmPriorityAPI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPriorityAPI));
            tableLayoutPanel1 = new TableLayoutPanel();
            groupBox1 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            txtbInputIPN = new TextBox();
            txtbInputMFPN = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            textBox5 = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            btnPrinSticker = new Button();
            button2 = new Button();
            btnMFG = new Button();
            txtbPART = new TextBox();
            label6 = new Label();
            tableLayoutPanel6 = new TableLayoutPanel();
            rbtMFG = new RadioButton();
            rbtIN = new RadioButton();
            tbtOUT = new RadioButton();
            rbtFTK = new RadioButton();
            txtbIN = new TextBox();
            txtbOUT = new TextBox();
            cmbPackCode = new ComboBox();
            tableLayoutPanel7 = new TableLayoutPanel();
            cmbPreCode = new ComboBox();
            cmbPostCode = new ComboBox();
            txtbDecoder = new TextBox();
            groupBox2 = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            comboBox1 = new ComboBox();
            groupBox3 = new GroupBox();
            dataGridView1 = new DataGridView();
            txtbFilterIPN = new TextBox();
            groupBox4 = new GroupBox();
            dataGridView2 = new DataGridView();
            txtbWHSID = new TextBox();
            btnClearIpnFilter = new Button();
            btnGetMFPNs = new Button();
            tableLayoutPanel4 = new TableLayoutPanel();
            gbxINSERT = new GroupBox();
            tableLayoutPanel5 = new TableLayoutPanel();
            lblIPN = new Label();
            lblDESC = new Label();
            lblMANUF = new Label();
            btnINSERTlogpart = new Button();
            txtbIPN = new TextBox();
            txtbDESC = new TextBox();
            txtbMNF = new TextBox();
            lblMFPN = new Label();
            txtbMFPN = new TextBox();
            txtbBuffer = new TextBox();
            btnClear = new Button();
            btnBULKinsert = new Button();
            groupBox5 = new GroupBox();
            txtbPing = new TextBox();
            txtLog = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            tableLayoutPanel7.SuspendLayout();
            groupBox2.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView2).BeginInit();
            tableLayoutPanel4.SuspendLayout();
            gbxINSERT.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            groupBox5.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel4, 3, 0);
            tableLayoutPanel1.Controls.Add(txtLog, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 36.75595F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 29.9107151F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Size = new Size(1229, 659);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox1, 2);
            groupBox1.Controls.Add(tableLayoutPanel2);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(608, 236);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Print Sticker";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 3;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel2.Controls.Add(txtbInputIPN, 0, 1);
            tableLayoutPanel2.Controls.Add(txtbInputMFPN, 0, 3);
            tableLayoutPanel2.Controls.Add(textBox3, 0, 5);
            tableLayoutPanel2.Controls.Add(textBox4, 0, 7);
            tableLayoutPanel2.Controls.Add(textBox5, 0, 9);
            tableLayoutPanel2.Controls.Add(label1, 0, 0);
            tableLayoutPanel2.Controls.Add(label2, 0, 2);
            tableLayoutPanel2.Controls.Add(label3, 0, 4);
            tableLayoutPanel2.Controls.Add(label4, 0, 6);
            tableLayoutPanel2.Controls.Add(label5, 0, 8);
            tableLayoutPanel2.Controls.Add(btnPrinSticker, 2, 2);
            tableLayoutPanel2.Controls.Add(button2, 2, 0);
            tableLayoutPanel2.Controls.Add(btnMFG, 2, 8);
            tableLayoutPanel2.Controls.Add(txtbPART, 1, 1);
            tableLayoutPanel2.Controls.Add(label6, 1, 0);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel6, 1, 9);
            tableLayoutPanel2.Controls.Add(txtbIN, 1, 7);
            tableLayoutPanel2.Controls.Add(txtbOUT, 2, 7);
            tableLayoutPanel2.Controls.Add(cmbPackCode, 1, 5);
            tableLayoutPanel2.Controls.Add(tableLayoutPanel7, 1, 3);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 10;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel2.Size = new Size(602, 214);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // txtbInputIPN
            // 
            txtbInputIPN.Dock = DockStyle.Fill;
            txtbInputIPN.Location = new Point(3, 24);
            txtbInputIPN.Name = "txtbInputIPN";
            txtbInputIPN.PlaceholderText = "input IPN";
            txtbInputIPN.Size = new Size(194, 23);
            txtbInputIPN.TabIndex = 0;
            txtbInputIPN.TextAlign = HorizontalAlignment.Center;
            txtbInputIPN.KeyDown += textBox1_KeyDown;
            // 
            // txtbInputMFPN
            // 
            txtbInputMFPN.Dock = DockStyle.Fill;
            txtbInputMFPN.Location = new Point(3, 66);
            txtbInputMFPN.Name = "txtbInputMFPN";
            txtbInputMFPN.PlaceholderText = "input MFPN";
            txtbInputMFPN.Size = new Size(194, 23);
            txtbInputMFPN.TabIndex = 1;
            txtbInputMFPN.TextAlign = HorizontalAlignment.Center;
            txtbInputMFPN.KeyDown += txtbInputMFPN_KeyDown;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Location = new Point(3, 108);
            textBox3.Name = "textBox3";
            textBox3.PlaceholderText = "Description from DB";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(194, 23);
            textBox3.TabIndex = 2;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox4
            // 
            textBox4.Dock = DockStyle.Fill;
            textBox4.Location = new Point(3, 150);
            textBox4.Name = "textBox4";
            textBox4.PlaceholderText = "Manufacturer from DB";
            textBox4.ReadOnly = true;
            textBox4.Size = new Size(194, 23);
            textBox4.TabIndex = 3;
            textBox4.TextAlign = HorizontalAlignment.Center;
            // 
            // textBox5
            // 
            textBox5.Dock = DockStyle.Fill;
            textBox5.Location = new Point(3, 192);
            textBox5.Name = "textBox5";
            textBox5.PlaceholderText = "input QTY";
            textBox5.Size = new Size(194, 23);
            textBox5.TabIndex = 4;
            textBox5.TextAlign = HorizontalAlignment.Center;
            textBox5.KeyDown += textBox5_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Dock = DockStyle.Fill;
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(194, 21);
            label1.TabIndex = 5;
            label1.Text = "IPN";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Dock = DockStyle.Fill;
            label2.Location = new Point(3, 42);
            label2.Name = "label2";
            label2.Size = new Size(194, 21);
            label2.TabIndex = 6;
            label2.Text = "MFPN";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Location = new Point(3, 84);
            label3.Name = "label3";
            label3.Size = new Size(194, 21);
            label3.TabIndex = 7;
            label3.Text = "DESCRIPTION";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Fill;
            label4.Location = new Point(3, 126);
            label4.Name = "label4";
            label4.Size = new Size(194, 21);
            label4.TabIndex = 8;
            label4.Text = "MANUFACTURER";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Fill;
            label5.Location = new Point(3, 168);
            label5.Name = "label5";
            label5.Size = new Size(194, 21);
            label5.TabIndex = 9;
            label5.Text = "QTY";
            label5.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnPrinSticker
            // 
            btnPrinSticker.Dock = DockStyle.Fill;
            btnPrinSticker.Location = new Point(403, 45);
            btnPrinSticker.Name = "btnPrinSticker";
            tableLayoutPanel2.SetRowSpan(btnPrinSticker, 2);
            btnPrinSticker.Size = new Size(196, 36);
            btnPrinSticker.TabIndex = 10;
            btnPrinSticker.Text = "Print Sticker";
            btnPrinSticker.UseVisualStyleBackColor = true;
            btnPrinSticker.Click += btnPrintSticker_Click;
            // 
            // button2
            // 
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(403, 3);
            button2.Name = "button2";
            tableLayoutPanel2.SetRowSpan(button2, 2);
            button2.Size = new Size(196, 36);
            button2.TabIndex = 11;
            button2.Text = "CLEAR";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // btnMFG
            // 
            btnMFG.Dock = DockStyle.Fill;
            btnMFG.Location = new Point(403, 171);
            btnMFG.Name = "btnMFG";
            tableLayoutPanel2.SetRowSpan(btnMFG, 2);
            btnMFG.Size = new Size(196, 40);
            btnMFG.TabIndex = 12;
            btnMFG.Text = "MOVE";
            btnMFG.UseVisualStyleBackColor = true;
            btnMFG.Click += btnMFG_Click;
            // 
            // txtbPART
            // 
            txtbPART.Dock = DockStyle.Fill;
            txtbPART.Location = new Point(203, 24);
            txtbPART.Name = "txtbPART";
            txtbPART.ReadOnly = true;
            txtbPART.Size = new Size(194, 23);
            txtbPART.TabIndex = 13;
            txtbPART.TextAlign = HorizontalAlignment.Center;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Fill;
            label6.Location = new Point(203, 0);
            label6.Name = "label6";
            label6.Size = new Size(194, 21);
            label6.TabIndex = 14;
            label6.Text = "PARTid";
            label6.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 4;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel6.Controls.Add(rbtMFG, 0, 0);
            tableLayoutPanel6.Controls.Add(rbtIN, 1, 0);
            tableLayoutPanel6.Controls.Add(tbtOUT, 3, 0);
            tableLayoutPanel6.Controls.Add(rbtFTK, 2, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(203, 192);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(194, 19);
            tableLayoutPanel6.TabIndex = 16;
            // 
            // rbtMFG
            // 
            rbtMFG.AutoSize = true;
            rbtMFG.Dock = DockStyle.Fill;
            rbtMFG.Location = new Point(3, 3);
            rbtMFG.Name = "rbtMFG";
            rbtMFG.Size = new Size(42, 13);
            rbtMFG.TabIndex = 15;
            rbtMFG.Text = "MFG";
            rbtMFG.UseVisualStyleBackColor = true;
            rbtMFG.CheckedChanged += rbtMFG_CheckedChanged;
            // 
            // rbtIN
            // 
            rbtIN.AutoSize = true;
            rbtIN.Dock = DockStyle.Fill;
            rbtIN.Location = new Point(51, 3);
            rbtIN.Name = "rbtIN";
            rbtIN.Size = new Size(42, 13);
            rbtIN.TabIndex = 16;
            rbtIN.Text = "IN";
            rbtIN.UseVisualStyleBackColor = true;
            rbtIN.CheckedChanged += rbtIN_CheckedChanged;
            // 
            // tbtOUT
            // 
            tbtOUT.AutoSize = true;
            tbtOUT.Dock = DockStyle.Fill;
            tbtOUT.Location = new Point(147, 3);
            tbtOUT.Name = "tbtOUT";
            tbtOUT.Size = new Size(44, 13);
            tbtOUT.TabIndex = 17;
            tbtOUT.Text = "OUT";
            tbtOUT.UseVisualStyleBackColor = true;
            tbtOUT.CheckedChanged += tbtOUT_CheckedChanged;
            // 
            // rbtFTK
            // 
            rbtFTK.AutoSize = true;
            rbtFTK.Location = new Point(99, 3);
            rbtFTK.Name = "rbtFTK";
            rbtFTK.Size = new Size(42, 13);
            rbtFTK.TabIndex = 18;
            rbtFTK.TabStop = true;
            rbtFTK.Text = "FTK";
            rbtFTK.UseVisualStyleBackColor = true;
            rbtFTK.CheckedChanged += rbtFTK_CheckedChanged;
            // 
            // txtbIN
            // 
            txtbIN.Dock = DockStyle.Fill;
            txtbIN.Location = new Point(203, 150);
            txtbIN.Name = "txtbIN";
            txtbIN.PlaceholderText = "Supplier`s Transfer Document";
            txtbIN.ReadOnly = true;
            txtbIN.Size = new Size(194, 23);
            txtbIN.TabIndex = 17;
            txtbIN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbOUT
            // 
            txtbOUT.Dock = DockStyle.Fill;
            txtbOUT.Location = new Point(403, 150);
            txtbOUT.Name = "txtbOUT";
            txtbOUT.PlaceholderText = "Sent / Moved to...";
            txtbOUT.ReadOnly = true;
            txtbOUT.Size = new Size(196, 23);
            txtbOUT.TabIndex = 18;
            txtbOUT.TextAlign = HorizontalAlignment.Center;
            // 
            // cmbPackCode
            // 
            cmbPackCode.Dock = DockStyle.Fill;
            cmbPackCode.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbPackCode.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, 0);
            cmbPackCode.FormattingEnabled = true;
            cmbPackCode.Location = new Point(403, 108);
            cmbPackCode.Name = "cmbPackCode";
            tableLayoutPanel2.SetRowSpan(cmbPackCode, 2);
            cmbPackCode.Size = new Size(196, 40);
            cmbPackCode.Sorted = true;
            cmbPackCode.TabIndex = 19;
            // 
            // tableLayoutPanel7
            // 
            tableLayoutPanel7.ColumnCount = 3;
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel7.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel7.Controls.Add(cmbPreCode, 0, 0);
            tableLayoutPanel7.Controls.Add(cmbPostCode, 2, 0);
            tableLayoutPanel7.Controls.Add(txtbDecoder, 1, 0);
            tableLayoutPanel7.Dock = DockStyle.Fill;
            tableLayoutPanel7.Location = new Point(203, 66);
            tableLayoutPanel7.Name = "tableLayoutPanel7";
            tableLayoutPanel7.RowCount = 1;
            tableLayoutPanel2.SetRowSpan(tableLayoutPanel7, 3);
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel7.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel7.Size = new Size(194, 57);
            tableLayoutPanel7.TabIndex = 20;
            // 
            // cmbPreCode
            // 
            cmbPreCode.Dock = DockStyle.Fill;
            cmbPreCode.FormattingEnabled = true;
            cmbPreCode.Items.AddRange(new object[] { "1P", "6P1P", "pm:" });
            cmbPreCode.Location = new Point(3, 3);
            cmbPreCode.Name = "cmbPreCode";
            cmbPreCode.Size = new Size(58, 23);
            cmbPreCode.TabIndex = 0;
            // 
            // cmbPostCode
            // 
            cmbPostCode.Dock = DockStyle.Fill;
            cmbPostCode.FormattingEnabled = true;
            cmbPostCode.Items.AddRange(new object[] { "30P", "K1K", "6P2", ",qty:" });
            cmbPostCode.Location = new Point(131, 3);
            cmbPostCode.Name = "cmbPostCode";
            cmbPostCode.Size = new Size(60, 23);
            cmbPostCode.TabIndex = 1;
            // 
            // txtbDecoder
            // 
            txtbDecoder.Dock = DockStyle.Fill;
            txtbDecoder.Location = new Point(67, 3);
            txtbDecoder.Name = "txtbDecoder";
            txtbDecoder.Size = new Size(58, 23);
            txtbDecoder.TabIndex = 2;
            txtbDecoder.TextAlign = HorizontalAlignment.Center;
            txtbDecoder.KeyDown += txtbDecoder_KeyDown;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 4);
            groupBox2.Controls.Add(tableLayoutPanel3);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 245);
            groupBox2.Name = "groupBox2";
            tableLayoutPanel1.SetRowSpan(groupBox2, 2);
            groupBox2.Size = new Size(1223, 411);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Warehouses";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 3;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel3.Controls.Add(comboBox1, 0, 0);
            tableLayoutPanel3.Controls.Add(groupBox3, 0, 2);
            tableLayoutPanel3.Controls.Add(txtbFilterIPN, 0, 1);
            tableLayoutPanel3.Controls.Add(groupBox4, 2, 2);
            tableLayoutPanel3.Controls.Add(txtbWHSID, 2, 0);
            tableLayoutPanel3.Controls.Add(btnClearIpnFilter, 1, 1);
            tableLayoutPanel3.Controls.Add(btnGetMFPNs, 1, 0);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel3.Size = new Size(1217, 389);
            tableLayoutPanel3.TabIndex = 2;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 12F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 3);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(399, 29);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // groupBox3
            // 
            tableLayoutPanel3.SetColumnSpan(groupBox3, 2);
            groupBox3.Controls.Add(dataGridView1);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(3, 79);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(804, 307);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "WH";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(798, 285);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // txtbFilterIPN
            // 
            txtbFilterIPN.Dock = DockStyle.Fill;
            txtbFilterIPN.Location = new Point(3, 41);
            txtbFilterIPN.Name = "txtbFilterIPN";
            txtbFilterIPN.PlaceholderText = "filter IPN";
            txtbFilterIPN.Size = new Size(399, 23);
            txtbFilterIPN.TabIndex = 3;
            txtbFilterIPN.TextAlign = HorizontalAlignment.Center;
            txtbFilterIPN.KeyUp += textBox6_KeyUp_1;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(dataGridView2);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(813, 79);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(401, 307);
            groupBox4.TabIndex = 4;
            groupBox4.TabStop = false;
            groupBox4.Text = "Movements for IPN";
            // 
            // dataGridView2
            // 
            dataGridView2.AllowUserToAddRows = false;
            dataGridView2.AllowUserToDeleteRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView2.Dock = DockStyle.Fill;
            dataGridView2.Location = new Point(3, 19);
            dataGridView2.Name = "dataGridView2";
            dataGridView2.ReadOnly = true;
            dataGridView2.RowTemplate.Height = 25;
            dataGridView2.Size = new Size(395, 285);
            dataGridView2.TabIndex = 0;
            dataGridView2.CellDoubleClick += dataGridView2_CellDoubleClick;
            dataGridView2.CellMouseDown += dataGridView2_CellMouseDown;
            // 
            // txtbWHSID
            // 
            txtbWHSID.Dock = DockStyle.Fill;
            txtbWHSID.Location = new Point(813, 3);
            txtbWHSID.Name = "txtbWHSID";
            txtbWHSID.ReadOnly = true;
            txtbWHSID.Size = new Size(401, 23);
            txtbWHSID.TabIndex = 5;
            // 
            // btnClearIpnFilter
            // 
            btnClearIpnFilter.Dock = DockStyle.Fill;
            btnClearIpnFilter.Location = new Point(408, 41);
            btnClearIpnFilter.Name = "btnClearIpnFilter";
            btnClearIpnFilter.Size = new Size(399, 32);
            btnClearIpnFilter.TabIndex = 6;
            btnClearIpnFilter.Text = "Clear IPN filter";
            btnClearIpnFilter.UseVisualStyleBackColor = true;
            btnClearIpnFilter.Click += btnClearIpnFilter_Click;
            // 
            // btnGetMFPNs
            // 
            btnGetMFPNs.Dock = DockStyle.Fill;
            btnGetMFPNs.Location = new Point(408, 3);
            btnGetMFPNs.Name = "btnGetMFPNs";
            btnGetMFPNs.Size = new Size(399, 32);
            btnGetMFPNs.TabIndex = 7;
            btnGetMFPNs.Text = "GET MFPNs";
            btnGetMFPNs.UseVisualStyleBackColor = true;
            btnGetMFPNs.Click += btnGetMFPNs_Click;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 83.3713F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 16.6287022F));
            tableLayoutPanel4.Controls.Add(gbxINSERT, 0, 0);
            tableLayoutPanel4.Controls.Add(groupBox5, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(924, 3);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 2;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 19.7247715F));
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 80.27523F));
            tableLayoutPanel4.Size = new Size(302, 236);
            tableLayoutPanel4.TabIndex = 5;
            // 
            // gbxINSERT
            // 
            gbxINSERT.Controls.Add(tableLayoutPanel5);
            gbxINSERT.Dock = DockStyle.Fill;
            gbxINSERT.Location = new Point(3, 3);
            gbxINSERT.Name = "gbxINSERT";
            tableLayoutPanel4.SetRowSpan(gbxINSERT, 2);
            gbxINSERT.Size = new Size(245, 230);
            gbxINSERT.TabIndex = 2;
            gbxINSERT.TabStop = false;
            gbxINSERT.Text = "INSERT";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.ColumnCount = 3;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 72F));
            tableLayoutPanel5.Controls.Add(lblIPN, 0, 1);
            tableLayoutPanel5.Controls.Add(lblDESC, 0, 3);
            tableLayoutPanel5.Controls.Add(lblMANUF, 0, 4);
            tableLayoutPanel5.Controls.Add(btnINSERTlogpart, 2, 4);
            tableLayoutPanel5.Controls.Add(txtbIPN, 1, 1);
            tableLayoutPanel5.Controls.Add(txtbDESC, 1, 3);
            tableLayoutPanel5.Controls.Add(txtbMNF, 1, 4);
            tableLayoutPanel5.Controls.Add(lblMFPN, 0, 2);
            tableLayoutPanel5.Controls.Add(txtbMFPN, 1, 2);
            tableLayoutPanel5.Controls.Add(txtbBuffer, 1, 0);
            tableLayoutPanel5.Controls.Add(btnClear, 2, 0);
            tableLayoutPanel5.Controls.Add(btnBULKinsert, 0, 0);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 19);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 5;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutPanel5.Size = new Size(239, 208);
            tableLayoutPanel5.TabIndex = 4;
            // 
            // lblIPN
            // 
            lblIPN.AutoSize = true;
            lblIPN.Dock = DockStyle.Fill;
            lblIPN.Location = new Point(3, 41);
            lblIPN.Name = "lblIPN";
            lblIPN.Size = new Size(77, 41);
            lblIPN.TabIndex = 2;
            lblIPN.Text = "IPN";
            lblIPN.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblDESC
            // 
            lblDESC.AutoSize = true;
            lblDESC.Dock = DockStyle.Fill;
            lblDESC.Location = new Point(3, 123);
            lblDESC.Name = "lblDESC";
            lblDESC.Size = new Size(77, 41);
            lblDESC.TabIndex = 2;
            lblDESC.Text = "DESC";
            lblDESC.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblMANUF
            // 
            lblMANUF.AutoSize = true;
            lblMANUF.Dock = DockStyle.Fill;
            lblMANUF.Location = new Point(3, 164);
            lblMANUF.Name = "lblMANUF";
            lblMANUF.Size = new Size(77, 44);
            lblMANUF.TabIndex = 2;
            lblMANUF.Text = "MANUF";
            lblMANUF.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnINSERTlogpart
            // 
            btnINSERTlogpart.Dock = DockStyle.Fill;
            btnINSERTlogpart.Location = new Point(169, 167);
            btnINSERTlogpart.Name = "btnINSERTlogpart";
            btnINSERTlogpart.Size = new Size(67, 38);
            btnINSERTlogpart.TabIndex = 3;
            btnINSERTlogpart.Text = "INSERT";
            btnINSERTlogpart.UseVisualStyleBackColor = true;
            btnINSERTlogpart.Click += btnINSERTlogpart_Click;
            // 
            // txtbIPN
            // 
            txtbIPN.Dock = DockStyle.Fill;
            txtbIPN.Location = new Point(86, 44);
            txtbIPN.Multiline = true;
            txtbIPN.Name = "txtbIPN";
            txtbIPN.PlaceholderText = "paste IPN";
            txtbIPN.Size = new Size(77, 35);
            txtbIPN.TabIndex = 0;
            txtbIPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbDESC
            // 
            txtbDESC.Dock = DockStyle.Fill;
            txtbDESC.Location = new Point(86, 126);
            txtbDESC.Multiline = true;
            txtbDESC.Name = "txtbDESC";
            txtbDESC.PlaceholderText = "paste DESC";
            txtbDESC.Size = new Size(77, 35);
            txtbDESC.TabIndex = 1;
            txtbDESC.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbMNF
            // 
            txtbMNF.Dock = DockStyle.Fill;
            txtbMNF.Location = new Point(86, 167);
            txtbMNF.Multiline = true;
            txtbMNF.Name = "txtbMNF";
            txtbMNF.PlaceholderText = "paste MANUF";
            txtbMNF.Size = new Size(77, 38);
            txtbMNF.TabIndex = 1;
            txtbMNF.TextAlign = HorizontalAlignment.Center;
            // 
            // lblMFPN
            // 
            lblMFPN.AutoSize = true;
            lblMFPN.Dock = DockStyle.Fill;
            lblMFPN.Location = new Point(3, 82);
            lblMFPN.Name = "lblMFPN";
            lblMFPN.Size = new Size(77, 41);
            lblMFPN.TabIndex = 2;
            lblMFPN.Text = "MFPN";
            lblMFPN.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtbMFPN
            // 
            txtbMFPN.Dock = DockStyle.Fill;
            txtbMFPN.Location = new Point(86, 85);
            txtbMFPN.Multiline = true;
            txtbMFPN.Name = "txtbMFPN";
            txtbMFPN.PlaceholderText = "paste MFPN";
            txtbMFPN.Size = new Size(77, 35);
            txtbMFPN.TabIndex = 0;
            txtbMFPN.TextAlign = HorizontalAlignment.Center;
            // 
            // txtbBuffer
            // 
            txtbBuffer.Dock = DockStyle.Fill;
            txtbBuffer.Location = new Point(86, 3);
            txtbBuffer.Multiline = true;
            txtbBuffer.Name = "txtbBuffer";
            txtbBuffer.PlaceholderText = "paste mixed string";
            txtbBuffer.Size = new Size(77, 35);
            txtbBuffer.TabIndex = 4;
            txtbBuffer.TextAlign = HorizontalAlignment.Center;
            txtbBuffer.KeyDown += txtbBuffer_KeyDown;
            // 
            // btnClear
            // 
            btnClear.Dock = DockStyle.Fill;
            btnClear.Location = new Point(169, 3);
            btnClear.Name = "btnClear";
            btnClear.Size = new Size(67, 35);
            btnClear.TabIndex = 5;
            btnClear.Text = "Clear";
            btnClear.UseVisualStyleBackColor = true;
            btnClear.Click += btnClear_Click;
            // 
            // btnBULKinsert
            // 
            btnBULKinsert.Dock = DockStyle.Fill;
            btnBULKinsert.Location = new Point(3, 3);
            btnBULKinsert.Name = "btnBULKinsert";
            btnBULKinsert.Size = new Size(77, 35);
            btnBULKinsert.TabIndex = 6;
            btnBULKinsert.Text = "BULK";
            btnBULKinsert.UseVisualStyleBackColor = true;
            btnBULKinsert.Click += btnBULKinsert_Click;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(txtbPing);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(254, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(45, 40);
            groupBox5.TabIndex = 3;
            groupBox5.TabStop = false;
            groupBox5.Text = "tProc";
            // 
            // txtbPing
            // 
            txtbPing.Dock = DockStyle.Fill;
            txtbPing.Location = new Point(3, 19);
            txtbPing.Name = "txtbPing";
            txtbPing.ReadOnly = true;
            txtbPing.Size = new Size(39, 23);
            txtbPing.TabIndex = 0;
            txtbPing.TextAlign = HorizontalAlignment.Center;
            // 
            // txtLog
            // 
            txtLog.Dock = DockStyle.Fill;
            txtLog.Location = new Point(617, 3);
            txtLog.Name = "txtLog";
            txtLog.Size = new Size(301, 236);
            txtLog.TabIndex = 6;
            txtLog.Text = "";
            // 
            // FrmPriorityAPI
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1229, 659);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmPriorityAPI";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "PriorityAPI";
            WindowState = FormWindowState.Maximized;
            Load += FrmPriorityAPI_Load;
            tableLayoutPanel1.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            tableLayoutPanel6.ResumeLayout(false);
            tableLayoutPanel6.PerformLayout();
            tableLayoutPanel7.ResumeLayout(false);
            tableLayoutPanel7.PerformLayout();
            groupBox2.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView2).EndInit();
            tableLayoutPanel4.ResumeLayout(false);
            gbxINSERT.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            tableLayoutPanel5.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox5.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox1;
        private TableLayoutPanel tableLayoutPanel2;
        private TextBox txtbInputIPN;
        private TextBox txtbInputMFPN;
        private TextBox textBox3;
        private TextBox textBox4;
        private TextBox textBox5;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Button btnPrinSticker;
        private Button button2;
        private GroupBox groupBox2;
        private ComboBox comboBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private GroupBox groupBox3;
        private DataGridView dataGridView1;
        private TextBox txtbFilterIPN;
        private GroupBox groupBox4;
        private DataGridView dataGridView2;
        private GroupBox gbxINSERT;
        private Button btnINSERTlogpart;
        private Label lblDESC;
        private Label lblIPN;
        private TextBox txtbDESC;
        private TextBox txtbIPN;
        private Label lblMFPN;
        private TextBox txtbMFPN;
        private Label lblMANUF;
        private TextBox txtbMNF;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBox5;
        private TableLayoutPanel tableLayoutPanel5;
        private TextBox txtbPing;
        private TextBox txtbBuffer;
        private Button btnClear;
        private Button btnMFG;
        private TextBox txtbPART;
        private Label label6;
        private TextBox txtbWHSID;
        private RichTextBox txtLog;
        private RadioButton rbtMFG;
        private TableLayoutPanel tableLayoutPanel6;
        private RadioButton rbtIN;
        private RadioButton tbtOUT;
        private TextBox txtbIN;
        private TextBox txtbOUT;
        private ComboBox cmbPackCode;
        private RadioButton rbtFTK;
        private Button btnBULKinsert;
        private Button btnClearIpnFilter;
        private Button btnGetMFPNs;
        private TableLayoutPanel tableLayoutPanel7;
        private ComboBox cmbPreCode;
        private ComboBox cmbPostCode;
        private TextBox txtbDecoder;
    }
}