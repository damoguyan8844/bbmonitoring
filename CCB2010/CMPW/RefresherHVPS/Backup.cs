//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Threading;
//using System.Diagnostics;
//using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;

//using JOYFULL.CMPW.Data;
//using JOYFULL.CMPW.Data.Access;

//namespace JOYFULL.CMPW.Refresher
//{
//    public partial class Form1 : Form
//    {
//        [Serializable]
//        internal class Settings
//        {
//            public string AppTitle { get; set; }
//            public int Interval { get; set; }
//            public Keys KeyData { get; set; }
//            public string KeyDesc { get; set; }
//        }

//        Settings S { get; set; }
//        Keys _keyData = Keys.None;
//        string[] menuItems = new string[]{
//            "停止", "退出", "显示主窗体"
//        };

//        public Form1()
//        {
//            InitializeComponent();

//            # region Initialize Settings
//            BinaryFormatter fmt = new BinaryFormatter();
//            string fileName = "Settings.dat";
//            if ( !File.Exists( fileName ) )
//            {
//                Settings s = new Settings();
//                s.AppTitle = ""; s.Interval = 60; s.KeyDesc = ""; s.KeyData = Keys.None;
//                StreamWriter sw = new StreamWriter( fileName );
//                fmt.Serialize( sw.BaseStream, s );
//                sw.Close();
//            }

//            StreamReader sr = new StreamReader( fileName );
//            S = fmt.Deserialize( sr.BaseStream ) as Settings;
//            sr.Close();
//            # endregion

//            # region Initialize Controls
//            notifyIcon.Icon = resource.keyboard;
//            txtSpan.TextChanged += new EventHandler( txtSpan_TextChanged );
//            txtSpan.Leave += new EventHandler( txtSpan_Leave );
//            foreach ( string item in menuItems )
//            {
//                mnuContextMenu.Items.Add( item );
//            }
//            mnuContextMenu.ItemClicked +=
//                new ToolStripItemClickedEventHandler( mnuContextMenu_ItemClicked );

//            foreach ( Process p in Process.GetProcesses() )
//            {
//                if ( !string.IsNullOrEmpty( p.MainWindowTitle ) )
//                    cbxApps.Items.Add( p.MainWindowTitle );
//            }
//            //cbxApps.Items.AddRange(
//            //        (from p in Process.GetProcesses()
//            //        where !string.IsNullOrEmpty( p.MainWindowTitle )
//            //        select p.MainWindowTitle).ToArray()
//            //    );
//            cbxApps.SelectedIndexChanged +=
//                ( ( object sender, EventArgs arg ) =>
//                {
//                    btnApply.Enabled = true;
//                    S.AppTitle = cbxApps.Text;
//                } );

//            if ( cbxApps.Items.Contains( S.AppTitle ) )
//            {
//                cbxApps.SelectedIndex = cbxApps.Items.IndexOf( S.AppTitle );
//            }
//            txtSpan.Text = S.Interval.ToString();
//            txtKey.Text = S.KeyDesc;
//            btnApply.Enabled = false;
//            # endregion
//        }

//        void txtSpan_TextChanged( object sender, EventArgs e )
//        {
//            txtSpan_Leave( null, null );
//        }

//        void txtSpan_Leave( object sender, EventArgs e )
//        {
//            int span = 0;
//            if ( int.TryParse( txtSpan.Text, out span ) )
//            {
//                if ( span > 3600 )
//                {
//                    MessageBox.Show( "定时器间隔不应大于60分钟" );
//                }
//                else if ( span < 1 )
//                {
//                    MessageBox.Show( "定时器间隔不应小于1秒钟" );
//                }
//                else if ( span != S.Interval )
//                {
//                    btnApply.Enabled = true;
//                }
//                return;
//            }
//            MessageBox.Show( "定时器间隔设置错误，请输入1~3600之间的整数" );
//        }

//        void mnuContextMenu_ItemClicked( object sender, ToolStripItemClickedEventArgs e )
//        {
//            ToolStripItem itemClicked = e.ClickedItem;
//            switch ( itemClicked.Text )
//            {
//                case "停止":
//                    itemClicked.Text = "启动";
//                    break;
//                case "启动":
//                    itemClicked.Text = "停止";
//                    break;
//                case "退出":
//                    break;
//                case "显示主窗体":
//                    break;
//            }
//        }

//        private void btnHide_Click( object sender, EventArgs e )
//        {

//        }


//        private void btnKey_Click( object sender, EventArgs e )
//        {
//            FrmKey frm = new FrmKey();
//            DialogResult result = frm.ShowDialog();
//            if ( result == DialogResult.OK )
//            {
//                _keyData = frm.KeyData;
//                txtKey.Text = frm.Description;
//                btnApply.Enabled = true;
//            }
//            frm.Dispose();
//        }

//        private void btnApply_Click( object sender, EventArgs e )
//        {
//            Settings s = new Settings();
//            s.AppTitle = cbxApps.Text;
//            s.Interval = int.Parse( txtSpan.Text );
//            s.KeyDesc = txtKey.Text;
//            s.KeyData = _keyData;
//            btnApply.Enabled = false;
//            BinaryFormatter fmt = new BinaryFormatter();
//            StreamWriter sw = new StreamWriter( "Settings.dat" );
//            fmt.Serialize( sw.BaseStream, s );
//            sw.Close();
//            S = s;
//        }
//        private void ResetTimer( int intervalInSeconds )
//        {
//            timer1.Interval = intervalInSeconds * 1000;
//            timer1.Start();
//        }

//        private void timer1_Tick( object sender, EventArgs e )
//        {
//            //SendKey.SendKeys.WINFOCUS w = SendKey.SendKeys.GetWinHandles1( cbxApps.Text );
//            //SendKey.SendKeys.Send( "a", w, true, false );
//            string error = KeySender.Send( S.AppTitle, S.KeyData, S.KeyDesc );
//            if ( !string.IsNullOrEmpty( error ) )
//            {
//                btnStart_Click( null, null );
//                MessageBox.Show( error );
//            }
//        }

//        private void btnStart_Click( object sender, EventArgs e )
//        {
//            if ( btnStart.Text == "开始" )
//            {
//                btnStart.Text = "暂停";
//                cbxApps.SelectedIndex = cbxApps.Items.IndexOf( S.AppTitle );
//                txtKey.Text = S.KeyDesc;
//                txtSpan.Text = S.Interval.ToString();
//                btnApply.Enabled = cbxApps.Enabled = btnKey.Enabled = txtSpan.Enabled = false;
//                ResetTimer( S.Interval );
//            }
//            else
//            {
//                btnStart.Text = "开始";
//                cbxApps.Enabled = btnKey.Enabled = txtSpan.Enabled = true;
//                timer1.Stop();
//            }
//        }

//        private void button1_Click( object sender, EventArgs e )
//        {
//            DbCore dbCore = new DbCore( "CMPW" );
//            //string sql = "SELECT * FROM Users";
//            //DBCommandWrapper cmd = 
//            //    dbCore.GetSqlStringCommandWrapper( sql ) as AccessCommandWrapper;
//            //DataSet ds = dbCore.ExecuteDataSet( cmd );
//            //var o = dbCore.ExecuteScalar( cmd );
//            dbCore.BeginTransaction();
//            try
//            {
//                for ( int i = 3; i < 10; ++i )
//                {
//                    string sql = "INSERT INTO USERS VALUES( " + i.ToString() +
//                        ", 'Shot', 'IBM', 'US' )";
//                    int num = dbCore.ExecuteNonQuery( CommandType.Text, sql );
//                    if ( num != 1 )
//                    {
//                        dbCore.RollbackTransaction();
//                    }
//                }
//                dbCore.CommitTransaction();
//            }
//            catch
//            {
//                dbCore.RollbackTransaction();
//            }
//        }
//    }
//}
