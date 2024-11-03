using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static appsizerGUI.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI : Form
    {
        public appsizerGUI()
        {
            InitializeComponent();

            Control[] controls = { btnSaveWindow, btnRemoveWindow, windowX, windowY, windowWidth, windowHeight, btnRefresh, btnWindowTools, btnApply };
            windowOperationControls = controls;
            UpdateWindowControlsEnabledStatus();

            useCalibration.Checked = enableWindowBorderCalibration;
        }

        private Control[] windowOperationControls;
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
                menuItem.Click += new EventHandler((_s, _e) =>
                {
                    currentWindow = window;
                    RefreshPosition();
                });

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
            currentWindow.SetPosition((int)windowX.Value, (int)windowY.Value, (int)windowWidth.Value, (int)windowHeight.Value);
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

            if (!enabled) statusLabel.Text = "Please select a window first!";
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

            currentWindow = config.SavedWindows[savedWindowSelector.SelectedIndex];
            currentWindow.FindWindow();

            UpdateView();

            Cursor.Current = Cursors.Arrow;
        }

        private void OnListDesktopProfiles(object sender, EventArgs e)
        {

        }

        private void OnToggleCalibrate(object sender, EventArgs e)
        {
            enableWindowBorderCalibration = useCalibration.Checked;
        }

        private void OnWindowToolsClick(object sender, EventArgs e)
        {
            windowToolsBorder.Text = $"Border: ({currentWindow.Border.Left}, {currentWindow.Border.Top}, {currentWindow.Border.Right}, {currentWindow.Border.Bottom})";

            var windowStyle = currentWindow.GetWindowStyle();

            while (windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorEnd) - windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) > 1)
            {
                windowToolsMenu.Items.RemoveAt(windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorStart) + 1);
            }

            foreach (int style in Enum.GetValues(typeof(WindowStyles)))
            {
                var menuItem = new ToolStripMenuItem()
                {
                    Text = ((WindowStyles)style).ToString(),
                    Checked = windowStyle.Is((WindowStyles)style),
                    CheckOnClick = true,
                };
                menuItem.CheckedChanged += new EventHandler((_s, _e) =>
                {
                    windowStyle.Set((WindowStyles)style, menuItem.Checked);
                    currentWindow.SetWindowStyle(windowStyle);
                    UpdateView();
                });

                windowToolsMenu.Items.Insert(windowToolsMenu.Items.IndexOf(windowToolsStyleSeparatorEnd), menuItem);
            }

            windowToolsMenu.Show(btnWindowTools, new Point(btnWindowTools.Width, btnWindowTools.Height), ToolStripDropDownDirection.Left);
        }

        private void OnCenterClicked(object sender, EventArgs e)
        {
            currentWindow.PutToCenter(useAboveTaskbar.Checked);
            UpdateView();
        }
    }
}
