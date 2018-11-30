using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Windows;

namespace FolderIconChangerWPF
{
    public static class ApplicationSingleInstance
    {
        static Mutex mutex;
        const int SW_RESTORE = 9;

        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int IsIconic(IntPtr hWnd);

        /// <summary>
		/// GetCurrentInstanceWindowHandle
		/// </summary>
		/// <returns></returns>
		private static IntPtr GetCurrentInstanceWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                // Get the first instance that is not this instance, has the
                // same process name and was started from the same file name
                // and location. Also check that the process has a valid
                // window handle in this session to filter out other user's
                // processes.
                if (_process.Id != process.Id &&
                    _process.MainModule.FileName == process.MainModule.FileName &&
                    _process.MainWindowHandle != IntPtr.Zero)
                {
                    hWnd = _process.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }
        /// <summary>
        /// SwitchToCurrentInstance
        /// </summary>
        public static void SwitchToCurrentInstance()
        {
            IntPtr hWnd = GetCurrentInstanceWindowHandle();
            if (hWnd != IntPtr.Zero)
            {
                // Restore window if minimised. Do not restore if already in
                // normal or maximised window state, since we don't want to
                // change the current state of the window.
                if (IsIconic(hWnd) != 0)
                {
                    ShowWindow(hWnd, SW_RESTORE);
                }

                // Set foreground window.
                SetForegroundWindow(hWnd);
            }
        }

        /// <summary>
        /// Checks if current application is already running or not
        /// </summary>
        /// <param name="Shutdown">Shutdown (Exit) the application if it already running</param>
        /// <param name="switchToCurrentInstance">Switch To Current Instance if it already running</param>
        /// <returns></returns>
        public static bool IsAlreadyRunning(bool Shutdown = true, bool switchToCurrentInstance = true)
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;
            bool bCreatedNew;

            mutex = new Mutex(true, "Global\\" + sExeName, out bCreatedNew);
            if (bCreatedNew) mutex.ReleaseMutex();
            var res = !bCreatedNew;

            if (res)
            {
                if (switchToCurrentInstance) SwitchToCurrentInstance();
                if (Shutdown) Application.Current.Shutdown();
            }
            return res;
        }

        /// <summary>
        /// Checks if current application is already running or not
		/// </summary>
		/// <returns>returns true if already running</returns>
        public static Task<bool> IsAlreadyRunningAsync(bool ShutdownAndSwitchToCurrentInstance = true)
        {
            return Task.Run(() => IsAlreadyRunning(ShutdownAndSwitchToCurrentInstance));
        }
    }
}
