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
    /// 无法获取图标
    /// </summary>
    class Logic_YinBaoTongXiTong : SubSystemLogic
    {
        static public string _SystemName = "银保通系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 14;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "银保重空交易";
        static public string _SystemSignContent2 = "银保联机交易";
        static public Rect _SystemSignRange = new Rect(0, 0, 390, 35);
        static public int _SystemSignThreshHold = 128;

        //public override bool EnsureSystem()
        //{
        //    string txt = string.Empty;
        //    IntPtr hBmp = _SystemBitMap.Clone(
        //        new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
        //        _SystemBitMap.PixelFormat).GetHbitmap();
        //    string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
        //    bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
        //        25, 600, 180, 740, _SystemDigitThreshHold,
        //        fileName, out txt);
        //    SystemsHandler.DeleteObject(hBmp);
        //    hBmp = IntPtr.Zero;
        //    //警告信息
        //    return recSuccessed &&
        //        (txt.Contains("警告") || txt.Contains("信息"));
        //}

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Error("Check System: " + _SystemID.ToString());
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
                //log.Error("Check System: 2 " + _SystemID.ToString());

                if (CheckSystemSign(strContent, _SystemSignContent))
               {
                  return true;
               }
            }
            //log.Error("Check System: 3 " + _SystemID.ToString());
            return false;
        }


        public Logic_YinBaoTongXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            condition.lastCheckValue = "无异常";
        //  base.CheckCondition(_SystemID, sysMonitor, condition);
        }
    }
}
