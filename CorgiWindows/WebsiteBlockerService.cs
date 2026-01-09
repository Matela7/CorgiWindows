using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CorgiWindows
{
    public class WebsiteBlockerService
    {
        private readonly WebsiteBlockerModel _model;
        private Thread? _monitoringThread;
        private bool _isRunning;
        private CancellationTokenSource? _cancellationTokenSource;

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

        public WebsiteBlockerService(WebsiteBlockerModel model)
        {
            _model = model ?? throw new ArgumentNullException(nameof(model));
        }

        public void RunThread()
        {
            if (_isRunning)
            {
                return;
            }

            Debug.WriteLine("Starting website blocker thread...");
            Debug.WriteLine($"Blocked websites count: {_model.BlockedWebsites.Count}");
            
            _isRunning = true;
            _cancellationTokenSource = new CancellationTokenSource();
            _monitoringThread = new Thread(() => MonitorBrowsers(_cancellationTokenSource.Token))
            {
                IsBackground = true
            };
            _monitoringThread.Start();
        }

        public void StopThread()
        {
            if (!_isRunning)
            {
                return;
            }

            Debug.WriteLine("Stopping website blocker thread...");
            
            _isRunning = false;
            _cancellationTokenSource?.Cancel();
            _monitoringThread?.Join(1000);
            _cancellationTokenSource?.Dispose();
            _cancellationTokenSource = null;
            _monitoringThread = null;
        }

        private void MonitorBrowsers(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    string windowTitle = GetActiveWindowTitle();
                    
                    if (!string.IsNullOrWhiteSpace(windowTitle) && IsBlockedWebsiteOpen(windowTitle))
                    {
                        OpenCorgiOrgyPopup();
                        Thread.Sleep(500);
                    }

                    Thread.Sleep(100);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error in monitoring thread: {ex.Message}");
                }
            }
        }

        private List<Process> GetBrowserProcesses()
        {
            var browserNames = new[] { "chrome", "firefox", "msedge", "opera", "brave" };
            var processes = new List<Process>();

            foreach (var browserName in browserNames)
            {
                processes.AddRange(Process.GetProcessesByName(browserName));
            }

            return processes;
        }

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, buff, nChars) > 0)
            {
                return buff.ToString();
            }

            return string.Empty;
        }

        private bool IsBlockedWebsiteOpen(string windowTitle)
        {
            if (string.IsNullOrWhiteSpace(windowTitle))
            {
                return false;
            }

            Debug.WriteLine($"Checking window title: {windowTitle}");
            
            foreach (var blockedSite in _model.BlockedWebsites)
            {
                Debug.WriteLine($"Comparing with blocked site: {blockedSite.Url}");
                
                if (windowTitle.Contains(blockedSite.Url, StringComparison.OrdinalIgnoreCase))
                {
                    Debug.WriteLine($"BLOCKED! Found match for: {blockedSite.Url}");
                    return true;
                }
            }

            return false;
        }

        private void OpenCorgiOrgyPopup()
        {
            try
            {
                string url = "https://corgiorgy.com";
                Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error opening corgi orgy popup: {ex.Message}");
            }
        }
    }
}
