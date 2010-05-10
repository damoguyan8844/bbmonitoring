using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JOYFULL.CMPW.Digit;
using System.Drawing;
using log4net;
namespace JOYFULL.CMPW.Presentation.SystemsHandler
{
    /// <summary>
    /// Finished
    /// </summary>
    
    class Logic_CCBSZhongDuanXiTong : Logic_BWSystemLogic
    {
        static public string _SystemName = "CCBS分行终端系统";
        static public string _SystemParasSettingFile ="BWParas.dat";
        static public int _SystemID = 1;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        public override bool EnsureSystem()
        {
            //string txt = string.Empty;
            //IntPtr hBmp = _SystemBitMap.Clone(
            //    new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
            //    _SystemBitMap.PixelFormat).GetHbitmap();
            //string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
            //bool recSuccessed = Digit.Digit.RecognitionWhiteText(hBmp,
            //    15, 600, 385, 630, _SystemDigitThreshHold,
            //    fileName, out txt);
            //SystemsHandler.DeleteObject(hBmp);
            //hBmp = IntPtr.Zero;
            ////  输入状态」 F1 一帮助， ESC 一退出
            //return recSuccessed &&
            //    (txt.Contains("状态") || txt.Contains("帮助") || txt.Contains("退出"));

            string txt = string.Empty;
            IntPtr hBmp = _SystemBitMap.Clone(
                new System.Drawing.Rectangle( 0, 0, _SystemBitMap.Width, _SystemBitMap.Height ),
                _SystemBitMap.PixelFormat ).GetHbitmap();
            string fileName = SystemsHandler.GetSystemFolder( _SystemID ) + "ensure.bmp";
            // 输入状态」 F1 一帮助
            bool recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 163, 625, 177, 650, fileName,
                _SystemDigitThreshHold, true );
            recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );

            if ( recSuccessed && txt == "1" )
            {// 退出，F3 - 查询上页
                recSuccessed = ANNWrapper.SaveBlockToBMP4( hBmp, 415, 625, 430, 650, fileName,
                    _SystemDigitThreshHold, true );
                recSuccessed = recSuccessed && m_DigitParser.TryParse( fileName,
                    JOYFULL.CMPW.Digit.Digit.DigitType.DIGIT_INT, out txt );
            }
            SystemsHandler.DeleteObject( hBmp );
            hBmp = IntPtr.Zero;
            return recSuccessed && txt == "3";
        }

        public Logic_CCBSZhongDuanXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER+ _SystemParasSettingFile;
        }

        static public int[]grid_lines = new int[] {246,273,304,330,358,390,415,445,472,500,527,554};
        static public int grid_digit_left = 9;
        static public int grid_dight_right = 160;

        static public int grid_text_left = 524;
        static public int grid_text_right = 800;
        
        static public  int _SystemDigitThreshHold = 180;

        public override void CheckCondition( SystemsSetting.Condition condition)
        {
            //base.CheckCondition(_SystemID ,sysMonitor, condition);
            string systemFolder = SystemsHandler.GetSystemFolder(_SystemID);
            if (systemFolder.Length < 1) return;
           
            System.Drawing.Bitmap bitSystem = SystemsHandler.GetSystemScreen(_SystemID);
            
            if (bitSystem == null)  return;

            IntPtr hBitMap= IntPtr.Zero;
            
            try
            {
                hBitMap = bitSystem.GetHbitmap();

                Rect range = new Rect(grid_digit_left, 0, grid_dight_right, 0);
                for (int i = 0; i < grid_lines.Length; i++)
                {
                    range.Top = grid_lines[i] - 13;
                    range.Bottom = grid_lines[i] + 13;
                    range.Left = grid_digit_left;
                    range.Right = grid_dight_right;

                    string tempDigitName = systemFolder + condition.Label + "_" + range.Top.ToString() + "_" + range.Bottom.ToString() + "_digit.bmp";

                    string strContentLocal="";
                    if (ANNWrapper.SaveBlockToBMP3(hBitMap, range.Left, range.Top, range.Right, range.Bottom, tempDigitName))
                    {
                        bool bContinue = true;
                        bContinue = bContinue && ANNWrapper.RevertBlackWhiteBMP(tempDigitName);
                        bContinue = bContinue && ANNWrapper.BlackWhiteBMP(tempDigitName, _SystemDigitThreshHold);

                        
                        if (bContinue && m_DigitParser.TryParse(tempDigitName, Digit.Digit.DigitType.DIGIT_INT, out strContentLocal))
                        {
                            if (strContentLocal.Length > 2 && //以00结尾则直接检测下一行
                                //strContentLocal.Substring(strContentLocal.Length - 2) == "00")
                                strContentLocal.EndsWith( "00" ) )
                                continue;
                        }
                    }

                    if (strContentLocal.Length > 0 && //营业单位代码被选中 
                        strContentLocal.Length < 6)   //致使其识别失败
                        continue;

                    range.Top = grid_lines[i] - 15;
                    range.Bottom = grid_lines[i] + 15;
                    range.Left = grid_text_left;
                    range.Right = grid_text_right;

                    string tempTextName = systemFolder + condition.Label + "_" + range.Top.ToString() + "_" + range.Bottom.ToString() + ".tif";
                    string strContent="";
                    if (Digit.Digit.RecognitionWhiteText(hBitMap, range.Left, range.Top, range.Right, range.Bottom,
                        _SystemSignThreshHold, tempTextName, out strContent))
                    {
                        base.CheckConditionWithValue(_SystemID, condition, strContent,false);
                        if (!condition.IsHasException && //日间
                            condition.Label == "系统通讯异常")
                        {
                            base.CheckConditionWithValue(_SystemID, condition, strContent, true);
                            break;
                        }
                        if (condition.IsHasException && //日终
                            condition.Label == "日终未签退机构")
                        {
                            base.CheckConditionWithValue(_SystemID, condition, strContent, true);
                            break;
                        }
                    }
                }
                SystemsHandler.DeleteObject(hBitMap);
                bitSystem.Dispose();
                bitSystem = null;
            }
            catch (System.Exception e)
            {
                if(hBitMap!=IntPtr.Zero)
                    SystemsHandler.DeleteObject(hBitMap);

                log.Error("\r\nSource:" + e.Source + "\r\nMessage:" + e.Message + "\r\n" + e.StackTrace);
            }
        }

    }
}
