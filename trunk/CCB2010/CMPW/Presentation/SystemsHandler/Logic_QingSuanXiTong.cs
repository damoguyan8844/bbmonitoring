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
    class Logic_QingSuanXiTong:SubSystemLogic
    {
        static public string _SystemName = "清算系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 10;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    25, 575, 165, 670, 190,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////查询分析表试算平衡表其它
            //return recSuccessed &&
            //    (txt.Contains("分析") || txt.Contains("平衡") || txt.Contains("其它"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // 机构代码 5028000000
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 410, 360, 540, 390, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && ( txt.Contains("280") || txt.Contains("0000"));
        }

        public Logic_QingSuanXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            m_DigitParser.TextThreshHold = _SystemDigitThreshHold;
            base.CheckCondition(_SystemID,  condition);
        }
        /// <summary>
        /// 
        /// 清算：
        /// step 0：
        /// （310, 58, 650, 120） 180   ：中国建设银行资金清算 【 业务管理子系统 】
        /// （135, 215, 595, 260） 180  ：【 系统管理：签到 】
        /// （135, 435, 595, 480） 180  ：签到申请密钥成功！任意键返回．
        /// 
        /// 
        /// </summary>
        static public bool _IsSignedIn=false;
        static private bool _Is_Step_0_Passed = false;

        public static void TryCheckInSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap, string titleContent)
        {
            if (_IsSignedIn || !titleContent.Contains("业务管理子系统 ")) return;

            string titleName; 
            string strContent;
            
            titleName  = SystemsHandler.GetSystemFolder(_SystemID) + "step0.tif";
            strContent = "";
            if (!_Is_Step_0_Passed && Digit.Digit.RecognitionWhiteText(hBitMap,
                135,435,595,480,180,titleName, out strContent))
            {
                if(strContent.Contains("签到")&&
                   strContent.Contains("密钥")&&
                   strContent.Contains("成功"))
                {
                    _Is_Step_0_Passed = true;
                }
            }
            if (_Is_Step_0_Passed)
            {
                SystemsHandler.CheckInSystem_Ex(_SystemID);
                _IsSignedIn = true;
            }
        }
    }
}
