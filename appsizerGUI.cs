using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;

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
