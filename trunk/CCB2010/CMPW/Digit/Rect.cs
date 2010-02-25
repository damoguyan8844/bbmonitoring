using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Digit
{
    public class Rect
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Right { get; set; }
        public int Bottom { get; set; }

        public Rect( string x1y1x2y2)
        {
            string[] strs = x1y1x2y2.Split(';');
            
            if(strs.Length>3)
            {
                Left = Int32.Parse(strs[0]);
                Top = Int32.Parse(strs[1]);

                Right = Int32.Parse(strs[2]);
                Bottom = Int32.Parse(strs[3]);
            }
        }
        public Rect(int x1,int y1,int x2,int y2)
        {
            Left = x1;
            Top = y1;
            Right = x2;
            Bottom = y2;
        }
    }
}
