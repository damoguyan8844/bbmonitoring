/* http://www.pinvoke.net/default.aspx/user32.keybd_event
 * http://www.codeguru.com/vb/gen/vb_system/keyboard/article.php/c14629
 * http://mwinapi.sourceforge.net/
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using ManagedWinapi;
using ManagedWinapi.Windows;
using System.IO;

namespace JOYFULL.CMPW.Refresher
{
    internal static class KeySender
    {
        [DllImport( "kernel32.dll", SetLastError = true ) ]
        static extern bool SwitchToThread( );
        [DllImport( "user32.dll" )]
        static extern uint GetQueueStatus( uint flags );
        [DllImport( "user32.dll" )]
        private static extern void keybd_event( byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo );
 

        static void PressKey( Keys key, string desc )
        {
            //const int KEYEVENTF_EXTENDEDKEY = 0x1;
            //const int KEYEVENTF_KEYUP = 0x2;
            //keybd_event( ( byte )key, 0x45, KEYEVENTF_EXTENDEDKEY, UIntPtr.Zero );
            //keybd_event( ( byte )key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP,
            //    UIntPtr.Zero );
            using( new LockKeyResetter() )
            {
                KeyboardKey ctrl = null;
                KeyboardKey shift = null;
                KeyboardKey alt = null;
                desc = desc.ToLower();
                if( desc.Contains( "ctrl" ) )
                {
                    ctrl = new KeyboardKey( Keys.ControlKey );
                    ctrl.Press();
                }
                if( desc.Contains( "shift" ) )
                {
                    shift = new KeyboardKey( Keys.ShiftKey );
                    shift.Press();
                }
                if( desc.Contains( "alt" ) )
                {
                    alt = new KeyboardKey( Keys.RButton|Keys.ShiftKey );
                    alt.Press();
                }

                KeyboardKey kbk = new KeyboardKey( key );
                kbk.PressAndRelease();
                if( ctrl != null ) ctrl.Release();
                if( shift != null ) shift.Release();
                if( alt != null ) alt.Release();
            }
        }

        static void PressKeyArray( Keys[] keys )
        {
            //foreach( Keys key in keys )
            //{
            //    PressKey( key );
            //}
        }

        public static string Send( string title, Keys key, string desc )
        {
            string err = string.Empty;
            try 
            {
                Process p = null;
                IntPtr hWnd = IntPtr.Zero;
                foreach( Process item in Process.GetProcesses() )
                {
                    if( item.MainWindowTitle.Contains( title ) )
                    {
                        p = item;
                        hWnd = item.MainWindowHandle;
                        break;
                    }
                }
                if( hWnd == IntPtr.Zero )
                {
                    err = "【" + title + "】已关闭，请检查。";
                    return err;
                }
                SystemWindow sw = new SystemWindow( hWnd );
                FormWindowState fws = sw.WindowState;
                bool enabled = sw.Enabled;
                bool visibilityFlag = sw.VisibilityFlag;
                SystemWindow swForeground = SystemWindow.ForegroundWindow;

                sw.Enabled = true;
                if( sw.WindowState == FormWindowState.Minimized )
                    sw.WindowState = FormWindowState.Normal;
                sw.VisibilityFlag = true;

                SystemWindow.ForegroundWindow = sw;
                p.WaitForInputIdle();
                PressKey( key, desc );
                System.Windows.Forms.SendKeys.Flush();

                //Thread.Sleep( 1000 );
                //SwitchToThread();
                //uint status = 0;
                //do
                //{
                //    System.Windows.Forms.SendKeys.Flush();
                //    status = GetQueueStatus( 511 );
                //}
                //while( status == 0 );
                ////swForeground.Process.WaitForInputIdle(200);

                //SystemWindow.ForegroundWindow = swForeground;
                sw.VisibilityFlag = visibilityFlag;
                sw.Enabled = enabled;
            }
            catch( Exception e )
            {
                err = e.Message;
            }
            //Log(key.ToString());
            return err;
        }

        static void Log( string s )
        {
            System.IO.StreamWriter sw = new StreamWriter("C:/log.txt", true);
            sw.Write(DateTime.Now.ToString("hhmmss"));
            sw.WriteLine(s);
            sw.Close();
        }
    }
}
