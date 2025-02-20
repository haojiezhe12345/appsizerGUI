using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using appsizerGUI.Core;
using static appsizerGUI.Core.Core;
using static appsizerGUI.DLLImports;

namespace appsizerGUI
{
    public partial class appsizerGUI : Form
    {
        public appsizerGUI()
        {
            InitializeComponent();

            windowOperationControls = new Control[] {
                btnSaveWindow, btnRemoveWindow,
                windowX, windowY, windowWidth, windowHeight,
                btnRefresh, btnQuickResize, btnWindowTools, btnApply,
            };
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

            btnQuickResize.AddDownTriangle();
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

            copyStatusMenu.Enabled = enabled;
            uiUpdateHandlerEnabled = enabled;

            if (!enabled) statusLabel.Text = "Please select a window first!";
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
            // ShowWindow
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

            // Window styles
            while (windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorEnd) - windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) > 1)
            {
                windowToolsMenu.Items.RemoveAt(windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) + 1);
            }

            var windowStyle = currentWindow.GetWindowStyle<WindowStyles>();
            var windowExStyle = currentWindow.GetWindowStyle<WindowExStyles>();

            AddWindowStylesToWindowTools(windowStyle);
            AddWindowStylesToWindowTools(windowExStyle);

            // Always on top
            windowToolsAlwaysOnTop.Checked = windowExStyle.Is(WindowExStyles.WS_EX_TOPMOST);

            // Window has border
            windowToolsHasBorder.Checked = windowStyle.HasBorder();

            // Border
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

        private void OnAlwaysOnTopClicked(object sender, EventArgs e)
        {
            currentWindow.SetAlwaysOnTop(windowToolsAlwaysOnTop.Checked);
        }

        private void OnToggleBorderClicked(object sender, EventArgs e)
        {
            currentWindow.HasBorder = !windowToolsHasBorder.Checked;
            UpdateView();
        }

        private void OnQuickResizeClick(object sender, EventArgs e)
        {
            var resolutions = new List<(int width, int height, string description)>
            {
                (640, 480, "4:3"),
                (854, 480, "16:9"),
                (800, 600, "4:3"),
                (960, 540, "16:9"),
                (0, 0, ""),
                (1024, 768, "4:3"),
                (1280, 720, "720p"),
                (1280, 800, "16:10"),
                (1366, 768, "16:9"),
                (1600, 900, "16:9"),
                (0, 0, ""),
                (1440, 1080, "4:3"),
                (1920, 1080, "1080p"),
                (1920, 1200, "16:10"),
                (2560, 1080, "21:9"),
                (0, 0, ""),
                (1920, 1440, "4:3"),
                (2160, 1440, "3:2"),
                (2560, 1440, "2.5K"),
                (2560, 1600, "16:10"),
                (3440, 1440, "21:9"),
                (0, 0, ""),
                (3840, 2160, "4K"),
                (5120, 2880, "5K"),
                (7680, 4320, "8K"),
            };

            quickResizeMenu.Items.Clear();

            quickResizeMenu.Items.AddRange(new ToolStripItem[] {
                quickResizeBorderlessFullscreen,
                quickResizeBorderlessAboveTaskbar,
                new ToolStripSeparator(),
            });

            quickResizeBorderlessFullscreen.Checked = false;
            quickResizeBorderlessAboveTaskbar.Checked = false;

            if (!currentWindow.HasBorder &&
                currentWindow.X == 0 &&
                currentWindow.Y == 0)
            {
                if (currentWindow.Width == ScreenWidth && currentWindow.Height == ScreenHeight)
                {
                    quickResizeBorderlessFullscreen.Checked = true;
                }
                if (currentWindow.Width == WorkingAreaWidth && currentWindow.Height == WorkingAreaHeight)
                {
                    quickResizeBorderlessAboveTaskbar.Checked = true;
                }
            }

            foreach (var (width, height, description) in resolutions)
            {
                if (width == 0)
                {
                    quickResizeMenu.Items.Add(new ToolStripSeparator());
                    continue;
                }

                var menuItem = new ToolStripMenuItem()
                {
                    Text = $"{width} x {height} ({description})",
                    Checked = currentWindow.Width == width && currentWindow.Height == height,
                };
                menuItem.Click += delegate
                {
                    currentWindow.QuickResize(width, height, height <= WorkingAreaHeight);
                    UpdateView();
                };
                quickResizeMenu.Items.Add(menuItem);
            }

            quickResizeMenu.Show(btnQuickResize, new Point(0, btnQuickResize.Height), ToolStripDropDownDirection.Right);
        }

        private async void OnBorderlessFullscreenClick(object sender, EventArgs e)
        {
            await currentWindow.MakeBorderless();
            UpdateView();
        }

        private async void OnBorderlessAboveTaskbarClick(object sender, EventArgs e)
        {
            await currentWindow.MakeBorderless(true);
            UpdateView();
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
