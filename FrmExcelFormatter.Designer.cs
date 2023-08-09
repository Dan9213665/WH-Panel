namespace WH_Panel
{
    partial class FrmExcelFormatter
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmExcelFormatter));
            groupBox1 = new GroupBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            btnGetSourceFile = new Button();
            groupBox2 = new GroupBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            dataGridView1 = new DataGridView();
            btnSetDataHeader = new Button();
            btnSaveAs = new Button();
            groupBox1.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            groupBox2.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(tableLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1407, 422);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Excel formatter";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(btnGetSourceFile, 0, 0);
            tableLayoutPanel1.Controls.Add(groupBox2, 0, 1);
            tableLayoutPanel1.Controls.Add(btnSetDataHeader, 1, 0);
            tableLayoutPanel1.Controls.Add(btnSaveAs, 2, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 90F));
            tableLayoutPanel1.Size = new Size(1401, 400);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // btnGetSourceFile
            // 
            btnGetSourceFile.BackgroundImage = Properties.Resources.documents_files_history_64;
            btnGetSourceFile.BackgroundImageLayout = ImageLayout.Zoom;
            btnGetSourceFile.Dock = DockStyle.Fill;
            btnGetSourceFile.Location = new Point(3, 3);
            btnGetSourceFile.Name = "btnGetSourceFile";
            btnGetSourceFile.Size = new Size(461, 34);
            btnGetSourceFile.TabIndex = 0;
            btnGetSourceFile.Text = "Get Source FIle";
            btnGetSourceFile.TextAlign = ContentAlignment.MiddleLeft;
            btnGetSourceFile.UseVisualStyleBackColor = true;
            btnGetSourceFile.Click += btnGetSourceFile_Click;
            // 
            // groupBox2
            // 
            tableLayoutPanel1.SetColumnSpan(groupBox2, 3);
            groupBox2.Controls.Add(tabControl1);
            groupBox2.Dock = DockStyle.Fill;
            groupBox2.Location = new Point(3, 43);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(1395, 354);
            groupBox2.TabIndex = 1;
            groupBox2.TabStop = false;
            groupBox2.Text = "File contents";
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(3, 19);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1389, 332);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(dataGridView1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(1381, 304);
            tabPage1.TabIndex = 0;
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToOrderColumns = true;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 3);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(1375, 298);
            dataGridView1.TabIndex = 0;
            dataGridView1.CellClick += dataGridView1_CellClick;
            // 
            // btnSetDataHeader
            // 
            btnSetDataHeader.BackgroundImage = (Image)resources.GetObject("btnSetDataHeader.BackgroundImage");
            btnSetDataHeader.BackgroundImageLayout = ImageLayout.Zoom;
            btnSetDataHeader.Dock = DockStyle.Fill;
            btnSetDataHeader.Location = new Point(470, 3);
            btnSetDataHeader.Name = "btnSetDataHeader";
            btnSetDataHeader.Size = new Size(461, 34);
            btnSetDataHeader.TabIndex = 2;
            btnSetDataHeader.Text = "Set Header";
            btnSetDataHeader.TextAlign = ContentAlignment.MiddleLeft;
            btnSetDataHeader.UseVisualStyleBackColor = true;
            btnSetDataHeader.Click += btnSetDataHeader_Click;
            // 
            // btnSaveAs
            // 
            btnSaveAs.BackgroundImage = (Image)resources.GetObject("btnSaveAs.BackgroundImage");
            btnSaveAs.BackgroundImageLayout = ImageLayout.Zoom;
            btnSaveAs.Dock = DockStyle.Fill;
            btnSaveAs.Location = new Point(937, 3);
            btnSaveAs.Name = "btnSaveAs";
            btnSaveAs.Size = new Size(461, 34);
            btnSaveAs.TabIndex = 3;
            btnSaveAs.Text = "Save XLSX";
            btnSaveAs.TextAlign = ContentAlignment.MiddleLeft;
            btnSaveAs.UseVisualStyleBackColor = true;
            btnSaveAs.Click += btnSaveAs_Click;
            // 
            // FrmExcelFormatter
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1407, 422);
            Controls.Add(groupBox1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FrmExcelFormatter";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "FrmExcelFormatter";
            WindowState = FormWindowState.Maximized;
            Load += FrmExcelFormatter_Load;
            groupBox1.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox groupBox1;
        private Button btnGetSourceFile;
        private GroupBox groupBox2;
        private DataGridView dataGridView1;
        private TableLayoutPanel tableLayoutPanel1;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnSetDataHeader;
        private Button btnSaveAs;
    }
}