namespace WH_Panel
{
    partial class FrmIPNgenerator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmIPNgenerator));
            tableLayoutPanel1 = new TableLayoutPanel();
            textBox3 = new TextBox();
            groupBox5 = new GroupBox();
            richTextBox1 = new RichTextBox();
            groupBox4 = new GroupBox();
            textBox2 = new TextBox();
            groupBox3 = new GroupBox();
            comboBox2 = new ComboBox();
            groupBox2 = new GroupBox();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            comboBox1 = new ComboBox();
            button1 = new Button();
            checkBox1 = new CheckBox();
            groupBox6 = new GroupBox();
            tableLayoutPanel2 = new TableLayoutPanel();
            rtb_DKAPIinput = new RichTextBox();
            button2 = new Button();
            comboBox3 = new ComboBox();
            comboBox4 = new ComboBox();
            rtb_out = new RichTextBox();
            rtb_log = new RichTextBox();
            tableLayoutPanel1.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox6.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 5;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.Controls.Add(textBox3, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox5, 3, 0);
            tableLayoutPanel1.Controls.Add(groupBox4, 2, 0);
            tableLayoutPanel1.Controls.Add(groupBox3, 4, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 0);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(button1, 4, 1);
            tableLayoutPanel1.Controls.Add(checkBox1, 0, 2);
            tableLayoutPanel1.Controls.Add(groupBox6, 0, 3);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 4;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.31579F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 26.31579F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10.5263157F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 36.8421059F));
            tableLayoutPanel1.Size = new Size(1233, 508);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // textBox3
            // 
            tableLayoutPanel1.SetColumnSpan(textBox3, 4);
            textBox3.Dock = DockStyle.Fill;
            textBox3.Font = new Font("Segoe UI", 36F);
            textBox3.Location = new Point(3, 136);
            textBox3.Name = "textBox3";
            textBox3.ReadOnly = true;
            textBox3.Size = new Size(978, 71);
            textBox3.TabIndex = 5;
            textBox3.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(richTextBox1);
            groupBox5.Dock = DockStyle.Fill;
            groupBox5.Location = new Point(741, 3);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(240, 127);
            groupBox5.TabIndex = 4;
            groupBox5.TabStop = false;
            groupBox5.Text = "Description";
            // 
            // richTextBox1
            // 
            richTextBox1.Dock = DockStyle.Fill;
            richTextBox1.Font = new Font("Segoe UI", 18F);
            richTextBox1.Location = new Point(3, 19);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(234, 105);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = "";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(textBox2);
            groupBox4.Dock = DockStyle.Fill;
            groupBox4.Location = new Point(495, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(240, 127);
            groupBox4.TabIndex = 3;
            groupBox4.TabStop = false;
            groupBox4.Text = "MFPN";
            // 
            // textBox2
            // 
            textBox2.Dock = DockStyle.Fill;
            textBox2.Font = new Font("Segoe UI", 18F);
            textBox2.Location = new Point(3, 19);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(234, 39);
            textBox2.TabIndex = 1;
            textBox2.TextAlign = HorizontalAlignment.Center;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBox2);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Location = new Point(987, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(243, 127);
            groupBox3.TabIndex = 2;
            groupBox3.TabStop = false;
            groupBox3.Text = "Manufacturer";
            // 
            // comboBox2
            // 
            comboBox2.AutoCompleteMode = AutoCompleteMode.Suggest;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.Font = new Font("Segoe UI", 18F);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(3, 19);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(237, 40);
            comboBox2.TabIndex = 1;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBox1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(249, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(240, 127);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "Next Available Number";
            // 
            // textBox1
            // 
            textBox1.Dock = DockStyle.Fill;
            textBox1.Font = new Font("Segoe UI", 18F);
            textBox1.Location = new Point(3, 19);
            textBox1.Name = "textBox1";
            textBox1.ReadOnly = true;
            textBox1.Size = new Size(234, 39);
            textBox1.TabIndex = 0;
            textBox1.TextAlign = HorizontalAlignment.Center;
            textBox1.KeyUp += textBox1_KeyUp;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(comboBox1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(3, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(240, 127);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Type";
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.Font = new Font("Segoe UI", 18F);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 19);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(234, 40);
            comboBox1.TabIndex = 0;
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged_1;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(987, 136);
            button1.Name = "button1";
            tableLayoutPanel1.SetRowSpan(button1, 2);
            button1.Size = new Size(243, 180);
            button1.TabIndex = 6;
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(3, 269);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(95, 19);
            checkBox1.TabIndex = 7;
            checkBox1.Text = "CUSTOM IPN";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // groupBox6
            // 
            groupBox6.AutoSize = true;
            tableLayoutPanel1.SetColumnSpan(groupBox6, 5);
            groupBox6.Controls.Add(tableLayoutPanel2);
            groupBox6.Dock = DockStyle.Fill;
            groupBox6.Location = new Point(3, 322);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(1227, 183);
            groupBox6.TabIndex = 8;
            groupBox6.TabStop = false;
            groupBox6.Text = "DKapi";
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 5;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel2.Controls.Add(rtb_DKAPIinput, 1, 0);
            tableLayoutPanel2.Controls.Add(button2, 4, 0);
            tableLayoutPanel2.Controls.Add(comboBox3, 0, 0);
            tableLayoutPanel2.Controls.Add(comboBox4, 2, 0);
            tableLayoutPanel2.Controls.Add(rtb_out, 1, 1);
            tableLayoutPanel2.Controls.Add(rtb_log, 3, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 19);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(1221, 161);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // rtb_DKAPIinput
            // 
            rtb_DKAPIinput.BackColor = SystemColors.InfoText;
            rtb_DKAPIinput.Dock = DockStyle.Fill;
            rtb_DKAPIinput.Location = new Point(247, 3);
            rtb_DKAPIinput.Name = "rtb_DKAPIinput";
            rtb_DKAPIinput.Size = new Size(238, 74);
            rtb_DKAPIinput.TabIndex = 5;
            rtb_DKAPIinput.Text = "";
            rtb_DKAPIinput.KeyDown += richTextBox5_KeyDown;
            // 
            // button2
            // 
            button2.BackColor = SystemColors.ActiveCaptionText;
            button2.BackgroundImage = (Image)resources.GetObject("button2.BackgroundImage");
            button2.BackgroundImageLayout = ImageLayout.Stretch;
            button2.Dock = DockStyle.Fill;
            button2.Location = new Point(979, 3);
            button2.Name = "button2";
            tableLayoutPanel2.SetRowSpan(button2, 2);
            button2.Size = new Size(239, 155);
            button2.TabIndex = 6;
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // comboBox3
            // 
            comboBox3.Dock = DockStyle.Fill;
            comboBox3.FormattingEnabled = true;
            comboBox3.Items.AddRange(new object[] { "1P", "6P1P", "pm:" });
            comboBox3.Location = new Point(3, 3);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(238, 23);
            comboBox3.TabIndex = 7;
            // 
            // comboBox4
            // 
            comboBox4.Dock = DockStyle.Fill;
            comboBox4.FormattingEnabled = true;
            comboBox4.Items.AddRange(new object[] { "30P", "K1K", "6P2", ",qty:" });
            comboBox4.Location = new Point(491, 3);
            comboBox4.Name = "comboBox4";
            comboBox4.Size = new Size(238, 23);
            comboBox4.TabIndex = 8;
            // 
            // rtb_out
            // 
            rtb_out.BackColor = SystemColors.InfoText;
            rtb_out.Dock = DockStyle.Fill;
            rtb_out.Location = new Point(247, 83);
            rtb_out.Name = "rtb_out";
            rtb_out.Size = new Size(238, 75);
            rtb_out.TabIndex = 9;
            rtb_out.Text = "";
            // 
            // rtb_log
            // 
            rtb_log.BackColor = SystemColors.InfoText;
            rtb_log.Dock = DockStyle.Fill;
            rtb_log.ForeColor = Color.Lime;
            rtb_log.Location = new Point(735, 3);
            rtb_log.Name = "rtb_log";
            tableLayoutPanel2.SetRowSpan(rtb_log, 2);
            rtb_log.Size = new Size(238, 155);
            rtb_log.TabIndex = 10;
            rtb_log.Text = "";
            // 
            // FrmIPNgenerator
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1233, 508);
            Controls.Add(tableLayoutPanel1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmIPNgenerator";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmIPNgenerator";
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            groupBox5.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private GroupBox groupBox5;
        private GroupBox groupBox4;
        private GroupBox groupBox3;
        private GroupBox groupBox2;
        private GroupBox groupBox1;
        private RichTextBox richTextBox1;
        private TextBox textBox2;
        private ComboBox comboBox2;
        private TextBox textBox1;
        private ComboBox comboBox1;
        private TextBox textBox3;
        private Button button1;
        private CheckBox checkBox1;
        private GroupBox groupBox6;
        private TableLayoutPanel tableLayoutPanel2;
        private RichTextBox rtb_DKAPIinput;
        private Button button2;
        private ComboBox comboBox3;
        private ComboBox comboBox4;
        private RichTextBox rtb_out;
        private RichTextBox rtb_log;
    }
}