using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace WH_Panel
{
    public partial class FrmQRPrint : Form
    {
        public FrmQRPrint()
        {
            InitializeComponent();
            UpdateControlColors(this);
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }
        private void UpdateControlColors(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                // Update control colors based on your criteria
                control.BackColor = Color.Black;
                control.ForeColor = Color.White;
                // Recursively update controls within containers
                if (control.Controls.Count > 0)
                {
                    UpdateControlColors(control);
                }
            }
        }
        // Call the method for each form
    }
}
