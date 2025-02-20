using System;
using System.Windows.Forms;
using static appsizerGUI.Core.Core;

namespace appsizerGUI
{
    public partial class appsizerGUI
    {
        private void OnCopyStatusProcessName(object sender, EventArgs e)
        {
            Clipboard.SetText(currentWindow.ProcessName);
        }

        private void OnCopyStatusPID(object sender, EventArgs e)
        {
            Clipboard.SetText(currentWindow.Pid.ToString());
        }

        private void OnCopyStatusClass(object sender, EventArgs e)
        {
            Clipboard.SetText(currentWindow.Class);
        }

        private void OnCopyStatusHandle(object sender, EventArgs e)
        {
            Clipboard.SetText($"0x{currentWindow.Handle.ToInt64():X}");
        }
    }
}
