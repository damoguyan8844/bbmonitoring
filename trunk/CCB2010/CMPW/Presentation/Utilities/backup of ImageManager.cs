//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Media.Imaging;
//using System.Configuration;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Threading;
//using System.Windows.Media.Animation;

//namespace JOYFULL.CMPW.Presentation
//{
//    internal class ImageManager
//    {
//        SystemsHandler.SystemsHandler _handler;
        
//        Image _large, _clicked;
//        List<Image> _imgList;
//        Label _alert;
//        List<Label> _lblList;

//        readonly int SOURCE_COUNT = 14;
//        readonly int CONTROL_MIDDLE;
//        readonly string IMAGE_FOLDER;
//        readonly double IMAGE_WIDTH;
//        readonly double SPACE_WIDTH = 20;

//        int _indexMiddle = 0;
//        Thread _thdMove;
//        Thread _flash;

//        public delegate void ImageClickCallback();
//        ImageClickCallback _callback;

//        public ImageManager( Image imgLarge, Image[] imgList, 
//            Label lblAlert, Label[] lblList, SystemsHandler.SystemsHandler handler,
//            ImageClickCallback callback )
//        {
//            _large = imgLarge; 
//            _imgList = new List<Image>( imgList );
//            _alert = lblAlert;
//            _lblList = new List<Label>( lblList );
//            _handler = handler;
//            _callback = callback;

//            CONTROL_MIDDLE = _imgList.Count / 2;
//            IMAGE_WIDTH = imgList[ 0 ].Width;
//            IMAGE_FOLDER = ConfigurationManager.AppSettings["ImageShared"];

//            for( int i = 0; i < _imgList.Count; ++i )
//            {
//                Image img = _imgList[ i ];
//                Highlight.Fill( img );
//                string uri = IMAGE_FOLDER + 
//                    ((i - CONTROL_MIDDLE + SOURCE_COUNT ) % SOURCE_COUNT).ToString("d3") + 
//                    ".bmp";

//                if(System.IO.File.Exists(uri))
//                {
//                    img.SetSourceThreadSafe( uri );
//                }
//                img.Cursor = Cursors.Hand;
//                img.MouseLeftButtonUp += 
//                    new MouseButtonEventHandler( Image_MouseLeftButtonUp );
//                img.MouseLeftButtonDown += 
//                    new MouseButtonEventHandler( Image_MouseLeftButtonDown );
//                TranslateTransform tr = 
//                    new TranslateTransform( ( i - CONTROL_MIDDLE ) * (SPACE_WIDTH + IMAGE_WIDTH), 0.0 );
//                img.RenderTransform = tr;

//                _lblList[ i ].RenderTransform = tr.Clone();
//                _lblList[ i ].Opacity = 0.0;
//            }

//            SwitchLargeImage();
//            Highlight.Enable( _imgList[ CONTROL_MIDDLE ] );

//            _alert.Opacity = 0.0;

//            _thdMove = new Thread( new ThreadStart( DoMove ) );
//            _thdMove.Start();
//        }

//        /// <summary>
//        /// triggered when the left key was pressed
//        /// </summary>
//        public void MoveLeft()
//        {
//            --_steps;
//        }

//        /// <summary>
//        /// triggered when the right key was pressed
//        /// </summary>
//        public void MoveRight()
//        {
//            ++_steps;
//        }

//        void Image_MouseLeftButtonUp( object sender, MouseButtonEventArgs e )
//        {
//            Image img = sender as Image;
//            if( img == _clicked )
//            {
//                int ctrIndex = _imgList.IndexOf( img );
//                _steps += CONTROL_MIDDLE - ctrIndex;
//            }
//            _clicked = null;
//            _callback();
//        }
//        void Image_MouseLeftButtonDown( object sender, MouseButtonEventArgs e )
//        {
//            _clicked = sender as Image;
//        }

//        void SwitchLargeImage( )
//        {
//            string srcPath = IMAGE_FOLDER + _indexMiddle.ToString("d3") + ".bmp";
//            _large.SetSourceThreadSafe( srcPath );
//        }

//        const int DURATION_MOVEMENT = 300;
//        const int DURATION_CHECKING = 350;
//        int _steps = 0;
//        private void DoMove()
//        {
//            while( true )
//            {
//                if( _steps > 0 )
//                {
//                    --_steps;
//                    DoMoveRight();
//                }
//                else if( _steps < 0 )
//                {
//                    ++_steps;
//                    DoMoveLeft();
//                }
//                Thread.Sleep( DURATION_CHECKING );
//            }
//        }

//        private void DoMoveRight( )
//        {
//            ResetLabelStatus();

//            List<Image> vec = new List<Image>();
//            Image rightest = _imgList[ _imgList.Count - 1 ];
//            vec.Add( rightest ); // move the rightest item to leftest

//            List<Label> lblVec = new List<Label>();
//            Label lblRightest = _lblList[ _lblList.Count - 1 ];
//            lblVec.Add( lblRightest );
//            for( int index = 0; index < _imgList.Count - 1; ++index )
//            {
//                Image img = _imgList[ index ];
//                vec.Add( img );
//                img.TranslateAnimationThreadSafe( IMAGE_WIDTH + SPACE_WIDTH,
//                    0.0, TimeSpan.FromMilliseconds( DURATION_MOVEMENT ) );
//                Label lbl = _lblList[ index ];
//                lblVec.Add( lbl );
//                lbl.TranslateAnimationThreadSafe( IMAGE_WIDTH + SPACE_WIDTH,
//                    0.0, TimeSpan.FromMilliseconds( DURATION_MOVEMENT ) );
//            }
//            _imgList = vec;
//            _indexMiddle = ( _indexMiddle - 1 + SOURCE_COUNT ) % SOURCE_COUNT;
//            SwitchLargeImage();
//            Highlight.Enable( _imgList[ CONTROL_MIDDLE ] );
//            Highlight.Disable( _imgList[ CONTROL_MIDDLE + 1 ] );
//            rightest.TranslateAnimationThreadSafe(
//                 -( IMAGE_WIDTH + SPACE_WIDTH ) * ( _imgList.Count - 1 ),
//                 0.0, TimeSpan.FromMilliseconds( 0 ) );
//            string srcPath = IMAGE_FOLDER +
//                ( ( _indexMiddle - _imgList.Count / 2 + SOURCE_COUNT ) % SOURCE_COUNT ).ToString("d3")
//                + ".bmp";

//            if (System.IO.File.Exists(srcPath))
//            {
//                rightest.SetSourceThreadSafe(srcPath);
//            }

//            _lblList = lblVec;
//            lblRightest.TranslateAnimationThreadSafe(
//                 -( IMAGE_WIDTH + SPACE_WIDTH ) * ( _imgList.Count - 1 ),
//                 0.0, TimeSpan.FromMilliseconds( 0 ) );
//        }

//        private void DoMoveLeft( )
//        {
//            ResetLabelStatus();

//            List<Image> vec = new List<Image>();
//            Image leftest = _imgList[ 0 ];
//            List<Label> lblVec = new List<Label>();
//            Label lblLeftest = _lblList[ 0 ];   
//            for( int index = 1; index < _imgList.Count; ++index )
//            {
//                Image img = _imgList[ index ];
                
//                vec.Add( img );
//                img.TranslateAnimationThreadSafe( -IMAGE_WIDTH - SPACE_WIDTH,
//                    0.0, TimeSpan.FromMilliseconds( DURATION_MOVEMENT ) );
//                Label lbl = _lblList[ index ];
//                lblVec.Add( lbl );
//                lbl.TranslateAnimationThreadSafe( -IMAGE_WIDTH - SPACE_WIDTH,
//                    0.0, TimeSpan.FromMilliseconds( DURATION_MOVEMENT ) );
//            }
//            vec.Add( leftest ); // move the leftest item to rightest
//            _imgList = vec;
//            _indexMiddle = ( _indexMiddle + 1 + SOURCE_COUNT ) % SOURCE_COUNT;
//            SwitchLargeImage();
//            Highlight.Enable( _imgList[ CONTROL_MIDDLE ] );
//            Highlight.Disable( _imgList[ CONTROL_MIDDLE - 1 ] );
//            leftest.TranslateAnimationThreadSafe(
//                 ( IMAGE_WIDTH + SPACE_WIDTH ) * ( _imgList.Count - 1 ),
//                 0.0, TimeSpan.FromMilliseconds( 0 ) );
//            string srcPath = IMAGE_FOLDER +
//                ( ( _indexMiddle + _imgList.Count / 2 + SOURCE_COUNT ) % SOURCE_COUNT ).ToString("d3")
//                + ".bmp";
//            leftest.SetSourceThreadSafe( srcPath );

//            lblVec.Add( lblLeftest );
//            _lblList = lblVec;
//            lblLeftest.TranslateAnimationThreadSafe(
//                 ( IMAGE_WIDTH + SPACE_WIDTH ) * ( _imgList.Count - 1 ),
//                 0.0, TimeSpan.FromMilliseconds( 0 ) );
//        }

//        public void ResetLabelStatus()
//        {
//            //int indexLeftSystem = _indexMiddle - _lblList.Count / 2;
//            //for( int i = 0; i < _lblList.Count; ++i )
//            //{
//            //    int indexSys = ( indexLeftSystem + i + SOURCE_COUNT ) % SOURCE_COUNT;
//            //    if ( _handler.IsSystemHasException( indexSys ) )
//            //        _lblList[ i ].SetOpacityThreadSafe( 0.8 );
//            //    else
//            //        _lblList[ i ].SetOpacityThreadSafe( 0.0 );
//            //}
//        }

//        public void SetImage( int index )
//        {
//            string path = IMAGE_FOLDER + index.ToString("d3") + ".bmp";
//            _large.SetSourceThreadSafe( path );
//            for( int i = 0; i < _imgList.Count; ++i )
//            {
//                path = IMAGE_FOLDER + (
//                    ( index - CONTROL_MIDDLE + i + SOURCE_COUNT ) % SOURCE_COUNT ).ToString("d3") +
//                    ".bmp";
//                _imgList[ i ].SetSourceThreadSafe( path );
//            }
//        }

//        public void StartFlash()
//        {
//            if ( _flash == null )
//                _flash = new Thread( new ThreadStart( Flash ) );
//            if ( !_flash.IsAlive )
//                _flash.Start();
//        }

//        public void StopFlash()
//        {
//            if ( _flash != null &&
//                _flash.ThreadState == ThreadState.Running )
//                _flash.Suspend();
//        }

//        private void Flash( )
//        {
            
//            //bool fadeout = true;
//            //double MIN_OPACITY = 0.1;
//            //double MAX_OPACITY = 1.0;
//            //int DURATION = 3000;
//            //while( true )
//            //{
//            //    if ( fadeout )
//            //        _large.SetOpacityAnimation( MAX_OPACITY, MIN_OPACITY,
//            //            TimeSpan.FromMilliseconds( DURATION ) );
//            //    else
//            //        _large.SetOpacityAnimation( MIN_OPACITY, MAX_OPACITY,
//            //            TimeSpan.FromMilliseconds( DURATION ) );
//            //    fadeout = !fadeout;
//            //    Thread.Sleep( DURATION + 100 );
//            //}

//            bool fadeout = true;
//            double MIN_OPACITY = 0.1;
//            double MAX_OPACITY = 1.0;
//            int DURATION = 1000;
//            FadeWrapper f = new FadeWrapper( _large.Parent as UIElement, _large );
//            while ( true )
//            {
//                if ( fadeout )
//                    f.Fadeout( 0, DURATION );
//                else
//                    f.Fadein( DURATION );
//                fadeout = !fadeout;
//                Thread.Sleep( DURATION );
//            }
//        }

//        public int IndexMiddle
//        {
//            get
//            {
//                return _indexMiddle;
//            }
//        }
//    }
//}
