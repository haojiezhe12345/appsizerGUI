using System;
using System.Runtime.InteropServices;
using System.Text;

namespace appsizerGUI
{
    public static class DLLImports
    {
        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(IntPtr hWnd);


        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool QueryFullProcessImageName(IntPtr hProcess, int dwFlags, StringBuilder lpExeName, ref int lpdwSize);


        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, out Rect lpRect);
        [DllImport("user32.dll")]
        public static extern bool GetClientRect(IntPtr hwnd, out Rect lpRect);
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }

        [DllImport("user32.dll")]
        public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll")]
        public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
        public const int GWL_STYLE = -16;
        public const int GWL_EXSTYLE = -20;

        [DllImport("user32.dll")]
        public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        public const int SWP_NOSIZE = 0x0001;
        public const int SWP_NOMOVE = 0x0002;

        [DllImport("user32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        public static bool ShowWindow(IntPtr hWnd, ShowWindowParam nCmdShow) => ShowWindow(hWnd, (int)nCmdShow);
        public enum ShowWindowParam
        {
            SW_HIDE,
            SW_SHOWNORMAL,
            SW_SHOWMINIMIZED,
            SW_SHOWMAXIMIZED,
            SW_SHOWNOACTIVATE,
            SW_SHOW,
            SW_MINIMIZE,
            SW_SHOWMINNOACTIVE,
            SW_SHOWNA,
            SW_RESTORE,
            SW_SHOWDEFAULT,
            SW_FORCEMINIMIZE,
        }


        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);
        public const int PROCESS_QUERY_LIMITED_INFORMATION = 0x1000;

        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr hObject);
    }
}
