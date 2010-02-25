using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Digit;

namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    
    class Logic_QingSuanZhiLianXiTong : SubSystemLogic
    {
        static public string _SystemName = "清算直联系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 11;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    290, 630, 660, 660, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////任意键刷新， q / Q / Esc 键退出．
            //return recSuccessed && 
            //    (txt.Contains("任意") || txt.Contains("刷新") || txt.Contains("退出"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // 大额报文  退回笔数  往帐汇划  0
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 260, 580, 290, 605, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName, 
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );
            
            if( recSuccessed && txt == "0" )
            {// 大额报文  退回笔数  往帐事务  0
                recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 410, 580, 430, 605, fileName,
                    _SystemDigitThreshHold, true );
                recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                    JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );
            }
            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && txt == "0";
        }

        public Logic_QingSuanZhiLianXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            base.CheckCondition(_SystemID, condition);
        }

        /// <summary>
        /// 清算直联：
        /// step 0 :
        /// （210, 275, 695, 315） 180   ：【 建设银行清算直联系统 】
        /// step 1：
        /// （310, 58, 650, 120） 180   ：中国建设银行清算直联管理系统
        /// （15, 625, 910, 665） 180  ：清算直联系统进行工前处理
        /// step 2：
        /// （310, 58, 650, 120） 180   ：中国建设银行清算直联管理系统
        /// （15, 625, 910, 665） 180  ：启动往帐接收主机成功，任意键返回！
        /// step 3：
        /// （310, 58, 650, 120） 180   ：中国建设银行清算直联管理系统
        /// （15, 625, 910, 665） 180  ：启动往帐发送清算成功，任意键返回！
        /// step 4：
        /// （310, 58, 650, 120） 180   ：中国建设银行清算直联管理系统
        /// （15, 625, 910, 665） 180  ：启动来帐接收清算成功，任意键返回
        /// step 5：
        /// （310, 58, 650, 120） 180   ：中国建设银行清算直联管理系统
        /// （15, 625, 910, 665） 180  ：启动来帐发送主机成功，任意键返回！
        /// </summary>
        static public bool _IsSignedIn = false;
        static private bool _Is_Step_1_Passed = false;
        static private bool _Is_Step_2_Passed = false;
        static private bool _Is_Step_3_Passed = false;
        static private bool _Is_Step_4_Passed = false;
        static private bool _Is_Step_5_Passed = false;

        public static void TryCheckInSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap, string titleContent)
        {
            if (_IsSignedIn || !titleContent.Contains("清算直联")) return ;
            
            string titleName;
            string strContent;
            
            titleName = SystemsHandler.GetSystemFolder(_SystemID) + "step1.tif";
            strContent = "";
            if(Digit.Digit.RecognitionWhiteText(hBitMap,
                135, 435, 595, 480, 180, titleName, out strContent))
            {
                if (!_Is_Step_1_Passed && strContent.Contains("清算直联") && strContent.Contains("工前处理"))
                {
                    _Is_Step_1_Passed = true;
                }
                if (!_Is_Step_2_Passed && strContent.Contains("往帐接收") && strContent.Contains("成功"))
                {
                    _Is_Step_2_Passed = true;
                }
                if (!_Is_Step_3_Passed && strContent.Contains("发送清算") && strContent.Contains("成功"))
                {
                    _Is_Step_3_Passed = true;
                }
                if (!_Is_Step_4_Passed && strContent.Contains("接收清算") && strContent.Contains("成功"))
                {
                    _Is_Step_4_Passed = true;
                }
                if (!_Is_Step_5_Passed && strContent.Contains("来帐发送") && strContent.Contains("成功"))
                {
                    _Is_Step_5_Passed = true;
                }
            }
           
            if (_Is_Step_1_Passed &&
                _Is_Step_2_Passed &&
                _Is_Step_3_Passed &&
                _Is_Step_4_Passed &&
                _Is_Step_5_Passed )
            {
                SystemsHandler.CheckInSystem_Ex(_SystemID);
                _IsSignedIn = true;
            }
        }
    }
}
