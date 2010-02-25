using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Identifier
{
    public class Identifier
    {
        public const int INVALID_SYSTEM_ID = -1;
        public Identifier()
        {

        }

        public SystemEnum Identify( Bitmap bmp )
        {
            SystemEnum ret = SystemEnum.INVALID;
            if( bmp != null )
            {

            }
            return ret;
        }

        //private SystemEnum IdentifyEndPc( Bitmap bmp )
        //{
        //    SystemEnum ret = SystemEnum.INVALID;

        //    string path = "temp.bmp";
        //    const int left = 310;
        //    const int top = 58;
        //    const int right = 650;
        //    const int bottom = 120;
        //    const int threshHold = 180;
        //    ANNWrapper.SaveBlockToBMP3(bmp.GetHbitmap(),
        //        left, top, right, bottom, path);
        //    if( ANNWrapper.RevertBlackWhiteBMP( path ) &&
        //        ANNWrapper.BlackWhiteBMP( path, threshHold ) &&
        //        ANNWrapper.ConvertBMP2TIF( path, path ) )
        //    {
        //        byte[] data = new byte[256];
        //        if( ANNWrapper.OCRFile(path, data) )
        //        {
        //            string title = System.Text.Encoding.GetEncoding("GB2312").GetString(data, 0, data.Length);
        //            if (title.Contains("中国建设银行大额支付系统汇划业务监控"))
        //                ret = SystemEnum.DA_E_YE_WU;
        //            else if (title.Contains("中国建设银行大额支付系统事务信息监控"))
        //                ret = SystemEnum.DA_E_SHI_WU;
        //        }

        //    }

        //}
    }
}
