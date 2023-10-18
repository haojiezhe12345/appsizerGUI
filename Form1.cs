using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
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
            Cursor.Current = Cursors.WaitCursor;
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
        int borderWidth = 7;
        //int taskbarHeight = 30;
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
            calibrate.CheckedChanged -= new EventHandler(toggleCalibrate);
        }
        public void addValueChangedHandler()
        {
            x.ValueChanged += new EventHandler(setPos);
            y.ValueChanged += new EventHandler(setPos);
            w.ValueChanged += new EventHandler(setPos);
            h.ValueChanged += new EventHandler(setPos);
            calibrate.CheckedChanged += new EventHandler(toggleCalibrate);
        }
        private static List<string> GetSetting(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            JavaScriptSerializer js = new JavaScriptSerializer();
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                value = "[]";
            string[] array = js.Deserialize<string[]>(value);
            return array.OfType<string>().ToList();
        }
        private static void SetSetting(string key, List<string> value)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(value);
            Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            config.AppSettings.Settings.Remove(key);
            config.AppSettings.Settings.Add(key, json);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }
        private void refreshPos(object sender, EventArgs e)
        {
            removeValueChangedHandler();
            if (sender == null)
            {
                handle = getWinHandle(window.Text);
            }
            Rect pos = new Rect();
            GetWindowRect(handle, ref pos);
            x.Value = pos.Left + borderWidth;
            y.Value = pos.Top;
            w.Value = pos.Right - pos.Left - borderWidth * 2;
            h.Value = pos.Bottom - pos.Top - borderWidth;
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
            SetWindowPos(handle, IntPtr.Zero, left - borderWidth, top, width + borderWidth * 2, height + borderWidth, 0);
        }
        private void centerWin(object sender, EventArgs e)
        {
            removeValueChangedHandler();
            int screenwidth;
            int screenheight;
            if (abovetaskbar.Checked)
            {
                screenwidth = Screen.PrimaryScreen.WorkingArea.Width;
                screenheight = Screen.PrimaryScreen.WorkingArea.Height;
            }
            else
            {
                screenwidth = Screen.PrimaryScreen.Bounds.Width;
                screenheight = Screen.PrimaryScreen.Bounds.Height;
            }
            int width = (int)w.Value;
            int height = (int)h.Value;
            int left = (screenwidth - width) / 2;
            int top = (screenheight - height) / 2;
            x.Value = left;
            y.Value = top;
            updateRightBottom();
            SetWindowPos(handle, IntPtr.Zero, left - borderWidth, top, width + borderWidth * 2, height + borderWidth, 0);
            addValueChangedHandler();
        }
        private void listWin(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            winlist.DropDownItems.Clear();
            foreach (Process pList in Process.GetProcesses())
                if (!String.IsNullOrEmpty(pList.MainWindowTitle))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    item.Text = pList.MainWindowTitle;
                    item.Tag = pList.MainWindowHandle;
                    item.Click += new EventHandler(selectWin);
                    winlist.DropDownItems.Add(item);
                }
        }
        private void selectWin(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;
            window.Text = item.Text;
            handle = (IntPtr)item.Tag;
            refreshPos(0, null);
        }
        private void exit(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void toggleCalibrate(object sender, EventArgs e)
        {
            if (calibrate.Checked == true)
                borderWidth = 7;
            else
                borderWidth = 0;
            refreshPos(0, null);
        }
        private void toggleAboveTaskbar(object sender, EventArgs e)
        {
            /*
            if (abovetaskbar.Checked == true)
                taskbarHeight = 30;
            else
                taskbarHeight = 0;
            */
        }
        private void saveWin(object sender, EventArgs e)
        {
            if (window.Text == "savedWindows")
            {
                MessageBox.Show("Saving window named \"savedWindows\" is not supported");
                return;
            }
            Cursor.Current = Cursors.WaitCursor;
            var savedWindows = GetSetting("savedWindows");
            savedWindows.Add(window.Text);
            savedWindows = savedWindows.Distinct().ToList();
            SetSetting("savedWindows", savedWindows);
            List<string> winSetting = new List<string>();
            winSetting.Add(((int)x.Value).ToString());
            winSetting.Add(((int)y.Value).ToString());
            winSetting.Add(((int)w.Value).ToString());
            winSetting.Add(((int)h.Value).ToString());
            winSetting.Add(calibrate.Checked.ToString());
            SetSetting(window.Text, winSetting);
            System.Threading.Thread.Sleep(65);
            Cursor.Current = Cursors.Arrow;
        }
        private void listSavedWin(object sender, EventArgs e)
        {
            window.Items.Clear();
            var savedWindows = GetSetting("savedWindows");
            foreach (string wname in savedWindows)
            {
                window.Items.Add(wname);
            }
        }
        private void loadSavedWin(object sender, EventArgs e)
        {
            removeValueChangedHandler();
            var winSetting = GetSetting(window.Text);
            x.Value = Int32.Parse(winSetting[0]);
            y.Value = Int32.Parse(winSetting[1]);
            w.Value = Int32.Parse(winSetting[2]);
            h.Value = Int32.Parse(winSetting[3]);
            calibrate.Checked = bool.Parse(winSetting[4]);
            handle = getWinHandle(window.Text);
            if (calibrate.Checked == true)
                borderWidth = 7;
            else
                borderWidth = 0;
            updateRightBottom();
            addValueChangedHandler();
        }
        private void removeWin(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            var savedWindows = GetSetting("savedWindows");
            savedWindows = savedWindows.Distinct().ToList();
            savedWindows.Remove(window.Text);
            SetSetting("savedWindows", savedWindows);
            System.Threading.Thread.Sleep(40);
            Cursor.Current = Cursors.Arrow;
        }
    }
}
