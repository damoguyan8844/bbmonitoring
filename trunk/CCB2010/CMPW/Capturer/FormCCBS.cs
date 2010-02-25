using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.IO;


/*
 * ccbs截屏操作
 * 0307 --> Enter --> 1 --> capture screen --> F4 --> F4 --|
 *                           ^                             |
 *                           |          N times            |
 *                           |                             |
 *                           -------------------------------
 * 
 */

namespace JOYFUL.CMPW.Capturer
{
    public partial class FormCCBS : Form
    {

        const string TITLE = "业务1";

        Thread _thdCapture = null;
        public FormCCBS( )
        {
            InitializeComponent();
            InitCbxApp();
            InitPath();
            for( int i = 0; i < cbxApps.Items.Count; ++i )
            {
                var o = cbxApps.Items[ i ].ToString();
                if( o.ToString().Contains( TITLE ) )
                {
                    cbxApps.SelectedIndex = i;
                    break;
                }
            }
        }

        private void InitPath()
        {
            txtPath.Text = @"C:\CCBS截屏";
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



        private void btnStart_Click( object sender, EventArgs e )
        {
            DirectoryInfo di = new System.IO.DirectoryInfo( txtPath.Text );
            if ( !di.Exists )
            {
                try
                {
                    di.Create();
                }
                catch( Exception ex )
                {
                    MessageBox.Show( "文件保存路径[" + txtPath + "]错误，请检查" );
                    return;
                }
            }

            string app = cbxApps.SelectedItem.ToString();
            string title = null;
            foreach ( Process p in Process.GetProcesses() )
            {
                if ( p.MainWindowTitle.Contains( app ) )
                {
                    title = p.MainWindowTitle;
                    break;
                }
            }
            if ( string.IsNullOrEmpty( title ) )
            {
                string s = "应用程序[" + app + "]未启动";
                MessageBox.Show( s );

                return;
            }

            _thdCapture = new Thread( new ParameterizedThreadStart( Send ) );
            _thdCapture.Start( title );
            
        }

        private void btnBrowse_Click( object sender, EventArgs e )
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.RootFolder = Environment.SpecialFolder.MyComputer;
            dlg.ShowNewFolderButton = true;
            dlg.Description = "请选择截屏文件的保存目录";
            if( dlg.ShowDialog() == DialogResult.OK )
            {
                txtPath.Text = dlg.SelectedPath;
            }
        }

        private void btnClose_Click( object sender, EventArgs e )
        {
            try
            {
                if ( _thdCapture != null )
                {
                    if ( _thdCapture.ThreadState == System.Threading.ThreadState.Running )
                        _thdCapture.Abort();
                    _thdCapture = null;
                }
            }
            catch ( System.Exception ex )
            {
            }
            this.Close();
            this.Dispose();
            Process.GetCurrentProcess().Kill();
        }

        private void Send( object obj )
        {
            string title = obj.ToString();

            // 0307
            KeySender.Send( title, Keys.Back, "" );
            Thread.Sleep(20);
            KeySender.Send( title, Keys.Back, "" );
            Thread.Sleep(20);
            KeySender.Send( title, Keys.Back, "" );
            Thread.Sleep(20);
            KeySender.Send( title, Keys.Back, "" );
            Thread.Sleep(20);
            KeySender.Send( title, Keys.D0, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D3, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D0, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.D7, "" );

            Thread.Sleep( 1500 );

            KeySender.Send(title, Keys.Back, "");
            Thread.Sleep(20);
            KeySender.Send(title, Keys.Back, "");
            Thread.Sleep(20);
            KeySender.Send(title, Keys.Back, "");
            Thread.Sleep(20);
            KeySender.Send(title, Keys.Back, "");

            KeySender.Send( title, Keys.Enter, "" );
            Thread.Sleep( 1500 );

            //1
            KeySender.Send( title, Keys.D1, "" );
            Thread.Sleep( 20 );
            KeySender.Send( title, Keys.Back, "" );

            int index = 1;
            int number = int.Parse( txtNumber.Text );
            while( index <= number)
            {
                Thread.Sleep( 3000 );
                Bitmap bmp = CaptureScreen.GetDesktopImage();
                bmp.Save( txtPath.Text + "\\" + index.ToString() + ".bmp" );
                KeySender.Send( title, Keys.F4, "" );
                Thread.Sleep( 100 );
                KeySender.Send( title, Keys.F4, "" );
                ++index;
            } 
            ActivateThreadSafe();
            MessageBox.Show( "CCBS系统自动截屏完成" );
        }

        private delegate void ActivateCallback();
        private void ActivateThreadSafe()
        {
            if ( this.InvokeRequired )
            {
                ActivateCallback a = new ActivateCallback( ActivateThreadSafe );
                this.Invoke( a, null );
            }
            else
                this.Activate();
        }
        private void cbxApps_DropDown( object sender, EventArgs e )
        {
            InitCbxApp();
        }

        private void FadeSend( object obj )
        {
            // do nothing
        }

        private void txtNumber_TextChanged( object sender, EventArgs e )
        {
            string s = txtNumber.Text;
            int number = 0;
            if( !string.IsNullOrEmpty( s ) && 
                ( !int.TryParse( s, out number ) || number < 1 ) )
            {
                txtNumber.SelectAll();
                MessageBox.Show( "请输入大于1的整数" );
            }
        }
    }
}
