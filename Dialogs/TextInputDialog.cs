using System;
using System.Windows.Forms;

namespace appsizerGUI.Dialogs
{
    public partial class TextInputDialog : Form
    {
        public TextInputDialog()
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
