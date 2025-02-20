using System;
using System.Linq;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI
    {
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
    }
}
