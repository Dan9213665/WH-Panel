namespace WH_Panel
{
    partial class FrmStockCounter
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
            comboBox3 = new ComboBox();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            groupBox3 = new GroupBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            groupBox4 = new GroupBox();
            groupBox5 = new GroupBox();
            dataGridView1 = new DataGridView();
            tableLayoutPanel1 = new TableLayoutPanel();
            checkBox1 = new CheckBox();
            groupBox6 = new GroupBox();
            comboBox1 = new ComboBox();
            btnLoadReport = new Button();
            button2 = new Button();
            lblCalc = new Label();
            button3 = new Button();
            openFileDialog1 = new OpenFileDialog();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            groupBox6.SuspendLayout();
            SuspendLayout();
            // 
            // comboBox3
            // 
            comboBox3.BackColor = Color.DarkOrange;
            comboBox3.Cursor = Cursors.Hand;
            comboBox3.Dock = DockStyle.Fill;
            comboBox3.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox3.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboBox3.ForeColor = Color.White;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(3, 19);
            comboBox3.Name = "comboBox3";
            comboBox3.RightToLeft = RightToLeft.No;
            comboBox3.Size = new Size(240, 38);
            comboBox3.Sorted = true;
            comboBox3.TabIndex = 8;
            comboBox3.SelectedIndexChanged += comboBox3_SelectedIndexChanged;
            comboBox3.MouseClick += comboBox3_MouseClick;
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI", 12F);
            textBox1.Location = new Point(3, 19);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(240, 29);
            textBox1.TabIndex = 9;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.Enter += textBox1_Enter;
            textBox1.KeyDown += textBox1_KeyDown;
            textBox1.Leave += textBox1_Leave;
            textBox1.MouseDoubleClick += textBox1_MouseDoubleClick;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox3);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 60);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(246, 51);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            groupBox1.Text = "Warehouse selector";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(255, 60);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(246, 51);
            groupBox2.TabIndex = 11;
            groupBox2.TabStop = false;
            groupBox2.Text = "IPN";
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(textBox2);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(507, 60);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(246, 51);
            groupBox3.TabIndex = 12;
            groupBox3.TabStop = false;
            groupBox3.Text = "MFPN";
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Font = new Font("Segoe UI", 12F);
            textBox2.Location = new Point(3, 19);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(240, 29);
            textBox2.TabIndex = 0;
            textBox2.TextAlign = HorizontalAlignment.Center;
            textBox2.Enter += textBox2_Enter;
            textBox2.KeyDown += textBox2_KeyDown;
            textBox2.Leave += textBox2_Leave;
            // 
            // textBox3
            // 
            textBox3.Dock = DockStyle.Fill;
            textBox3.Font = new Font("Segoe UI", 12F);
            textBox3.Location = new Point(3, 19);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(240, 29);
            textBox3.TabIndex = 13;
            textBox3.TextAlign = HorizontalAlignment.Center;
            textBox3.Enter += textBox3_Enter;
            textBox3.KeyDown += textBox3_KeyDown;
            textBox3.Leave += textBox3_Leave;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox3);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(759, 60);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(246, 51);
            groupBox4.TabIndex = 14;
            groupBox4.TabStop = false;
            groupBox4.Text = "Qty";
            // 
            // groupBox5
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox5, 5);
            groupBox5.Controls.Add(dataGridView1);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(3, 117);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(1254, 458);
            groupBox5.TabIndex = 15;
            groupBox5.TabStop = false;
            groupBox5.Text = "DB";
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 19);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.Size = new Size(1248, 436);
            dataGridView1.TabIndex = 0;
            dataGridView1.MouseDown += dataGridView1_MouseDown;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(groupBox5, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox4, 3, 1);
            tableLayoutPanel1.Controls.Add(groupBox3, 2, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 1);
            tableLayoutPanel1.Controls.Add(checkBox1, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox6, 4, 1);
            tableLayoutPanel1.Controls.Add(btnLoadReport, 0, 0);
            tableLayoutPanel1.Controls.Add(button2, 2, 0);
            tableLayoutPanel1.Controls.Add(lblCalc, 3, 0);
            tableLayoutPanel1.Controls.Add(button3, 4, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel1.Size = new Size(1260, 578);
            tableLayoutPanel1.TabIndex = 17;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Dock = DockStyle.Fill;
            checkBox1.Location = new Point(255, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(246, 51);
            checkBox1.TabIndex = 16;
            checkBox1.Text = "Display In stock only";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(comboBox1);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(1011, 60);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(246, 51);
            groupBox6.TabIndex = 17;
            groupBox6.TabStop = false;
            groupBox6.Text = "Package";
            // 
            // comboBox1
            // 
            comboBox1.AutoCompleteCustomSource.AddRange(new string[] { "5\"", "7\"", "10\"", "13\"", "15\"", "Bag", "Box", "Stick", "Stick_in_a_bag", "Tray" });
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBox1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            comboBox1.FormattingEnabled = true;
            comboBox1.Items.AddRange(new object[] { "5\"", "7\"", "10\"", "13\"", "15\"", "Bag", "Box", "Stick", "Stick_in_a_bag", "Tray" });
            comboBox1.Location = new Point(3, 19);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(240, 38);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
            // 
            // btnLoadReport
            // 
            btnLoadReport.BackgroundImage = Properties.Resources.reportBtn;
            btnLoadReport.BackgroundImageLayout = ImageLayout.Stretch;
            btnLoadReport.Dock = DockStyle.Fill;
            btnLoadReport.Location = new Point(3, 3);
            btnLoadReport.Name = "btnLoadReport";
            btnLoadReport.Size = new Size(246, 51);
            btnLoadReport.TabIndex = 18;
            btnLoadReport.UseVisualStyleBackColor = false;
            btnLoadReport.Click += btnLoadReport_Click;
            btnLoadReport.MouseDown += button1_MouseDown;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.Screenshot_2025_02_18_125408;
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Dock = DockStyle.Fill;
            button2.Font = new Font("Segoe UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            button2.ForeColor = Color.White;
            button2.Location = new Point(507, 3);
            button2.Name = "button2";
            button2.Size = new Size(246, 51);
            button2.TabIndex = 19;
            button2.Text = "Recalculate Balance";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // lblCalc
            // 
            lblCalc.AutoSize = true;
            lblCalc.Dock = DockStyle.Fill;
            lblCalc.Font = new Font("Segoe UI", 14.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblCalc.Location = new Point(759, 0);
            lblCalc.Name = "lblCalc";
            lblCalc.Size = new Size(246, 57);
            lblCalc.TabIndex = 20;
            lblCalc.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // button3
            // 
            button3.BackgroundImage = Properties.Resources.openWH;
            button3.BackgroundImageLayout = ImageLayout.Stretch;
            button3.Dock = DockStyle.Fill;
            button3.Location = new Point(1011, 3);
            button3.Name = "button3";
            button3.Size = new Size(246, 51);
            button3.TabIndex = 21;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // FrmStockCounter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1260, 578);
            Controls.Add(tableLayoutPanel1);
            Name = "FrmStockCounter";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmStockCounter";
            WindowState = FormWindowState.Maximized;
            Load += FrmStockCounter_Load;
            groupBox1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox6.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ComboBox comboBox3;
        private TextBox textBox1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private GroupBox groupBox3;
        private TextBox textBox2;
        private TextBox textBox3;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel1;
        private CheckBox checkBox1;
        private GroupBox groupBox6;
        private ComboBox comboBox1;
        private Button btnLoadReport;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private Label lblCalc;
        private Button button3;
    }
}