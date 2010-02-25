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
//        Image _large, _clicked;
//        List<Image> _imgList;

//        readonly int SOURCE_COUNT = 14;
//        readonly int CONTROL_MIDDLE;
//        readonly string IMAGE_FOLDER;
//        readonly double IMAGE_WIDTH;
//        readonly double SPACE_WIDTH = 20;

//        int _indexMiddle = 0;
//        //Thread _thdMove;
//        //Thread _flash;

//        public delegate void ImageClickCallback();
//        ImageClickCallback _callback;

//        public ImageManager( Image imgLarge, Image[] imgList,
//            ImageClickCallback callback )
//        {
//            _large = imgLarge; 
//            _imgList = new List<Image>( imgList );
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
//                img.SetSourceThreadSafe( uri );
//                img.Cursor = Cursors.Hand;
//                img.MouseLeftButtonUp += 
//                    new MouseButtonEventHandler( Image_MouseLeftButtonUp );
//                img.MouseLeftButtonDown += 
//                    new MouseButtonEventHandler( Image_MouseLeftButtonDown );
//                TranslateTransform tr = 
//                    new TranslateTransform( ( i - CONTROL_MIDDLE ) * (SPACE_WIDTH + IMAGE_WIDTH), 0.0 );
//                img.RenderTransform = tr;
//            }

//            SwitchLargeImage();
//            Highlight.Enable( _imgList[ CONTROL_MIDDLE ] );
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

//        private void DoMoveRight( )
//        {
//        }

//        private void DoMoveLeft( )
//        {

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

//        public int IndexMiddle
//        {
//            get
//            {
//                return _indexMiddle;
//            }
//        }
//    }
//}
