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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.btnValens = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnWorkProgramm = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.btnLEADERTECH = new System.Windows.Forms.Button();
            this.btnSHILAT = new System.Windows.Forms.Button();
            this.btnFIELDIN = new System.Windows.Forms.Button();
            this.btnNETLINE = new System.Windows.Forms.Button();
            this.btnVAYYAR = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnValens
            // 
            this.btnValens.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.btnValens.BackgroundImage = global::WH_Panel.Properties.Resources.valens;
            this.btnValens.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnValens.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnValens.Location = new System.Drawing.Point(381, 3);
            this.btnValens.Name = "btnValens";
            this.btnValens.Size = new System.Drawing.Size(120, 74);
            this.btnValens.TabIndex = 0;
            this.btnValens.UseVisualStyleBackColor = false;
            this.btnValens.Click += new System.EventHandler(this.button1_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.groupBox1.Controls.Add(this.flowLayoutPanel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(892, 187);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Elige scopum";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.Controls.Add(this.btnWorkProgramm);
            this.flowLayoutPanel1.Controls.Add(this.button3);
            this.flowLayoutPanel1.Controls.Add(this.button1);
            this.flowLayoutPanel1.Controls.Add(this.button2);
            this.flowLayoutPanel1.Controls.Add(this.btnLEADERTECH);
            this.flowLayoutPanel1.Controls.Add(this.btnSHILAT);
            this.flowLayoutPanel1.Controls.Add(this.btnValens);
            this.flowLayoutPanel1.Controls.Add(this.btnFIELDIN);
            this.flowLayoutPanel1.Controls.Add(this.btnNETLINE);
            this.flowLayoutPanel1.Controls.Add(this.btnVAYYAR);
            this.flowLayoutPanel1.Controls.Add(this.button4);
            this.flowLayoutPanel1.Controls.Add(this.button5);
            this.flowLayoutPanel1.Controls.Add(this.button6);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(886, 165);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // btnWorkProgramm
            // 
            this.btnWorkProgramm.BackgroundImage = global::WH_Panel.Properties.Resources._6_2;
            this.btnWorkProgramm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnWorkProgramm.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnWorkProgramm.Location = new System.Drawing.Point(3, 3);
            this.btnWorkProgramm.Name = "btnWorkProgramm";
            this.btnWorkProgramm.Size = new System.Drawing.Size(120, 75);
            this.btnWorkProgramm.TabIndex = 3;
            this.btnWorkProgramm.UseVisualStyleBackColor = true;
            this.btnWorkProgramm.Click += new System.EventHandler(this.btnWorkProgramm_Click);
            // 
            // button3
            // 
            this.button3.BackgroundImage = global::WH_Panel.Properties.Resources.search;
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button3.Location = new System.Drawing.Point(3, 84);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 74);
            this.button3.TabIndex = 9;
            this.button3.Text = "Summum Inquisitionis";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.BackgroundImage = global::WH_Panel.Properties.Resources.kitLabelPrint;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.Location = new System.Drawing.Point(129, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(120, 73);
            this.button1.TabIndex = 7;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click_2);
            // 
            // button2
            // 
            this.button2.BackgroundImage = global::WH_Panel.Properties.Resources.packingSlip;
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button2.Location = new System.Drawing.Point(129, 82);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 76);
            this.button2.TabIndex = 8;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // btnLEADERTECH
            // 
            this.btnLEADERTECH.BackColor = System.Drawing.Color.White;
            this.btnLEADERTECH.BackgroundImage = global::WH_Panel.Properties.Resources.leadertech;
            this.btnLEADERTECH.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnLEADERTECH.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLEADERTECH.Location = new System.Drawing.Point(255, 3);
            this.btnLEADERTECH.Name = "btnLEADERTECH";
            this.btnLEADERTECH.Size = new System.Drawing.Size(120, 75);
            this.btnLEADERTECH.TabIndex = 4;
            this.btnLEADERTECH.UseVisualStyleBackColor = false;
            this.btnLEADERTECH.Click += new System.EventHandler(this.btnLEADERTECH_Click);
            // 
            // btnSHILAT
            // 
            this.btnSHILAT.BackColor = System.Drawing.Color.White;
            this.btnSHILAT.BackgroundImage = global::WH_Panel.Properties.Resources.shilat1;
            this.btnSHILAT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSHILAT.Location = new System.Drawing.Point(255, 84);
            this.btnSHILAT.Name = "btnSHILAT";
            this.btnSHILAT.Size = new System.Drawing.Size(120, 74);
            this.btnSHILAT.TabIndex = 6;
            this.btnSHILAT.UseVisualStyleBackColor = false;
            this.btnSHILAT.Click += new System.EventHandler(this.btnSHILAT_Click);
            // 
            // btnFIELDIN
            // 
            this.btnFIELDIN.BackColor = System.Drawing.Color.White;
            this.btnFIELDIN.BackgroundImage = global::WH_Panel.Properties.Resources.Fieldin;
            this.btnFIELDIN.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFIELDIN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFIELDIN.Location = new System.Drawing.Point(381, 83);
            this.btnFIELDIN.Name = "btnFIELDIN";
            this.btnFIELDIN.Size = new System.Drawing.Size(120, 75);
            this.btnFIELDIN.TabIndex = 2;
            this.btnFIELDIN.UseVisualStyleBackColor = false;
            this.btnFIELDIN.Click += new System.EventHandler(this.btnFIELDIN_Click);
            // 
            // btnNETLINE
            // 
            this.btnNETLINE.BackColor = System.Drawing.Color.White;
            this.btnNETLINE.BackgroundImage = global::WH_Panel.Properties.Resources.netline;
            this.btnNETLINE.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnNETLINE.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnNETLINE.Location = new System.Drawing.Point(507, 3);
            this.btnNETLINE.Name = "btnNETLINE";
            this.btnNETLINE.Size = new System.Drawing.Size(120, 74);
            this.btnNETLINE.TabIndex = 1;
            this.btnNETLINE.UseVisualStyleBackColor = false;
            this.btnNETLINE.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // btnVAYYAR
            // 
            this.btnVAYYAR.BackColor = System.Drawing.Color.White;
            this.btnVAYYAR.BackgroundImage = global::WH_Panel.Properties.Resources.vayyar;
            this.btnVAYYAR.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnVAYYAR.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnVAYYAR.Location = new System.Drawing.Point(507, 83);
            this.btnVAYYAR.Name = "btnVAYYAR";
            this.btnVAYYAR.Size = new System.Drawing.Size(120, 75);
            this.btnVAYYAR.TabIndex = 5;
            this.btnVAYYAR.UseVisualStyleBackColor = false;
            this.btnVAYYAR.Click += new System.EventHandler(this.btnVAYYAR_Click);
            // 
            // button4
            // 
            this.button4.BackgroundImage = global::WH_Panel.Properties.Resources.CIS;
            this.button4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button4.Location = new System.Drawing.Point(633, 3);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 75);
            this.button4.TabIndex = 10;
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Image = global::WH_Panel.Properties.Resources.documents_files_history_64;
            this.button5.Location = new System.Drawing.Point(633, 84);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(120, 74);
            this.button5.TabIndex = 11;
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.BackgroundImage = global::WH_Panel.Properties.Resources.STM;
            this.button6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button6.Location = new System.Drawing.Point(759, 3);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 74);
            this.button6.TabIndex = 12;
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "Imperium Tabula Principalis";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(892, 187);
            this.Controls.Add(this.groupBox1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Imperium Tabula Principalis UPDATED 202212231139";
            this.TopMost = true;
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    

        #endregion

        private Button btnValens;
        private GroupBox groupBox1;
        private FlowLayoutPanel flowLayoutPanel1;
        private Button btnNETLINE;
        private Button btnFIELDIN;
        private NotifyIcon notifyIcon1;
        private Button btnWorkProgramm;
        private Button btnLEADERTECH;
        private Button btnVAYYAR;
        private Button btnSHILAT;
        private Button button1;
        private Button button2;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
    }
}