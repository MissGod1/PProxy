using System;
using System.Diagnostics;

namespace PProxy.Controller
{
    class ProcessController
    {
        private Process process;

        public ProcessController(EventHandler handler)
        {
            this.process = GetProcess(handler);
        }

        private Process GetProcess(EventHandler handler)
        {
            var p = new Process();
            p.StartInfo.FileName = "PProxy-cli.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.CreateNoWindow = true;
            p.EnableRaisingEvents = true;
            p.Exited += handler;
            return p;
        }

        public void Start()
        {
            process.Start();
        }

        public void Stop()
        {
            process.Kill();
        }
    }
}
