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
        public AutoClosingMessageBox(string message, int timeout, Color backColor)
        {
            messageLabel = new System.Windows.Forms.Label
            {
                Text = message,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Arial", 16, FontStyle.Bold),
                BackColor = backColor, // Use the backColor parameter
                ForeColor = Color.White
            };
            closeTimer = new System.Windows.Forms.Timer
            {
                Interval = timeout
            };
            closeTimer.Tick += CloseTimer_Tick;
            Controls.Add(messageLabel);
            StartPosition = FormStartPosition.CenterScreen;
            Size = new Size(450, 100);
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
