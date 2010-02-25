using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.IO;
using JOYFULL.CMPW.Model;
using System.Diagnostics;
using System.Threading;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// http://www.infosysblogs.com/microsoft/2008/09/how_to_write_custom_main_metho.html
    /// </summary>
    public partial class App : Application
    {
        static Mutex mutex = new Mutex( true, "{BEB86289-5445-45dc-91B9-2061F11D370F}" );


        private static NotifyIconWrapper component;
        public static Operator Oper {get;set;}
        public static MonitorJob MtJob { get; set; }
        [STAThread]
        public static void Main()
        {
            if ( mutex.WaitOne( TimeSpan.Zero, true ) )
            {

                log4net.LogManager.GetLogger(typeof(App)).Error("Start System");
                KillRemainingprocess();
                EnsureFolders();
                RegisterWindows();

                App app = new App();
                Window w = SwitchToWindow( "Login" );
                app.Run( w );
            }
            else
            {
                MessageBox.Show( "集中监控预警系统正在运行中......" );
            }
        }

        private static void KillRemainingprocess()
        {
            Process c = Process.GetCurrentProcess();
            var list = Process.GetProcesses();
            foreach (Process p in list) 
            {
                if( p != c && p.ProcessName.Contains( "集中监控预警系统.exe" ))
                {
                    p.Kill();
                }
            }
        }

        static Dictionary<string, Window> _list = new Dictionary<string,Window>();

        protected override void OnStartup( StartupEventArgs e )
        {
            base.OnStartup( e );
            this.ShutdownMode = ShutdownMode.OnExplicitShutdown;
            component = new NotifyIconWrapper();
        }
        protected override void OnExit( ExitEventArgs e )
        {
            base.OnExit( e );
            if( component != null )
            {
                component.Dispose();
                component = null;
            }
        }

        public static void KillProcess()
        {
            if (component != null)
            {
                component.Dispose();
                component = null;
            }
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
            ///// <summary>
        ///// 当用户要注销Windows系统或关闭电脑时，提示其正常关闭本系统
        ///// </summary>
        ///// <param name="e"></param>
        //protected override void OnSessionEnding( SessionEndingCancelEventArgs e )
        //{
        //    base.OnSessionEnding( e );
        //    MessageBox.Show( "请正常关闭集中监控预警系统！" );
        //    e.Cancel = true;
        //}

        private static void EnsureFolders()
        {
            string appPath = System.IO.Path.GetFullPath( "." );
            string imageFolder = ConfigurationManager.AppSettings[ "ImageFolder" ];
            DirectoryInfo di = new DirectoryInfo( imageFolder );
            if( !di.Exists )
            {
                di.Create();
                string srcFolder = appPath + "\\Images\\";
                string[] files = Directory.GetFiles( srcFolder );
                foreach( string file in files )
                {
                    string fileName = file.Substring( file.LastIndexOf( "\\" ) + 1 );
                    File.Copy( file, imageFolder + fileName );
                }
            }
            
            string dailyFolder = ConfigurationManager.AppSettings["DailyFolder"];
            di = new DirectoryInfo(dailyFolder);
            if (!di.Exists)
            {
                di.Create();
            }


            string alertFolder = ConfigurationManager.AppSettings["AlertFolder"];
            di = new DirectoryInfo(alertFolder);
            if (!di.Exists)
            {
                di.Create();
                string srcFolder = appPath + "\\Alerts\\";
                string[] files = Directory.GetFiles(srcFolder);
                foreach (string file in files)
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                    File.Copy(file, alertFolder + fileName);
                }
            }

            string parasFolder = ConfigurationManager.AppSettings["ParasFolder"];
            di = new DirectoryInfo(parasFolder);
            if (!di.Exists)
            {
                di.Create();
                string srcFolder = appPath + "\\ParasFolder\\";
                string[] files = Directory.GetFiles(srcFolder);
                foreach (string file in files)
                {
                    string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                    File.Copy(file, parasFolder + fileName);
                }
            }
            
            string recErrorsFolder = ConfigurationManager.AppSettings["RecErrorFolder"];
            di = new DirectoryInfo(recErrorsFolder);
            if (!di.Exists)
            {
                di.Create();
            }

            string errorFolder = ConfigurationManager.AppSettings["ErrorFolder"];
            di = new DirectoryInfo(recErrorsFolder);
            if (!di.Exists)
            {
                di.Create();
            }

            string bizConfigFile = ConfigurationManager.AppSettings[ "BizConfigFile" ];
            if( !File.Exists( bizConfigFile ) )
            {
                File.Copy( appPath + "\\biz.xml", bizConfigFile );
            }

            string sysConfigFile = ConfigurationManager.AppSettings[ "SysSettingFile" ];
            if( !File.Exists( sysConfigFile ) )
            {
                File.Copy( appPath + "\\SystemsSetting.xml", sysConfigFile );
            }

            string remindingFile = ConfigurationManager.AppSettings[ "RemindingFile" ];
            if ( !File.Exists( remindingFile ) )
            {
                File.Copy( appPath + "\\TimeReminding.xml", remindingFile );
            }

            string dbFile = ConfigurationManager.AppSettings[ "DatabaseFile" ];
            if( !File.Exists( dbFile ) )
            {
                File.Copy( appPath + "\\CMPW.mdb", dbFile );
            }
        }

        private static void RegisterWindows()
        {
            var w = new FullWindow();
            w.OnTop = false;
            _list.Add("FullWindow", w);
        }

        [STAThread]
        public static Window SwitchToWindow( string type )
        {
            Window ret = null;
            foreach( KeyValuePair<string,Window> w in _list )
            {
                w.Value.HideThreadSafe();
                if ( w.Key == "Login" )
                {
                    Oper = ( w.Value as Login ).CurrentOperator;
                }
                if( w.Key == "Monitor" && type != w.Key )
                {
                    ( w.Value as Monitor ).OnTop = false;
                }
                if( w.Key == "FullWindow" && type != w.Key )
                {
                    ( w.Value as FullWindow ).OnTop = false;
                }
                if ( w.Key == type )
                {
                    w.Value.ShowThreadSafe();
                    ret = w.Value;
                }
            }
            if( ret == null )
            {
                switch( type )
                {
                    case "Login":
                        ret = new Login(); break;
                    case "Config":
                        ret = new Config(); break;
                    case "Monitor":
                        ret = new Monitor();
                        (ret as Monitor).OnTop = true;
                        break;
                    case "FullWindow":
                        ret = new FullWindow();
                        (ret as FullWindow).OnTop = true;
                        break;
                }
                _list.Add( type, ret );
                ret.ShowThreadSafe();
            }
            if( ret is Monitor )
            {
                (ret as Monitor).StopAlarm();
                ( ret as Monitor ).fullWindow = null;
                ( ret as Monitor ).Reminding = false;
                ( ret as Monitor ).OnTop = true;
            }
            else if( ret is FullWindow )
            {
                ( ret as FullWindow ).OnTop = true;
            }
            return ret;
        }

        public static void LogOutOperator()
        {
            if (Oper == null) return;
            if( !Oper.IsAdmin )
            {
                DAL.MonitorJobDal mjDal = new DAL.MonitorJobDal();
                MtJob.EndTime = DateTime.Now;
                mjDal.UpdateMonitorJob(MtJob);
            }
        }
    }
}
