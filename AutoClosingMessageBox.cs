using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace WH_Panel
{
    public class AutoClosingMessageBox : Form
    {
        private System.Windows.Forms.Label messageLabel;
        private System.Windows.Forms.Timer closeTimer;
        //public AutoClosingMessageBox(string message, int timeout, Color backColor)
        //{
        //    messageLabel = new System.Windows.Forms.Label
        //    {
        //        Text = message,
        //        Dock = DockStyle.Fill,
        //        TextAlign = ContentAlignment.MiddleCenter,
        //        Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold),
        //        BackColor = backColor, // Use the backColor parameter
        //        ForeColor = Color.White
        //    };
        //    closeTimer = new System.Windows.Forms.Timer
        //    {
        //        Interval = timeout
        //    };
        //    closeTimer.Tick += CloseTimer_Tick;
        //    Controls.Add(messageLabel);
        //    StartPosition = FormStartPosition.CenterScreen;
        //    Size = new Size(450, 100);
        //    FormBorderStyle = FormBorderStyle.FixedDialog;
        //    MaximizeBox = false;
        //    MinimizeBox = false;
        //    ShowInTaskbar = false;
        //    TopMost = true;
        //    closeTimer.Start();
        //}

        public AutoClosingMessageBox(string message, int timeout, Color backColor)
        {
            // Define the font beforehand so we can use it for calculations
            System.Drawing.Font messageFont = new System.Drawing.Font("Arial", 16, FontStyle.Bold);

            // 1. Calculate the exact size needed for the text strings
            // We add padding (e.g., 50px width, 40px height) to ensure it breathes nicely inside the window borders
            Size proposedSize = new Size(int.MaxValue, int.MaxValue); // Unconstrained initial check
            Size textSize = TextRenderer.MeasureText(message, messageFont, proposedSize, TextFormatFlags.WordBreak);

            int calculatedWidth = textSize.Width + 100;
            int calculatedHeight = textSize.Height + 100;

            // 2. Set sensible boundaries (Minimums and Maximums)
            // Prevents extremely short text from making a tiny box, or massive text from spilling past the screen bounds
            int finalWidth = Math.Clamp(calculatedWidth, 450, 900);
            int finalHeight = Math.Clamp(calculatedHeight, 100, 500);

            // Apply the dynamically calculated dimensions to the Form
            this.Size = new Size(finalWidth, finalHeight);

            // Setup the message label control
            messageLabel = new System.Windows.Forms.Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = messageFont,
                BackColor = backColor,
                ForeColor = Color.White
            };

            closeTimer = new System.Windows.Forms.Timer
            {
                Interval = timeout
            };
            closeTimer.Tick += CloseTimer_Tick;

            Controls.Add(messageLabel);

            // Form Layout Settings
            StartPosition = FormStartPosition.CenterScreen;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowInTaskbar = false;
            TopMost = true;

            closeTimer.Start();
        }
        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            closeTimer.Stop();
            Close();
        }
        public static void Show(string message, int timeout, Color _backColor)
        {
            using (var form = new AutoClosingMessageBox(message, timeout, _backColor))
            {
                form.ShowDialog();
            }
        }
    }
}
