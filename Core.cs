using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
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

        public static bool enableWindowBorderCalibration = true;

        public class Window
        {
            public string ProcessPath { get; set; }
            [XmlIgnore]
            public string ProcessName => Path.GetFileName(ProcessPath);
            public uint Pid { get; set; }
            public string Title { get; set; }
            public string Class { get; set; }
            [XmlIgnore]
            public IntPtr Handle { get; set; }
            public int Handle_Int
            {
                get => Handle.ToInt32();
                set => Handle = (IntPtr)value;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public int Width { get; set; }
            public int Height { get; set; }
            [XmlIgnore]
            public int Right => ScreenWidth - X - Width;
            [XmlIgnore]
            public int Bottom => ScreenHeight - Y - Height;
            [XmlIgnore]
            public Rect Border { get; set; }

            [XmlIgnore]
            public bool IsValid => IsWindow(Handle);

            public bool GetPosition()
            {
                if (GetWindowRect(Handle, out Rect windowRect) && GetClientRect(Handle, out Rect clientRect))
                {
                    GetWindowBorder(windowRect, clientRect);

                    X = windowRect.Left + Border.Left;
                    Y = windowRect.Top + Border.Top;
                    Width = windowRect.Right - windowRect.Left - Border.Left - Border.Right;
                    Height = windowRect.Bottom - windowRect.Top - Border.Top - Border.Bottom;

                    return true;
                }
                else return false;
            }

            public bool SetPosition(int x, int y, int width, int height, bool bringToFront = true)
            {
                return
                    GetWindowBorder()
                    && (bringToFront
                        ? SetWindowPos(Handle, IntPtr.Zero, x - Border.Left, y - Border.Top, width + Border.Left + Border.Right, height + Border.Top + Border.Bottom, 0)
                        : MoveWindow(Handle, x - Border.Left, y - Border.Top, width + Border.Left + Border.Right, height + Border.Top + Border.Bottom, true))
                    && GetPosition();
            }

            public bool SetPosition()
            {
                return
                    GetWindowBorder()
                    && MoveWindow(Handle, X - Border.Left, Y - Border.Top, Width + Border.Left + Border.Right, Height + Border.Top + Border.Bottom, true);
            }

            public void GetWindowBorder(Rect windowRect, Rect clientRect)
            {
                var windowStyle = GetWindowStyle<WindowStyles>();

                if (windowStyle.Is(WindowStyles.WS_MINIMIZE) || !enableWindowBorderCalibration)
                {
                    Border = new Rect { Top = 0, Left = 0, Right = 0, Bottom = 0 };
                }
                else
                {
                    var windowWidth = windowRect.Right - windowRect.Left;
                    var clientWidth = clientRect.Right - clientRect.Left;
                    var borderWidth = (windowWidth - clientWidth) / 2;

                    if (windowStyle.Is(WindowStyles.WS_MAXIMIZE))
                    {
                        Border = new Rect { Top = borderWidth, Left = borderWidth, Right = borderWidth, Bottom = borderWidth };
                    }
                    else
                    {
                        borderWidth = borderWidth * 7 / 8;
                        Border = new Rect { Top = 0, Left = borderWidth, Right = borderWidth, Bottom = borderWidth };
                    }
                }
            }

            public bool GetWindowBorder()
            {
                if (GetWindowRect(Handle, out Rect windowRect) && GetClientRect(Handle, out Rect clientRect))
                {
                    GetWindowBorder(windowRect, clientRect);
                    return true;
                }
                return false;
            }

            public WindowStyle<T> GetWindowStyle<T>() where T : Enum
            {
                return new WindowStyle<T>(GetWindowLong(Handle, typeof(T) == typeof(WindowStyles) ? GWL_STYLE : GWL_EXSTYLE));
            }

            public Task<int> SetWindowStyleAsync<T>(WindowStyle<T> style) where T : Enum
            {
                return Task.Run(async () =>
                {
                    var result = SetWindowLong(Handle, typeof(T) == typeof(WindowStyles) ? GWL_STYLE : GWL_EXSTYLE, style.Style);

                    await Task.Delay(10);

                    GetPosition();
                    return result;
                });
            }

            public bool SetAlwaysOnTop(bool value)
            {
                return SetWindowPos(Handle, (IntPtr)(value ? -1 : -2), 0, 0, 0, 0, SWP_NOSIZE | SWP_NOMOVE);
            }

            public bool PutToCenter(bool aboveTaskbar = false)
            {
                return currentWindow.SetPosition(
                      (ScreenWidth - Width) / 2,
                      ((aboveTaskbar ? WorkingAreaHeight : ScreenHeight) - Height) / 2,
                      Width, Height
                  );
            }

            public async Task MakeBorderless()
            {
                var windowStyle = GetWindowStyle<WindowStyles>();

                windowStyle.Set(WindowStyles.WS_SIZEBOX, false);
                windowStyle.Set(WindowStyles.WS_CAPTION, false);

                await SetWindowStyleAsync(windowStyle);

                SetPosition(0, 0, ScreenWidth, ScreenHeight);
            }

            public bool FindWindow()
            {
                Window window = null;

                var windowList = GetWindowList();

                var windows = windowList.Where(x => x.Title == Title);
                if (!windows.Any())
                {
                    windows = windowList.Where(x => x.ProcessName == ProcessName && x.Class == Class);
                }

                if (windows.Count() == 1)
                {
                    window = windows.First();
                }
                else if (windows.Count() > 1)
                {
                    windows = windows.Where(x => x.ProcessName == ProcessName && x.Class == Class);

                    if (windows.Count() == 1)
                    {
                        window = windows.First();
                    }
                    else if (windows.Count() > 1)
                    {
                        window = windows.Where(x => x.Handle == Handle).FirstOrDefault() ?? windows.First();
                    }
                }

                if (window != null)
                {
                    ProcessPath = window.ProcessPath;
                    Pid = window.Pid;
                    Title = window.Title;
                    Class = window.Class;
                    Handle = window.Handle;
                    return true;
                }
                else return false;
            }
        }

        public class WindowStyle<T> where T : Enum
        {
            public int Style { get; set; }

            public WindowStyle() { }
            public WindowStyle(int style) => Style = style;

            public bool Is(T style)
            {
                var s = Convert.ToInt32(style);
                return (Style & s) == s;
            }

            public void Set(T style, bool value)
            {
                var s = Convert.ToInt32(style);
                Style = value ? Style | s : (Style & ~s);
            }
        }

        public enum WindowStyles
        {
            WS_MAXIMIZE = 0x01000000,
            WS_MINIMIZE = 0x20000000,
            WS_CAPTION = 0x00C00000,
            WS_SYSMENU = 0x00080000,
            WS_SIZEBOX = 0x00040000,
            WS_MAXIMIZEBOX = 0x00010000,
            WS_MINIMIZEBOX = 0x00020000,
            WS_VISIBLE = 0x10000000,
            WS_DISABLED = 0x08000000,
        }

        public enum WindowExStyles
        {
            WS_EX_TOPMOST = 0x00000008,
            WS_EX_TOOLWINDOW = 0x00000080,
            WS_EX_WINDOWEDGE = 0x00000100,
            WS_EX_CLIENTEDGE = 0x00000200,
            WS_EX_APPWINDOW = 0x00040000,
        }

        public class DesktopProfile
        {
            public string Name { get; set; }
            public List<Window> Windows { get; set; } = new List<Window>();
        }

        public class Config
        {
            public List<Window> SavedWindows { get; set; } = new List<Window>();
            public List<DesktopProfile> DesktopProfiles { get; set; } = new List<DesktopProfile>();

            public const string ConfigFilePath = "appsizerGUI_config.xml";

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

        public static int SaveDesktop(string profileName)
        {
            config.Reload();

            var windows = GetWindowList();
            windows.ForEach(w => w.GetPosition());

            var profile = new DesktopProfile
            {
                Name = profileName,
                Windows = windows
            };

            var extstingProfileIndex = config.DesktopProfiles.FindIndex(x => x.Name == profileName);

            if (extstingProfileIndex >= 0)
            {
                config.DesktopProfiles[extstingProfileIndex] = profile;
            }
            else
            {
                config.DesktopProfiles.Add(profile);
            }

            config.Save();
            return windows.Count;
        }

        public static (int windowCount, int successCount) RestoreDesktop(string profileName)
        {
            var profile = config.DesktopProfiles.First(x => x.Name == profileName);
            int success = 0;

            foreach (var window in profile.Windows)
            {
                if (window.FindWindow() &&
                    window.SetPosition())
                    success++;
            }

            return (profile.Windows.Count, success);
        }

        public static void RenameDesktop(string profileName, string newName)
        {
            config.Reload();
            config.DesktopProfiles.First(x => x.Name == profileName).Name = newName;
            config.Save();
        }

        public static void DeleteDesktop(string profileName)
        {
            config.Reload();
            config.DesktopProfiles.RemoveAll(x => x.Name == profileName);
            config.Save();
        }
    }
}
