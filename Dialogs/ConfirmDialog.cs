using System;
using System.Windows.Forms;

namespace appsizerGUI.Dialogs
{
    public partial class ConfirmDialog : Form
    {
        public ConfirmDialog()
        {
            InitializeComponent();
        }

        public virtual void OnOkClicked(object sender, EventArgs e) { }

        public virtual void OnCancelClicked(object sender, EventArgs e) { }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }

    public class SimpleConfirmDialog : ConfirmDialog
    {
        public SimpleConfirmDialog(string text)
        {
            TextLabel.Text = text;
        }

        public SimpleConfirmDialog(string title, string text)
        {
            Text = title;
            TextLabel.Text = text;
        }

        public bool Confirmed;

        public override void OnOkClicked(object sender, EventArgs e)
        {
            Confirmed = true;
            Close();
        }

        public override void OnCancelClicked(object sender, EventArgs e)
        {
            Confirmed = false;
            Close();
        }

        public static bool ShowConfirm(string text)
        {
            var dialog = new SimpleConfirmDialog(text)
            {
                Width = text.Length * 7
            };
            dialog.ShowDialog();
            return dialog.Confirmed;
        }
    }
}
