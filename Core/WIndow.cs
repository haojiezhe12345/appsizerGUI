using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System;
using static appsizerGUI.Core.Core;
using static appsizerGUI.DLLImports;

namespace appsizerGUI.Core
{
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
        public bool IsMaximized { get; set; }
        [XmlIgnore]
        public int Right => ScreenWidth - X - Width;
        [XmlIgnore]
        public int Bottom => ScreenHeight - Y - Height;
        [XmlIgnore]
        public Rect Border { get; set; }
        [XmlIgnore]
        public bool IsMinimized { get; set; }

        [XmlIgnore]
        public bool IsValid => IsWindow(Handle);
        [XmlIgnore]
        public bool IsVisible => IsWindowVisible(Handle);
        [XmlIgnore]
        public bool HasBorder
        {
            get => currentWindow.GetWindowStyle<WindowStyles>().HasBorder();
            set
            {
                var style = currentWindow.GetWindowStyle<WindowStyles>();
                style.Set(WindowStyles.WS_MAXIMIZEBOX, value);
                style.Set(WindowStyles.WS_MINIMIZEBOX, value);
                style.Set(WindowStyles.WS_SIZEBOX, value);
                style.Set(WindowStyles.WS_SYSMENU, value);
                style.Set(WindowStyles.WS_CAPTION, value);
                currentWindow.SetWindowStyleAsync(style).Wait();
            }
        }

        public bool GetPosition()
        {
            if (GetWindowRect(Handle, out Rect windowRect) && GetClientRect(Handle, out Rect clientRect))
            {
                var windowStyle = GetWindowStyle<WindowStyles>();

                IsMaximized = windowStyle.Is(WindowStyles.WS_MAXIMIZE);
                IsMinimized = windowStyle.Is(WindowStyles.WS_MINIMIZE);

                GetWindowBorder(windowRect, clientRect, windowStyle);

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

        public void GetWindowBorder(Rect windowRect, Rect clientRect, WindowStyle<WindowStyles> windowStyle)
        {
            if (windowStyle.Is(WindowStyles.WS_MINIMIZE) || borderCalibrationMethod == BorderCalibrationMethod.Native)
            {
                Border = new Rect { Top = 0, Left = 0, Right = 0, Bottom = 0 };
            }
            else
            {
                var windowWidth = windowRect.Right - windowRect.Left;
                var windowHeight = windowRect.Bottom - windowRect.Top;

                var clientWidth = clientRect.Right - clientRect.Left;
                var clientHeight = clientRect.Bottom - clientRect.Top;

                var borderWidth = (windowWidth - clientWidth) / 2;

                if (borderCalibrationMethod == BorderCalibrationMethod.ClientArea)
                {
                    Border = new Rect { Top = windowHeight - clientHeight - borderWidth, Left = borderWidth, Right = borderWidth, Bottom = borderWidth };
                }
                else if (windowStyle.Is(WindowStyles.WS_MAXIMIZE))
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
                GetWindowBorder(windowRect, clientRect, GetWindowStyle<WindowStyles>());
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

        public bool QuickResize(int width, int height, bool aboveTaskbar = false)
        {
            return currentWindow.SetPosition(
                  (ScreenWidth - width) / 2,
                  ((aboveTaskbar ? WorkingAreaHeight : ScreenHeight) - height) / 2,
                  width, height
            );
        }

        public bool PutToCenter(bool aboveTaskbar = false)
        {
            return QuickResize(Width, Height, aboveTaskbar);
        }

        public async Task MakeBorderless(bool aboveTaskbar = false)
        {
            await SetWindowStyleAsync(new WindowStyle<WindowStyles>((uint)WindowStyles.WS_VISIBLE));
            await SetWindowStyleAsync(new WindowStyle<WindowExStyles>((uint)WindowExStyles.WS_EX_APPWINDOW));

            if (aboveTaskbar)
                SetPosition(0, 0, WorkingAreaWidth, WorkingAreaHeight);
            else
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
}
