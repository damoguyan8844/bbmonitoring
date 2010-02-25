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
    /// 解决方案未定
    /// </summary>
    
    class Logic_ZhiPiaoYingXiangXiTong : SubSystemLogic
    {
        static public string _SystemName = "支票影像系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 8;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "全国票据影像";
        static public Rect _SystemSignRange = new Rect(128,115,360,145);
        static public int _SystemSignThreshHold = 80;

        //public override bool EnsureSystem()
        //{
        //    string txt = string.Empty;
        //    IntPtr hBmp = _SystemBitMap.Clone(
        //        new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
        //        _SystemBitMap.PixelFormat).GetHbitmap();
        //    string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
        //    bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
        //        420, 820, 685, 900, 120,
        //        fileName, out txt);
        //    SystemsHandler.DeleteObject(hBmp);
        //    hBmp = IntPtr.Zero;
        //    //省行柜员 【 0030 关：王鹏 】
        //    return recSuccessed &&
        //        (txt.Contains("省行") || txt.Contains("柜员"));
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
                titleName,out strContent))
            {
                //log.Error("Check System 2: " + _SystemID.ToString());

                if (CheckSystemSign(strContent, _SystemSignContent))
                {
                    return true;
                }
            }
            //log.Error("Check System 3: " + _SystemID.ToString());
            return false;
        }

        public Logic_ZhiPiaoYingXiangXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }
        static public int _SystemDigitThreshHold = 120;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            string strContent;
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + condition.Label + ".bmp";
            
            if (condition.Label.Contains("工作日期"))
            {
                if (Digit.Digit.ComplexTextParse_1(titleName, 120, out strContent))
                {
                    //" 2 009 一 12 一 14  "
                    strContent = strContent.Replace(" ", "");
                    strContent = strContent.Replace( "一","");
                    DateTime tomorrow = DateTime.Today.AddDays(1);
                    condition.StrValue = tomorrow.ToString("yyMMdd");
                    base.CheckConditionWithValue(_SystemID,  condition,strContent,true);
                }
            }
            else
            {
                //if (Digit.Digit.ComplexTextParse_1(titleName, 110, out strContent))
                {
                    strContent = condition.StrValue;
                    base.CheckConditionWithValue(_SystemID, condition, strContent,true);
                }
            }
              
            return;
        //  base.CheckCondition(_SystemID, sysMonitor, condition);
        }
    }
}
