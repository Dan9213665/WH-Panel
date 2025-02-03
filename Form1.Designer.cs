using System.Windows.Forms;
namespace WH_Panel
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;
        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            groupBox1 = new GroupBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            groupBox4 = new GroupBox();
            button4 = new Button();
            btnWorkProgramm = new Button();
            groupBox6 = new GroupBox();
            button1 = new Button();
            groupBox9 = new GroupBox();
            button14 = new Button();
            groupBox2 = new GroupBox();
            button15 = new Button();
            button9 = new Button();
            groupBox5 = new GroupBox();
            button16 = new Button();
            groupBox10 = new GroupBox();
            button10 = new Button();
            button6 = new Button();
            button11 = new Button();
            gpbxPMB = new GroupBox();
            btnFrmPMB = new Button();
            groupBox7 = new GroupBox();
            button12 = new Button();
            button5 = new Button();
            btnMFPN = new Button();
            button3 = new Button();
            groupBox3 = new GroupBox();
            button13 = new Button();
            button8 = new Button();
            groupBox11 = new GroupBox();
            button7 = new Button();
            button2 = new Button();
            notifyIcon1 = new NotifyIcon(components);
            groupBox1.SuspendLayout();
            flowLayoutPanel1.SuspendLayout();
            groupBox4.SuspendLayout();
            groupBox6.SuspendLayout();
            groupBox9.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox5.SuspendLayout();
            groupBox10.SuspendLayout();
            gpbxPMB.SuspendLayout();
            groupBox7.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox11.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.BackgroundImageLayout = ImageLayout.Zoom;
            groupBox1.Controls.Add(flowLayoutPanel1);
            groupBox1.Dock = DockStyle.Fill;
            groupBox1.ForeColor = SystemColors.ControlLightLight;
            groupBox1.Location = new Point(0, 0);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(574, 285);
            groupBox1.TabIndex = 1;
            groupBox1.TabStop = false;
            groupBox1.Text = "Elige scopum";
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoSize = true;
            flowLayoutPanel1.Controls.Add(groupBox4);
            flowLayoutPanel1.Controls.Add(groupBox6);
            flowLayoutPanel1.Controls.Add(groupBox9);
            flowLayoutPanel1.Controls.Add(groupBox5);
            flowLayoutPanel1.Controls.Add(groupBox10);
            flowLayoutPanel1.Controls.Add(gpbxPMB);
            flowLayoutPanel1.Controls.Add(groupBox7);
            flowLayoutPanel1.Controls.Add(groupBox3);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(3, 19);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(568, 263);
            flowLayoutPanel1.TabIndex = 2;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(button4);
            groupBox4.Controls.Add(btnWorkProgramm);
            groupBox4.ForeColor = Color.White;
            groupBox4.Location = new Point(3, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new Size(132, 127);
            groupBox4.TabIndex = 25;
            groupBox4.TabStop = false;
            groupBox4.Text = "Work Program";
            // 
            // button4
            // 
            button4.BackColor = Color.Transparent;
            button4.BackgroundImage = (Image)resources.GetObject("button4.BackgroundImage");
            button4.BackgroundImageLayout = ImageLayout.Zoom;
            button4.Location = new Point(82, 78);
            button4.Name = "button4";
            button4.Size = new Size(44, 41);
            button4.TabIndex = 12;
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click_1;
            // 
            // btnWorkProgramm
            // 
            btnWorkProgramm.BackgroundImage = Properties.Resources._6_2;
            btnWorkProgramm.BackgroundImageLayout = ImageLayout.Stretch;
            btnWorkProgramm.Cursor = Cursors.Hand;
            btnWorkProgramm.Dock = DockStyle.Fill;
            btnWorkProgramm.Location = new Point(3, 19);
            btnWorkProgramm.Name = "btnWorkProgramm";
            btnWorkProgramm.Size = new Size(126, 105);
            btnWorkProgramm.TabIndex = 3;
            btnWorkProgramm.UseVisualStyleBackColor = true;
            btnWorkProgramm.Click += btnWorkProgramm_Click;
            // 
            // groupBox6
            // 
            groupBox6.Controls.Add(button1);
            groupBox6.ForeColor = Color.White;
            groupBox6.Location = new Point(3, 136);
            groupBox6.Name = "groupBox6";
            groupBox6.Size = new Size(132, 123);
            groupBox6.TabIndex = 18;
            groupBox6.TabStop = false;
            groupBox6.Text = "Priority WH";
            // 
            // button1
            // 
            button1.BackColor = Color.Black;
            button1.BackgroundImage = (Image)resources.GetObject("button1.BackgroundImage");
            button1.BackgroundImageLayout = ImageLayout.Zoom;
            button1.Dock = DockStyle.Fill;
            button1.Location = new Point(3, 19);
            button1.Name = "button1";
            button1.Size = new Size(126, 101);
            button1.TabIndex = 7;
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click_2;
            // 
            // groupBox9
            // 
            groupBox9.Controls.Add(button14);
            groupBox9.Controls.Add(groupBox2);
            groupBox9.ForeColor = Color.White;
            groupBox9.Location = new Point(141, 3);
            groupBox9.Name = "groupBox9";
            groupBox9.Size = new Size(134, 125);
            groupBox9.TabIndex = 21;
            groupBox9.TabStop = false;
            groupBox9.Text = "AGNOSTIC WH";
            // 
            // button14
            // 
            button14.BackgroundImage = (Image)resources.GetObject("button14.BackgroundImage");
            button14.BackgroundImageLayout = ImageLayout.Zoom;
            button14.Dock = DockStyle.Fill;
            button14.Location = new Point(3, 19);
            button14.Name = "button14";
            button14.Size = new Size(128, 103);
            button14.TabIndex = 12;
            button14.UseVisualStyleBackColor = true;
            button14.Click += button14_Click_1;
            button14.MouseDown += button14_MouseClick;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(button15);
            groupBox2.Controls.Add(button9);
            groupBox2.ForeColor = Color.White;
            groupBox2.Location = new Point(57, 55);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(62, 53);
            groupBox2.TabIndex = 24;
            groupBox2.TabStop = false;
            groupBox2.Text = "Excel Ripper";
            groupBox2.Visible = false;
            // 
            // button15
            // 
            button15.BackgroundImage = (Image)resources.GetObject("button15.BackgroundImage");
            button15.BackgroundImageLayout = ImageLayout.Stretch;
            button15.Location = new Point(6, 83);
            button15.Name = "button15";
            button15.Size = new Size(56, 36);
            button15.TabIndex = 14;
            button15.UseVisualStyleBackColor = true;
            button15.Visible = false;
            button15.Click += button15_Click;
            // 
            // button9
            // 
            button9.BackgroundImage = (Image)resources.GetObject("button9.BackgroundImage");
            button9.BackgroundImageLayout = ImageLayout.Stretch;
            button9.ForeColor = Color.Black;
            button9.Location = new Point(68, 83);
            button9.Name = "button9";
            button9.Size = new Size(61, 34);
            button9.TabIndex = 10;
            button9.UseVisualStyleBackColor = true;
            button9.Visible = false;
            button9.Click += button9_Click_1;
            // 
            // groupBox5
            // 
            groupBox5.Controls.Add(button16);
            groupBox5.ForeColor = Color.White;
            groupBox5.Location = new Point(141, 134);
            groupBox5.Name = "groupBox5";
            groupBox5.Size = new Size(134, 125);
            groupBox5.TabIndex = 26;
            groupBox5.TabStop = false;
            groupBox5.Text = "Priority BOM";
            // 
            // button16
            // 
            button16.BackgroundImage = Properties.Resources.Screenshot_2025_01_12_071621;
            button16.BackgroundImageLayout = ImageLayout.Stretch;
            button16.Dock = DockStyle.Fill;
            button16.Location = new Point(3, 19);
            button16.Name = "button16";
            button16.Size = new Size(128, 103);
            button16.TabIndex = 8;
            button16.UseVisualStyleBackColor = true;
            button16.Click += button16_Click;
            // 
            // groupBox10
            // 
            groupBox10.BackColor = Color.Black;
            groupBox10.Controls.Add(button10);
            groupBox10.Controls.Add(button6);
            groupBox10.Controls.Add(button11);
            groupBox10.ForeColor = Color.White;
            groupBox10.Location = new Point(281, 3);
            groupBox10.Name = "groupBox10";
            groupBox10.Size = new Size(132, 125);
            groupBox10.TabIndex = 22;
            groupBox10.TabStop = false;
            groupBox10.Text = "Boomer";
            // 
            // button10
            // 
            button10.BackColor = Color.Transparent;
            button10.BackgroundImage = (Image)resources.GetObject("button10.BackgroundImage");
            button10.BackgroundImageLayout = ImageLayout.Zoom;
            button10.Location = new Point(76, 72);
            button10.Name = "button10";
            button10.Size = new Size(39, 36);
            button10.TabIndex = 2;
            button10.UseVisualStyleBackColor = false;
            button10.Click += button10_Click_1;
            // 
            // button6
            // 
            button6.BackgroundImage = (Image)resources.GetObject("button6.BackgroundImage");
            button6.BackgroundImageLayout = ImageLayout.Stretch;
            button6.Location = new Point(76, 34);
            button6.Name = "button6";
            button6.Size = new Size(39, 32);
            button6.TabIndex = 1;
            button6.UseVisualStyleBackColor = true;
            button6.Click += button6_Click_1;
            // 
            // button11
            // 
            button11.BackgroundImage = (Image)resources.GetObject("button11.BackgroundImage");
            button11.BackgroundImageLayout = ImageLayout.Stretch;
            button11.Dock = DockStyle.Fill;
            button11.ForeColor = Color.Black;
            button11.Location = new Point(3, 19);
            button11.Name = "button11";
            button11.Size = new Size(126, 103);
            button11.TabIndex = 0;
            button11.UseVisualStyleBackColor = true;
            button11.Click += button11_Click_1;
            // 
            // gpbxPMB
            // 
            gpbxPMB.Controls.Add(btnFrmPMB);
            gpbxPMB.ForeColor = Color.White;
            gpbxPMB.Location = new Point(281, 134);
            gpbxPMB.Name = "gpbxPMB";
            gpbxPMB.Size = new Size(138, 122);
            gpbxPMB.TabIndex = 27;
            gpbxPMB.TabStop = false;
            gpbxPMB.Text = "Piority Multi BOM";
            // 
            // btnFrmPMB
            // 
            btnFrmPMB.BackgroundImage = (Image)resources.GetObject("btnFrmPMB.BackgroundImage");
            btnFrmPMB.BackgroundImageLayout = ImageLayout.Stretch;
            btnFrmPMB.Dock = DockStyle.Fill;
            btnFrmPMB.Location = new Point(3, 19);
            btnFrmPMB.Name = "btnFrmPMB";
            btnFrmPMB.Size = new Size(132, 100);
            btnFrmPMB.TabIndex = 0;
            btnFrmPMB.UseVisualStyleBackColor = true;
            btnFrmPMB.Click += btnFrmPMB_Click;
            // 
            // groupBox7
            // 
            groupBox7.Controls.Add(button12);
            groupBox7.Controls.Add(button5);
            groupBox7.Controls.Add(btnMFPN);
            groupBox7.Controls.Add(button3);
            groupBox7.ForeColor = Color.White;
            groupBox7.Location = new Point(425, 3);
            groupBox7.Name = "groupBox7";
            groupBox7.Size = new Size(134, 125);
            groupBox7.TabIndex = 19;
            groupBox7.TabStop = false;
            groupBox7.Text = "PANDA-tabase";
            // 
            // button12
            // 
            button12.BackgroundImage = (Image)resources.GetObject("button12.BackgroundImage");
            button12.BackgroundImageLayout = ImageLayout.Zoom;
            button12.Location = new Point(64, 21);
            button12.Name = "button12";
            button12.Size = new Size(64, 48);
            button12.TabIndex = 13;
            button12.UseVisualStyleBackColor = true;
            button12.Click += button12_Click;
            // 
            // button5
            // 
            button5.BackgroundImageLayout = ImageLayout.Zoom;
            button5.Image = Properties.Resources.documents_files_history_64;
            button5.Location = new Point(64, 75);
            button5.Name = "button5";
            button5.Size = new Size(65, 42);
            button5.TabIndex = 11;
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // btnMFPN
            // 
            btnMFPN.BackColor = Color.Black;
            btnMFPN.BackgroundImage = (Image)resources.GetObject("btnMFPN.BackgroundImage");
            btnMFPN.BackgroundImageLayout = ImageLayout.Zoom;
            btnMFPN.Location = new Point(8, 75);
            btnMFPN.Name = "btnMFPN";
            btnMFPN.Size = new Size(50, 42);
            btnMFPN.TabIndex = 12;
            btnMFPN.UseVisualStyleBackColor = false;
            btnMFPN.Click += btnMFPN_Click;
            // 
            // button3
            // 
            button3.BackgroundImage = Properties.Resources.search;
            button3.BackgroundImageLayout = ImageLayout.Zoom;
            button3.Location = new Point(8, 21);
            button3.Name = "button3";
            button3.Size = new Size(50, 48);
            button3.TabIndex = 9;
            button3.UseVisualStyleBackColor = true;
            button3.Click += button3_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(button13);
            groupBox3.Controls.Add(button8);
            groupBox3.Controls.Add(groupBox11);
            groupBox3.Controls.Add(button2);
            groupBox3.ForeColor = Color.White;
            groupBox3.Location = new Point(425, 134);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(134, 125);
            groupBox3.TabIndex = 15;
            groupBox3.TabStop = false;
            groupBox3.Text = "WH Packing Slip";
            // 
            // button13
            // 
            button13.BackgroundImage = Properties.Resources.box_214671;
            button13.BackgroundImageLayout = ImageLayout.Zoom;
            button13.Location = new Point(74, 68);
            button13.Name = "button13";
            button13.Size = new Size(54, 49);
            button13.TabIndex = 10;
            button13.UseVisualStyleBackColor = true;
            button13.Click += button13_Click;
            // 
            // button8
            // 
            button8.BackgroundImage = Properties.Resources.search;
            button8.BackgroundImageLayout = ImageLayout.Center;
            button8.Location = new Point(8, 68);
            button8.Name = "button8";
            button8.Size = new Size(54, 51);
            button8.TabIndex = 9;
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click;
            // 
            // groupBox11
            // 
            groupBox11.BackColor = Color.Black;
            groupBox11.Controls.Add(button7);
            groupBox11.Enabled = false;
            groupBox11.ForeColor = Color.White;
            groupBox11.Location = new Point(16, 22);
            groupBox11.Name = "groupBox11";
            groupBox11.Size = new Size(103, 40);
            groupBox11.TabIndex = 23;
            groupBox11.TabStop = false;
            groupBox11.Text = "Finished Goods LOG";
            groupBox11.Visible = false;
            // 
            // button7
            // 
            button7.BackgroundImage = (Image)resources.GetObject("button7.BackgroundImage");
            button7.BackgroundImageLayout = ImageLayout.Zoom;
            button7.Dock = DockStyle.Fill;
            button7.Enabled = false;
            button7.Location = new Point(3, 19);
            button7.Name = "button7";
            button7.Size = new Size(97, 18);
            button7.TabIndex = 0;
            button7.UseVisualStyleBackColor = true;
            button7.Visible = false;
            button7.Click += button7_Click_1;
            // 
            // button2
            // 
            button2.BackgroundImage = Properties.Resources.packingSlip;
            button2.BackgroundImageLayout = ImageLayout.Zoom;
            button2.Location = new Point(8, 17);
            button2.Name = "button2";
            button2.Size = new Size(120, 102);
            button2.TabIndex = 8;
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // notifyIcon1
            // 
            notifyIcon1.Icon = (Icon)resources.GetObject("notifyIcon1.Icon");
            notifyIcon1.Text = "Imperium Tabula Principalis";
            notifyIcon1.Visible = true;
            notifyIcon1.MouseClick += notifyIcon1_MouseClick;
            notifyIcon1.MouseDoubleClick += notifyIcon1_MouseDoubleClick;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoScroll = true;
            AutoSize = true;
            BackColor = Color.Black;
            ClientSize = new Size(574, 285);
            Controls.Add(groupBox1);
            ForeColor = SystemColors.ControlText;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Imperium Tabula Principalis UPDATED 202212231139";
            FormClosing += Form1_FormClosing;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel1.ResumeLayout(false);
            groupBox4.ResumeLayout(false);
            groupBox6.ResumeLayout(false);
            groupBox9.ResumeLayout(false);
            groupBox2.ResumeLayout(false);
            groupBox5.ResumeLayout(false);
            groupBox10.ResumeLayout(false);
            gpbxPMB.ResumeLayout(false);
            groupBox7.ResumeLayout(false);
            groupBox3.ResumeLayout(false);
            groupBox11.ResumeLayout(false);
            ResumeLayout(false);
        }
        #endregion
        private GroupBox groupBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private NotifyIcon notifyIcon1;
        private Button btnWorkProgramm;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button5;
        private GroupBox groupBox3;
        private Button button8;
        private GroupBox groupBox6;
        private GroupBox groupBox7;
        private Button button13;
        private Button button14;
        private GroupBox groupBox9;
        private GroupBox groupBox10;
        private Button button11;
        private GroupBox groupBox11;
        private Button button7;
        private Button button4;
        private GroupBox groupBox2;
        private Button button6;
        private Button button9;
        private GroupBox groupBox4;
        private Button button10;
        private Button button12;
        private Button button15;
        private Button btnMFPN;
        private Button button16;
        private GroupBox groupBox5;
        private GroupBox gpbxPMB;
        private Button btnFrmPMB;
    }
}