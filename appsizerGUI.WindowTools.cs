using appsizerGUI.Core;
using System;
using System.Drawing;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;
using static appsizerGUI.DLLImports;

namespace appsizerGUI
{
    public partial class appsizerGUI
    {
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
    }
}
