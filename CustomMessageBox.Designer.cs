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
            btnSeven = new Button();
            btnBag = new Button();
            btn13 = new Button();
            btnStick = new Button();
            btnTray = new Button();
            SuspendLayout();
            // 
            // messageLabel
            // 
            messageLabel.AutoSize = true;
            messageLabel.Location = new Point(10, 90);
            messageLabel.Name = "messageLabel";
            messageLabel.Size = new Size(0, 15);
            messageLabel.TabIndex = 0;
            // 
            // btnUncount
            // 
            btnUncount.Location = new Point(12, 53);
            btnUncount.Name = "btnUncount";
            btnUncount.Size = new Size(99, 28);
            btnUncount.TabIndex = 1;
            btnUncount.Text = "UNCOUNT";
            btnUncount.UseVisualStyleBackColor = true;
            btnUncount.Click += btnUncount_Click;
            // 
            // btnDelete
            // 
            btnDelete.Location = new Point(189, 53);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new Size(102, 28);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "DELETE";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new Point(117, 53);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(66, 29);
            btnCancel.TabIndex = 3;
            btnCancel.Text = "CANCEL";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // btnSeven
            // 
            btnSeven.Location = new Point(12, 4);
            btnSeven.Name = "btnSeven";
            btnSeven.Size = new Size(43, 43);
            btnSeven.TabIndex = 4;
            btnSeven.Text = "7\"";
            btnSeven.UseVisualStyleBackColor = true;
            btnSeven.Click += btnSeven_Click;
            // 
            // btnBag
            // 
            btnBag.Location = new Point(68, 4);
            btnBag.Name = "btnBag";
            btnBag.Size = new Size(43, 43);
            btnBag.TabIndex = 4;
            btnBag.Text = "Bag";
            btnBag.UseVisualStyleBackColor = true;
            btnBag.Click += btnBag_Click;
            // 
            // btn13
            // 
            btn13.Location = new Point(126, 4);
            btn13.Name = "btn13";
            btn13.Size = new Size(43, 43);
            btn13.TabIndex = 4;
            btn13.Text = "13\"";
            btn13.UseVisualStyleBackColor = true;
            btn13.Click += btn13_Click;
            // 
            // btnStick
            // 
            btnStick.Location = new Point(189, 4);
            btnStick.Name = "btnStick";
            btnStick.Size = new Size(43, 43);
            btnStick.TabIndex = 4;
            btnStick.Text = "Stick";
            btnStick.UseVisualStyleBackColor = true;
            btnStick.Click += btnStick_Click;
            // 
            // btnTray
            // 
            btnTray.Location = new Point(246, 4);
            btnTray.Name = "btnTray";
            btnTray.Size = new Size(43, 43);
            btnTray.TabIndex = 4;
            btnTray.Text = "Tray";
            btnTray.UseVisualStyleBackColor = true;
            btnTray.Click += btnTray_Click;
            // 
            // CustomMessageBox
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(303, 267);
            ControlBox = false;
            Controls.Add(btnTray);
            Controls.Add(btnStick);
            Controls.Add(btn13);
            Controls.Add(btnBag);
            Controls.Add(btnSeven);
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

        private void btnSeven_Click(object sender, EventArgs e)
        {
            Result = DialogResult.OK; // Action for CANCEL
            this.Close();
        }

        private void btnBag_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Abort; // Action for CANCEL
            this.Close();
        }

        private void btn13_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Continue; // Action for CANCEL
            this.Close();
        }

        private void btnStick_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Ignore; // Action for CANCEL
            this.Close();
        }

        private void btnTray_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Retry; // Action for CANCEL
            this.Close();
        }

        private Button btnSeven;
        private Button btnBag;
        private Button btn13;
        private Button btnStick;
        private Button btnTray;
    }
}
