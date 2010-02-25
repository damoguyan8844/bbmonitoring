using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

namespace JOYFULL.CMPW.Presentation
{
    internal class FadeWrapper
    {
        const int INTERVAL = 100; // millisecond 
        private UIElement _container, _element;
        Thread _thdFade;
        int _span = 0;
        int _outRemainSpan = 0;
        bool _bOut = true;

        public FadeWrapper( UIElement container, UIElement element )
        {
             _thdFade = new Thread( Fade );
             _container = container;
            _element = element;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="remain"></param>
        /// <param name="interval">in millisecond</param>
        public void Fadeout( int remain, int span )
        {
            _span = span;
            _outRemainSpan = remain;
            _bOut = true;
            if( _thdFade.IsAlive )
                _thdFade.Abort();
            _thdFade = new Thread( new ThreadStart( Fade ) );
            _thdFade.Start();
        }

        public void Fadein( int span )
        {
            _span = span;
            _bOut = false;
            if( _thdFade.IsAlive )
                _thdFade.Abort();
            _thdFade = new Thread( new ThreadStart( Fade ) );
            _thdFade.Start();
        }

        private void Fade( )
        {
            _container.Dispatcher.BeginInvoke( ( ThreadStart )delegate( )
            {
                _element.Visibility = Visibility.Visible;
                _element.Opacity = _bOut ? 1.0 : 0.0;
            },
                    null );
            if( _bOut )
            {
                Thread.Sleep( _outRemainSpan );
            }

            int times = _span / INTERVAL;
            double addition = 1.0 / times;
            for( int i = 1; i <= times; ++i )
            {
                Thread.Sleep( INTERVAL );
                _container.Dispatcher.BeginInvoke( ( ThreadStart )delegate( )
                {
                    if( _bOut )
                        _element.Opacity = 1.0 - addition * i;
                    else
                        _element.Opacity = addition * i;
                },
                    null );
            }
            
            _container.Dispatcher.BeginInvoke( ( ThreadStart )delegate( )
            {
                _element.Visibility = _bOut ? Visibility.Hidden : Visibility.Visible;
                _element.Opacity = 1.0;
            },
                    null );
        }
    }
}
