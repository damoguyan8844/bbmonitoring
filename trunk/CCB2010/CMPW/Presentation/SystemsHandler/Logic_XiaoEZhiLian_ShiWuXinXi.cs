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
    class Logic_XiaoEZhiLian_ShiWuXinXi : SubSystemLogic
    {
        static public string _SystemName = "小额直联系统(非实时事务)";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 7;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    220, 625, 620, 665, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////ESC -- 退出：每隔 30 秒自动刷新
            //return recSuccessed && 
            //    (txt.Contains("退出") || txt.Contains("自动") || txt.Contains("刷新"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // ESC -- 退出：每隔 30 秒自动刷新
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 425, 630, 453, 660, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && ( txt == "30" || txt == "40" );
        }

        public Logic_XiaoEZhiLian_ShiWuXinXi()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
        }

        static public int _SystemDigitThreshHold = 180;
        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if (condition.Label.Contains("未发送笔数"))
            {
               
                //获取已接受
                string receiveValue = GetConditionValue(_SystemID, condition.Type, condition.Label+"_1");            
                //获取已发送
                string sendValue = GetConditionValue(_SystemID, condition.Type, condition.Label + "_2");


                log.Info(_SystemID.ToString() + "  Condition Left:" + condition.Label + "NewValue:" + receiveValue.ToString());
                log.Info(_SystemID.ToString() + "  Condition Right:" + condition.Label + "NewValue:" + sendValue.ToString());
               
                Int32 receive=-1,send=0;
                if (Int32.TryParse(receiveValue,out receive) && Int32.TryParse(sendValue,out send))
                {
                    receive-=send;
                    base.CheckConditionWithValue(_SystemID, condition, receive.ToString(),true);
                } 
            }
            else
            {
                base.CheckCondition(_SystemID, condition);
            }
        }
    }
}
