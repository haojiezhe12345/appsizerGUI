using System;
using System.Linq;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI
    {
        private void OnListDesktopProfiles(object sender, EventArgs e)
        {
            menuSaveDesktop.DropDownItems.Clear();
            menuRestoreDesktop.DropDownItems.Clear();
            menuDesktopProfileManage.DropDownItems.Clear();

            var hasProfiles = config.DesktopProfiles.Count > 0;

            menuSaveDesktop.DropDownItems.Add(menuSaveDesktopNewProfile);
            menuRestoreDesktop.Enabled = hasProfiles;
            menuDesktopProfileManage.Enabled = hasProfiles;

            foreach (var desktop in config.DesktopProfiles)
            {
                var menuItemSave = new ToolStripMenuItem(desktop.Name);
                menuItemSave.Click += delegate
                {
                    ShowDesktopSaveSuccessMessage(
                        desktop.Name,
                        SaveDesktop(desktop.Name)
                    );
                };
                menuSaveDesktop.DropDownItems.Add(menuItemSave);

                var menuItemLoad = new ToolStripMenuItem(desktop.Name);
                menuItemLoad.Click += delegate
                {
                    var (windowCount, successCount) = RestoreDesktop(desktop.Name);
                    statusLabel.Text = $"Restored {successCount}/{windowCount} windows from \"{desktop.Name}\"";
                };
                menuRestoreDesktop.DropDownItems.Add(menuItemLoad);

                var menuItemManage = new ToolStripMenuItem(desktop.Name);

                var menuItemManageRename = new ToolStripMenuItem("Rename");
                menuItemManageRename.Click += delegate { new DesktopProfileRenameDialog(desktop.Name).ShowDialog(this); };

                var menuItemManageDelete = new ToolStripMenuItem("Delete");
                menuItemManageDelete.Click += delegate
                {
                    DeleteDesktop(desktop.Name);
                    statusLabel.Text = $"Profile \"{desktop.Name}\" deleted";
                };

                menuItemManage.DropDownItems.AddRange(new[] {
                    menuItemManageRename,
                    menuItemManageDelete,
                });
                menuDesktopProfileManage.DropDownItems.Add(menuItemManage);
            }
        }

        private void ShowDesktopSaveSuccessMessage(string name, (int newTotal, int updated) updateResult)
        {
            if (updateResult.newTotal == updateResult.updated)
            {
                statusLabel.Text = $"Saved {updateResult.newTotal} windows to \"{name}\"";
            }
            else
            {
                statusLabel.Text = $"Updated {updateResult.updated} windows in \"{name}\", new total: {updateResult.newTotal}";
            }
        }

        private void OnAddDesktopProfileClick(object sender, EventArgs e)
        {
            new DesktopProfileAddDialog().ShowDialog(this);
        }

        private void OnShowAllMinimizedWindowsClick(object sender, EventArgs e)
        {
            statusLabel.Text = $"Restored {ShowAllMinimizedWindows()} minimized windows";
        }

        private class DesktopProfileAddDialog : appsizerGUI_TextInputDialog
        {
            public DesktopProfileAddDialog()
            {
                Text = "New profile";
                InputLabel.Text = "Profile name:";

                int i = 0;
                do
                {
                    Input.Text = $"Profile {++i:D2}";
                }
                while (config.DesktopProfiles.Any(x => x.Name == Input.Text));
            }

            public override void OnOkClicked(object sender, EventArgs e)
            {
                if (Input.Text.Length == 0) return;

                var windowCount = SaveDesktop(Input.Text);

                try
                {
                    ((appsizerGUI)Owner).ShowDesktopSaveSuccessMessage(Input.Text, windowCount);
                }
                catch { }

                Close();
            }
        }

        private class DesktopProfileRenameDialog : appsizerGUI_TextInputDialog
        {
            private readonly string profileName;

            public DesktopProfileRenameDialog(string profileName)
            {
                Text = "Rename profile";
                InputLabel.Text = "Profile name:";
                Input.Text = profileName;
                this.profileName = profileName;
            }

            public override void OnOkClicked(object sender, EventArgs e)
            {
                if (Input.Text.Length == 0) return;

                RenameDesktop(profileName, Input.Text);

                try
                {
                    ((appsizerGUI)Owner).statusLabel.Text = $"Profile \"{profileName}\" renamed to \"{Input.Text}\"";
                }
                catch { }

                Close();
            }
        }
    }
}
