using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Digit
{
    internal static class DigitMap
    {
        public const int WIDTH = 6;
        public const int HEIGHT = 9;
        public static bool TryMatch( int[,] source, int r, int c, out int ret )
        {
            for( int i = 0; i < 10; ++i )
            {
                int[,] item = _maps[ i ];
                if( Match( item, source, r, c ) )
                {
                    ret = i;
                    return true;
                }
            }
            ret = -1;
            return false;
        }

        private static bool Match(int[,] item,int[,] source, int r, int c)
        {
 	        for( int i = 0; i < HEIGHT; ++i )
            {
                for( int j = 0; j < WIDTH; ++j )
                {
                    if( item[i,j] != source[r + i,c + j])
                        return false;
                }
            }
            return true;
        }

        static DigitMap()
        {
            _maps = new List<int[,]>();
            _maps.Add( map0 );
            _maps.Add( map1 );
            _maps.Add( map2 );
            _maps.Add( map3 );
            _maps.Add( map4 );
            _maps.Add( map5 );
            _maps.Add( map6 );
            _maps.Add( map7 );
            _maps.Add( map8 );
            _maps.Add( map9 );
        }
        private static List< int[,]> _maps;
        private static int[ , ] map0 = new int[ , ]
        {
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	0,	0,	0},
            {0,	0,	0,	0,	0,	0},
            {0,	0,	0,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1}
        };
        private static int[ , ] map1 = new int[ , ]
        {
            {1,	1,	0,	0,	1,	1},
            {1,	0,	0,	0,	1,	1},
            {0,	0,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {0,	0,	0,	0,	0,	0}
        };
        private static int[ , ] map2 = new int[ , ]
        {
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	0,	0,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	0,	0,	1,	1,	1},
            {0,	0,	1,	1,	1,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	0,	0,	0,	0}
        };
        private static int[ , ] map3 = new int[ , ]
        {
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	0,	0,	0,	1},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1}
        };
        private static int[ , ] map4 = new int[ , ]
        {
            {1,	1,	1,	1,	0,	1},
            {1,	1,	1,	0,	0,	1},
            {1,	1,	0,	0,	0,	1},
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	0,	0,	1},
            {0,	0,	0,	0,	0,	0},
            {1,	1,	1,	0,	0,	1},
            {1,	1,	1,	0,	0,	1},
            {1,	1,	0,	0,	0,	0}
        };
        private static int[ , ] map5 = new int[ , ]
        {
            {0,	0,	0,	0,	0,	0},
            {0,	0,	1,	1,	1,	1},
            {0,	0,	1,	1,	1,	1},
            {0,	0,	0,	0,	0,	1},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1}
        };
        private static int[ , ] map6 = new int[ , ]
        {
            {1,	1,	0,	0,	0,	1},
            {1,	0,	0,	1,	1,	1},
            {0,	0,	1,	1,	1,	1},
            {0,	0,	1,	1,	1,	1},
            {0,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1}
        };
        private static int[ , ] map7 = new int[ , ]
        {
            {0,	0,	0,	0,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	0,	0,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1},
            {1,	1,	0,	0,	1,	1}
        };
        private static int[ , ] map8 = new int[ , ]
        {
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	1}
        };
        private static int[ , ] map9 = new int[ , ]
        {
            {1,	0,	0,	0,	0,	1},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {0,	0,	1,	1,	0,	0},
            {1,	0,	0,	0,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	1,	0,	0},
            {1,	1,	1,	0,	0,	1},
            {1,	0,	0,	0,	1,	1}
        };
    }
}
