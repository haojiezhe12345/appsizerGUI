using System;
using System.Windows.Forms;

namespace appsizerGUI
{
    public partial class appsizerGUI_TextInputDialog : Form
    {
        public appsizerGUI_TextInputDialog()
        {
            InitializeComponent();
        }

        public virtual void OnOkClicked(object sender, EventArgs e) { }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }
}
