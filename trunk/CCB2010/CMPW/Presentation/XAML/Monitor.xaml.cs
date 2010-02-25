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
using System.Configuration;
using System.IO;
using System.Drawing;
using JOYFUL.CMPW.Presentation;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// Monitor.xaml 的交互逻辑
    /// </summary>
    public partial class Monitor : Window
    {
        static readonly string IMAGE_SHARE_FOLDER =
            ConfigurationManager.AppSettings["ImageShared"];

        static readonly log4net.ILog log =
            log4net.LogManager.GetLogger(typeof(Monitor));

        private readonly object _lockSwitch = new object();

        int _sysId = 0;
        List<int> _sysIdToImage = new List<int>(5);

        bool _autoSwitch = true;
        bool _captured = false;
        bool _clickFinished = true;
        public bool Reminding = false;

        Key _keyPressed;
        Thread _thdCheck = null;
        public FullWindow fullWindow;

        public bool OnTop = true;

        Dictionary<int, bool> _exQueue = new Dictionary<int, bool>();
        List<int> _rizhongList = new List<int>();
        List<string> _reminderList = new List<string>();

        List<int> _enabled = new List<int>();
        Dictionary<int, DateTime> _disabled = new Dictionary<int, DateTime>();

        SystemsHandler.SystemsHandler _sysHandler;
        TimeReminder _reminder;

        public Monitor( )
        {
            InitializeComponent();
            InitializeSysHandler();
            InitializeImages();

            _reminder = new TimeReminder();

            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyUpEvent, new KeyEventHandler(Page_KeyUp), true);
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(Page_KeyDown), true);

            _thdCheck = new Thread(new ThreadStart(DoCheck));
            _thdCheck.Start();
        }

        private void Page_KeyDown( object sender, KeyEventArgs e )
        {
            try
            {
                if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.F4)
                {
                    _keyPressed = e.Key;
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void Page_KeyUp( object sender, KeyEventArgs e )
        {
            try
            {
                if( !_clickFinished ) return;
                if( OnTop && e.Key == _keyPressed )
                {
                    if (e.Key == Key.Left)
                    {
                        //lock(_lockSwitch)
                        {
                            _clickFinished = false;
	                        UserSwitch();
	                        int sysId = GetIndexFromImage(img2);
                            SetImage(sysId);
                            _clickFinished = true;
                        }
                    }
                    else if (e.Key == Key.Right)
                    {
                        //lock(_lockSwitch)
                        {
                            _clickFinished = false;
	                        UserSwitch();
	                        int sysId = GetIndexFromImage(img4); 
	                        SetImage(sysId);
                            _clickFinished = true;
                        }
                    }
                }
                _keyPressed = Key.None;
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void DoSwitch()
        {
            //lock( _lockSwitch )
            {
	            if (_autoSwitch)
	            {
                    if (!_clickFinished) return;
                    _clickFinished = false;
	                int sysId = GetIndexFromImage(img4);
	                SetImage(sysId);
                    _clickFinished = true;
	            }
	            else
                {   //手动轮切情形下也需更新图片
                    if (!_clickFinished) return;
                    _clickFinished = false;
                    SetImage(_sysId); 
                    _clickFinished = true;
	            }
            }
        }

        public void StopAlarm()
        {
            //int sysId = GetIndexFromImage(img3);
            _sysHandler.StopRemindingAlarm();
            for (int i = 1; i <= 14; ++i)
                _sysHandler.StopAlarm(i);
        }
        void TryResumeSuspendSystem()
        {
            List<int> listToEnable = new List<int>();
            foreach (KeyValuePair<int, DateTime> pair in _disabled)
            {
                if (pair.Value < DateTime.Now)
                {
                    _sysHandler.StartMonitor(pair.Key);
                    listToEnable.Add(pair.Key);
                    log.Error("恢复监控 - " +
                        SystemsHandler.SystemsHandler.GetSystemName(pair.Key));
                }
            }
            foreach( int i in listToEnable )
            {
                _enabled.Add(i);
                _disabled.Remove(i);
            }
        }

        private void DoCheck()
        {
            log.Info("DoCheck Thread ID:" + SystemsHandler.SystemsHandler.GetCurrentThreadId().ToString());
            int tick = 0;
            while( true )
            {
                int exceptionAt = 0;
                try
                {
                    # region refresh the images
                    if ( !_captured && tick >= 1000 &&
                        SystemsHandler.SystemsHandler.g_checkedInSystem.Count > 0 
                        ) //如未获取图片，则每秒钟尝试一次更新图片
                    {
                        exceptionAt=1;
                        int sysId = SystemsHandler.SystemsHandler.g_checkedInSystem[0];
                        SetImage(sysId);
                        tick = 0;
                    }
                    else if ( OnTop && tick > 10000 ) //根据客户意见修改为10s 2009-12-21
                    {
                        exceptionAt = 2;
                        log.Info("DoSwitch : Switch");

                        DoSwitch();
                        tick = 0;
                    }
                    # endregion
                    exceptionAt = 3;
                    TryResumeSuspendSystem();
                    string remindingInfo = string.Empty;
                    if( _reminder.TryGetRemindInfo( out remindingInfo ) )
                    {
                        _reminderList.Add( remindingInfo );
                    }
                    exceptionAt = 4;
                    int exSysId = _sysHandler.GetFirstExceptionSystemID();
                    const int NULL_EXCEPTION = -1;
                    # region no exception
                    if (exSysId == NULL_EXCEPTION) //无异常
                    {
                        if ( fullWindow != null && !Reminding ) //正在满屏显示异常，则返回监控界面
                        {
                            SetImage( _sysId );
                            exceptionAt = 5;
                            fullWindow.OnTop = false;
                            this.OnTop = true;
                            App.SwitchToWindow("Monitor"); // back to Monitor window
                            fullWindow = null;
                            _autoSwitch = true;
                            btnAutoSwitch.SetIsEnabledThreadSafe(false);
                            btnUserSwitch.SetIsEnabledThreadSafe(true);
                        }
                        if (_exQueue.Count > 0)
                        {
                            //foreach (KeyValuePair<int,bool> pair in _exQueue)
                            //{
                            //    //show removing message
                            //}
                            _exQueue.Clear();
                        }
                        if (_reminderList.Count > 0 && !Reminding )// 重要时间点 提示
                        {
                            exceptionAt = 6;
                            string info_file = _reminderList[0];
                            string info = info_file.Substring(0, info_file.LastIndexOf("__"));
                            string source = info_file.Substring(info_file.LastIndexOf("__") + 2);

                            _sysHandler.LanchRemindingAlert(info,source);
                            fullWindow = App.SwitchToWindow("FullWindow") as FullWindow;
                            this.OnTop = false;
                            fullWindow.OnTop = true;
                            fullWindow.SetReminder(info);
                            Reminding = true;
                            _reminderList.RemoveAt(0);
                        }
                    }
                    # endregion
                    else
                    {
                        exceptionAt = 7;
                        //更新异常系统列表
                        for (int i = 1; i <= 14; ++i)
                        {
                            if (_sysHandler.IsSystemHasException(i))
                            {
                                if (!_exQueue.ContainsKey(i))
                                {
                                    _exQueue.Add(i, false);
                                    //show adding message
                                }
                            }
                            else if (_exQueue.ContainsKey(i))
                            {
                                _exQueue.Remove(i);
                                //show removing message
                            }
                        }
                        if( fullWindow == null ) //全屏显示优先级最高且未被全屏显示过的系统  
                        {
                            int index = 0;
                            bool toFullWindow = false;
                            for( int i = 1; i <= 14; ++i )
                            //foreach( KeyValuePair<int,bool> pair in _exQueue )
                            {
                                int sysId = i;
                                //int sysId = pair.Key;

                                exceptionAt = 100+sysId;
                                
                                
                                if( _exQueue.ContainsKey( sysId ) && _exQueue[ sysId ] != true )//一般异常
                                {
                                    toFullWindow = true;
                                    index = sysId; //记录系统id，为后面设置已预警标志做准备
                                }
                                else //日终且未全屏提示
                                if ( _sysHandler.IsRiZhongException( sysId ) 
                                     && !_rizhongList.Contains(sysId) )
                                {
                                    _rizhongList.Add(sysId);
                                    toFullWindow = true;
                                }

                                if( toFullWindow )
                                {
                                    string title = SystemsHandler.SystemsHandler.GetSystemName(sysId);
                                    string detail = string.Empty;
                                    int alertType = 3;
                                    if ( _sysHandler.IsSystemHasException( sysId )
                                        && _exQueue[ sysId ] != true ) //一般异常且未曾满屏,取异常具体信息
                                    {
                                        alertType = 1;
                                        detail = _sysHandler.GetExceptionDesc(sysId);
                                        if (string.IsNullOrEmpty(detail))
                                            detail = " ";
                                    }

                                    Bitmap bmp = _sysHandler.GetSystemImage(sysId);

                                    fullWindow = App.SwitchToWindow("FullWindow") as FullWindow;
                                    fullWindow.OnTop = true;
                                    this.OnTop = false;
                                    fullWindow.Initialize( title, detail );
                                    fullWindow.ReplaceImage(bmp);
                                    bmp.Dispose();
                                    bmp = null;
                                    

                                    //如果系统原有异常已提示且未消除，这时有日终信息到来
                                    //若裸调LanchAlert会播放异常音乐！！！
                                    //所以需要在调用端确定音乐类型
                                    //_sysHandler.LanchAlert(sysId);
                                    _sysHandler.LanchAlertWithType( sysId, alertType );

                                    _autoSwitch = false;
                                    btnUserSwitch.SetIsEnabledThreadSafe(false);
                                    btnAutoSwitch.SetIsEnabledThreadSafe(true);
                                    SetImage(sysId);
                                    
                                    break;
                                }
                            }
                            if ( index != 0 ) //设置“异常已预警"标志
                            {
                                _exQueue[ index ] = true;
                            }
                            if( !toFullWindow ) // 无异常和日终，则检查是否有“重要时间点”
                            {
                                if( _reminderList.Count > 0 )
                                {
                                    exceptionAt = 200;

                                    string info_file = _reminderList[0];
                                    string info = info_file.Substring(0, info_file.LastIndexOf("__"));
                                    string source = info_file.Substring(info_file.LastIndexOf("__") + 2);

                                    _sysHandler.LanchRemindingAlert(info,source);
                                    
                                    fullWindow = App.SwitchToWindow( "FullWindow" ) as FullWindow;
                                    fullWindow.OnTop = true;
                                    this.OnTop = false;
                                    fullWindow.SetReminder( info );
                                    Reminding = true;
                                    _reminderList.RemoveAt( 0 );
                                    
                                }
                            }
                        }
                        else if( !Reminding ) //全屏且不是在提示“重要时间点”
                        {
                            //如果异常和日终均已消除，则切换回监控界面
                            if( ( fullWindow.IsReportingException() //正在预警异常，且异常已消除
                                    && !_sysHandler.IsSystemHasException(_sysId ) )
                                || ( !fullWindow.IsReportingException() //正在预警日终，且日终已消除
                                    && !_sysHandler.IsRiZhongException( _sysId ) ) )
                            {
                                exceptionAt = 201;
                                // switch back to monitor window
                                fullWindow.OnTop = false;
                                this.OnTop = true;
                                App.SwitchToWindow("Monitor");
                                fullWindow = null;
                                _autoSwitch = true;
                                btnAutoSwitch.SetIsEnabledThreadSafe(false);
                                btnUserSwitch.SetIsEnabledThreadSafe(true);
                                SetImage(_sysId);
                            }
                            else
                            { //更新异常信息。可能上次异常已消除，但新的异常又发生了
                                exceptionAt = 202;
                                string detail = string.Empty;
                                //if( _sysHandler.GetAlertType( _sysId ) != 3 )
                                if( fullWindow.IsReportingException() )
                                {
                                    detail = _sysHandler.GetExceptionDesc(_sysId);
                                    fullWindow.SetExDetail(detail);
                                }
                                Bitmap bmp = _sysHandler.GetSystemImage(_sysId);
                                fullWindow.ReplaceImage(bmp);
                                bmp.Dispose();
                            }
                        }
                    }
                    const int INTERNAL_CHECKING = 500;
                    Thread.Sleep(INTERNAL_CHECKING);
                    tick += INTERNAL_CHECKING;
                }
                catch (System.Exception e)
                {
                    log4net.LogManager.GetLogger( typeof(Monitor)).
                        Error("\r\n Exception at:"+exceptionAt.ToString()+"\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
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
            var frm = new FormChangePassword( App.Oper.Name, App.Oper.Password );
            if( frm.ShowDialog() == System.Windows.Forms.DialogResult.OK )
            {
                App.Oper.Password = frm.Password;
                new OperatorDal().UpdateOperator( App.Oper );
            }
        }

        private void btnAutoSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnTop = true;
            _autoSwitch = true;
            btnAutoSwitch.SetIsEnabledThreadSafe(false);
            btnUserSwitch.SetIsEnabledThreadSafe(true);
        }

        private void btnUserSwitch_Click(object sender, RoutedEventArgs e)
        {
            OnTop = true;
            UserSwitch();
        }

        public void UserSwitch()
        {
            _autoSwitch = false;
            btnAutoSwitch.SetIsEnabledThreadSafe(true);
            btnUserSwitch.SetIsEnabledThreadSafe(false);
        }

        private void btnError_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime now = DateTime.Now;
                string folder = ConfigurationManager.AppSettings["ErrorFolder"]
                     + now.ToString("yyyyMMdd");
                if (!Directory.Exists(folder))
                {
                    new DirectoryInfo(folder).Create();
                }
                //save image to folder
                int sysId = _sysId;
                var bmp = _sysHandler.GetSystemImage(sysId);
                bmp.Save(folder + "\\" + now.ToString( "HHmmss " ) + 
                    SystemsHandler.SystemsHandler.GetSystemName(sysId) + ".bmp");
                bmp.Dispose();

                //save text to folder
                string info = _sysHandler.GetSystemLastCheckInfo(sysId);
                StreamWriter sw = new StreamWriter(folder + "\\" + 
                    now.ToString( "HHmmss" ) + ".txt");
                sw.Write(info);
                sw.Flush();
                sw.Close();
            }
            catch (System.Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_thdCheck != null && _thdCheck.IsAlive)
                    _thdCheck.Abort();

                if (_sysHandler != null)
                {
                    _sysHandler.Close();

                }
                App.LogOutOperator();
                WriteDaily();
                if (_sysHandler != null)
                {
                    _sysHandler.Destroy();
                }
                App.KillProcess();
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void cbxDisable_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                cbxDisable.Items.Clear();
                foreach (int id in _enabled)
                {
                    string sysName =
                        SystemsHandler.SystemsHandler.GetSystemName(id);
                    cbxDisable.Items.Add(sysName);
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void cbxEnable_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                cbxEnable.Items.Clear();
                foreach (KeyValuePair<int, DateTime> pair in _disabled)
                {
                    int key = pair.Key;
                    string sysName =
                        SystemsHandler.SystemsHandler.GetSystemName(key);
                    cbxEnable.Items.Add(sysName);
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void cbxDisable_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        private void cbxEnable_SelectionChanged(object sender,
            SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxEnable.SelectedItem != null)
                {
                    string name = cbxEnable.SelectedItem.ToString();
                    int index = SystemsHandler.SystemsHandler.GetSystemId(name);
                    _sysHandler.StartMonitor(index);
                    _enabled.Add(index);
                    _enabled.Sort();
                    _disabled.Remove(index);
                }
                cbxEnable.SelectedItem = null;
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }
        # endregion

        private void Window_Closed(object sender, EventArgs e)
        {
            App.LogOutOperator();
            if (_thdCheck != null && _thdCheck.IsAlive)
                _thdCheck.Abort();
            if (_sysHandler != null)
            {
                _sysHandler.Close();
            }
            
            if( _sysHandler != null )
            {
                _sysHandler.Destroy();
                _sysHandler = null;
            }
            App.KillProcess();
        }

        private void img_MouseDown( object sender, MouseButtonEventArgs e )
        {
            try
            {
                if (e.ClickCount == 2)
                {
                    fullWindow = App.SwitchToWindow("FullWindow") as FullWindow;
                    string detail = _sysHandler.GetExceptionDesc(_sysId);
                    string title = string.IsNullOrEmpty(detail) ? "" :
                        SystemsHandler.SystemsHandler.GetSystemName(_sysId);
                    fullWindow.Initialize(title, detail);
                    var bmp = _sysHandler.GetSystemImage(_sysId);
                    fullWindow.ReplaceImage(bmp);
                    bmp.Dispose();
                    OnTop = false;
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }


        # region 日志记录
        private void WriteDaily()
        {
            string adminPs = new OperatorDal().GetAdminPswd();
            int today = (int)Math.Floor(DateTime.Today.ToOADate());
            string folder = ConfigurationManager.AppSettings["DailyFolder"];
            string prefix = DateTime.Today.ToString("yyyyMMdd");
            Exportor exportor = null;
            try
            {
                DAL.MonitorJobDal mjDal = new DAL.MonitorJobDal();
                var mj = mjDal.GetMonitorJob(today, App.Oper.ID);
                mj.EndTime = DateTime.Now;
                mjDal.UpdateMonitorJob(mj);

                exportor = new Exportor();
                exportor.NewBook();
                exportor.Write(1, 1, "集中监控日志");

                exportor.Write(3, 1, "操作员：");
                exportor.Write(3, 2, App.Oper.Name);
                exportor.Write(3, 3, "工作日期");
                exportor.Write(3, 4, DateTime.Today.ToString("yyyy-MM-dd"));

                exportor.Write(4, 1, "监控系统日启时间");
                var mjFirst = mjDal.GetFirst(today);
                exportor.Write(4, 2, mjFirst.StartTime.ToString("HH:mm:ss"));
                exportor.Write(4, 3, "监控系统日终时间");
                exportor.Write(4, 4, mj.EndTime.ToString("HH:mm:ss"));

                exportor.Write(6, 1, "生产系统运行记录");

                DAL.SubSystemMonitorJobDal ssmjDal = new SubSystemMonitorJobDal();

                //大侠一棵葱 16:12:14
                //除了清算系统、清算直联系统、大额前置系统、大额支付汇划报文、大额支付事务报文、
                //重客系统外，其他的界面将签到时间改为监控启用时间
                int[] specialSystems = new int[] { 1, 4, 5, 7, 8, 12, 13, 14 };

                int row = 7;
                for (int i = 1; i <= 14; ++i)
                {
                    //ssmjDal.GetSubSystemById( i );
                    var ssmj = ssmjDal.GetByDateAndSystemId(DateTime.Today, i);
                    if (ssmj == null) continue;
                    bool fakeCheckIn = specialSystems.Contains( i );
                    string txtCheckIn = fakeCheckIn ?
                        "监控启用时间:" : "签到时间:";
                    string txtCheckOut = fakeCheckIn ?
                        "停止监控时间:" : "签退时间:";
                    exportor.Write(row, 1,
                        SystemsHandler.SystemsHandler.GetSystemName(i) + txtCheckIn);
                    exportor.Write( row, 2, fakeCheckIn ?
                        mj.StartTime.ToString( "HH:mm:ss" ) :
                        ssmj.SignedInTime.ToString( "HH:mm:ss" ) );
                    exportor.Write(row, 3,
                        SystemsHandler.SystemsHandler.GetSystemName(i) + txtCheckOut );
                    exportor.Write(row, 4, ssmj.SignedOutTime.ToString("HH:mm:ss"));

                    ++row;
                    exportor.Write(row, 1, "异常事件");
                    exportor.Write(row, 2, "发生时间");
                    exportor.Write(row, 3, "排除时间");

                    //DAL.ExceptionalEventsDal exDal = new ExceptionalEventsDal();
                    DAL.ExEventDal eeDal = new ExEventDal();
                    var exList = eeDal.Get(i, DateTime.Today);
                    if (exList == null) continue;
                    foreach (ExEvent item in exList)
                    {
                        //普通异常如果到监控系统停止监控时仍未消除
                        //它们的消除时间也设为停止监控时间
                        DateTime dt = item.Solved;
                        if ( dt < DateTime.Today ) dt = mj.EndTime;

                        ++row;
                        exportor.Write(row, 1, item.Description);
                        exportor.Write(row, 2, item.Found.ToString("HH:mm:ss"));
                        exportor.Write(row, 3, dt.ToString("HH:mm:ss"));
                    }
                }
                string path = folder + prefix + "_集中监控日记.xls";
                if (File.Exists(path))
                    File.Delete(path);
                exportor.Save( path, adminPs );
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }

            try 
            {
                if( exportor == null )
                    exportor = new Exportor();
                int row = 1;
                exportor.NewBook();

                exportor.Write( row, 1, "支付系统日间业务量登记表" );
                exportor.Write( ++row, 1, "系统名称：大额支付系统" );
                
                var lvpsDal = new LargeValuePaymentSystemBizDal();
                var lvpsList = lvpsDal.GetAllByDate(today);
                if( lvpsList != null && lvpsList.Length > 0 )
                {
                    exportor.Write( ++row, 1, "状态时间" );
                    exportor.Write( row, 2, "往帐接收" );
                    exportor.Write( row, 3, "往帐被拒绝" );
                    exportor.Write( row, 4, "往帐发送失败" );
                    exportor.Write( row, 5, "排队" );
                    exportor.Write( row, 6, "来帐接收" );
                    exportor.Write( row, 7, "来帐被拒绝" );
                    exportor.Write( row, 8, "来帐未响应" );
                    exportor.Write( row, 9, "查询总笔数" );
                    exportor.Write( row, 10, "未回复查询" );

                    var uqList = new UnansweredQueryDAL().GetAllByDate( today );
                    var timeList = new List<DateTime>();//8:40-17:30, last
                    timeList.Add( DateTime.Today.AddHours( 8 ).AddMinutes( 40 ) );
                    DateTime dt = DateTime.Today.AddHours( 9 );
                    while ( dt <= DateTime.Today.AddHours( 17 ).AddMinutes( 30 ) )
                    {
                        timeList.Add( dt );
                        dt = dt.AddMinutes( 30 );
                    }
                    dt = GetLastTime( lvpsList );
                    if ( dt > timeList[ timeList.Count - 1 ] ) //日终时间在17:30之后
                        timeList.Add( dt );

                    foreach( DateTime dtRecord in timeList )
                    {
                        ++row;
                        exportor.Write( row, 1, dtRecord.ToString( "HH:mm" ) );

                        LargeValuePaymentSystemBiz lvps = GetNearestLvps( lvpsList, dtRecord );
                        if( lvps != null )
                        {
                            exportor.Write( row, 2, lvps.ExpenditureAccept.ToString() );
                            exportor.Write( row, 3, lvps.ExpenditureReject.ToString() );
                            exportor.Write( row, 4, lvps.ExpenditureFailure.ToString() );

                            exportor.Write( row, 5, lvps.Queue.ToString() );
                            exportor.Write( row, 6, lvps.ReceiptAccept.ToString() );
                            exportor.Write( row, 7, lvps.ReceiptReject.ToString() );
                            exportor.Write( row, 8, lvps.ReceiptFailure.ToString() );
                        }

                        UnansweredQuery uq = GetNearestUq( uqList, dtRecord );
                        if( uq != null )
                        {
                            exportor.Write( row, 10, uq.Value.ToString() );
                        }
                    }

                }
                else
                {
                    exportor.Write( ++row, 1, "本日无记录" );
                }

                ++row; // blank row

                exportor.Write( ++row, 1, "系统名称：小额支付系统" );
                var svpsDal = new SmallValuePaymentSystemBizDal();
                var svpsList = svpsDal.GetAllByDate(today);
                if( svpsList != null && svpsList.Length > 0 )
                {
                    exportor.Write( ++row, 1, "状态时间" );
                    exportor.Write( row, 2, "往帐接收" );
                    exportor.Write( row, 3, "往账异常" );
                    exportor.Write( row, 4, "已发包数" );
                    exportor.Write( row, 5, "来账接收" );
                    exportor.Write( row, 6, "来账异常" );
                    exportor.Write( row, 7, "来账已发" );
                    exportor.Write( row, 8, "查询总笔数" );
                    exportor.Write( row, 9, "未回复查询" );
                
                    var timeList = new List<DateTime>(); //8:00-18:00
                    DateTime dt = DateTime.Today.AddHours( 8 );
                    while ( dt <= DateTime.Today.AddHours( 18 ) )
                    {
                        timeList.Add( dt );
                        dt = dt.AddMinutes( 30 );
                    }
                    foreach ( DateTime dtRecord in timeList )
                    {
                        ++row;
                        SmallValuePaymentSystemBiz svps = GetNearestSvps( svpsList, dtRecord );
                        if( svps != null )
                        {
                            exportor.Write( row, 1, dtRecord.ToString( "HH:mm" ) );
                            exportor.Write( row, 2, svps.ExpenditureAccept.ToString() );
                            exportor.Write( row, 3, svps.ExpenditureException.ToString() );
                            exportor.Write( row, 4, svps.SentPackageCount.ToString() );
                            exportor.Write( row, 5, svps.ReceiptAccept.ToString() );
                            exportor.Write( row, 6, svps.ReceiptException.ToString() );
                            exportor.Write( row, 7, svps.ReceiptPackageCount.ToString() );
                        }
                    }
                }
                else
                {
                    exportor.Write( ++row, 1, "本日无记录" );
                }

                string path = folder + prefix + "_支付系统日间业务量登记表.xls";
                if (File.Exists(path))
                    File.Delete(path);
                exportor.Save( path, adminPs );
                exportor.Dispose();
            }
            catch (System.Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
                if (exportor != null)
                    exportor.Dispose();
            }
        }

        private SmallValuePaymentSystemBiz GetNearestSvps( SmallValuePaymentSystemBiz[] svpsList, DateTime dtRecord )
        {
            double span = Double.MaxValue;
            int index = -1;
            for( int i = 0; i < svpsList.Length; ++i )
            {
                var ts = Math.Abs((svpsList[ i ].Time - dtRecord).TotalMilliseconds);
                if( ts < span )
                {
                    span = ts;
                    index = i; 
                }
            }
            return index == -1 ? null : svpsList[ index ];
        }

        private DateTime GetLastTime( LargeValuePaymentSystemBiz[] lvpsList )
        {
            DateTime dt = DateTime.Today;
            foreach( var item in lvpsList )
            {
                if ( item.Time > dt )
                    dt = item.Time;
            }
            return dt;
        }

        private UnansweredQuery GetNearestUq( IList<UnansweredQuery> uqList, DateTime dtRecord )
        {
            if ( uqList == null ) return null;
            double span = Double.MaxValue;
            int index = -1;
            for ( int i = 0; i < uqList.Count; ++i )
            {
                var ts = Math.Abs( ( uqList[ i ].Time - dtRecord ).TotalMilliseconds );
                if ( ts < span )
                {
                    span = ts;
                    index = i;
                }
            }
            return index == -1 ? null : uqList[ index ];
        }

        private LargeValuePaymentSystemBiz GetNearestLvps( LargeValuePaymentSystemBiz[] lvpsList, DateTime dtRecord )
        {
            double span = Double.MaxValue;
            int index = -1;
            for ( int i = 0; i < lvpsList.Length; ++i )
            {
                var ts = Math.Abs( ( lvpsList[ i ].Time - dtRecord ).TotalMilliseconds );
                if ( ts < span )
                {
                    span = ts;
                    index = i;
                }
            }
            return index == -1 ? null : lvpsList[ index ];
        }

        # endregion

        # region Image Control
        System.Windows.Controls.Image _imgClicked = null;
        void InitializeImages()
        {
            System.Windows.Controls.Image[] list = new System.Windows.Controls.Image[]{
                img0, img1, img2, img3, img4, img5, img6 };
            Label[] lblList = new Label[]{
                lbl0, lbl1, lbl2, lbl3, lbl4, lbl5, lbl6 };
            double SPACE_WIDTH = 20;
            double IMAGE_WIDTH = img0.Width;
            int CONTROL_MIDDLE = 3;

            for (int i = 0; i < list.Length; ++i)
            {
                System.Windows.Controls.Image img = list[i];
                Highlight.Fill(img);
                img.Cursor = Cursors.Hand;
                img.MouseLeftButtonUp +=
                    new MouseButtonEventHandler(Image_MouseLeftButtonUp);
                img.MouseLeftButtonDown +=
                    new MouseButtonEventHandler(Image_MouseLeftButtonDown);
                
                TranslateTransform tr =
                    new TranslateTransform((i - CONTROL_MIDDLE) * (SPACE_WIDTH + IMAGE_WIDTH), 0.0);
                img.RenderTransform = tr;

                Label lbl = lblList[i];
                lbl.RenderTransform = tr.Clone();
                lbl.SetVisibilityThreadSafe(Visibility.Hidden);
            }

            lblAlert.SetVisibilityThreadSafe(Visibility.Hidden);
        }

        /// <summary>
        /// 响应鼠标点击事件，如果用户点击小图，则将大图更换成小图内容
        /// 并将轮切方式转换成手动轮切
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Controls.Image img = sender as System.Windows.Controls.Image;
                if ( img != null && img == _imgClicked)
                {
                    if (!_clickFinished) return;//上次点击未处理完毕
                    //lock(_lockSwitch)
                    {
                        _clickFinished = false;

	                    UserSwitch();
	                    int sysId = GetIndexFromImage(img);
	                    SetImage(sysId);
                    }
                }
                _imgClicked = null;
                _clickFinished = true;
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }            
        }
        void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _imgClicked = sender as System.Windows.Controls.Image;
        }
        
        /// <summary>
        /// 将大图设置为指定系统的图片
        /// </summary>
        /// <param name="objSysId"></param>
        void SetImage(int sysId)
        {
            try
            {
                log.Info("SetImage: " + sysId.ToString());

                //lock(_lockSwitch)
                {
                    if (sysId == -1) return;
                    _sysIdToImage = _sysHandler.GetMonitoringIdList(sysId, 5);
                    if (_sysIdToImage == null) return;

                    _captured = true;
                    _sysId = sysId;
                    var bmp = _sysHandler.GetSystemImage(_sysIdToImage[2]);
                    if (bmp == null) return;

                    img.SetSourceThreadSafe(bmp);
                    img3.SetSourceThreadSafe(bmp);
                    if (bmp != null) bmp.Dispose();
                   

                    txtInfo.SetTextThreadSafe(_sysHandler.GetSystemLastCheckInfo(sysId));

                    bmp = _sysHandler.GetSystemImage(_sysIdToImage[0]);
                    img1.SetSourceThreadSafe(bmp);
                    if (bmp != null) bmp.Dispose();

                    bmp = _sysHandler.GetSystemImage(_sysIdToImage[1]);
                    img2.SetSourceThreadSafe(bmp);
                    if (bmp != null) bmp.Dispose();

                    bmp = _sysHandler.GetSystemImage(_sysIdToImage[3]);
                    img4.SetSourceThreadSafe(bmp);
                    if (bmp != null) bmp.Dispose();

                    bmp = _sysHandler.GetSystemImage(_sysIdToImage[4]);
                    img5.SetSourceThreadSafe(bmp);
                    if (bmp != null)  bmp.Dispose();

                    SetLabelStatus( lbl1, _sysIdToImage[ 0 ] );
                    SetLabelStatus( lbl2, _sysIdToImage[ 1 ] );
                    SetLabelStatus( lbl3, _sysIdToImage[ 2 ] );
                    SetLabelStatus( lblAlert, _sysIdToImage[ 2 ] );
                    SetLabelStatus( lbl4, _sysIdToImage[ 3 ] );
                    SetLabelStatus( lbl5, _sysIdToImage[ 4 ] );
                }
            }
            catch(Exception e)
            {
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
                
            }
        }

        void SetLabelStatus( Label lbl, int sysId )
        {
            if ( _sysHandler.IsSystemHasException( sysId ) )
            {
                lbl.SetContentThreadSafe( "异常" );
                lbl.SetVisibilityThreadSafe( Visibility.Visible );
            }
            else if ( _sysHandler.IsRiZhongException( sysId ) )
            {
                lbl.SetContentThreadSafe( "日终" );
                lbl.SetVisibilityThreadSafe( Visibility.Visible );
            }
            else
                lbl.SetVisibilityThreadSafe( Visibility.Hidden );
        }
        # endregion
        
        private int GetIndexFromImage(System.Windows.Controls.Image img)
        {
            try
            {
                if (img == null || _sysIdToImage == null ||
                    _sysIdToImage.Count < 5 )
                    return -1;

                string name = img.GetNameThreadSafe();
                if (img1 != null && name == img1.GetNameThreadSafe())
                    return _sysIdToImage[0];
                else if (img2 != null && name == img2.GetNameThreadSafe())
                    return _sysIdToImage[1];
                else if (img3 != null && name == img3.GetNameThreadSafe())
                    return _sysIdToImage[2];
                else if (img4 != null && name == img4.GetNameThreadSafe())
                    return _sysIdToImage[3];
                else if (img5 != null && name == img5.GetNameThreadSafe())
                    return _sysIdToImage[4];
            }
            catch (System.Exception e)
            {
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return -1;
        }
    }
}
