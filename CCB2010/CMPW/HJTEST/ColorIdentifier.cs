using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace HJTEST
{
    internal class ColorIdentifier
    {
        public static void Interest()
        {
            string path = "C:/Test.bmp";
            Bitmap bmp = new Bitmap( path );

            Bitmap interest = new Bitmap( 500, 500, bmp.PixelFormat );
            Graphics g = Graphics.FromImage( interest );
            //g.PageUnit = GraphicsUnit.Pixel;
            //g.Clear( Color.Transparent );
            g.DrawImage( bmp,
                //new RectangleF( 0, 10, 0, 410 ),
                //new RectangleF( 10f, 190f, 20f, 600f ), 
                new RectangleF( 0, 500, 0, 500 ),
                new RectangleF( 0f, 500f, 0f, 500f ),
                GraphicsUnit.Pixel );
            g.Dispose();

            string dest = "C:/Interest.bmp";
            interest.Save( dest );
        }

        public static Image Get()
        {
            string path = "C:/Test.bmp";
            Bitmap bmp = new Bitmap( path );

            Bitmap interest = new Bitmap( 510, 510, bmp.PixelFormat );
            Graphics g = Graphics.FromImage( interest );
            //g.PageUnit = GraphicsUnit.Pixel;
            //g.Clear( Color.Transparent );
            g.DrawLine( new Pen( Color.Red ), new Point( 0, 0 ), new Point( 400, 400 ) );
            g.DrawImage( bmp,
                //new RectangleF( 0, 10, 0, 410 ),
                //new RectangleF( 10f, 190f, 20f, 600f ), 
                new RectangleF( 0, 500, 0, 500 ),
                new RectangleF( 0f, 500f, 0f, 500f ),
                GraphicsUnit.Pixel );
            g.Dispose();
            return interest;
        }
    }
}
