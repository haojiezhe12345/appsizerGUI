using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Xml;
using static appsizerGUI.DLLImports;

namespace appsizerGUI
{
    public static class Core
    {
        public static int ScreenWidth => Screen.PrimaryScreen.Bounds.Width;
        public static int ScreenHeight => Screen.PrimaryScreen.Bounds.Height;
        public static int WorkingAreaWidth => Screen.PrimaryScreen.WorkingArea.Width;
        public static int WorkingAreaHeight => Screen.PrimaryScreen.WorkingArea.Height;

        public static Config config = Config.Load();
        public static Window currentWindow;

        public class Window
        {
            public string ProcessName { get; set; }
            [XmlIgnore]
            public uint Pid { get; set; }
            public string Title { get; set; }
            public string Class { get; set; }
            [XmlIgnore]
            public IntPtr Handle { get; set; }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            [XmlIgnore]
            public int Right => ScreenWidth - X - Width;
            [XmlIgnore]
            public int Bottom => ScreenHeight - Y - Height;
            public int BorderWidth { get; set; }

            [XmlIgnore]
            public bool IsValid => IsWindow(Handle);

            public bool GetPosition()
            {
                if (GetWindowRect(Handle, out Rect windowRect) && GetClientRect(Handle, out Rect clientRect))
                {
                    var windowWidth = windowRect.Right - windowRect.Left;
                    var clientWidth = clientRect.Right - clientRect.Left;
                    BorderWidth = (windowWidth - clientWidth) / 2 * 7 / 8;

                    X = windowRect.Left + BorderWidth;
                    Y = windowRect.Top;
                    Width = windowRect.Right - windowRect.Left - BorderWidth * 2;
                    Height = windowRect.Bottom - windowRect.Top - BorderWidth;

                    return true;
                }
                else return false;
            }

            public bool SetPosition(int x, int y, int width, int height)
            {
                if (SetWindowPos(Handle, IntPtr.Zero, x - BorderWidth, y, width + BorderWidth * 2, height + BorderWidth, SWP_NOZORDER))
                {
                    GetPosition();
                    return true;
                }
                else return false;
            }

            public bool SetPosition()
            {
                return SetWindowPos(Handle, IntPtr.Zero, X - BorderWidth, Y, Width + BorderWidth * 2, Height + BorderWidth, SWP_NOZORDER);
            }

            public bool PutToCenter(bool aboveTaskbar = false)
            {
                return currentWindow.SetPosition(
                      (ScreenWidth - Width) / 2,
                      ((aboveTaskbar ? WorkingAreaHeight : ScreenHeight) - Height) / 2,
                      Width, Height
                  );
            }

            public bool FindWindow()
            {
                var window = GetWindowList().FirstOrDefault(x => x.Title == Title);
                if (window != null)
                {
                    Pid = window.Pid;
                    Handle = window.Handle;
                    return true;
                }
                else return false;
            }
        }

        public class Config
        {
            public List<Window> SavedWindows { get; set; } = new List<Window>();

            public static readonly string ConfigFilePath = "appsizerGUI_config.xml";

            public void Save()
            {
                var serializer = new XmlSerializer(typeof(Config));
                using (var writer = XmlWriter.Create(ConfigFilePath, new XmlWriterSettings { Indent = true }))
                {
                    serializer.Serialize(writer, this);
                }
            }

            public void Reload() => config = Load();

            public static Config Load()
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(Config));
                    using (var reader = XmlReader.Create(ConfigFilePath))
                    {
                        return (Config)serializer.Deserialize(reader);
                    }
                }
                catch
                {
                    return new Config();
                }
            }
        }

        public static List<Window> GetWindowList()
        {
            var windows = new List<Window>();
            var processes = Process.GetProcesses();

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

                        var window = new Window
                        {
                            ProcessName = processes.FirstOrDefault(p => p.Id == pid).ProcessName ?? "",
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
    }
}
