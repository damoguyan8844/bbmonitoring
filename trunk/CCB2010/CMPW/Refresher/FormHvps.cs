using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;

namespace JOYFULL.CMPW.Refresher
{
    public partial class FormHvps : Form
    {
        public FormHvps()
        {
            InitializeComponent();
        }
        System.Threading.Timer _timer = null;
        const int INTERVAL = 1000;
        const string TITLE = "CNAPS商业行前台业务系统";
        //const string TITLE = "new";

        private void btnStart_Click(object sender, EventArgs e)
        {
            string s = btnStart.Text;
            switch( s )
            {
                case "开始":
                    _timer = new System.Threading.Timer(
                        new TimerCallback( Send ), null, 0, INTERVAL);
                    btnStart.Text = "暂停";
                    break;
                case "暂停":
                    Pause();
                    break;
                case "恢复":
                    _timer = new System.Threading.Timer(
                        new TimerCallback( Send ), null, 0, INTERVAL );
                    btnStart.Text = "暂停";
                    break;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            if( _timer != null )
                _timer.Dispose();
            _timer = null;
            Close();
            Dispose();
            Application.ExitThread();
            Application.Exit();
            Process.GetCurrentProcess().Close();
        }

        private void Send( object obj )
        {
            string title = null;
            foreach (Process p in Process.GetProcesses())
            {
                if (p.MainWindowTitle.Contains(TITLE))
                {
                    title = p.MainWindowTitle;
                    break;
                }
            }
            if (string.IsNullOrEmpty(title))
            {
                string s = "应用程序{" + TITLE + "}未启动";
                _timer.Change(System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite);
                btnStart.Text = "恢复";
                MessageBox.Show(s);
                return;
            }

            KeySender.Send(title, Keys.PageDown, "");
            KeySender.Send(title, Keys.PageDown, "");
            KeySender.Send(title, Keys.PageDown, "");
            KeySender.Send(title, Keys.PageDown, "");
            KeySender.Send(title, Keys.PageDown, "");
            KeySender.Send( title, Keys.Up, "" );
            KeySender.Send( title, Keys.Up, "" );
            KeySender.Send( title, Keys.Up, "" );
            KeySender.Send( title, Keys.Up, "" );
            KeySender.Send( title, Keys.Up, "" );
        }

        private void FormHvps_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == 'p' || e.KeyChar == 'P' )
                Pause();
        }

        private void btnStart_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == 'p' || e.KeyChar == 'P' )
                Pause();
        }

        private void btnClose_KeyPress( object sender, KeyPressEventArgs e )
        {
            if ( e.KeyChar == 'p' || e.KeyChar == 'P' )
                Pause();
        }

        private void Pause()
        {
            if ( btnStart.Text == "恢复" )
                return;

            _timer.Change( System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite );
            btnStart.Text = "恢复";
        }

        private void FormHvps_Activated( object sender, EventArgs e )
        {
            btnClose.Focus();
        }
    }
}
