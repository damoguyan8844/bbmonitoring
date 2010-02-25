/*
 * http://www.codeproject.com/KB/WPF/ThreadSafeWPF.aspx
 * 
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Report;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// Monitor.xaml 的交互逻辑
    /// </summary>
    public partial class Monitor : Window
    {
        int _temp = -1;
        bool _autoSwitch = true;

        ImageManager _imgMgr;

        Key _keyPressed;
        Thread _thdCheck;
        FullWindow _fullWindow;

        public bool OnTop = true;

        List<int> _exQueue = new List<int>();
        List<int> _enabled = new List<int>();
        Dictionary<int, DateTime> _disabled = new Dictionary<int, DateTime>();

        Operator _operator;

        SystemsHandler.SystemsHandler _sysHandler;

        public Monitor( Operator op )
        {
            InitializeComponent();

            _operator = op;
            InitializeSysHandler();
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyUpEvent, new KeyEventHandler(Page_KeyUp), true);
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(Page_KeyDown), true);


            _imgMgr = new ImageManager( img,
                new Image[] { img0, img1, img2, img3, img4, img5, img6 }, 
                lblAlert, 
                new Label[] { lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6 },
                _sysHandler, UserSwitch );

            _thdCheck = new Thread(new ThreadStart(DoCheck));
            _thdCheck.Start();

        }

        private void Page_KeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.F4 )
            {
                _keyPressed = e.Key;
            }
        }

        private void Page_KeyUp( object sender, KeyEventArgs e )
        {
            if( OnTop && e.Key == _keyPressed )
            {
                if (e.Key == Key.Left)
                {
                    btnAutoSwitch.IsEnabled = false;
                    btnUserSwitch.IsEnabled = true;
                    _imgMgr.MoveLeft();
                }
                else if (e.Key == Key.Right)
                {
                    btnAutoSwitch.IsEnabled = false;
                    btnUserSwitch.IsEnabled = true;
                    _imgMgr.MoveRight();
                }
                else if (e.Key == Key.F4)
                {
                    _fullWindow = App.SwitchToWindow("FullWindow") as FullWindow;
                    _fullWindow.Initialize();
                    _fullWindow.ReplaceImage(_imgMgr.IndexMiddle);
                    OnTop = false;
                }
            }
            _keyPressed = Key.None;
        }
        private void DoSwitch(object o)
        {
            if (_autoSwitch)
            {
                _imgMgr.MoveLeft();
                int id = _imgMgr.IndexMiddle + 2;
                txtInfo.SetTextThreadSafe(
                                _sysHandler.GetSystemInfo(id));
            }
        }

        void TryResumeSuspendSystem()
        {
            foreach( KeyValuePair< int, DateTime> pair in _disabled )
            {
                if( pair.Value < DateTime.Now )
                {
                    _sysHandler.StartMonitor( pair.Key );
                }
            }
        }

        private void DoCheck()
        {
            int tick = 0;
            while( true )
            {
                try
                {
                    if (tick > 20000)
                    {
                        DoSwitch(null);
                        tick = 0;
                    }

                    TryResumeSuspendSystem();

                    int exSysId = _sysHandler.GetFirstExceptionSystemID();
                    const int NULL_EXCEPTION = -1;
                    if (exSysId == NULL_EXCEPTION)
                    {
                        if (_fullWindow != null)
                        {
                            _fullWindow.OnTop = false;
                            App.SwitchToWindow("Monitor"); // back to Monitor window
                            _fullWindow = null;
                        }
                        if (_exQueue.Count > 0)
                        {
                            foreach (int i in _exQueue)
                            {
                                //show removing message
                            }
                            _exQueue.Clear();
                        }
                    }
                    else
                    {
                        if (!_exQueue.Contains(exSysId))
                        {
                            //txtInfo.Text = _sysHandler.GetSystemInfo(exSysId);
                            txtInfo.SetTextThreadSafe(
                                _sysHandler.GetSystemInfo(exSysId));
                            _sysHandler.LanchAlert(exSysId);

                            _imgMgr.SetImage(exSysId);
                            _fullWindow = App.SwitchToWindow("FullWindow") as FullWindow;
                            _fullWindow.MaximizeThreadSafe();
                            _fullWindow.ReplaceImage(exSysId);
                            _fullWindow.Initialize();
                            _exQueue.Add(exSysId);
                        }
                        for (int i = 0; i < _sysHandler.SubSystemCount; ++i)
                        {
                            if (_sysHandler.IsSystemHasException(i))
                            {
                                if (!_exQueue.Contains(i))
                                {
                                    _exQueue.Add(i);
                                    //show adding message
                                }
                            }
                            else if (_exQueue.Contains(i))
                            {
                                _exQueue.Remove(i);
                                //show removing message
                            }
                        }
                    }
                    const int INTERNAL_CHECKING = 500;
                    Thread.Sleep(INTERNAL_CHECKING);
                    tick += 500;
                }
                catch (System.Exception e)
                {
                    log4net.LogManager.GetLogger( typeof(Monitor)).
                        Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);

                }

            }
        }

        private void InitializeSysHandler()
        {
            int[] ids = new int[] {
                1, 2, 3, 4, 5, 6, 7 ,8, 9, 10, 11, 12, 13, 14 };

            //int[] ids = new int[] { 11 };

            _enabled.AddRange( ids ); //所有系统监控启动，暂停监控下拉框应包含所有系统
            List<int> sysIDs = new List<int>(ids);
            _sysHandler = new SystemsHandler.SystemsHandler(sysIDs);

            _sysHandler.m_MonitorJob = App.MtJob;
            _sysHandler.m_Operator = App.Oper;

            foreach (int sysID in sysIDs)
            {
                _sysHandler.StartMonitor(sysID);
            }
        }


        # region UI events
        private void btnChangePs_Click( object sender, RoutedEventArgs e )
        {
            var frm = new FormChangePassword( _operator.Password );
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                _operator.Password = frm.Password;
                new OperatorDal().UpdateOperator( _operator );
            }
        }

        private void btnAutoSwitch_Click(object sender, RoutedEventArgs e)
        {
            _autoSwitch = true;
            btnAutoSwitch.IsEnabled = false;
            btnUserSwitch.IsEnabled = true;
        }

        private void btnUserSwitch_Click(object sender, RoutedEventArgs e)
        {
            UserSwitch();
        }

        private void UserSwitch()
        {
            _autoSwitch = false;
            btnAutoSwitch.IsEnabled = true;
            btnUserSwitch.IsEnabled = false;
            txtInfo.SetTextThreadSafe(
                _sysHandler.GetSystemInfo(_imgMgr.IndexMiddle + 1) );
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            string folder = "";
            //save image to folder
            //save text to folder
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            DoLogout();
            // create report files
            Window_Closed(null, null);
        }

        private void cbxDisable_DropDownOpened(object sender, EventArgs e)
        {

            cbxDisable.Items.Clear();
            foreach( int id in _enabled )
            {
                string sysName =
                    SystemsHandler.SystemsHandler.GetSystemName( id );
                cbxDisable.Items.Add( sysName );
            }
        }

        private void cbxEnable_DropDownOpened(object sender, EventArgs e)
        {
            cbxEnable.Items.Clear();
            foreach (KeyValuePair<int,DateTime> pair in _disabled)
            {
                int key = pair.Key;
                string sysName =
                    SystemsHandler.SystemsHandler.GetSystemName( key );
                cbxEnable.Items.Add( sysName );
            }
        }

        private void cbxDisable_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if (cbxDisable.SelectedItem == null)
                return;
            FormPause frm = new FormPause();
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                string name = cbxDisable.SelectedItem.ToString();
                int id = SystemsHandler.SystemsHandler.GetSystemId( name );
                switch( frm.ResumeType )
                {
                    case FormPause.ResumeTypeEnum.Auto:
                        int interval = frm.Interval; // in minute
                        _disabled.Add( id, DateTime.Now.AddMinutes( interval ) );
                        _sysHandler.StopMonitor(id, false);
                        break;
                    case FormPause.ResumeTypeEnum.Manual:
                        _disabled.Add( id, DateTime.Today.AddYears( 1 ) );
                        _sysHandler.StopMonitor(id, false);
                        break;
                    case FormPause.ResumeTypeEnum.Exit:
                        _sysHandler.StopMonitor(id, true);
                        _disabled.Add( id, DateTime.Today.AddYears( 1 ) );
                        break;
                }
                _enabled.Remove(id);
            }
            cbxDisable.SelectedItem = null;
        }

        private void cbxEnable_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            if( cbxEnable.SelectedItem != null )
            {
	            string name = cbxEnable.SelectedItem.ToString();
                int index = SystemsHandler.SystemsHandler.GetSystemId( name );
                _sysHandler.StartMonitor( index );
	            _enabled.Add(index);
                _enabled.Sort();
	            _disabled.Remove(index);
            }
            cbxEnable.SelectedItem = null;
        }
        # endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            _sysHandler.Close();
            _sysHandler = null;
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        private void img_MouseDown( object sender, MouseButtonEventArgs e )
        {
            if( e.ClickCount == 2 )
            {
                _fullWindow = App.SwitchToWindow( "FullWindow" ) as FullWindow;
                _fullWindow.Initialize();
                _fullWindow.ReplaceImage( _imgMgr.IndexMiddle );
                OnTop = false;

            }
        }

        private void DoLogout()
        {
            DAL.MonitorJobDal mjDal = new DAL.MonitorJobDal();
            int today = (int)Math.Floor( DateTime.Today.ToOADate() );
            var mj = mjDal.GetMonitorJob( today, _operator.ID );
            mj.EndTime = DateTime.Now;
            mjDal.UpdateMonitorJob( mj );

            Exportor exportor = new Exportor();
            exportor.Write( 1, 1, "集中监控日志" );

            exportor.Write( 3, 1, "操作员：" );
            exportor.Write( 3, 2, _operator.Name );
            exportor.Write( 3, 3, "工作日期" );
            exportor.Write( 3, 4, DateTime.Today.ToString( "yyyy-mm-dd" ) );

            exportor.Write( 4, 1, "监控系统日启时间" );
            exportor.Write( 4, 2, mj.StartTime.ToString( "hh:mm:ss" ) );
            exportor.Write( 4, 3, "监控系统日终时间" );
            exportor.Write( 4, 4, mj.EndTime.ToString( "hh:mm:ss" ) );

            exportor.Write( 6, 1, "生产系统运行记录" );

            DAL.SubSystemMonitorJobDal ssmjDal = new SubSystemMonitorJobDal();
            ssmjDal.RemoveMeaninglessRecords();

            int row = 7;
            for ( int i = 1; i <= 14; ++i )
            {
                ssmjDal.GetSubSystemById( i );

            }

            exportor.Save( "C:/test.xls" );
            exportor.Dispose();
            
        }






    }
}
