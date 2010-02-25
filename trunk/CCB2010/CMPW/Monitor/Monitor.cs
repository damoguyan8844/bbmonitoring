using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace JOYFULL.CMPW.Monitor
{
    public class Monitor
    {
        Process _proc = null;
        public Monitor()
        {
        }

        public void Start()
        {
            //ProcessStartInfo info = new ProcessStartInfo();
            //info.FileName = "ping.exe";
            //info.Arguments = " " + this.txtServerIP.Text;
            //info.UseShellExecute = false;
            //info.RedirectStandardOutput = true;
            //info.CreateNoWindow = true;
            //Process proc = Process.Start( info );

            var psi = new ProcessStartInfo();
            psi.FileName = "TC1000.exe";
            psi.UseShellExecute = false;
            psi.RedirectStandardOutput = true;
            psi.CreateNoWindow = true;
            _proc = Process.Start( psi );
        }

        public void Close()
        {
            _proc.Close();
            _proc.Dispose();
        }
    }
}
