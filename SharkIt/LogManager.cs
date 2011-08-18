using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SharkIt
{
    public static class LogManager
    {
        public enum Level
        {
            Debug,
            Info,
            Warning,
            Error
        }

        private static object m_lock = new object();
        private static StreamWriter m_ts = File.CreateText("sharkit.log");

        public static void Log(Level lvl, string source, string msg)
        {
            lock(m_lock)
            {
                m_ts.WriteLine("[" + DateTime.Now + "] [" + lvl + "] [" + source + "] " + msg);
                m_ts.Flush();
            }
        }
    }
}
