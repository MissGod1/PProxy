using System.Diagnostics;
using System;

namespace AwesomeProject.Controller
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
            p.StartInfo.FileName = "AwesomeProject-cli.exe";
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
