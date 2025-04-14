namespace WH_Panel
{
    partial class WarningAvlDialogForm
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
            lblMessage = new Label();
            pictureBox1 = new PictureBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            pictureBox2Yes = new PictureBox();
            pictureBoxNo = new PictureBox();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2Yes).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNo).BeginInit();
            SuspendLayout();
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Fill;
            lblMessage.Location = new Point(204, 470);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(195, 111);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "label1";
            // 
            // pictureBox1
            // 
            tableLayoutPanel1.SetColumnSpan(pictureBox1, 3);
            pictureBox1.Dock = DockStyle.Fill;
            pictureBox1.Location = new Point(3, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(598, 464);
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 3;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.3333321F));
            tableLayoutPanel1.Controls.Add(pictureBox1, 0, 0);
            tableLayoutPanel1.Controls.Add(lblMessage, 1, 1);
            tableLayoutPanel1.Controls.Add(pictureBox2Yes, 0, 1);
            tableLayoutPanel1.Controls.Add(pictureBoxNo, 2, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 111F));
            tableLayoutPanel1.Size = new Size(604, 581);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // pictureBox2Yes
            // 
            pictureBox2Yes.Dock = DockStyle.Fill;
            pictureBox2Yes.Location = new Point(3, 473);
            pictureBox2Yes.Name = "pictureBox2Yes";
            pictureBox2Yes.Size = new Size(195, 105);
            pictureBox2Yes.TabIndex = 4;
            pictureBox2Yes.TabStop = false;
            pictureBox2Yes.Click += pictureBox2Yes_Click;
            // 
            // pictureBoxNo
            // 
            pictureBoxNo.Dock = DockStyle.Fill;
            pictureBoxNo.Location = new Point(405, 473);
            pictureBoxNo.Name = "pictureBoxNo";
            pictureBoxNo.Size = new Size(196, 105);
            pictureBoxNo.TabIndex = 5;
            pictureBoxNo.TabStop = false;
            pictureBoxNo.Click += pictureBoxNo_Click;
            // 
            // WarningAvlDialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(604, 581);
            Controls.Add(tableLayoutPanel1);
            Name = "WarningAvlDialogForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "WarningAvlDialogForm";
            Load += WarningAvlDialogForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            tableLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2Yes).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxNo).EndInit();
            ResumeLayout(false);
        }
        #endregion
        private Label lblMessage;
        private PictureBox pictureBox1;
        private TableLayoutPanel tableLayoutPanel1;
        private PictureBox pictureBox2Yes;
        private PictureBox pictureBoxNo;
    }
}