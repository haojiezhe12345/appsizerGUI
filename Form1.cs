using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appsizerGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public static IntPtr getWinHandle(string wName)
        {
            foreach (Process pList in Process.GetProcesses())
                if (pList.MainWindowTitle == wName)
                    return pList.MainWindowHandle;
            MessageBox.Show("Window not found");
            return IntPtr.Zero;
        }
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        public struct Rect
        {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
        }
        IntPtr handle;
        int a = 7;
        int taskbarHeight = 30;
        public void updateRightBottom()
        {
            int screenwidth = Screen.PrimaryScreen.Bounds.Width;
            int screenheight = Screen.PrimaryScreen.Bounds.Height;
            r.Text = (screenwidth - x.Value - w.Value).ToString();
            b.Text = (screenheight - y.Value - h.Value).ToString();
        }
        public void removeValueChangedHandler()
        {
            x.ValueChanged -= new EventHandler(setPos);
            y.ValueChanged -= new EventHandler(setPos);
            w.ValueChanged -= new EventHandler(setPos);
            h.ValueChanged -= new EventHandler(setPos);
        }
        public void addValueChangedHandler()
        {
            x.ValueChanged += new EventHandler(setPos);
            y.ValueChanged += new EventHandler(setPos);
            w.ValueChanged += new EventHandler(setPos);
            h.ValueChanged += new EventHandler(setPos);
        }
        private void refreshPos(object sender, EventArgs e)
        {
            removeValueChangedHandler();
            string windowname = window.Text;
            handle = getWinHandle(windowname);
            Rect pos = new Rect();
            GetWindowRect(handle, ref pos);
            x.Value = pos.Left + a;
            y.Value = pos.Top;
            w.Value = pos.Right - pos.Left - a * 2;
            h.Value = pos.Bottom - pos.Top - a;
            updateRightBottom();
            addValueChangedHandler();
        }
        [DllImport("user32.dll")]
        static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
        private void setPos(object sender, EventArgs e)
        {
            int left = (int)x.Value;
            int top = (int)y.Value;
            int width = (int)w.Value;
            int height = (int)h.Value;
            updateRightBottom();
            SetWindowPos(handle, IntPtr.Zero, left - a, top, width + a * 2, height + a, 0);
        }
        private void centerWin(object sender, EventArgs e)
        {
            removeValueChangedHandler();
            int screenwidth = Screen.PrimaryScreen.Bounds.Width;
            int screenheight = Screen.PrimaryScreen.Bounds.Height;
            int width = (int)w.Value;
            int height = (int)h.Value;
            int left = (screenwidth - width) / 2;
            int top = (screenheight - height - taskbarHeight) / 2;
            x.Value = left;
            y.Value = top;
            updateRightBottom();
            SetWindowPos(handle, IntPtr.Zero, left - a, top, width + a * 2, height + a, 0);
            addValueChangedHandler();
        }
        private void listWin(object sender, EventArgs e)
        {
            window.Items.Clear();
            foreach (Process pList in Process.GetProcesses())
                if (!String.IsNullOrEmpty(pList.MainWindowTitle))
                    window.Items.Add(pList.MainWindowTitle);
        }
        private void exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void toggleCalibrate(object sender, EventArgs e)
        {
            if (calibrate.Checked == true)
                a = 7;
            else
                a = 0;
            refreshPos(null, null);
        }
        private void toggleAboveTaskbar(object sender, EventArgs e)
        {
            if (abovetaskbar.Checked == true)
                taskbarHeight = 30;
            else
                taskbarHeight = 0;
        }
    }
}
