﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SharkIt
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new SharkIt());
            Application.Run(new Main());
        }
    }
}
