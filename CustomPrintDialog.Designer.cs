namespace WH_Panel
{
    partial class CustomPrintDialog
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
            tableLayoutPanel1 = new TableLayoutPanel();
            btn3 = new Button();
            btn2 = new Button();
            btn1 = new Button();
            btnFullKit = new Button();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 4;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutPanel1.Controls.Add(btn3, 2, 0);
            tableLayoutPanel1.Controls.Add(btn2, 1, 0);
            tableLayoutPanel1.Controls.Add(btn1, 0, 0);
            tableLayoutPanel1.Controls.Add(btnFullKit, 3, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 1;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Size = new Size(584, 121);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // btn3
            // 
            btn3.Dock = DockStyle.Fill;
            btn3.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point);
            btn3.Location = new Point(295, 3);
            btn3.Name = "btn3";
            btn3.Size = new Size(140, 115);
            btn3.TabIndex = 2;
            btn3.Text = "3";
            btn3.UseVisualStyleBackColor = true;
            btn3.Click += btn3_Click_1;
            // 
            // btn2
            // 
            btn2.Dock = DockStyle.Fill;
            btn2.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point);
            btn2.Location = new Point(149, 3);
            btn2.Name = "btn2";
            btn2.Size = new Size(140, 115);
            btn2.TabIndex = 1;
            btn2.Text = "2";
            btn2.UseVisualStyleBackColor = true;
            btn2.Click += btn2_Click_1;
            // 
            // btn1
            // 
            btn1.Dock = DockStyle.Fill;
            btn1.Font = new Font("Segoe UI", 26.25F, FontStyle.Bold, GraphicsUnit.Point);
            btn1.Location = new Point(3, 3);
            btn1.Name = "btn1";
            btn1.Size = new Size(140, 115);
            btn1.TabIndex = 0;
            btn1.Text = "1";
            btn1.UseVisualStyleBackColor = true;
            btn1.Click += btn1_Click_1;
            // 
            // btnFullKit
            // 
            btnFullKit.Dock = DockStyle.Fill;
            btnFullKit.Font = new Font("Segoe UI", 18F, FontStyle.Bold, GraphicsUnit.Point);
            btnFullKit.Location = new Point(441, 3);
            btnFullKit.Name = "btnFullKit";
            btnFullKit.Size = new Size(140, 115);
            btnFullKit.TabIndex = 3;
            btnFullKit.Text = "Full Kit Sticker";
            btnFullKit.UseVisualStyleBackColor = true;
            btnFullKit.Click += btnFullKit_Click;
            // 
            // CustomPrintDialog
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(584, 121);
            Controls.Add(tableLayoutPanel1);
            Name = "CustomPrintDialog";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Select QTY of copies";
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Button btn2;
        private Button btn1;
        private Button btn3;
        private Button btnFullKit;
    }
}