using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static appsizerGUI.Core;
using static appsizerGUI.DLLImports;

namespace appsizerGUI
{
    public partial class appsizerGUI : Form
    {
        public appsizerGUI()
        {
            InitializeComponent();

            windowOperationControls = new Control[] { btnSaveWindow, btnRemoveWindow, windowX, windowY, windowWidth, windowHeight, btnRefresh, btnWindowTools, btnApply };
            UpdateWindowControlsEnabledStatus();

            foreach (int m in Enum.GetValues(typeof(BorderCalibrationMethod)))
            {
                var method = (BorderCalibrationMethod)m;
                var text = method.ToString();

                switch (method)
                {
                    case BorderCalibrationMethod.Native:
                        text += " (usually 8px, Windows 8.1 or earlier)";
                        break;
                    case BorderCalibrationMethod.Calibrated:
                        text += " (1px, Windows 10/11)";
                        break;
                    case BorderCalibrationMethod.ClientArea:
                        text = "Client area (exclude the border completely)";
                        break;
                }

                var menuItem = new ToolStripMenuItem(text);
                menuItem.Click += delegate
                {
                    foreach (ToolStripMenuItem x in menuWindowBorderSelect.DropDownItems)
                    {
                        x.Checked = false;
                    }
                    menuItem.Checked = true;

                    borderCalibrationMethod = method;
                    if (uiUpdateHandlerEnabled) RefreshPosition();
                };

                menuWindowBorderSelect.DropDownItems.Add(menuItem);
            }
            ((ToolStripMenuItem)menuWindowBorderSelect.DropDownItems[(int)borderCalibrationMethod]).Checked = true;

            btnWindowTools.AddDownTriangle();
        }

        private readonly Control[] windowOperationControls;
        private bool uiUpdateHandlerEnabled = false;

        private void ListWindows(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            menuWindowSelect.DropDownItems.Clear();

            foreach (var window in GetWindowList().OrderBy(x => x.Title))
            {
                Bitmap image = null;
                try
                {
                    image = Icon.ExtractAssociatedIcon(window.ProcessPath).ToBitmap();
                }
                catch { }

                var menuItem = new ToolStripMenuItem
                {
                    Text = $"{window.Title}  [{window.ProcessName} ({window.Pid}) - 0x{window.Handle.ToInt64():X}]",
                    Image = image,
                };
                menuItem.Click += delegate
                {
                    currentWindow = window;
                    RefreshPosition();
                };

                menuWindowSelect.DropDownItems.Add(menuItem);
            }

            Cursor.Current = Cursors.Arrow;
        }

        private void OnRefreshClicked(object sender, EventArgs e)
        {
            RefreshPosition();
        }

        private void OnSetPosition(object sender, EventArgs e)
        {
            if (!uiUpdateHandlerEnabled) return;
            currentWindow.SetPosition((int)windowX.Value, (int)windowY.Value, (int)windowWidth.Value, (int)windowHeight.Value, optionBringToFront.Checked);
            UpdateView();
        }

        private void RefreshPosition()
        {
            Cursor.Current = Cursors.WaitCursor;

            if (!currentWindow.GetPosition())
            {
                currentWindow.FindWindow();
                currentWindow.GetPosition();
            }

            UpdateView();

            Cursor.Current = Cursors.Arrow;
        }

        private void UpdateView()
        {
            uiUpdateHandlerEnabled = false;

            savedWindowSelector.Text = currentWindow.Title;

            windowX.Value = currentWindow.X;
            windowY.Value = currentWindow.Y;
            windowWidth.Value = currentWindow.Width;
            windowHeight.Value = currentWindow.Height;
            windowRight.Text = currentWindow.Right.ToString();
            windowBottom.Text = currentWindow.Bottom.ToString();

            statusLabel.Text = currentWindow.IsValid
                ? $"{currentWindow.ProcessName} ({currentWindow.Pid}) - {currentWindow.Class} (0x{currentWindow.Handle.ToInt64():X})"
                : "Window not found!";

            UpdateWindowControlsEnabledStatus();

            uiUpdateHandlerEnabled = true;
        }

        private void UpdateWindowControlsEnabledStatus()
        {
            var enabled = currentWindow != null;

            foreach (var control in windowOperationControls)
            {
                control.Enabled = enabled;
            }

            if (enabled)
            {
                uiUpdateHandlerEnabled = true;
            }
            else
            {
                uiUpdateHandlerEnabled = false;
                statusLabel.Text = "Please select a window first!";
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

        private void SaveCurrentWindow(object sender, EventArgs e)
        {
            var existingWindow = config.SavedWindows.FirstOrDefault(x => x.Title == currentWindow.Title);

            if (existingWindow != null)
            {
                var index = config.SavedWindows.IndexOf(existingWindow);
                config.SavedWindows[index] = currentWindow;
            }
            else
            {
                config.SavedWindows.Add(currentWindow);
            }

            config.Save();
        }

        private void RemoveCurrentWindow(object sender, EventArgs e)
        {
            config.SavedWindows.RemoveAll(x => x.Title == currentWindow.Title);
            config.Save();
        }

        private void ListSavedWindows(object sender, EventArgs e)
        {
            savedWindowSelector.Items.Clear();
            config.Reload();
            foreach (var window in config.SavedWindows)
            {
                savedWindowSelector.Items.Add(window.Title);
            }
        }

        private void LoadSavedWindow(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            SetCurrentWindow(savedWindowSelector.SelectedIndex);
            UpdateView();

            Cursor.Current = Cursors.Arrow;
        }

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

        private void OnAddDesktopProfileClick(object sender, EventArgs e)
        {
            new DesktopProfileAddDialog().ShowDialog(this);
        }

        private void OnShowAllMinimizedWindowsClick(object sender, EventArgs e)
        {
            statusLabel.Text = $"Restored {ShowAllMinimizedWindows()} minimized windows";
        }

        private void OnWindowToolsClick(object sender, EventArgs e)
        {
            windowToolsShowWindow.DropDownItems.Clear();

            foreach (int param in Enum.GetValues(typeof(ShowWindowParam)))
            {
                var menuItem = new ToolStripMenuItem(((ShowWindowParam)param).ToString());
                menuItem.Click += delegate
                {
                    ShowWindow(currentWindow.Handle, param);
                    RefreshPosition();
                };
                windowToolsShowWindow.DropDownItems.Add(menuItem);
            }

            while (windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorEnd) - windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) > 1)
            {
                windowToolsMenu.Items.RemoveAt(windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) + 1);
            }

            var windowStyle = currentWindow.GetWindowStyle<WindowStyles>();
            var windowExStyle = currentWindow.GetWindowStyle<WindowExStyles>();

            windowToolsAlwaysOnTop.Checked = windowExStyle.Is(WindowExStyles.WS_EX_TOPMOST);

            AddWindowStylesToWindowTools(windowStyle);
            AddWindowStylesToWindowTools(windowExStyle);

            windowToolsBorder.Text = $"Border: ({currentWindow.Border.Left}, {currentWindow.Border.Top}, {currentWindow.Border.Right}, {currentWindow.Border.Bottom})";

            windowToolsMenu.Show(btnWindowTools, new Point(btnWindowTools.Width, btnWindowTools.Height), ToolStripDropDownDirection.Left);
        }

        private void AddWindowStylesToWindowTools<T>(WindowStyle<T> windowStyle) where T : Enum
        {
            foreach (var styleName in Enum.GetNames(typeof(T)))
            {
                var styleEnum = (T)Enum.Parse(typeof(T), styleName);

                var menuItem = new ToolStripMenuItem()
                {
                    Text = styleName,
                    Checked = windowStyle.Is(styleEnum),
                    CheckOnClick = true,
                };
                menuItem.CheckedChanged += async delegate
                {
                    windowStyle.Set(styleEnum, menuItem.Checked);
                    await currentWindow.SetWindowStyleAsync(windowStyle);
                    UpdateView();
                };

                windowToolsMenu.Items.Insert(windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorEnd), menuItem);
            }
        }

        private void OnCenterClicked(object sender, EventArgs e)
        {
            currentWindow.PutToCenter(useAboveTaskbar.Checked);
            UpdateView();
        }

        private async void OnBorderlessClicked(object sender, EventArgs e)
        {
            await currentWindow.MakeBorderless();
            UpdateView();
        }

        private void OnAlwaysOnTopClicked(object sender, EventArgs e)
        {
            currentWindow.SetAlwaysOnTop(windowToolsAlwaysOnTop.Checked);
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

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                Close();
            }
        }
    }

    public static class ButtonIconExtensions
    {
        public static void AddDownTriangle(this Button button, ContentAlignment align = ContentAlignment.MiddleRight)
        {
            button.ImageAlign = align;
            button.Paint += DrawTriangleAsync;
        }

        public static void DrawTriangleAsync(object sender, EventArgs e)
        {
            var button = (Button)sender;

            Task.Run(() =>
            {
                Task.Delay(10).Wait();
                int triangleHeight = (int)Math.Round(button.Height * 6.0 / 23.0);

                Bitmap triangleDown = new Bitmap(triangleHeight * 2 - 1, triangleHeight);

                using (var g = Graphics.FromImage(triangleDown))
                using (var brush = new SolidBrush(Color.Black))
                {
                    g.FillPolygon(
                        brush,
                        new[] {
                            new Point(1, 1),
                            new Point(triangleDown.Width - 1, 0),
                            new Point(triangleDown.Width / 2, triangleDown.Height),
                        }
                    );
                }

                button.Image = triangleDown;
            });
        }
    }
}
