using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JOYFULL.CMPW.Digit
{
    public class Digit
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(typeof(Digit));
        public enum DigitType
        {
            DIGIT_STRING = 0x0001,
            DIGIT_INT = 0x0010,
            DIGIT_TIME = 0x0100
        }

        public Digit()
        {
            IsBlackText = false;
            TextThreshHold = 128;
        }

        public  bool TryParseOld( System.Drawing.Bitmap bmp, out List<List<int>> ret )
        {
            List<List<int>> arr = new List<List<int>>();

            int width = bmp.Width;
            int height = bmp.Height;
            int[ , ] data = new int[ height, width ];
            for( int j = 0; j < height; ++j )
            {
                for( int i = 0; i < width; ++i )
                {
                    bool bBlack = bmp.GetPixel( i, j ).GetBrightness() < 0.5;
                    data[ j, i ] = bBlack ? 1 : 0;
                }
            }
            //img.Dispose();

            for( int r = 0; r < height - DigitMap.HEIGHT; ++r )
            {
                List<int> vec = new List<int>();
                bool matched = false;
                int lastDigitX = width;
                string s = string.Empty;
                for( int c = 0; c < width - DigitMap.WIDTH; ++c )
                {
                    int num = -1;
                    if( DigitMap.TryMatch( data, r, c, out num ) )
                    {
                        matched = true;
                        lastDigitX = c;
                        s += num.ToString();
                        c += DigitMap.WIDTH - 1;
                    }
                    else if( c - lastDigitX > DigitMap.WIDTH + 1 )
                    {
                        vec.Add( int.Parse( s ) );
                        s = string.Empty;
                        lastDigitX = width;
                    }
                }
                if( matched )
                {
                    arr.Add( vec );
                    r += DigitMap.HEIGHT - 1;
                }
            }
            ret = arr;

            return true;
        }

        public  bool TryParseOldNum( System.Drawing.Bitmap bmp, 
            int row, int col, out int ret )
        {
            ret = -1;
            if( row < 1 || col < 1 ) return false;
            List<List<int>> arr;
            if( TryParseOld( bmp, out arr ) && arr.Count >= row )
            {
                List<int> vec = arr[ row - 1 ];
                if( vec.Count >= col )
                {
                    ret = vec[ col - 1 ];
                    return true;
                }
            }
            return false;
        }

        private static string _SettingFile = "";

        public string ParameterSettingFile
        {
            get;
            set;
        }

        public int TextThreshHold
        {
            get;
            set;
        }

        public bool IsBlackText
        {
            get;
            set;
        }

        public  bool InitParameterSetting()
        {
            if (_SettingFile != ParameterSettingFile)
            {
                if (ANNWrapper.LoadBPParameters(ParameterSettingFile))
                    _SettingFile = ParameterSettingFile;
                else
                    return false;
            }
            return true;
        }


        //public bool TryParse( System.Drawing.Bitmap bmp, Rect rec,DigitType type ,out string ret )
        //{
        //    ret = "";
        //    if (!InitParameterSetting()) return false;


        //    return false;
        //}

        
        public bool TryParse(string bmpFile,DigitType type,out string ret)
        {
            ret = "";

            if (type == DigitType.DIGIT_STRING)
            {
                if (IsBlackText)
                    return ComplexTextParse_2(bmpFile,TextThreshHold, out ret);
                else
                    return ComplexTextParse_1(bmpFile,TextThreshHold,out ret);
            }      
            else if(type == DigitType.DIGIT_INT)
            {
                if (!InitParameterSetting()) return false;
                return NumberParse_1(bmpFile, out ret);
            }

            return false;
        }

        static public bool NumberParse_1(string bmpFile, out string ret)
        {
            ret = string.Empty;
            IntPtr hdibHandle =IntPtr.Zero;
            try
            {
                hdibHandle= ANNWrapper.ReadDIBFile(bmpFile);
                if (hdibHandle == IntPtr.Zero) return false;

                int[] intRes = new int[64];

				if(ANNWrapper.Convert256toGray(hdibHandle) &&
                ANNWrapper.ConvertGrayToWhiteBlack(hdibHandle) &&
                ANNWrapper.RemoveScatterNoise(hdibHandle) )
                {

                    Int32 charRectID = ANNWrapper.CharSegment(hdibHandle);

                    if (charRectID >= 0)
                    {
                        IntPtr newHdibHandle = ANNWrapper.AutoAlign(hdibHandle, charRectID);

                        if (newHdibHandle != IntPtr.Zero && ANNWrapper.Recognition_EX(newHdibHandle, charRectID, intRes))
                        {
                            foreach (int value in intRes)
                            {
                                if (value == -1)
                                {
                                    break;
                                }
                                else if (value >= 10)
                                {
                                    ret = string.Empty;
                                    break;
                                }
                                ret += value.ToString();
                            }
                        }
                        ANNWrapper.ReleaseDIBFile(newHdibHandle);
                    }
                }
            }
            catch (System.Exception e)
            {
            }

            if (hdibHandle!=IntPtr.Zero)
                ANNWrapper.ReleaseDIBFile(hdibHandle);
            
            return !string.IsNullOrEmpty( ret );
        }
        static public bool ComplexTextParse_1(string bmpFile, Int32 threshHold, out string strContent)
        {
            bool ret =true;
            strContent = "";
            try
            {
                ret = ret && ANNWrapper.RevertBlackWhiteBMP(bmpFile);
                ret = ret && ANNWrapper.BlackWhiteBMP(bmpFile, threshHold);
                ret = ret && ANNWrapper.ConvertBMP2TIF(bmpFile, bmpFile + "ComplexTextParse.tif");
                if (ret)
                {
                    byte[] tempParas = new byte[4096];
                    log.Info("ComplexTextParse_1: OCRFile " + bmpFile.ToString());
                    ret = ANNWrapper.OCRFile(bmpFile + "ComplexTextParse.tif", tempParas);
                    
                    if(ret)
                    {
                        strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(tempParas, 0, tempParas.Length);
                        strContent = strContent.Substring(0, strContent.IndexOf('\0'));
                    }
                }
            }
            catch (System.Exception e)
            {
                ret = false;
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return ret;
        }
        static public bool ComplexTextParse_2(string bmpFile, Int32 threshHold, out string strContent)
        {
            bool ret = true;
            strContent = "";
            try
            {
                ret = ret && ANNWrapper.BlackWhiteBMP(bmpFile, threshHold);
                ret = ret && ANNWrapper.ConvertBMP2TIF(bmpFile, bmpFile + "ComplexTextParse.tif");
                if (ret)
                {
                    byte[] tempParas = new byte[4096];
                    
                    log.Info("ComplexTextParse_2: OCRFile " + bmpFile.ToString());
                    ret = ANNWrapper.OCRFile(bmpFile + "ComplexTextParse.tif", tempParas);

                    if (ret)
                    {
                        strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(tempParas, 0, tempParas.Length);
                        strContent = strContent.Substring(0, strContent.IndexOf('\0'));
                    }
                }
            }
            catch (System.Exception e)
            {
                ret = false;
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return ret;
        }

        private static byte[] white_tempParas = new byte[4096];
        static public  bool RecognitionWhiteText(IntPtr hInputDIB, Int32 left, Int32 top, Int32 right,
            Int32 bottom, Int32 threshHold, string tifFileName, out string strContent)
        {
            bool ret = true;
            strContent = "";
            try
            {
                log.Info("RecognitionWhiteText: " + tifFileName.ToString());
                if (ANNWrapper.RecognitionWhiteText(hInputDIB,left,top,right,bottom,threshHold,tifFileName,white_tempParas))
                {
                    strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(white_tempParas, 0, white_tempParas.Length);
                    strContent = strContent.Substring(0, strContent.IndexOf('\0'));
                }
            }
            catch (System.Exception e)
            {
                ret = false;
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return ret;
        }

        private static byte[] black_tempParas = new byte[4096];
        static public bool RecognitionBlackText(IntPtr hInputDIB, Int32 left, Int32 top, Int32 right,
            Int32 bottom, Int32 threshHold, string tifFileName, out string strContent)
        {
            bool ret = true;
            strContent = "";
            try
            {
                log.Info("RecognitionBlackText: " + tifFileName.ToString());
                if (ANNWrapper.RecognitionBlackText(hInputDIB, left, top, right, bottom, threshHold, tifFileName, black_tempParas))
                {
                    strContent = System.Text.Encoding.GetEncoding("GB2312").GetString(black_tempParas, 0, black_tempParas.Length);
                    strContent = strContent.Substring(0, strContent.IndexOf('\0'));
                }
            }
            catch (System.Exception e)
            {
                ret = false;
                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
            return ret;
        }
    }
}
