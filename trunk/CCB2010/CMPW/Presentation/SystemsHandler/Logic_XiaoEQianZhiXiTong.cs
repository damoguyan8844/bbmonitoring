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
    class Logic_XiaoEQianZhiXiTong : SubSystemLogic
    {
        static public string _SystemName = "小额前置系统";
        static public string _SystemParasSettingFile = "BWParas.dat";
        static public int _SystemID = 4;
        static public System.Drawing.Bitmap _SystemBitMap = null;

        static public string _SystemSignContent = "状态更新日";
        static public Rect _SystemSignRange = new Rect(340, 80, 700, 215);
        static public int _SystemSignThreshHold = 110;

        //public override bool EnsureSystem()
        //{
        //    string txt = string.Empty;
        //    IntPtr hBmp = _SystemBitMap.Clone(
        //        new System.Drawing.Rectangle(0, 0, _SystemBitMap.Width, _SystemBitMap.Height),
        //        _SystemBitMap.PixelFormat).GetHbitmap();
        //    string fileName = SystemsHandler.GetSystemFolder(_SystemID) + "ensure.tif";
        //    bool recSuccessed = Digit.Digit.RecognitionBlackText(hBmp,
        //        435, 680, 700, 740, _SystemSignThreshHold,
        //        fileName, out txt);
        //    SystemsHandler.DeleteObject(hBmp);
        //    hBmp = IntPtr.Zero;
        //    //次组包时刻：”。： " " : " “哗录状态登录
        //    return recSuccessed &&
        //        (txt.Contains("组包") || txt.Contains("时刻"));
        //}

        static public bool CheckSystem(System.Drawing.Bitmap bitContent, IntPtr hBitMap)
        {
            //log.Info("Check System: " + _SystemID.ToString());
            
            string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "title.tif";
            string strContent;
            if (Digit.Digit.RecognitionBlackText(hBitMap,
                _SystemSignRange.Left,
                _SystemSignRange.Top,
                _SystemSignRange.Right,
                _SystemSignRange.Bottom,
                _SystemSignThreshHold,
                titleName, out strContent))
            {
                //log.Info("Check System: 2 " + _SystemID.ToString());
                
                if (Digit.Digit.ComplexTextParse_2(titleName, _SystemSignThreshHold, out strContent))
                {
                    if (CheckSystemSign(strContent, _SystemSignContent))
                    {
                        return true;
                    }
                }
            }
            //log.Info("Check System: 4 " + _SystemID.ToString());
            return false;
        }


        public Logic_XiaoEQianZhiXiTong()
        {
            m_DigitParser.ParameterSettingFile = SystemsHandler.PARAs_FOLDER + _SystemParasSettingFile;
            m_DigitParser.IsBlackText = true;
        }

        static public int _SystemDigitThreshHold = 160;
        static public int[] grid_lines_middle = new int[] { 145, 162, 179, 197, 214, 230, 246, 252 , 268, 282, 298,
        314, 330, 346, 362, 378, 394, 410, 426};

        public override void CheckCondition(SystemsSetting.Condition condition)
        {
            if (condition == null) return;
            if (condition.Label != "后台进程异常通知")
            {
                base.CheckCondition(_SystemID,condition);
                return;
            }

            System.Drawing.Bitmap bmpFile = SystemsHandler.GetSystemScreen(_SystemID);
            if (bmpFile == null) return;

            IntPtr hBitMap =IntPtr.Zero;
            try
            {
                hBitMap = bmpFile.GetHbitmap();
                Rect range = condition.Range;

                for (int index = 0; index < grid_lines_middle.Length; index++)
                {
                    range.Top = grid_lines_middle[index] - 15;
                    range.Bottom = grid_lines_middle[index] + 15;

                    string titleName = SystemsHandler.GetSystemFolder(_SystemID) + "_" + range.Top.ToString() + "_" + range.Bottom.ToString() + ".bmp";
                    if (ANNWrapper.SaveBlockToBMP3(hBitMap, range.Left, range.Top, range.Right, range.Bottom, titleName))
                    {
                        string ret;
                        if (m_DigitParser.TryParse(titleName, Digit.Digit.DigitType.DIGIT_STRING, out ret))
                        {
                            base.CheckConditionWithValue(_SystemID, condition, ret,false);
                            if (condition.IsHasException)
                            {
                                base.CheckConditionWithValue(_SystemID, condition, ret, true);
                                break;
                            }
                        }
                    }
                }
                SystemsHandler.DeleteObject(hBitMap);
                bmpFile.Dispose();
                bmpFile = null;
            }
            catch (System.Exception e)
            {
                if(hBitMap!=IntPtr.Zero)
                    SystemsHandler.DeleteObject(hBitMap);
                log.Error("\r\nSource:"+e.Source +"\r\nMessage:"+e.Message+"\r\n"+e.StackTrace);
            }
        }
    }
}
