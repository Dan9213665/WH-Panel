using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Seagull.BarTender.Print;
using WMPLib;
namespace WH_Panel
{
    public partial class WarningAvlDialogForm : Form
    {
        public WarningAvlDialogForm(string message)
        {
            InitializeComponent();
            InitializeCustomDialog(message);
        }
        private void WarningAvlDialogForm_Load(object sender, EventArgs e)
        {
            // Any initialization or code you want to execute when the form is loaded
        }
        //private WMPLib.WindowsMediaPlayer videoPlayer;
        //private void InitializeCustomDialog(string message)
        //{
        //    // Set message
        //    lblMessage.Text = message;
        //    // Load video from Resources folder
        //    string resourcesFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources");
        //    string videoFilePath = Path.Combine(resourcesFolder, "warning.mp4");
        //    // Check if the video file exists
        //    if (File.Exists(videoFilePath))
        //    {
        //        // Initialize video player
        //        videoPlayer = new WMPLib.WindowsMediaPlayer();
        //        videoPlayer.URL = videoFilePath;
        //        videoPlayer.settings.setMode("loop", true);
        //        videoPlayer.controls.play();
        //    }
        //    else
        //    {
        //        MessageBox.Show("Video file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        Close(); // Close the form if the video file is not found
        //    }
        //}
        private void InitializeCustomDialog(string message)
        {
            // Set message
            lblMessage.Text = message;
            // Load GIF from Resources folder
            string resourcesFolder = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Resources");
            string gifFilePath = Path.Combine(resourcesFolder, "warnGif.gif");
            // Check if the GIF file exists
            if (File.Exists(gifFilePath))
            {
                // Load the GIF into the PictureBox control
                pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                pictureBox1.Image = Image.FromFile(gifFilePath);
            }
            else
            {
                MessageBox.Show("GIF file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close(); // Close the form if the GIF file is not found
            }
            // Load GIF from Resources folder
            string gifYesFilePath = Path.Combine(resourcesFolder, "yes.gif");
            // Check if the GIF file exists
            if (File.Exists(gifYesFilePath))
            {
                // Load the GIF into the PictureBox control
                pictureBox2Yes.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBox2Yes.Image = Image.FromFile(gifYesFilePath);
            }
            else
            {
                MessageBox.Show("GIF file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close(); // Close the form if the GIF file is not found
            }
            string gifNoFilePath = Path.Combine(resourcesFolder, "no.gif");
            // Check if the GIF file exists
            if (File.Exists(gifNoFilePath))
            {
                // Load the GIF into the PictureBox control
                pictureBoxNo.SizeMode = PictureBoxSizeMode.StretchImage;
                pictureBoxNo.Image = Image.FromFile(gifNoFilePath);
            }
            else
            {
                MessageBox.Show("GIF file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Close(); // Close the form if the GIF file is not found
            }
        }
        private void pictureBox2Yes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
            Close();
        }
        private void pictureBoxNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
            Close();
        }
    }
}
