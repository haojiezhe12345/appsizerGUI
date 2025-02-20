using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using static appsizerGUI.DLLImports;

namespace appsizerGUI.Core
{
    public static partial class Core
    {
        public static int ScreenWidth => Screen.PrimaryScreen.Bounds.Width;
        public static int ScreenHeight => Screen.PrimaryScreen.Bounds.Height;
        public static int WorkingAreaWidth => Screen.PrimaryScreen.WorkingArea.Width;
        public static int WorkingAreaHeight => Screen.PrimaryScreen.WorkingArea.Height;

        public static Config config = Config.Load();
        public static Window currentWindow;

        public static BorderCalibrationMethod borderCalibrationMethod = BorderCalibrationMethod.Calibrated;

        public enum BorderCalibrationMethod
        {
            Native,
            Calibrated,
            ClientArea
        }

        public static List<Window> GetWindowList()
        {
            var windows = new List<Window>();

            EnumWindows(delegate (IntPtr hWnd, IntPtr lParam)
            {
                if (IsWindowVisible(hWnd))
                {
                    var windowTitleLength = GetWindowTextLength(hWnd);
                    if (windowTitleLength > 0)
                    {
                        var windowTitle = new StringBuilder(windowTitleLength + 1);
                        GetWindowText(hWnd, windowTitle, windowTitle.Capacity);

                        var className = new StringBuilder(256);
                        GetClassName(hWnd, className, className.Capacity);

                        GetWindowThreadProcessId(hWnd, out uint pid);

                        var ProcessPath = new StringBuilder(1024);
                        {
                            var hProcess = OpenProcess(PROCESS_QUERY_LIMITED_INFORMATION, false, pid);
                            var readSize = ProcessPath.Capacity;
                            QueryFullProcessImageName(hProcess, 0, ProcessPath, ref readSize);
                            CloseHandle(hProcess);
                        }

                        var window = new Window
                        {
                            ProcessPath = ProcessPath.ToString(),
                            Pid = pid,
                            Title = windowTitle.ToString(),
                            Class = className.ToString(),
                            Handle = hWnd,
                        };

                        windows.Add(window);
                    }
                }
                return true;
            }, IntPtr.Zero);

            return windows;
        }

        public static void SetCurrentWindow(int index)
        {
            currentWindow = config.SavedWindows[index];
            currentWindow.FindWindow();
        }
    }
}
