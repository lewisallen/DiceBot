using System;
using System.IO;

namespace DiceBot {
    public static class ConfigReader {
        public static string TOKEN;
        public static string PREFIX;

        public static void ReadConfig() {
            var reader = new StreamReader("config.cfg");
            var config = reader.ReadToEnd().Split('\n');
            foreach (string x in config) {
                if (x.ToLower().StartsWith("token=")) TOKEN = x.Substring(x.IndexOf("=")+1);
                else if (x.ToLower().StartsWith("prefix=")) PREFIX = x.Substring(x.IndexOf("=")+1);
                else throw new ArgumentException("Invalid configuration option.");
            }
        }
    }
}