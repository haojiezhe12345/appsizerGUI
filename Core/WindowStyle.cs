using System;
using System.Threading.Tasks;
using static appsizerGUI.DLLImports;

namespace appsizerGUI.Core
{
    public class WindowStyle<T> where T : Enum
    {
        public uint Style { get; set; }

        public WindowStyle() => Style = 0;
        public WindowStyle(uint style) => Style = style;

        public bool Is(T style)
        {
            var s = Convert.ToUInt32(style);
            return (Style & s) == s;
        }

        public void Set(T style, bool value)
        {
            var s = Convert.ToUInt32(style);
            Style = value ? (Style | s) : (Style & ~s);
        }
    }

    public static class WindowStyleExtensions
    {
        public static bool HasBorder(this WindowStyle<WindowStyles> style) => style.Is(WindowStyles.WS_SIZEBOX) || style.Is(WindowStyles.WS_CAPTION);
        public static void SetBorder(this WindowStyle<WindowStyles> style, bool hasBorder) {
            style.Set(WindowStyles.WS_MAXIMIZEBOX, hasBorder);
            style.Set(WindowStyles.WS_MINIMIZEBOX, hasBorder);
            style.Set(WindowStyles.WS_SIZEBOX, hasBorder);
            style.Set(WindowStyles.WS_SYSMENU, hasBorder);
            style.Set(WindowStyles.WS_CAPTION, hasBorder);
        }
    }

    public enum WindowStyles : uint
    {
        WS_MAXIMIZEBOX = 0x0001_0000,
        WS_MINIMIZEBOX = 0x0002_0000,
        //WS_GROUP = 0x0002_0000,
        WS_SIZEBOX = 0x0004_0000,
        WS_SYSMENU = 0x0008_0000,
        WS_HSCROLL = 0x0010_0000,
        WS_VSCROLL = 0x0020_0000,
        //WS_DLGFRAME = 0x0040_0000,
        //WS_BORDER = 0x0080_0000,
        WS_CAPTION = 0x00C0_0000,
        WS_MAXIMIZE = 0x0100_0000,
        WS_CLIPCHILDREN = 0x0200_0000,
        WS_CLIPSIBLINGS = 0x0400_0000,
        WS_DISABLED = 0x0800_0000,
        WS_VISIBLE = 0x1000_0000,
        WS_MINIMIZE = 0x2000_0000,
        WS_CHILD = 0x4000_0000,
        WS_POPUP = 0x8000_0000,
    }

    public enum WindowExStyles : uint
    {
        WS_EX_TOPMOST = 0x00000008,
        WS_EX_TOOLWINDOW = 0x00000080,
        WS_EX_WINDOWEDGE = 0x00000100,
        WS_EX_CLIENTEDGE = 0x00000200,
        WS_EX_APPWINDOW = 0x00040000,
        WS_EX_NOACTIVATE = 0x08000000,
    }

    public partial class Window
    {
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

        public int SetWindowStyle<T>(WindowStyle<T> style) where T : Enum
        {
            var task = SetWindowStyleAsync(style);
            task.Wait();
            return task.Result;
        }
    }
}
