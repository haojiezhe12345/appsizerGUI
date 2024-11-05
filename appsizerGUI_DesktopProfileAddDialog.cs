using System.Linq;
using System.Windows.Forms;
using static appsizerGUI.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI_DesktopProfileAddDialog : Form
    {
        public appsizerGUI_DesktopProfileAddDialog()
        {
            InitializeComponent();

            int i = 0;
            do
            {
                profileNameAdd.Text = $"Profile {++i:D2}";
            }
            while (config.DesktopProfiles.Any(x => x.Name == profileNameAdd.Text));
        }

        private void OnAddProfileClicked(object sender, System.EventArgs e)
        {
            if (profileNameAdd.Text.Length == 0) return;
            SaveDesktop(profileNameAdd.Text);
            Close();
        }
    }
}
