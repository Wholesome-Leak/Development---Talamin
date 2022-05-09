﻿using robotManager.Helpful;
using System.Drawing;

namespace WholesomeDungeons.Helper
{
    public class Logger
    {
        public static void LogError(string message)
        {
            Logging.Write($"[{Main.ProductName}]: {message}", Logging.LogType.Error, Color.DarkRed);
        }

        public static void Log(string message)
        {
            Logging.Write($"[{Main.ProductName}]: {message}", Logging.LogType.Normal, Color.DarkSlateBlue);
        }

        public static void LogDebug(string message)
        {
            Logging.Write($"[{Main.ProductName}]: {message}", Logging.LogType.Debug, Color.DarkGoldenrod);
        }
    }
}
