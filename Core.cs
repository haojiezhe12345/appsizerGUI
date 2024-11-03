﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
            public string ProcessPath { get; set; }
            [XmlIgnore]
            public string ProcessName => Path.GetFileName(ProcessPath);
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
            public Rect Border { get; set; }

            [XmlIgnore]
            public bool IsValid => IsWindow(Handle);

            public bool GetPosition()
            {
                if (GetWindowRect(Handle, out Rect windowRect) && GetClientRect(Handle, out Rect clientRect))
                {
                    var windowStyle = GetWindowLong(Handle, GWL_STYLE);

                    if ((windowStyle & WS_MINIMIZE) == WS_MINIMIZE)
                    {
                        Border = new Rect { Top = 0, Left = 0, Right = 0, Bottom = 0 };
                    }
                    else
                    {
                        var windowWidth = windowRect.Right - windowRect.Left;
                        var clientWidth = clientRect.Right - clientRect.Left;
                        var borderWidth = (windowWidth - clientWidth) / 2;

                        if ((windowStyle & WS_MAXIMIZE) == WS_MAXIMIZE)
                        {
                            Border = new Rect { Top = borderWidth, Left = borderWidth, Right = borderWidth, Bottom = borderWidth };
                        }
                        else
                        {
                            borderWidth = borderWidth * 7 / 8;
                            Border = new Rect { Top = 0, Left = borderWidth, Right = borderWidth, Bottom = borderWidth };
                        }

                    }

                    X = windowRect.Left + Border.Left;
                    Y = windowRect.Top + Border.Top;
                    Width = windowRect.Right - windowRect.Left - Border.Left - Border.Right;
                    Height = windowRect.Bottom - windowRect.Top - Border.Top - Border.Bottom;

                    return true;
                }
                else return false;
            }

            public bool SetPosition(int x, int y, int width, int height)
            {
                return SetWindowPos(Handle, IntPtr.Zero, x - Border.Left, y - Border.Top, width + Border.Left + Border.Right, height + Border.Top + Border.Bottom, SWP_NOZORDER) && GetPosition();
            }

            public bool SetPosition()
            {
                return SetPosition(X, Y, Width, Height);
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
                        var process = processes.FirstOrDefault(p => p.Id == pid);

                        var ProcessPath = new StringBuilder(1024);
                        {
                            var readSize = ProcessPath.Capacity;
                            QueryFullProcessImageName(process.Handle, 0, ProcessPath, ref readSize);
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
    }
}
