using System;
using System.Collections.Generic;
using System.Linq;
using static appsizerGUI.DLLImports;

namespace appsizerGUI.Core
{
    public class DesktopProfile
    {
        public string Name { get; set; }
        public List<Window> Windows { get; set; } = new List<Window>();
    }

    public static partial class Core
    {
        public static Func<Window, Func<Window, bool>>[] windowMatchers = {
            w => x => x.ProcessName == w.ProcessName &&
                      x.Handle == w.Handle,

            w => x => x.ProcessName == w.ProcessName &&
                      x.Class == w.Class &&
                      x.Title == w.Title,

            w => x => x.ProcessName == w.ProcessName &&
                      x.Class == w.Class &&
                      x.ProcessName != "ApplicationFrameHost.exe",

            w => x => x.ProcessName == w.ProcessName &&
                      x.Title == w.Title,
        };

        public static (int newTotal, int updated) SaveDesktop(string profileName)
        {
            var windows = GetWindowList();

            windows.ForEach(x => x.GetPosition());

            windows = windows.Where(x => !(
                x.IsMinimized ||
                x.Class == "Progman" ||
                x.ProcessName == "TextInputHost.exe"
            )).ToList();

            var extstingProfile = config.DesktopProfiles.FirstOrDefault(x => x.Name == profileName);

            int newTotal = 0;

            if (extstingProfile != null)
            {
                extstingProfile.Windows.RemoveAll(x => windows.Any(y => x.ProcessName == y.ProcessName));
                extstingProfile.Windows.AddRange(windows);

                newTotal = extstingProfile.Windows.Count;
            }
            else
            {
                config.DesktopProfiles.Add(new DesktopProfile
                {
                    Name = profileName,
                    Windows = windows
                });
                newTotal = windows.Count;
            }

            config.Save();

            return (newTotal, windows.Count);
        }

        public static (int windowCount, int successCount) RestoreDesktop(string profileName)
        {
            var profile = config.DesktopProfiles.First(x => x.Name == profileName);
            int success = 0;

            var profileWindows = new List<Window>(profile.Windows);
            var desktopWindows = GetWindowList();

            foreach (var match in windowMatchers)
            {
                var restoredWindows = new List<Window>();

                foreach (var profileWindow in profileWindows)
                {
                    var desktopWindow = desktopWindows.FirstOrDefault(match(profileWindow));

                    if (desktopWindow == null || desktopWindow.Handle == IntPtr.Zero) continue;

                    desktopWindow.GetPosition();
                    if (desktopWindow.IsMinimized)
                    {
                        if (!ShowWindow(desktopWindow.Handle, ShowWindowParam.SW_SHOWNOACTIVATE)) continue;
                    }

                    profileWindow.Handle = desktopWindow.Handle;
                    if (profileWindow.IsMaximized)
                    {
                        if (!ShowWindow(profileWindow.Handle, ShowWindowParam.SW_SHOWMAXIMIZED)) continue;
                    }
                    else
                    {
                        if (!profileWindow.SetPosition()) continue;
                    }

                    desktopWindows.Remove(desktopWindow);
                    restoredWindows.Add(profileWindow);
                    success++;
                }

                profileWindows.RemoveAll(x => restoredWindows.Contains(x));
            }

            return (profile.Windows.Count, success);
        }

        public static void RenameDesktop(string profileName, string newName)
        {
            config.DesktopProfiles.First(x => x.Name == profileName).Name = newName;
            config.Save();
        }

        public static void DeleteDesktop(string profileName)
        {
            config.DesktopProfiles.RemoveAll(x => x.Name == profileName);
            config.Save();
        }

        public static int ShowAllMinimizedWindows()
        {
            int success = 0;

            foreach (var win in GetWindowList())
            {
                win.GetPosition();
                if (win.IsMinimized)
                {
                    if (ShowWindow(win.Handle, ShowWindowParam.SW_SHOWNOACTIVATE)) success++;
                }
            }

            return success;
        }
    }
}
