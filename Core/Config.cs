using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Script.Serialization;
using System.Xml.Serialization;
using System.Xml;
using static appsizerGUI.Core.Core;

namespace appsizerGUI.Core
{
    public class Config
    {
        public List<Window> SavedWindows { get; set; } = new List<Window>();
        public List<DesktopProfile> DesktopProfiles { get; set; } = new List<DesktopProfile>();
        public List<(int width, int height, string description)> CustomResolutions { get; set; } = new List<(int width, int height, string description)>();

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
                try
                {
                    return new Config
                    {
                        SavedWindows = GetSavedWindowsV1()
                    };
                }
                catch
                {
                    return new Config();
                }
            }
        }

        private static List<string> GetSettingV1(string key)
        {
            ConfigurationManager.RefreshSection("appSettings");
            JavaScriptSerializer js = new JavaScriptSerializer();
            var value = ConfigurationManager.AppSettings[key];
            if (value == null)
                value = "[]";
            string[] array = js.Deserialize<string[]>(value);
            return array.OfType<string>().ToList();
        }

        private static List<Window> GetSavedWindowsV1()
        {
            List<Window> result = new List<Window>();

            foreach (var windowName in GetSettingV1("savedWindows"))
            {
                try
                {
                    var windowV1 = GetSettingV1(windowName);
                    result.Add(new Window
                    {
                        Title = windowName,
                        X = Int32.Parse(windowV1[0]),
                        Y = Int32.Parse(windowV1[1]),
                        Width = Int32.Parse(windowV1[2]),
                        Height = Int32.Parse(windowV1[3]),
                    });
                }
                catch { }
            }

            return result;
        }
    }
}
