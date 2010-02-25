using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    
    class Logic_ZhengQuanXiTong: SubSystemLogic
    {
        static public string _SystemName = "证券系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 13;
        static public System.Drawing.Bitmap _SystemBitMap = null;


        static public string _SystemSignContent = "网点号流水号";
        static public Rect _SystemSignRange = new Rect(0, 110, 600, 180);
        static public int _SystemSignThreshHold = 180;
        static public int _FailureTimes = 1;

        //public override bool EnsureSystem()
        //{
        //    string txt = string.Empty;
        //    IntPtr hBmp = _SystemBitMap.Clone(
        //        new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
        //        _SystemBitMap.PixelFormat).GetHbitmap();
        //    string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
        //    bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
        //        80, 940, 300, 990, 128,
        //        fileName, out txt);
        //    SystemsHandler.DeleteObject(hBmp);
        //    hBmp = IntPtr.Zero;
        //    //VT100 TCP 月 P 13 : 18 QQQO
        //    return recSuccessed &&
        //        (txt.Contains("VT100") || txt.Contains("TCP"));
        //}

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Info("Check System: " + _SystemID.ToString());
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "title.tif";
            string strContent;
            if (Digit.Digit.RecognitionWhiteText(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                _SystemSignThreshHold,
                titleName, out strContent))
            {
                //log.Info("Check System: 2 " + _SystemID.ToString());
                if (CheckSystemSign( strContent ,_SystemSignContent))
                {
                    return true;
                }
            }
            //log.Info("Check System: 3 " + _SystemID.ToString());

            return false;
        }
        public Logic_ZhengQuanXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
            m_DigitParser.TextThreshHold = _SystemDigitThreshHold;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if (condition.Label.Contains("失败次数"))
            {
                _FailureTimes = condition.IntValue;
                return ;
            }

            if (!condition.Label.Contains("存在失败交易"))
               base.CheckCondition(_SystemID, condition);
            else
            {
                string strContent;
                string titleName = SystemsHandler.GetSystemFolder(_SystemID) + condition.Label + ".bmp";
                if (Digit.Digit.ComplexTextParse_1(titleName, _SystemSignThreshHold, out strContent))
                {
                    int times = CountStringOccurrences( strContent, "失败" );
                    if (times <= _FailureTimes)
                        strContent = "";

                    base.CheckConditionWithValue(_SystemID, condition, strContent,true);
                }
            }
            return;
        }

        public static int CountStringOccurrences( string text, string pattern )
        {
            // Loop through all instances of the string 'text'.
            int count = 0;
            int i = 0;
            while ( ( i = text.IndexOf( pattern, i ) ) != -1 )
            {
                i += pattern.Length;
                count++;
            }
            return count;
        }

    }
}
