namespace WH_Panel
{
    partial class CustomMessageBox : Form
    {
        private System.ComponentModel.IContainer components = null;
        private Label messageLabel;
        private Button btnUncount;
        private Button btnDelete;
        private Button btnCancel;

        public DialogResult Result { get; private set; }

        public CustomMessageBox(string message)
        {
            InitializeComponent();
            messageLabel.Text = message;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            messageLabel = new Label();
            btnUncount = new Button();
            btnDelete = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(10, 60);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(0, 15);
            messageLabel.TabIndex = 0;
            // 
            // btnUncount
            // 
            btnUncount.Location = new Point(10, 12);
            btnUncount.Name = "btnUncount";
            btnUncount.Size = new Size(99, 28);
            btnUncount.TabIndex = 1;
            btnUncount.Text = "UNCOUNT";
            btnUncount.UseVisualStyleBackColor = true;
            btnUncount.Click += btnUncount_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(189, 12);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(102, 28);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "DELETE";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(117, 12);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(66, 49);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "CANCEL";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // CustomMessageBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(301, 251);
            ControlBox = false;
            Controls.Add(messageLabel);
            Controls.Add(btnUncount);
            Controls.Add(btnDelete);
            Controls.Add(btnCancel);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Name = "CustomMessageBox";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Confirm Action";
            Load += CustomMessageBox_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private void btnUncount_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Yes; // Custom action for UNCOUNT
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Result = DialogResult.No; // Custom action for DELETE
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel; // Action for CANCEL
            this.Close();
        }
    }
}
