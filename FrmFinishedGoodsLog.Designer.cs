namespace WH_Panel
{
    partial class FrmFinishedGoodsLog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFinishedGoodsLog));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            tableLayoutPanel1 = new TableLayoutPanel();
            label3 = new Label();
            txtbSN = new TextBox();
            comboBox2 = new ComboBox();
            comboBox1 = new ComboBox();
            button1 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            checkBox1 = new CheckBox();
            label1 = new Label();
            flowLayoutPanel2 = new FlowLayoutPanel();
            checkBox2 = new CheckBox();
            label2 = new Label();
            comboBox3 = new ComboBox();
            flowLayoutPanel3 = new FlowLayoutPanel();
            checkBox3 = new CheckBox();
            label4 = new Label();
            groupBox2 = new GroupBox();
            dataGridView1 = new DataGridView();
            btbFinalizeShipment = new Button();
            groupBox3 = new GroupBox();
            flowLayoutPanel4 = new FlowLayoutPanel();
            checkBox4 = new CheckBox();
            txtbComments = new TextBox();
            lblCounter = new Label();
            tableLayoutPanel2 = new TableLayoutPanel();
            lblLimit = new Label();
            txtbSetLimit = new TextBox();
            checkBox5 = new CheckBox();
            groupBox1 = new GroupBox();
            tableLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox3.SuspendLayout();
            flowLayoutPanel4.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
            tableLayoutPanel1.Controls.Add(label3, 0, 9);
            tableLayoutPanel1.Controls.Add(txtbSN, 0, 10);
            tableLayoutPanel1.Controls.Add(comboBox2, 0, 3);
            tableLayoutPanel1.Controls.Add(comboBox1, 0, 1);
            tableLayoutPanel1.Controls.Add(button1, 0, 11);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel2, 0, 2);
            tableLayoutPanel1.Controls.Add(comboBox3, 0, 5);
            tableLayoutPanel1.Controls.Add(flowLayoutPanel3, 0, 4);
            tableLayoutPanel1.Controls.Add(groupBox2, 1, 1);
            tableLayoutPanel1.Controls.Add(btbFinalizeShipment, 1, 11);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 6);
            tableLayoutPanel1.Controls.Add(lblCounter, 1, 0);
            tableLayoutPanel1.Controls.Add(tableLayoutPanel2, 0, 8);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 12;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 8.333332F));
            tableLayoutPanel1.Size = new Size(1146, 709);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label3.Location = new Point(3, 531);
            label3.Name = "label3";
            label3.Size = new Size(223, 59);
            label3.TabIndex = 6;
            label3.Text = "Input S/N";
            label3.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtbSN
            // 
            txtbSN.Dock = DockStyle.Fill;
            txtbSN.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            txtbSN.Location = new Point(3, 593);
            txtbSN.Name = "txtbSN";
            txtbSN.Size = new Size(223, 39);
            txtbSN.TabIndex = 7;
            txtbSN.TextAlign = HorizontalAlignment.Center;
            txtbSN.KeyDown += txtbSN_KeyDown;
            // 
            // comboBox2
            // 
            comboBox2.Dock = DockStyle.Fill;
            comboBox2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox2.FormattingEnabled = true;
            comboBox2.Location = new Point(3, 180);
            comboBox2.Name = "comboBox2";
            comboBox2.Size = new Size(223, 40);
            comboBox2.TabIndex = 4;
            comboBox2.Text = "select project";
            comboBox2.SelectionChangeCommitted += comboBox2_SelectionChangeCommitted;
            // 
            // comboBox1
            // 
            comboBox1.Dock = DockStyle.Fill;
            comboBox1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(3, 62);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(223, 40);
            comboBox1.TabIndex = 1;
            comboBox1.Text = "select customer";
            comboBox1.SelectionChangeCommitted += comboBox1_SelectionChangeCommitted;
            // 
            // button1
            // 
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            button1.Location = new Point(3, 652);
            button1.Name = "button1";
            button1.Size = new Size(223, 54);
            button1.TabIndex = 8;
            button1.Text = "Add Item to shipment";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(checkBox1);
            flowLayoutPanel1.Controls.Add(label1);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(223, 53);
            flowLayoutPanel1.TabIndex = 9;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            checkBox1.Location = new Point(3, 3);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(108, 19);
            checkBox1.TabIndex = 2;
            checkBox1.Text = "Lock customer";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label1.Location = new Point(3, 25);
            label1.Name = "label1";
            label1.Size = new Size(149, 32);
            label1.TabIndex = 0;
            label1.Text = "CUSTOMER:";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(checkBox2);
            flowLayoutPanel2.Controls.Add(label2);
            flowLayoutPanel2.Dock = DockStyle.Fill;
            flowLayoutPanel2.Location = new Point(3, 121);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(223, 53);
            flowLayoutPanel2.TabIndex = 10;
            // 
            // checkBox2
            // 
            checkBox2.AutoSize = true;
            checkBox2.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            checkBox2.Location = new Point(3, 3);
            checkBox2.Name = "checkBox2";
            checkBox2.Size = new Size(95, 19);
            checkBox2.TabIndex = 5;
            checkBox2.Text = "Lock project";
            checkBox2.UseVisualStyleBackColor = true;
            checkBox2.CheckedChanged += checkBox2_CheckedChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            label2.Location = new Point(3, 25);
            label2.Name = "label2";
            label2.Size = new Size(123, 32);
            label2.TabIndex = 3;
            label2.Text = "PROJECT:";
            label2.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboBox3
            // 
            comboBox3.Dock = DockStyle.Fill;
            comboBox3.FormattingEnabled = true;
            comboBox3.Location = new Point(3, 298);
            comboBox3.Name = "comboBox3";
            comboBox3.Size = new Size(223, 40);
            comboBox3.TabIndex = 12;
            comboBox3.Text = "na";
            comboBox3.SelectionChangeCommitted += comboBox3_SelectionChangeCommitted;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.Controls.Add(checkBox3);
            flowLayoutPanel3.Controls.Add(label4);
            flowLayoutPanel3.Dock = DockStyle.Fill;
            flowLayoutPanel3.Location = new Point(3, 239);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(223, 53);
            flowLayoutPanel3.TabIndex = 13;
            // 
            // checkBox3
            // 
            checkBox3.AutoSize = true;
            checkBox3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            checkBox3.Location = new Point(3, 3);
            checkBox3.Name = "checkBox3";
            checkBox3.Size = new Size(102, 19);
            checkBox3.TabIndex = 12;
            checkBox3.Text = "Lock Revision";
            checkBox3.UseVisualStyleBackColor = true;
            checkBox3.CheckedChanged += checkBox3_CheckedChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 25);
            label4.Name = "label4";
            label4.Size = new Size(125, 32);
            label4.TabIndex = 11;
            label4.Text = "REVISION";
            label4.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 2);
            groupBox2.Controls.Add(dataGridView1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(232, 62);
            groupBox2.Name = "groupBox2";
            tableLayoutPanel1.SetRowSpan(groupBox2, 10);
            groupBox2.Size = new Size(911, 584);
            groupBox2.TabIndex = 14;
            groupBox2.TabStop = false;
            groupBox2.Text = "Packed items list";
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = SystemColors.Window;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.False;
            dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 35);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(905, 546);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellDoubleClick += dataGridView1_CellDoubleClick;
            // 
            // btbFinalizeShipment
            // 
            btbFinalizeShipment.BackgroundImage = Properties.Resources.box_21467;
            btbFinalizeShipment.BackgroundImageLayout = ImageLayout.Zoom;
            tableLayoutPanel1.SetColumnSpan(btbFinalizeShipment, 2);
            btbFinalizeShipment.Dock = DockStyle.Fill;
            btbFinalizeShipment.Location = new Point(232, 652);
            btbFinalizeShipment.Name = "btbFinalizeShipment";
            btbFinalizeShipment.Size = new Size(911, 54);
            btbFinalizeShipment.TabIndex = 16;
            btbFinalizeShipment.Text = "Finalize shipment";
            btbFinalizeShipment.TextAlign = ContentAlignment.MiddleRight;
            btbFinalizeShipment.UseVisualStyleBackColor = true;
            btbFinalizeShipment.Click += button2_Click;
            // 
            // groupBox3
            // 
            groupBox3.AutoSize = true;
            groupBox3.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            groupBox3.Controls.Add(flowLayoutPanel4);
            groupBox3.Dock = DockStyle.Fill;
            groupBox3.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            groupBox3.Location = new Point(3, 357);
            groupBox3.Name = "groupBox3";
            tableLayoutPanel1.SetRowSpan(groupBox3, 2);
            groupBox3.Size = new Size(223, 112);
            groupBox3.TabIndex = 17;
            groupBox3.TabStop = false;
            groupBox3.Text = "PO / Comments";
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.Controls.Add(checkBox4);
            flowLayoutPanel4.Controls.Add(txtbComments);
            flowLayoutPanel4.Dock = DockStyle.Fill;
            flowLayoutPanel4.Location = new Point(3, 19);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(217, 90);
            flowLayoutPanel4.TabIndex = 0;
            // 
            // checkBox4
            // 
            checkBox4.AutoSize = true;
            checkBox4.Location = new Point(3, 3);
            checkBox4.Name = "checkBox4";
            checkBox4.Size = new Size(136, 19);
            checkBox4.TabIndex = 1;
            checkBox4.Text = "Lock PO/Comments";
            checkBox4.UseVisualStyleBackColor = true;
            checkBox4.CheckedChanged += checkBox4_CheckedChanged;
            // 
            // txtbComments
            // 
            txtbComments.Dock = DockStyle.Fill;
            txtbComments.Font = new Font("Segoe UI Black", 12F, FontStyle.Bold, GraphicsUnit.Point);
            txtbComments.Location = new Point(3, 28);
            txtbComments.Name = "txtbComments";
            txtbComments.Size = new Size(365, 29);
            txtbComments.TabIndex = 0;
            txtbComments.TextAlign = HorizontalAlignment.Center;
            txtbComments.KeyDown += txtbComments_KeyDown;
            // 
            // lblCounter
            // 
            lblCounter.AutoSize = true;
            lblCounter.Dock = DockStyle.Fill;
            lblCounter.Location = new Point(232, 0);
            lblCounter.Name = "lblCounter";
            lblCounter.Size = new Size(452, 59);
            lblCounter.TabIndex = 15;
            lblCounter.Text = "QTY:";
            lblCounter.TextAlign = ContentAlignment.MiddleRight;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.AutoSize = true;
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            tableLayoutPanel2.Controls.Add(lblLimit, 0, 1);
            tableLayoutPanel2.Controls.Add(txtbSetLimit, 1, 0);
            tableLayoutPanel2.Controls.Add(checkBox5, 0, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(3, 475);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Size = new Size(223, 53);
            tableLayoutPanel2.TabIndex = 18;
            // 
            // lblLimit
            // 
            lblLimit.AutoSize = true;
            lblLimit.Dock = DockStyle.Fill;
            lblLimit.Location = new Point(3, 26);
            lblLimit.Name = "lblLimit";
            lblLimit.Size = new Size(125, 27);
            lblLimit.TabIndex = 0;
            lblLimit.Text = "SET Limit:";
            lblLimit.TextAlign = ContentAlignment.MiddleLeft;
            lblLimit.DoubleClick += lblLimit_DoubleClick;
            // 
            // txtbSetLimit
            // 
            txtbSetLimit.Dock = DockStyle.Fill;
            txtbSetLimit.Location = new Point(134, 3);
            txtbSetLimit.Name = "txtbSetLimit";
            tableLayoutPanel2.SetRowSpan(txtbSetLimit, 2);
            txtbSetLimit.Size = new Size(238, 39);
            txtbSetLimit.TabIndex = 1;
            txtbSetLimit.TextAlign = HorizontalAlignment.Center;
            txtbSetLimit.KeyDown += textBox1_KeyDown;
            // 
            // checkBox5
            // 
            checkBox5.AutoSize = true;
            checkBox5.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point);
            checkBox5.Location = new Point(3, 3);
            checkBox5.Name = "checkBox5";
            checkBox5.Size = new Size(83, 19);
            checkBox5.TabIndex = 2;
            checkBox5.Text = "Lock Limit";
            checkBox5.UseVisualStyleBackColor = true;
            checkBox5.CheckedChanged += checkBox5_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1152, 731);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Finished Goods Logger";
            // 
            // FrmFinishedGoodsLog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1152, 731);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmFinishedGoodsLog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Finished Goods Logger";
            WindowState = FormWindowState.Maximized;
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            flowLayoutPanel2.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            flowLayoutPanel3.PerformLayout();
            groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox3.ResumeLayout(false);
            flowLayoutPanel4.ResumeLayout(false);
            flowLayoutPanel4.PerformLayout();
            tableLayoutPanel2.ResumeLayout(false);
            tableLayoutPanel2.PerformLayout();
            groupBox1.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
        private TableLayoutPanel tableLayoutPanel1;
        private Label label1;
        private ComboBox comboBox1;
        private GroupBox groupBox1;
        private CheckBox checkBox1;
        private Label label2;
        private ComboBox comboBox2;
        private CheckBox checkBox2;
        private Label label3;
        private TextBox txtbSN;
        private Button button1;
        private FlowLayoutPanel flowLayoutPanel1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label4;
        private ComboBox comboBox3;
        private FlowLayoutPanel flowLayoutPanel3;
        private CheckBox checkBox3;
        private GroupBox groupBox2;
        private DataGridView dataGridView1;
        private Label lblCounter;
        private Button btbFinalizeShipment;
        private GroupBox groupBox3;
        private TextBox txtbComments;
        private FlowLayoutPanel flowLayoutPanel4;
        private CheckBox checkBox4;
        private TableLayoutPanel tableLayoutPanel2;
        private Label lblLimit;
        private TextBox txtbSetLimit;
        private CheckBox checkBox5;
    }
}