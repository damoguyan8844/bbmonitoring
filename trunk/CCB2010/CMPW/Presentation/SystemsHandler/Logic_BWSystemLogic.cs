using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    public class Logic_BWSystemLogic : SubSystemLogic
    {
        static public Rect _SystemSignRange = new Rect(310, 58, 650, 125);
        static public int _SystemSignThreshHold = 180;

        static public int _SystemID_1 = 1;
        static public string _SystemSignContent_1 = "未签退营业机构";
        
        static public int _SystemID_3 = 3;
        static public string _SystemSignContent_3 = "大额支付系统汇划业务";

        static public int _SystemID_5 = 5;
        static public string _SystemSignContent_5 = "小额直联系统非实时业务";
        
        static public int _SystemID_6 = 6;
        static public string _SystemSignContent_6 = "大额支付系统事务";

        static public int _SystemID_7 = 7;
        static public string _SystemSignContent_7 = "小额直联系统非实时事务";

        static public int _SystemID_10 = 10;
        static public string _SystemSignContent_10 = "环境参数";
        
        static public int _SystemID_11 = 11;
        static public string _SystemSignContent_11 = "清算直联";

        
        static public int CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Error("Check BWSystem : " + _SystemID_1.ToString());
            string titleName = SystemsHandler.GetSystemFolder(_SystemID_5) + "title.tif";
            string strContent;
            if (Digit.Digit.RecognitionWhiteText(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                _SystemSignThreshHold,
                titleName, out strContent))
            {
                //log.Error("Check BWSystem 2: " + _SystemID_1.ToString());
                Logic_QingSuanXiTong.TryCheckInSystem(bitContent,hBitMap,strContent);
                Logic_QingSuanZhiLianXiTong.TryCheckInSystem(bitContent, hBitMap, strContent);

                if (CheckSystemSign(strContent ,_SystemSignContent_1))
                {
                    return _SystemID_1;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_3))
                {
                    return _SystemID_3;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_5))
                {
                    return _SystemID_5;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_6))
                {
                    return _SystemID_6;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_7))
                {
                    return _SystemID_7;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_10))
                {
                    return _SystemID_10;
                }
                else if (CheckSystemSign(strContent, _SystemSignContent_11))
                {
                    return _SystemID_11;
                }
                else 
                {
                    return SystemsHandler.INVALID_SYSTEMID;
                }
            }
            //log.Error("Check BWSystem 3: " + _SystemID_1.ToString());
            return SystemsHandler.INVALID_SYSTEMID;
        }
    }
}
