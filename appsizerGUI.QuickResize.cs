using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI
    {
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
    }
}
