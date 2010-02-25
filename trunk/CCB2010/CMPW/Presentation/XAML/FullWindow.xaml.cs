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
using System.Windows.Shapes;
using System.Configuration;
using System.Threading;
using System.Diagnostics;

namespace JOYFULL.CMPW.Presentation
{
    /// <summary>
    /// Interaction logic for FullWindow.xaml
    /// </summary>
    public partial class FullWindow : Window
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(FullWindow));
        public FullWindow()
        {
            InitializeComponent();
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyUpEvent, new KeyEventHandler(Page_KeyUp), true);
            EventManager.RegisterClassHandler(typeof(Window),
                Keyboard.KeyDownEvent, new KeyEventHandler(Page_KeyDown), true);
        }

        public bool OnTop = false;

        private Key _keyPressed;

        private void Page_KeyDown(object sender, KeyEventArgs e)
        {
            try 
            {
                if (e.Key == Key.Escape)
                {
                    _keyPressed = e.Key;
                }
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }

        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (OnTop && e.Key == _keyPressed && e.Key == Key.Escape)
                {
                    OnTop = false;
                    StopFlash();
                    Monitor monit=App.SwitchToWindow("Monitor")  as Monitor;
                    monit.OnTop = true;
                    monit.UserSwitch();
                }
                _keyPressed = Key.None;
            }
            catch (Exception ex)
            {
                log.Error("\r\nSource:" + ex.Source + "\r\nMessage:" + ex.Message + "\r\n" + ex.StackTrace);
            }
        }

        public void Initialize( string title, string detail )
        {
            OnTop = true;
            if( string.IsNullOrEmpty(title)) //无异常，用户在操作界面双击大图
            {
                txtTitle.SetVisibilityThreadSafe(Visibility.Hidden);
                txtDetail.SetVisibilityThreadSafe(Visibility.Hidden);
                img.SetVisibilityThreadSafe(Visibility.Visible);
            }
            else
            {
                txtDetail.SetTextThreadSafe( detail );
                if (string.IsNullOrEmpty(detail)) //日终 
                {
                    //根据用户需求,提示相关联系统的日终信息
                    if (title.Contains("大额"))
                        title = "大额事务,大额业务,大额前置系统";
                    else if (title.Contains("清算"))
                        title = "清算系统,清算直联系统";
                    txtTitle.SetTextThreadSafe(title + "日终，请处理");
                    txtDetail.SetVisibilityThreadSafe(Visibility.Hidden);
                }
                else
                {
                    txtTitle.SetTextThreadSafe(title + "出现异常，请处理");
                    txtDetail.SetVisibilityThreadSafe(Visibility.Visible);
                }

                txtDetail.SetTextThreadSafe(detail);
                txtTitle.SetVisibilityThreadSafe(Visibility.Visible);
                
                img.SetVisibilityThreadSafe(Visibility.Hidden);
                StartFlash();

                // bring this window to top
                this.MaximizeThreadSafe();
                this.SetTopMostThreadSafe(true);
                this.ActivateThreadSafe();
                this.ShowThreadSafe();
                this.SetTopMostThreadSafe(false);
                this.ShowThreadSafe();
                this.ActivateThreadSafe();
            }


            //FadeWrapper f = new FadeWrapper(this, info);
            //f.Fadeout(2000, 1000);

            //if (exceptionFound)
            //{
            //    ex.SetVisibilityThreadSafe(Visibility.Visible);
            //    //StartFlash();
            //    FadeWrapper f2 = new FadeWrapper(this, ex);
            //    f2.Fadeout(2000, 1000);
            //}
            //else
            //    ex.SetVisibilityThreadSafe(Visibility.Hidden);

            //if (!string.IsNullOrEmpty(desc))
            //{
            //    txtDesc.SetTextThreadSafe(desc);
            //    txtDesc.SetVisibilityThreadSafe(Visibility.Visible);
            //}
            //else
            //    txtDesc.SetVisibilityThreadSafe(Visibility.Hidden);

            //Thread.Sleep(3000);
            ////StopFlash();
            //ex.SetVisibilityThreadSafe(Visibility.Hidden);
            //img.SetVisibilityThreadSafe(Visibility.Visible);

        }

        Thread _flash = null;
        private void Flash()
        {
            FadeWrapper fadeTitle = new FadeWrapper(this as UIElement, txtTitle );
            FadeWrapper fadeDetail = new FadeWrapper(
                this as UIElement, txtDetail);
            bool showImage = false;
            //添加最大闪烁次数
            int maxFlashCount = 100;
            while (true)
            {
                --maxFlashCount;
                if( showImage )
                {
                    img.SetVisibilityThreadSafe(Visibility.Visible);
                    Thread.Sleep(8000);
                }
                else
                {
                    img.SetVisibilityThreadSafe(Visibility.Hidden);
                    fadeTitle.Fadeout(7000, 3000);
                    fadeDetail.Fadeout(7000, 3000);
                    //预警红字闪烁改为10秒
                    Thread.Sleep(10000);
                }

                showImage = !showImage;
                //if (fadeout)
                //    f.Fadeout(1000, DURATION);
                //else
                //    f.Fadein(DURATION);
                //fadeout = !fadeout;
                //Thread.Sleep(DURATION);
            }
        }

        private void StartFlash()
        {
            //if (_flash != null)
            //    _flash = new Thread(new ThreadStart(Flash));
            //if (_flash.ThreadState == System.Threading.ThreadState.Aborted ||
            //    _flash.ThreadState == System.Threading.ThreadState.Unstarted )
            //    _flash.Start();
            //if (_flash.ThreadState == System.Threading.ThreadState.Suspended ||
            //    _flash.ThreadState == System.Threading.ThreadState.Stopped )
            //    _flash.Resume();
            StopFlash();
            _flash = new Thread(new ThreadStart(Flash));
            _flash.Start();
        }

        public void StopFlash()
        {
            try
            {
                if (_flash != null)
                {
                    _flash.Abort();
                    _flash = null;
                }
            }
            catch( Exception e )
            {
                //log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }

        }

        public void ReplaceImage( System.Drawing.Bitmap bmp )
        {
            if (bmp == null)
                return;
            img.SetSourceThreadSafe(bmp);
        }

        public void SetExDetail( string detail )
        {
            txtDetail.SetTextThreadSafe(detail);
        }

        public bool IsReportingException()
        {
            string s = txtDetail.GetTextThreadSafe();
            return s.Length > 0;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.LogOutOperator(  );
            App.KillProcess();
        }

        internal void SetReminder( string info )
        {
            txtTitle.SetTextThreadSafe( info );
            txtTitle.SetVisibilityThreadSafe( Visibility.Visible );
            txtDetail.SetVisibilityThreadSafe( Visibility.Hidden );
            img.SetVisibilityThreadSafe( Visibility.Hidden );
            StartFlashReminder();
        }

        private void FlashReminder()
        {
            bool fadeout = false;
            FadeWrapper f = new FadeWrapper( this, txtTitle );
            while( true )
            {
                if ( fadeout )
                {
                    f.Fadeout( 7000, 3000 );
                    Thread.Sleep( 10000 );
                }
                else
                {
                    f.Fadein( 3000 );
                    Thread.Sleep( 3000 );
                }
                fadeout = !fadeout;
            }
        }
        private void StartFlashReminder()
        {
            StopFlash();
            _flash = new Thread( new ThreadStart( FlashReminder ) );
            _flash.Start();
        }
    }
}
