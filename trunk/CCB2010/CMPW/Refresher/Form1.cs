using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;


/*
 * ccbs刷新操作
 * 日间  Esc --> 1 --> 0306 --> Enter --> 1 等待5分钟(可设置)
 * 日终 Esc --> 1 --> 0306 --> Enter --> 1 --> F4 --> F4 等待8秒
 * 
 * 
 */

namespace JOYFULL.CMPW.Refresher
{
    public partial class Form1 : Form
    {

        const string TITLE = "业务1";

        System.Threading.Timer _timer = null;
        Thread _thdCheckOver = null;
        bool _refreshing = true;

        int _interval = 1;
        DateTime _over;
        string _app = null;

        public Form1( )
        {
            InitializeComponent();
            InitCbxApp();
            _over = DateTime.Today.AddHours( 20 );
            for( int i = 0; i < cbxApps.Items.Count; ++i )
            {
                var o = cbxApps.Items[ i ].ToString();
                if( o.ToString().Contains( TITLE ) )
                {
                    cbxApps.SelectedIndex = i;
                    break;
                }
            }
            //var item = cbxApps.Items[ 0 ];
            //item.
            //if ( cbxApps.Items.Contains( TITLE ) )
            //{
            //    MessageBox.Show( "found" );
            //    cbxApps.SelectedText = TITLE;
            //}
        }

        private void InitCbxApp()
        {
            cbxApps.Items.Clear();
            foreach ( Process p in Process.GetProcesses() )
            {
                if ( !string.IsNullOrEmpty( p.MainWindowTitle ) )
                    cbxApps.Items.Add( p.MainWindowTitle );
            }
        }

        bool ValidateInterval()
        {
            string info = string.Empty;
            int span = 0;
            if( int.TryParse( txtSpan.Text, out span ) )
            {
                const int DAY_TIME = 24 * 60;
                if( span > DAY_TIME )
                {
                    info = "定时器间隔不应大于24小时";
                }
                else if( span < 1 )
                {
                    info = "定时器间隔不应小于1分钟";
                }
            }
            else
                info = "定时器间隔设置错误，请输入1~1440之间的整数";
            if( !string.IsNullOrEmpty( info ) )
            {
                MessageBox.Show( info );
                return false;
            }
            return true;
        }

        private bool ValidateOverTime()
        {
            string s = txtOverTime.Text;
            string info = string.Empty;
            int temp = 0;
            if ( !int.TryParse( s, out temp ) ||
                s.Length != 6 )
            {
                info = "日终时间设置错误，请重新输入";
                MessageBox.Show( info );
                txtOverTime.SelectAll();
                txtOverTime.Focus();
                return false;
            }
            return true;
        }

        private void btnApply_Click( object sender, EventArgs e )
        {
            if( btnStart.Text == "暂停" )
            {
                MessageBox.Show( "请先暂停程序再修改设置" );
                return;
            }
            
            bool bSet = btnApply.Text == "设置";
            txtSpan.Enabled = bSet;
            cbxApps.Enabled = bSet;
            txtOverTime.Enabled = bSet;
            if( bSet )
            {
                btnApply.Text = "应用";
                InitCbxApp();
            }
            else
            {
                if ( !ValidateOverTime() || !ValidateInterval() )
                    return;
                btnStart.Enabled = true;
                _app = cbxApps.Text;
                _interval = int.Parse( txtSpan.Text );
                string s = txtOverTime.Text;
                _over = DateTime.Today;
                _over = _over.AddHours( int.Parse( s.Substring( 0, 2 ) ) );
                _over = _over.AddMinutes( int.Parse( s.Substring( 2, 2 ) ) );
                _over = _over.AddSeconds( int.Parse( s.Substring( 4, 2 ) ) );
                btnApply.Text = "设置";
            }
        }


        private void btnStart_Click( object sender, EventArgs e )
        {
            string s = btnStart.Text;
            long period = _interval * 60 * 1000;
            if ( !_refreshing )
                period = 8000;
            switch( s )
            {
                case "开始":
                    _timer = new System.Threading.Timer(
                        new TimerCallback( Send ), null, 0, period);
                    _thdCheckOver = new Thread( new ThreadStart( CheckOver ) );
                    _thdCheckOver.Start();
                    btnStart.Text = "暂停";
                    break;
                case "暂停":
                    _timer.Change( System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite );
                    _timer = new System.Threading.Timer(
                        new TimerCallback( FadeSend ), null,
                        System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite );
                    if( _thdCheckOver.ThreadState == System.Threading.ThreadState.Running )
                        _thdCheckOver.Suspend();
                    btnStart.Text = "恢复";
                    break;
                case "恢复":
                    if( btnApply.Text == "应用")
                    {
                        MessageBox.Show( "请先应用新设置" );
                        return;
                    }
                    _timer = new System.Threading.Timer(
                        new TimerCallback( Send ), null, 0, period );
                    if( _thdCheckOver.ThreadState == System.Threading.ThreadState.Suspended )
                        _thdCheckOver.Resume();
                    btnStart.Text = "暂停";
                    break;
            }
        }

        private void btnClose_Click( object sender, EventArgs e )
        {
            //if( _timer != null )
            //{
            //    _timer = null;
            //}

            _timer = null;
            //if ( _thdCheckOver != null &&
            //    _thdCheckOver.ThreadState == System.Threading.ThreadState.Running )
            //{
            //    _thdCheckOver.Abort();
            //}
            //_thdCheckOver.Abort();
            try
            {
                _thdCheckOver.Abort();
            }
            catch ( System.Exception ex )
            {
            }
            _thdCheckOver = null;
            Close();
            Dispose();
            Application.ExitThread();
            Application.Exit();
            Process.GetCurrentProcess().Close();
        }

        private void CheckOver()
        {
            while( true )
            {
                if( DateTime.Now >= _over )
                {
                    _timer.Change( System.Threading.Timeout.Infinite,
                        System.Threading.Timeout.Infinite );
                    _timer = new System.Threading.Timer(
                        new TimerCallback( Send ), null, 0, 8000 );
                    //_timer.Change( 0, 8000 ); // 隔8秒
                    _refreshing = false;
                    break;
                }
                Thread.Sleep( 500 );
            }
        }

        private void Send( object obj )
        {
            string title = null;
            foreach( Process p in Process.GetProcesses() )
            {
                if( p.MainWindowTitle.Contains( _app ) )
                {
                    title = p.MainWindowTitle;
                    break;
                }
            }
            if( string.IsNullOrEmpty( title ) )
            {
                string s = "应用程序[" + _app+ "]未启动";
                MessageBox.Show( s );
                return;
            }
            
            //var focus = JOYFULL.CMPW.SendKey.SendKeys.GetWinHandles1( title );

            // Esc --> 1
            KeySender.Send( title, Keys.Escape, "" );
            Thread.Sleep( 100 );
            KeySender.Send( title, Keys.D1, "" );
            Thread.Sleep( 300 );

            // 0306
            KeySender.Send( title, Keys.Back, "" );
            KeySender.Send( title, Keys.Back, "" );
            KeySender.Send( title, Keys.Back, "" );
            KeySender.Send( title, Keys.Back, "" );
            KeySender.Send( title, Keys.D0, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D3, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D0, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D6, "" );

            Thread.Sleep( 300 );

            // 340 --> Enter
            //KeySender.Send( title, Keys.Back, "" );
            //KeySender.Send( title, Keys.Back, "" );
            //KeySender.Send( title, Keys.Back, "" );
            //KeySender.Send( title, Keys.Back, "" );
            //KeySender.Send( title, Keys.D3, "" );
            //KeySender.Send( title, Keys.D4, "" );
            //KeySender.Send( title, Keys.D0, "" );
            //Thread.Sleep( 100 );
            KeySender.Send( title, Keys.Enter, "" );
            Thread.Sleep( 200 );

            //1
            KeySender.Send( title, Keys.D1, "" );
            //KeySender.Send( title, Keys.Back, "" );
            Thread.Sleep( 700 );

            if( !_refreshing )
                KeySender.Send( title, Keys.F4, "" );
            Thread.Sleep( 20 );
            if( !_refreshing )
                KeySender.Send( title, Keys.F4, "" );
            Thread.Sleep( 300 );
            // move the cursor right for 5 times
            KeySender.Send( title, Keys.Right, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.Right, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.Right, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.Right, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.Right, "" );
        }

        //private void SendInput( string keys, JOYFULL.CMPW.SendKey.SendKeys.WINFOCUS focus )
        //{
        //    JOYFULL.CMPW.SendKey.SendKeys.Send( keys, focus, true, true );
        //}

        private void cbxApps_DropDown( object sender, EventArgs e )
        {
            InitCbxApp();
        }

        private void FadeSend( object obj )
        {
            // do nothing
        }
    }
}
