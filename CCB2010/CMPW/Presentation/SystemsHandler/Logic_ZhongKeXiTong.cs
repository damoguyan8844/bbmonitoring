using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using JOYFULL.CMPW.DAL;
using JOYFULL.CMPW.Model;
using JOYFULL.CMPW.Digit;

using JOYFULL.CMPW.Presentation.SystemsSetting;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    class Logic_ZhongKeXiTong : SubSystemLogic
    {
        static public string _SystemName = "重客系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 9;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "0009";
        static public Rect _SystemSignRange = new Rect(55, 80, 140, 115);
        static public int _SystemSignThreshHold = 128;
        static public Digit.Digit _DigitParser = new Digit.Digit();

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
            //    150, 40, 360, 140, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////异常交易总笔数异常节点个数
            //return recSuccessed &&
            //    (txt.Contains("节点") || txt.Contains("个数"));

            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
            //    _SystemBitMap.PixelFormat ).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // 0009
            //bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 
            //    _SystemSignRange.Left,
            //    _SystemSignRange.Top,
            //    _SystemSignRange.Right,
            //    _SystemSignRange.Bottom, fileName,
            //    _SystemDigitThreshHold, false );
            //recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
            //    JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            //SystemsHandler.DeleteObject( hBmp );
            //hBmp = IntPtr.Zero;
            //return recSuccessed && ( txt.StartsWith("000"));
            return true;
        }

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Error("Check System: " + _SystemID.ToString());
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "title.bmp";
            if (ANNWrapper.SaveBlockToBMP3(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                titleName))
            {
                //log.Error("Check System: 2 " + _SystemID.ToString());

                _DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
                _DigitParser.TextThreshHold = _SystemSignThreshHold;

                ANNWrapper.BlackWhiteBMP(titleName, _SystemSignThreshHold);

                string strContent = "";
                if (_DigitParser.TryParse(titleName, Digit.Digit.DigitType.DIGIT_INT, out strContent))
                {
                    if (strContent == _SystemSignContent)
                    {
                        return true;
                    }
                }
                //log.Error("Check System: 3 " + _SystemID.ToString());
            }
            //log.Error("Check System: 4 " + _SystemID.ToString());
            return false;
        }


        public Logic_ZhongKeXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 128;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            base.CheckCondition(_SystemID, condition);
            return;
        }
    }
}
