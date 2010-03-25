using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ImageTest
{
    public partial class Form1 : Form
    {
        string FOLDER = @"C:\Documents and Settings\HJ\桌面\ImageTest\";
        bool[,] CHARACTOR = new bool[ , ]{
	        { true , false, false, false, false, false, false, false, false, false, false, false, true },
	        { false, true , true , false, true , true , true , true , true , false, false, false, true },
	        { false, false, true , false, true , false, false, false, true , false, true , false, true },
	        { true , false, false, false, true , false, true , false, true , false, true , false, true },
	        { false, true , false, false, true , false, true , false, true , false, true , false, true },
	        { false, true , false, false, true , false, true , false, true , false, true , false, true },
	        { false, false, false, true , true , false, true , false, true , false, true , false, true },
	        { false, false, true , false, true , false, true , false, true , false, true , false, true },
	        { true , true , false, false, true , false, true , false, true , false, true , false, true },
	        { false, true , false, false, false, false, true , false, false, false, false, false, true },
	        { false, true , false, false, false, true , false, true , false, false, false, false, true },
	        { false, true , false, false, true , false, false, false, true , false, true , false, true },
	        { false, true , false, true , false, false, false, false, true , false, false, true , false}
        };

        int CHARACTOR_WIDTH = 0;
        int CHARACTOR_HEIGHT = 0;
        public Form1()
        {
            InitializeComponent();
            CHARACTOR_HEIGHT = CHARACTOR.GetUpperBound( 0 );
            CHARACTOR_WIDTH = CHARACTOR.GetUpperBound( 1 );
        }

        private void button1_Click( object sender, EventArgs e )
        {
            Bitmap bmp = CaptureScreen.GetDesktopImage();
            int width = bmp.Width;
            int height = bmp.Height;
            bool[,] screen = new bool[ width, height ];
            for( int x = 0; x < width; ++x )
            {
                for( int y = 0; y < height; ++y )
                {
                    screen[ x, y ] = bmp.GetPixel( x, y ).GetBrightness() < 0.5;
                }
            }

            int count = 0;
            for( int x = 0; x < width - CHARACTOR_WIDTH; ++x )
            {
                for( int y = 0; y < height - CHARACTOR_HEIGHT; ++y )
                {
                    if( TryMatch( screen, x, y ) )
                        ++count;
                }
            }
            MessageBox.Show( count.ToString() );
        }

        private void button2_Click( object sender, EventArgs e )
        {
            string path = FOLDER + "测.bmp";
            Bitmap bmp = Bitmap.FromFile( path ) as Bitmap;
            int width = bmp.Width;
            int height = bmp.Height;

            StreamWriter sw = new StreamWriter( FOLDER + "encode.txt" );
            sw.WriteLine( "{" );
            for( int i = 0; i < height; ++i)
            {
                sw.Write( "\t{" );
                for( int j = 0; j < width; ++j )
                {
                    if( bmp.GetPixel( j, i ).GetBrightness() < 0.5 )
                        sw.Write( " true " );
                    else
                        sw.Write( " false" );
                    if( j != width - 1 )
                        sw.Write( "," );
                }
                sw.Write( "}" );
                if( i != height - 1 )
                    sw.Write( "," );
                sw.WriteLine( "" );
            }
            sw.Write( "}" );
            sw.Close();
        }

        private bool TryMatch( bool[,] screen, int x, int y )
        {
            for( int i = 0; i < CHARACTOR_HEIGHT; ++i )
            {
                for( int j = 0; j < CHARACTOR_WIDTH; ++j )
                {
                    if( screen[ x + j, y + i ] != CHARACTOR[ i, j ] )
                        return false;
                }
            }
            return true;
        }
    }
}
