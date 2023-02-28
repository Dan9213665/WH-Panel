namespace WH_Panel
{
    partial class frmkitLabelPrint
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmkitLabelPrint));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnBrowseToFile = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnPrintKitLabel = new System.Windows.Forms.Button();
            this.txtbPasteCPQ = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnBrowseToFile);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.btnPrintKitLabel);
            this.groupBox1.Controls.Add(this.txtbPasteCPQ);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 113);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Browse to file or Paste CUSTOMER_PN_QTY";
            // 
            // btnBrowseToFile
            // 
            this.btnBrowseToFile.Location = new System.Drawing.Point(20, 34);
            this.btnBrowseToFile.Name = "btnBrowseToFile";
            this.btnBrowseToFile.Size = new System.Drawing.Size(75, 23);
            this.btnBrowseToFile.TabIndex = 2;
            this.btnBrowseToFile.Text = "Browse";
            this.btnBrowseToFile.UseVisualStyleBackColor = true;
            this.btnBrowseToFile.Click += new System.EventHandler(this.btnBrowseToFile_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(440, 63);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(46, 38);
            this.button2.TabIndex = 1;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnPrintKitLabel
            // 
            this.btnPrintKitLabel.Image = global::WH_Panel.Properties.Resources.sendtoprinter;
            this.btnPrintKitLabel.Location = new System.Drawing.Point(243, 63);
            this.btnPrintKitLabel.Name = "btnPrintKitLabel";
            this.btnPrintKitLabel.Size = new System.Drawing.Size(51, 38);
            this.btnPrintKitLabel.TabIndex = 1;
            this.btnPrintKitLabel.Text = "Print";
            this.btnPrintKitLabel.UseVisualStyleBackColor = true;
            this.btnPrintKitLabel.Click += new System.EventHandler(this.btnPrintKitLabel_Click);
            // 
            // txtbPasteCPQ
            // 
            this.txtbPasteCPQ.Location = new System.Drawing.Point(101, 34);
            this.txtbPasteCPQ.Name = "txtbPasteCPQ";
            this.txtbPasteCPQ.Size = new System.Drawing.Size(385, 23);
            this.txtbPasteCPQ.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // frmkitLabelPrint
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 113);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmkitLabelPrint";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Kit label print";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
        }
        #endregion
        private GroupBox groupBox1;
        private TextBox txtbPasteCPQ;
        private Button button2;
        private Button btnPrintKitLabel;
        private Button btnBrowseToFile;
        private OpenFileDialog openFileDialog1;
    }
}