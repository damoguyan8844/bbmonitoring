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
            if (e.Key == Key.Escape)
            {
                _keyPressed = e.Key;
            }
        }

        private void Page_KeyUp(object sender, KeyEventArgs e)
        {
            if (OnTop && e.Key == _keyPressed && e.Key == Key.Escape )
            {
                OnTop = false;
                App.SwitchToWindow("Monitor");
                if( _flash != null )
                {
	                _flash.Abort();
	                _flash = null;
                }
            }
            _keyPressed = Key.None;
        }

        public void Initialize( bool exceptionFound, string desc )
        {
            FadeWrapper f = new FadeWrapper(this, info);
            f.Fadeout(3000, 3000);
            OnTop = true;
            if (exceptionFound)
            {
                ex.SetVisibilityThreadSafe(Visibility.Visible);
                StartFlash();
            }
            else
                ex.SetVisibilityThreadSafe(Visibility.Hidden);

            if (!string.IsNullOrEmpty(desc))
            {
                txtDesc.SetTextThreadSafe(desc);
                txtDesc.SetVisibilityThreadSafe(Visibility.Visible);
            }
            else
                txtDesc.SetVisibilityThreadSafe(Visibility.Hidden);

            // bring this window to top
            this.MaximizeThreadSafe();
            this.SetTopMostThreadSafe(true);
            this.ActivateThreadSafe();
            this.SetTopMostThreadSafe(false);
        }

        Thread _flash;
        private void Flash()
        {
            bool fadeout = true;
            int DURATION = 1000;
            FadeWrapper f = new FadeWrapper(this as UIElement, ex);
            while (true)
            {
                if (fadeout)
                    f.Fadeout(1000, DURATION);
                else
                    f.Fadein(DURATION);
                fadeout = !fadeout;
                Thread.Sleep(DURATION);
            }
        }

        private void StartFlash()
        {
            if (_flash == null)
                _flash = new Thread(new ThreadStart(Flash));
            if (!_flash.IsAlive)
                _flash.Start();
        }

        private void StopFlash()
        {
            if (_flash != null &&
                _flash.ThreadState == System.Threading.ThreadState.Running)
                _flash.Suspend();
        }

        public void ReplaceImage( System.Drawing.Bitmap bmp )
        {
            if (bmp == null)
                return;
            img.SetSourceThreadSafe(bmp);
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            App.LogOutOperator(  );
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}
