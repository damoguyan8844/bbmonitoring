using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;

namespace JOYFULL.CMPW.Refresher
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        static Mutex mutex = new Mutex( true, "{7A242CA1-8638-4949-B23D-CB70D6002A11}" );
        [STAThread]
        static void Main()
        {
            if ( mutex.WaitOne( TimeSpan.Zero, true ) )
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault( false );
                //Application.Run( new FormCCBS() );
                Application.Run( new FormHvps() );
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show( "大额前置系统监控辅助程序正在运行中。。。" );
            }
        }

        //[STAThread]
        //static void Main()
        //{
        //    Application.EnableVisualStyles();
        //    Application.SetCompatibleTextRenderingDefault( false );
        //    Application.Run( new Form1() );
        //    MessageBox.Show( "集中监控系统正在运行中。。。" );
        //}
    }
}
